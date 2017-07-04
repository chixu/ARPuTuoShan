/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class VideoTrackableBehavour : TrackableBehavour,
	ITrackableEventHandler
	{	
		VideoPlaybackBehaviour vpb;
//		public string videoName = "";
		override public void Start()
		{
			base.Start ();
			vpb = GetComponentInChildren<VideoPlaybackBehaviour> ();
			vpb.m_path = "file:///" + Path.Combine (Application.persistentDataPath, vpb.m_path);
			if (vpb.VideoPlayer != null)
				vpb.VideoPlayer.SetFilename (vpb.m_path);
		}

		override protected void OnTrackingFound()
		{
			base.OnTrackingFound ();
//			VideoPlaybackBehaviour[] currentVideos = FindObjectsOfType<VideoPlaybackBehaviour>();
//			foreach (VideoPlaybackBehaviour video in currentVideos) {
//				if(video.m_path.Contains (videoName))
//					video.VideoPlayer.Play (false, 0);
//			}
//			Transform transform = GetComponent<Transform>();
//			foreach (Transform child in transform) {
//
//			}
			vpb = GetComponentInChildren<VideoPlaybackBehaviour> ();
			vpb.VideoPlayer.Play (false, 0);
		}

		override protected void OnTrackingLost()
		{
			base.OnTrackingLost ();
			vpb = GetComponentInChildren<VideoPlaybackBehaviour> ();
			vpb.VideoPlayer.Pause ();
		}

	}
}
