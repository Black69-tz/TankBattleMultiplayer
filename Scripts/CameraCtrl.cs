using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CameraCtrl : NetworkBehaviour
{
	private AudioSource auSource;
	public AudioClip hitSound;
	public AudioClip shotSound;
	
	public float recoilForce =50000 ;
	public float roMin = 0;
	public float roMax = 0;
	float inputAngleX = 0;
	float inputAngleY = 0;
	public float rotateSpeed =3f;
	bool normalA = true;
	bool driverA = false;
	public static bool commandA = false;
	bool gunnerA = false; 
	
	public Transform turretp;
	public Transform barrelp;
	
	public Image loadingTex;
	
	public float loadTime = 4;
	float fireCool=0;
	public float curFireCool;
	
	public KeyCode shootKey = KeyCode.Space ;
	public Transform  projectileMount;
	public GameObject projectilePrefab;
	public GameObject fireSmoke;
	
	CustomNetworkManager manager;
	
	public Text playNameLabel;
	public GameObject  healthBar;
	
	private Rigidbody tankRigid;
	
	[SyncVar ]
	public string playerName = "";
	
	[SyncVar ]
	public float healthBarValue = 0;
	
	[HideInInspector]
	public GameObject killedBy;
	
	public Text scoreText;

	public GameObject healthBar2;
	//LifeHealth healthl;
	
	[SyncVar ]
	public int score = 0;
	
	
	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		Camera.main.orthographic = false;
		Camera .main.transform .SetParent (transform);
		Camera.main.transform.localPosition = new Vector3(0f, 5f, -10f);
		Camera.main.transform.localEulerAngles = new Vector3(20f, 0f, 0f);
		
		playerName = PlayerPrefs .GetString ("playerName","NewBie");
		
		//healthl .othPly = this;
	}
	
	void OnDisable()
	{
		if (isLocalPlayer && Camera.main != null)
		{
			
			//Camera.main.orthographic = true;
			Camera.main.transform.SetParent(null);
			//Camera.main.transform.localPosition = new Vector3(0f, 70f, 0f);
			//Camera.main.transform.localEulerAngles = new Vector3(20f, 0f, 0f);
			//tampPlayerName = "";
		}
	}
	
	void Start ()
	{
		manager = GetComponent<CustomNetworkManager >();
		tankRigid = GetComponent <Rigidbody >();
		auSource = GetComponent<AudioSource >();
		transform .GetComponent <LifeHealth >().othPly = this;
		
		normalA = true;
		driverA = false;
		commandA = false;
		gunnerA = false;
		auSource .clip = null;
	}
	
	void Update()
	{
		if(!isLocalPlayer )
			return;
			
		scoreText .text = "KILL : " + score .ToString ();
		if (curFireCool >= loadTime  && Input .GetKeyDown(shootKey ))
		{
			tankRigid .AddForce (turretp .forward * recoilForce, ForceMode.Impulse );
			
			CmdFire ();
			curFireCool = fireCool ;
		}
		else
		{
			curFireCool += 1 * Time .deltaTime ;
		}
		
		loadingTex.fillAmount = curFireCool /loadTime;
			
		if (gunnerA == true || normalA == true && Input .GetKey (KeyCode.LeftShift))
		{
			inputAngleX = Input .GetAxis ("Mouse X") * rotateSpeed;
			inputAngleY = Input .GetAxis ("Mouse Y") * rotateSpeed ;
		}
			
		else{
			inputAngleX = 0;
			inputAngleY = 0;
		}
		
		turretp  .Rotate(0,inputAngleX  ,0);
			
		Quaternion tempQua = barrelp .localRotation ;
		tempQua = tempQua * Quaternion .Euler (-1.0f * inputAngleY ,0.0f,0.0f);
			
		float barrelCurrentAngleX = tempQua .eulerAngles .x;
			
		if (barrelCurrentAngleX > 180.0f )
		{
			barrelCurrentAngleX = barrelCurrentAngleX - 360.0f ;
		}
			
		barrelCurrentAngleX = Mathf .Clamp (barrelCurrentAngleX ,roMin , roMax);
			
		barrelp  .localRotation = Quaternion .Euler (barrelCurrentAngleX , 0, 0);
			
		OneClickCam ();
		KeyCam ();
		
		CmdSendName (playerName);
		
		healthBarValue = transform .GetComponent<LifeHealth>().getCurHealth ();
		CmdHealthBarValue (healthBarValue /100f);
		//canvasC .gameObject .SetActive (false);
		
		playNameLabel .gameObject .SetActive (false );
	}
	
	[Command ]
	void CmdSendName(string namett)
	{
		RpcNameSends(namett);
	}
	
	[ClientRpc]
	void RpcNameSends(string sendname)
	{
		playNameLabel .text = sendname ;
	}
	
	[Command]
	void CmdHealthBarValue(float healthvalue)
	{
		RpcHealthBarValue (healthvalue );
	}
	
	[ClientRpc]
	void RpcHealthBarValue(float value)
	{
		healthBar .GetComponent<Slider >().value = value ;
		healthBar2 .GetComponent<Slider>().value = value ;
	}
	
	[Command]
	void CmdFire()
	{
		GameObject fires = Instantiate (fireSmoke ,projectileMount .position ,projectileMount .rotation );
		NetworkServer .Spawn (fires );
		GameObject projectile = Instantiate(projectilePrefab, projectileMount .position, projectileMount .rotation);
		NetworkServer.Spawn(projectile);
		
		Projectile pro = projectile .GetComponent<Projectile >();
		pro .owner = gameObject ;
		RpcOnfire ();
	}
	[ClientRpc ]
	void RpcOnfire ()
	{
		auSource .PlayOneShot(shotSound,1f);
	}
	
	void OneClickCam()
	{
		if( Input .GetKeyDown (KeyCode.Alpha1))
		{
			normalA = false;
			gunnerA = true; 
			driverA = false;
			commandA = false;
			Camera .main.transform .SetParent (turretp.transform );
			Camera .main.transform .localPosition = new Vector3 (-2.5f,1f,9f);
			Camera.main.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			healthBar2 .gameObject .SetActive (true);
		}
		if( Input .GetKeyDown (KeyCode.Alpha2))
		{
			normalA = false;
			gunnerA = false; 
			driverA = true;
			commandA = false;
			Camera .main.transform .SetParent (transform );
			Camera .main.transform .localPosition = new Vector3 (0f,0.1f,2.2f);
			Camera.main.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			healthBar2 .gameObject .SetActive (false );
		}
		if( Input .GetKeyDown (KeyCode.Alpha3))
		{
			normalA = false;
			gunnerA = false; 
			driverA = false;
			commandA = true;
			Camera .main.transform .SetParent (turretp.transform );
			Camera .main.transform .localPosition = new Vector3 (2.8f, 6.7f, -3.4f);
			Camera.main.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			healthBar2 .gameObject .SetActive (false );
		}
		if( Input .GetKeyDown (KeyCode.Alpha4))
		{
			normalA = true;
			gunnerA = false; 
			driverA = false;
			commandA = false;
			Camera .main.transform .SetParent (transform );
			Camera.main.transform.localPosition = new Vector3(0f, 5f, -10f);
			Camera.main.transform.localEulerAngles = new Vector3(20f, 0f, 0f);
			healthBar2 .gameObject .SetActive (false );
		}
	}
	
	void KeyCam()
	{
		if (Input .GetKeyDown (KeyCode.C))
		{
			if(normalA == true )
			{
				Camera .main.transform .SetParent (turretp.transform  );
				Camera .main.transform .localPosition = new Vector3 (-2.5f,1f,9f);
				Camera.main.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				normalA = false ;
				gunnerA = true;
				healthBar2 .gameObject .SetActive (true);
			}
			else if(gunnerA == true )
			{
				Camera .main.transform .SetParent (transform );
				Camera .main.transform .localPosition = new Vector3 (0f,0.1f,2.2f);
				Camera.main.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				driverA = true;
				gunnerA = false;
				healthBar2 .gameObject .SetActive (false );
			}
			else if (driverA == true)
			{
				Camera .main.transform .SetParent (turretp.transform );
				Camera .main.transform .localPosition = new Vector3 (2.8f, 6.7f, -3.4f);
				Camera.main.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				commandA = true;
				driverA = false;
				healthBar2 .gameObject .SetActive (false );
			}
			else /*if(commandA == true)*/
			{
				Camera .main.transform .SetParent (transform );
				Camera.main.transform.localPosition = new Vector3(0f, 5f, -10f);
				Camera.main.transform.localEulerAngles = new Vector3(20f, 0f, 0f);
				normalA = true;
				commandA = false;
				healthBar2 .gameObject .SetActive (false );
			}
		}
	}
	
	void OnCollisionEnter(Collision co)
	{
		
		GameObject some = co.gameObject ;
		Projectile pro = some .GetComponent<Projectile >();
		if(pro)
			auSource .PlayOneShot(hitSound ,5);
		
	}
}
