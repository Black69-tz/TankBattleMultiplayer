using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
	
	private AudioSource audSource;
	
    // Start is called before the first frame update
    void Start()
    {
	    audSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
	    audSource .pitch = 1f;
	    audSource .volume = 0.15f ;
	    float motorInput = Input.GetAxisRaw("Vertical");

	    float steerInput = Input.GetAxisRaw("Horizontal");
			
	    if(motorInput != 0 || steerInput != 0 )
	    {
		    audSource .pitch = 1.5f ;
		    audSource .volume = 0.5f ;
	    }
    }
}
