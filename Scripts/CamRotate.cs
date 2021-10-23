using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
	
	public float rotateSpeed =3f;
	float inputAngle;
    
    
	void Awake()
	{
		//Screen.SetResolution (900,500,false);
		Screen.SetResolution (1920,1080,true);
	}
    
    void Update()
	{
		if (CameraCtrl.commandA == true )
		{
			inputAngle = Input .GetAxis ("Mouse X") * rotateSpeed;
	    }
		else{
			inputAngle = 0;
		}
		transform .Rotate(0,inputAngle ,0);
	    
    }
}
