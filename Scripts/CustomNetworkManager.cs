using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[AddComponentMenu ("")]
public class CustomNetworkManager : NetworkManager 
{
	public string PlayerName { get ; set ;}
	
	public void SetAddress(string hostname)
	{
		networkAddress = hostname ;
		Debug .Log ("Hosting and Connect to play at "+ networkAddress);
	}
	
	/*public override void OnServerAddPlayer(NetworkConnection conn)
	{
		base.OnServerAddPlayer(conn);
		CameraCtrl pl = GetComponent<CameraCtrl >();
		pl.playerName = PlayerName ;
	}*/
	
	public void NameConfirm()
	{
		PlayerPrefs .SetString ("playerName",PlayerName );
	}
	
	public void ExitGame()
	{
		Application .Quit ();
	}
}
