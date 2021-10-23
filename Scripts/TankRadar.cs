using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class TankRadar : NetworkBehaviour 
{
	[HideInInspector ]
	public GameObject  CurrentCam;
	
	public GameObject [] target;
	//public float distLock = 100;
	public Texture2D TargetTexture;
	public Texture2D PlayerTexture;
	
	GameObject  targets;
	GameObject Player;
	
	void Start()
	{
		//CurrentCam = transform .GetComponent <Camera>();
		
	}
	void Update()
	{
		if(!isLocalPlayer )
			return ;
		CurrentCam = GameObject .FindGameObjectWithTag ("radarCam");
		
		if (!Player )
		{
			Player = this.gameObject ;
		}
		
		target = GameObject .FindGameObjectsWithTag ("Player");
		
	}
	
	
	private void DrawTargetLockOn(Transform aimtarget,bool locked)
	{
		Vector3 screenPos = CurrentCam.GetComponent<Camera >().WorldToScreenPoint (aimtarget.transform.position);
		if (locked )
		{
			GUI.DrawTexture (new Rect (screenPos.x - TargetTexture.width/300 , Screen.height - screenPos.y - TargetTexture.height/200 , TargetTexture.width/150, TargetTexture.height/150), PlayerTexture );
			//GUI.Label (new Rect (screenPos.x-19 , Screen.height - screenPos.y-35, 200,30), aimtarget.GetComponent <CameraCtrl >().pName + " ");
		}else{
			GUI.DrawTexture (new Rect (screenPos.x  - TargetTexture.width/300, Screen.height - screenPos.y - TargetTexture.height/200  , TargetTexture.width/150, TargetTexture.height/150), TargetTexture);
			//GUI.Label (new Rect (screenPos.x-19 , Screen.height - screenPos.y-35, 200,30), aimtarget.GetComponent <CameraCtrl >().pName + " ");
		}
				//GUI.DrawTexture (new Rect (screenPos.x - TargetTexture.width/4.8f , Screen.height - screenPos.y - TargetTexture.height/4.7f , TargetTexture.width/5, TargetTexture.height/5), TargetTexture);
		
	} 
	
	
	private void OnGUI()
	{
		for (int i = 0; i<target .Length ;i++)
		{
			targets = target[i].transform.gameObject  ;
	
			if (targets == Player )
			{
				DrawTargetLockOn (targets.transform ,true );
			}
			else{
				DrawTargetLockOn (targets.transform ,false );
			}
		}
	}
}
