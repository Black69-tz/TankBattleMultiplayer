using UnityEngine;
using Mirror;

	public class SelfDestroy : NetworkBehaviour
	{
		public float destroyAfter = 4;
        
		public override void OnStartServer()
		{
			Invoke(nameof (DestroySelf1), destroyAfter);
		}
	
		[Server]
		void DestroySelf1()
		{
			NetworkServer.Destroy(gameObject);
		}

	}

