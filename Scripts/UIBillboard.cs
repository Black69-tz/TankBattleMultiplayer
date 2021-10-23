using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboard : MonoBehaviour
{
	private Transform camTrans;
	private Transform trans;

        //initialize variables
    void Awake()
    {
    	camTrans = Camera.main.transform;
        trans = transform;
    }

        //always face the camera every frame
    void Update()
    {
    	transform.LookAt(trans.position + camTrans.rotation * Vector3.forward,camTrans.rotation * Vector3.up);
    }
}
