using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessagingCoroutine : MonoBehaviour {

//	private Dictionary<String, String> eventsMap;// = new Dictionary<string, string> ();
	
	// Use this for initialization
	void Start () {
//		if (eventsMap == null) {
//			eventsMap = new Dictionary<string, string> ();
//			initMap ();
//		}
//		StartCoroutine("tickCoroutine");
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	IEnumerator tickCoroutine()
	{
		Dictionary<String, String> eventsMap = initMap();

		while( true )
		{
//			sendMessage("event1");
			// do stuff here
//			Debug.Log( "some string in the console " + Time.time);

//			GameObject.FindObjectOfType<ServerMessage>().doSomething();

			System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create ("http://vr-jam.herokuapp.com/listen");
			var response = request.GetResponse();
			var sr = new System.IO.StreamReader (response.GetResponseStream());
			
			var message = sr.ReadToEnd();

			Debug.Log( "GOT MESSAGE: " + message);
			
			if (!String.IsNullOrEmpty (message))
			{
				var elements = message.Split('|');
				if (elements.Length == 3)
				{
					var states = elements[0] + "|" + elements[1];
					
					string value;
					if (eventsMap.TryGetValue(states, out value))
					{

						sendMessage(value);
//						PlayMakerFSM.BroadcastEvent("continue");
						return true;
					}
				}
			}
						
			// done checking, now wait 0.1 seconds
			yield return new WaitForSeconds(1f);
			
		}
	}

	void sendMessage(string message)
	{
		UnityEngine.Object[] objects = GameObject.FindObjectsOfType (typeof(PlayMakerFSM));
		foreach (var o in objects)
		{
			((PlayMakerFSM)o).SendEvent(message);
		}
	}

	private Dictionary<String, String> initMap()
	{
		Dictionary<String, String> eventsMap = new Dictionary<string, string> ();;
		eventsMap.Add("001initialNarration|001interruptPrompt", "event1");
		eventsMap.Add("003noInterruptResponse|004HolmesReadsWatson", "event2");
		eventsMap.Add("003yesInterruptResponse|004HolmesReadsWatson", "event2");
		eventsMap.Add("004HolmesReadsWatson|005WatsonSitsByFire", "event3");
		eventsMap.Add("007noInterruptResponse|008PetersonsHat", "event4");
		eventsMap.Add("007yesInterruptResponse|008PetersonsHat", "event4");
		eventsMap.Add("008PetersonsHat|009PetersonArrives", "event5");
		eventsMap.Add("009PetersonArrives|010interruptPrompt", "event6");
		eventsMap.Add("011dontGrabInterruptResponse|012WhyTheGoose", "event7");
		eventsMap.Add("012WhyTheGoose|013backstory", "event8");
		eventsMap.Add("013backstory|014interruptPrompt", "event9");
		eventsMap.Add("014interruptPrompt|015noInterruptResponse", "event10a");
		eventsMap.Add("014interruptPrompt|015yesInterruptResponse", "event10b");
		eventsMap.Add("016GooseIsBlack|017PetersonTellsHisStory", "event11");
		eventsMap.Add("017PetersonTellsHisStory|end_opening", "event12");
		eventsMap.Add("end_opening|start_hatScene1", "event13");

		return eventsMap;
	}
}
