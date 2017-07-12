using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RenderHeads.Media.AVProVideo;

//-----------------------------------------------------------------------------
// Copyright 2015-2016 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

public class VideoController : MonoBehaviour
{
	public MediaPlayer	_mediaPlayer;
	public Slider _videoSeekSlider;
	private bool _wasPlayingOnScrub;
	private float _setVideoSeekSliderValue;
	public static VideoController instant;

	//		public void OnMuteChange()
	//		{
	//			if (_mediaPlayer)
	//			{
	//				_mediaPlayer.Control.MuteAudio(_MuteToggle.isOn);
	//			}
	//		}

	public void OnPlayButton ()
	{
		if (_mediaPlayer) {
			_mediaPlayer.Control.Play ();
			//				SetButtonEnabled( "PlayButton", false );
			//				SetButtonEnabled( "PauseButton", true );
		}
	}

	public void OnPauseButton ()
	{
		if (_mediaPlayer) {
			_mediaPlayer.Control.Pause ();
			//				SetButtonEnabled( "PauseButton", false );
			//				SetButtonEnabled( "PlayButton", true );
		}
	}

	public void OnVideoSeekSlider ()
	{
		if (_mediaPlayer && _videoSeekSlider && _videoSeekSlider.value != _setVideoSeekSliderValue) {
			_mediaPlayer.Control.Seek (_videoSeekSlider.value * _mediaPlayer.Info.GetDurationMs ());
		}
	}

	public void OnVideoSliderDown ()
	{
		if (_mediaPlayer) {
			_wasPlayingOnScrub = _mediaPlayer.Control.IsPlaying ();
			if (_wasPlayingOnScrub) {
				_mediaPlayer.Control.Pause ();
				//					SetButtonEnabled( "PauseButton", false );
				//					SetButtonEnabled( "PlayButton", true );
			}
			OnVideoSeekSlider ();
		}
	}

	public void OnVideoSliderUp ()
	{
		if (_mediaPlayer && _wasPlayingOnScrub) {
			_mediaPlayer.Control.Play ();
			_wasPlayingOnScrub = false;

			//				SetButtonEnabled( "PlayButton", false );
			//				SetButtonEnabled( "PauseButton", true );
		}			
	}

	public void OnRewindButton ()
	{
		if (_mediaPlayer) {
			_mediaPlayer.Control.Rewind ();
		}
	}

	void Awake ()
	{
		VideoController.instant = this;
		if (_mediaPlayer) {
			_mediaPlayer.Events.AddListener (OnVideoEvent);
		}
	}

	void Update ()
	{
		if (_mediaPlayer && _mediaPlayer.Info != null && _mediaPlayer.Info.GetDurationMs () > 0f) {
			float time = _mediaPlayer.Control.GetCurrentTimeMs ();
			float d = time / _mediaPlayer.Info.GetDurationMs ();
			_setVideoSeekSliderValue = d;
			_videoSeekSlider.value = d;
		}
	}

	// Callback function to handle events
	public void OnVideoEvent (MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		switch (et) {
		case MediaPlayerEvent.EventType.ReadyToPlay:
			break;
		case MediaPlayerEvent.EventType.Started:
			break;
		case MediaPlayerEvent.EventType.FirstFrameReady:
			break;
		case MediaPlayerEvent.EventType.FinishedPlaying:
			break;
		}

		Debug.Log ("Event: " + et.ToString ());
	}

}
