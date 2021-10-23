using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
using Mirror ;

public class LifeHealth : NetworkBehaviour 
{
	public Transform  playerBody;
	[SerializeField ] float health  = 100;
	
	[HideInInspector]
	[SyncVar]
	public float curhealth ;
	
	CustomNetworkManager mymanager;
	
	float recoverPoint = 20f ;
	
	public Transform turrett;
	//	public GameObject expFlame;
	
	public CameraCtrl othPly;
	
	//public GameObject resPaw;
	
	public int respawnTime = 10 ;
	
	public GameObject infoPannel;
	public Image respawnTex; 
	public Text deathInfoText;
	public Text respawnDelay;
	//public GameObject disconnetBtn;
	
	void Start()
	{
		curhealth = health ;
		//expFlame .gameObject .SetActive (false );
		//resPaw .gameObject .SetActive (false );
		playerBody .gameObject .SetActive (true);
		infoPannel .gameObject .SetActive (false );
		mymanager = GetComponent<CustomNetworkManager >();
	}

	public float getCurHealth()
	{
		return curhealth ;
	}
	
	[Server ]
	public void getHit(Projectile proje)
	{
		if(curhealth >= 0)
		{
			curhealth -= proje .damage ;
		}
		
		if(curhealth <= 0)
		{
			CameraCtrl otherPly = proje .owner .GetComponent<CameraCtrl >();
			otherPly .score += 1;
			RpcRespawn();
			curhealth = health ;
		}
	}
	
	public void addHealth(float heal)
	{
		if(curhealth >= 0)
		{
			curhealth += heal ;
		}
	}
	/*void Update()
	{
		if(!isLocalPlayer )
			return ;
	}*/
	
	[ClientRpc]
	void RpcRespawn()
	{
		if(!isLocalPlayer )
			return ;
		
		DisplayDeath ();
	}
	
	[Command ]
	void CmdRespawn()
	{
		RpcRespawn ();
	}
	
	void DisplayDeath()
	{
		infoPannel .gameObject .SetActive (true);
		playerBody .gameObject .SetActive (false );
		transform .GetComponent <TankController >().enabled = false;
		//Camera .main.transform .SetParent (othPly .killedBy .transform );
		//Camera.main.transform.localPosition = new Vector3(0f, 5f, -10f);
		//Camera.main.transform.localEulerAngles = new Vector3(20f, 0f, 0f);
		CameraCtrl ctrlplayer = othPly .killedBy .GetComponent<CameraCtrl >();
		deathInfoText .text = "YOU ARE KILLED BY " + ctrlplayer.playNameLabel .text ;
		//disconnetBtn .gameObject .SetActive (true);
		//disconnetBtn.GetComponent<Button >().onClick = mymanager .StopClient();
		
		StartCoroutine (SpawnRoutine());
	}
	
	IEnumerator SpawnRoutine()
	{
		float targetTime = Time .time + respawnTime;
		
		while (targetTime - Time .time > 0)
		{
			respawnDelay .text = Mathf .Ceil (targetTime - Time .time).ToString ();
			respawnTex .fillAmount = targetTime - Time .time ;
			yield return null ;
		}
		//yield return new WaitForSeconds (5);
		//CmdRespawn();
		Respawn();
	}
	
	public void Respawn()
	{
		playerBody .gameObject .SetActive (true);
		infoPannel .gameObject .SetActive (false);
		//disconnetBtn .gameObject .SetActive (false);
		//Camera .main.transform .SetParent (transform );
		//Camera.main.transform.localPosition = new Vector3(0f, 5f, -10f);
		//Camera.main.transform.localEulerAngles = new Vector3(20f, 0f, 0f);
		transform .rotation = Quaternion .identity ;
		transform .GetComponent <TankController >().enabled = true;
		//resPaw .gameObject .SetActive (false );
		transform .position = NetworkManager .startPositions [Random .Range(0,NetworkManager .startPositions .Count )].position ;
	}
	
	[Command ]
	void CmdOnTankFire()
	{
		RpcCreateFire ();
	}
	
	[ClientRpc ]
	void RpcCreateFire()
	{
		//expFlame .gameObject .SetActive (true);
	}
	
	void OnTriggerEnter(Collider hitobj)
	{
		if(hitobj .gameObject .tag == "Health")
		{
			addHealth (recoverPoint );
			NetworkServer .Destroy (hitobj .gameObject );
		}
	}
}
