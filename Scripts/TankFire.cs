using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror ;


public class TankFire :NetworkBehaviour 
{
	public Image loadingTex;
	
	public float loadTime = 4;
	float fireCool=0;
	float curFireCool;
	
	
	public KeyCode shootKey = KeyCode.Space ;
	public Transform  projectileMount;
	public GameObject projectilePrefab;
	
	private Rigidbody tankRigid;
	
	
	void Awake()
	{
		tankRigid = GetComponent<Rigidbody >();
		
	}
	
	void FixedUpdate()
	{
		if(!isLocalPlayer )
		return;
		
			if (curFireCool >= loadTime  && Input .GetKeyDown(shootKey ))
			{
				CmdFire ();
				curFireCool = fireCool ;
			}
			else
			{
				curFireCool += 1 * Time .deltaTime ;
			}
		
			loadingTex.fillAmount = curFireCool /loadTime;
	
	}
	
	[Command]
	void CmdFire()
	{
		GameObject projectile = Instantiate(projectilePrefab, projectileMount .position, projectileMount .rotation);
		NetworkServer.Spawn(projectile);
	}
}