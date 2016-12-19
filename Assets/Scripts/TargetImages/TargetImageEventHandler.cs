using UnityEngine;
using System.Collections;
using Vuforia;

namespace AssemblyCSharp
{
	public class TargetImageEventHandler : MonoBehaviour, ITrackableEventHandler
	{
		private TrackableBehaviour mTrackableBehaviour;

		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
		}

		public void OnTrackableStateChanged(
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
				newStatus == TrackableBehaviour.Status.TRACKED ||
				newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				// Play audio when target is found
				//audio.Play();
				GameEvent.ImageTargetInitialized(this.gameObject);
			}
			else
			{
				// Stop audio when target is lost
				//audio.Stop();
				GameEvent.ImageTargetInitialized(null);
			}
		}   
	}
}