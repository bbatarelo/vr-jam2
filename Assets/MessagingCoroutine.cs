using UnityEngine;
using System.Collections;

public class MessagingCoroutine : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("tickCoroutine");
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	IEnumerator tickCoroutine()
	{
		while( true )
		{
			
			// do stuff here
			Debug.Log( "some string in the console " + Time.time);

//			GameObject.FindObjectOfType<ServerMessage>().triggerRcvd = true;
			
			// done checking, now wait 0.1 seconds
			yield return new WaitForSeconds(0.1f);
			
		}
	}
}
