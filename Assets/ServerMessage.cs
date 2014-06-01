using UnityEngine;
using System;
using System.Collections;
using System.Timers;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	public class ServerMessage : FsmStateAction
	{
		private bool gotMessage = false;
		private Dictionary<String, String> eventsMap = new Dictionary<string, string> ();
		Timer timer = new Timer (1000);

		public FsmString PendingEvent;
		
		public override void Reset()
		{
			eventsMap = new Dictionary<string, string> ();
			gotMessage = false;
			timer.Stop();
			timer = new Timer (1000);
		}
		
		public override void OnEnter()
		{
			initMap ();
			timer.Elapsed += onTick;
			timer.Start();
			
//			while (!gotMessage) {
//			}
		}

		private void onTick(System.Object source, ElapsedEventArgs e)
		{
			((Timer)source).Enabled = false;
			System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create ("http://vr-jam.herokuapp.com/listen");
			var response = request.GetResponse();
			var sr = new System.IO.StreamReader (response.GetResponseStream());
			
			var message = sr.ReadToEnd();
			
			if (!String.IsNullOrEmpty (message))
			{
				var elements = message.Split('|');
				if (elements.Length == 3)
				{
					var states = elements[0] + "|" + elements[1];
//					Console.Out.WriteLine ("States: " + states);
					
					string value;
					if (eventsMap.TryGetValue(states, out value))
					{
						((Timer)source).Stop();
						gotMessage = true;
						PendingEvent = value;
//						Console.Out.WriteLine ("Got message for event: " + value);
						Finish();
						return;
					}
				}
			}
//			Console.Out.WriteLine ("No message");
			((Timer)source).Enabled = true;
		}

		private void initMap()
		{
			
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
		}
	}
}