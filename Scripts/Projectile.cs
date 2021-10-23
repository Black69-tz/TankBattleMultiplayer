using UnityEngine;
using Mirror;
    public class Projectile : NetworkBehaviour
    {
	    public float destroyAfter = 5;
        public Rigidbody rigidBody;
	    public float force = 15000;
        
	    public GameObject Exp;
	    GameObject Explo;
	    public float damage = 20f ;
	    
	    [HideInInspector ]
	    [SyncVar ]
	    public GameObject owner;
	  
        void Start()
        {
	        rigidBody.AddForce(transform.forward * force,ForceMode .VelocityChange);
        }

        // destroy for everyone on the server
	    
	    void OnCollisionEnter(Collision co)
        {
	        
	        Explo = Instantiate(Exp, transform .position, transform.rotation);
	        NetworkServer.Spawn(Explo);
	        
	        GameObject obj = co.gameObject ;
	        CameraCtrl player = obj.GetComponent<CameraCtrl >();
	        LifeHealth  playerH = obj.GetComponent<LifeHealth >();
	        
	        if (player != null )
	        {
	        	if(player .gameObject == owner || player .gameObject ==null )
		        	return ;
	        	player.killedBy = owner ;
	        }
	        
	        if(playerH && playerH .curhealth > 0)
	        {
	        	if(!isServer) return;
	        	playerH .getHit (this );
	        }
	        
	        /*if(co.gameObject.tag == "Player")
	        {
	        	//NetworkServer .Destroy (co.gameObject );
	        	co.gameObject.transform .GetComponent <LifeHealth >().getHit (damage );
	        }*/
	        NetworkServer.Destroy(gameObject );
        }
    }
