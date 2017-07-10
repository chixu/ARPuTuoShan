/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class CustomTrackableEventHandler : MonoBehaviour,
	ITrackableEventHandler
	{

		public string videoPath;
		#region PRIVATE_MEMBER_VARIABLES

		private TrackableBehaviour mTrackableBehaviour;
		//private bool isForcedTrackingLost = false;
		//private ImageTargetBehaviour ITB;
		private TouchRotate touchRotate;
		public MediaPlayer mediaPlayer;

		#endregion // PRIVATE_MEMBER_VARIABLES

		//public GameObject myCanvas;
		//public GameObject xueguan;
		//public string name;


		#region UNTIY_MONOBEHAVIOUR_METHODS


		public virtual void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}

//			if (myCanvas) {
//				myCanvas.SetActive (false);
//			}

			touchRotate = this.GetComponentInChildren<TouchRotate> ();
//			mediaPlayer = this.GetComponentInChildren<MediaPlayer> ();
//			if (mediaPlayer) {
//				mediaPlayer.m_VideoPath = mediaPlayer.m_VideoPath.Replace ("{%persistentPath%}", Application.persistentDataPath);
//				Debug.Log (mediaPlayer.m_VideoPath);
//				mediaPlayer.PostStart ();
//			}
		}

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public void OnTrackableStateChanged(
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
				newStatus == TrackableBehaviour.Status.TRACKED ||
				newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				OnTrackingFound();
			}
			else
			{
				OnTrackingLost();
			}
		}

		#endregion // PUBLIC_METHODS


//		public void OnForcedLostTracking(){
//
//			isForcedTrackingLost = true;
//			OnTrackingLost ();
//
//		}


//		#endregion // PUBLIC_METHODS



//		#region PRIVATE_METHODS


		virtual protected void OnTrackingFound()
		{
//			if (touchRotate) {
//				touchRotate.enabled = true;
//			}
			Canvas[] canvas = GetComponentsInChildren<Canvas> (true);
			foreach (Canvas ca in canvas) {
				ca.gameObject.SetActive (true);
			}
			if(!string.IsNullOrEmpty(videoPath) && mediaPlayer) {
				mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.AbsolutePathOrURL, videoPath, true);
				mediaPlayer.Play ();
			}
		}


		virtual protected void OnTrackingLost()
		{
//			if (touchRotate) {
//				touchRotate.enabled = false;
//			}
			Canvas[] canvas = GetComponentsInChildren<Canvas> (true);
			foreach (Canvas ca in canvas) {
				ca.gameObject.SetActive (false);
			}
			if(!string.IsNullOrEmpty(videoPath) && mediaPlayer)
				mediaPlayer.Stop();
		}

//		#endregion // PRIVATE_METHODS
	}
}
