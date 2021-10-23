using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class UIMain1 : MonoBehaviour
{
	//public GameObject backHost;
	public GameObject backClient;
	float healthVal;
	
	void Start()
	{
		//backHost .GetComponent <Button >().enabled = false ;
		backClient .GetComponent <Button >().enabled = false ;
	}
	
	void Update()
	{
		healthVal = transform .GetComponent <LifeHealth >().getCurHealth();
		
		if (healthVal <= 0)
		{
			//backHost .GetComponent<Button >().enabled = true ; 
			backClient .GetComponent<Button >().enabled = true ;
		}
	}
}
