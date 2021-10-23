using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HealthSpawner : NetworkBehaviour 
{
	public GameObject repairkit;
	
	public float timer = 0;
   
	public override void OnStartServer()
	{
		for (int i = 0; i < 2; i++)
			SpawnHealth();
	}
	
	void Update()
	{
		timer += 1* Time .deltaTime ;
		if (timer > 60)
		{
			SpawnHealth ();
			timer = 0;
		}
	}
   
	public void SpawnHealth()
	{
		Vector3 spawnPosition = new Vector3(transform .position .x +Random.Range(-399, 400), 7, transform .position .z +Random.Range(-399, 400));
    	
		GameObject repair = Instantiate (repairkit ,spawnPosition ,transform .rotation );
		NetworkServer .Spawn (repair );
    }
}
