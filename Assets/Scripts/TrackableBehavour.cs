/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.UI;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class TrackableBehavour : MonoBehaviour,
	ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES

		private TrackableBehaviour mTrackableBehaviour;
		private bool isForcedTrackingLost = false;
		private ImageTargetBehaviour ITB;
		private TouchRotate touchRotate;

		#endregion // PRIVATE_MEMBER_VARIABLES

		public GameObject myCanvas;
		public GameObject xueguan;
		public string name;


		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Awake()
		{
			ITB = this.GetComponent<ImageTargetBehaviour> ();
			touchRotate = this.GetComponentInChildren<TouchRotate> ();
		}

		public virtual void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}

			if (myCanvas) {
				myCanvas.SetActive (false);
			}
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
				if (isForcedTrackingLost && newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {//if forced tracking lost, extened tracking will be disabled for one round
					isForcedTrackingLost = false;
					return;
				}
				OnTrackingFound();
			}
			else
			{
				isForcedTrackingLost = false;
				OnTrackingLost();
			}
		}


		public void OnForcedLostTracking(){

			isForcedTrackingLost = true;
			OnTrackingLost ();

		}


		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		virtual protected void OnTrackingFound()
		{
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
			Light[] LightComponents = GetComponentsInChildren<Light>(true);
			Canvas[] canvas = GetComponentsInChildren<Canvas> (true);

			// Enable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = true;
			}

			// Enable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = true;
			}

			// Enable light:
			foreach (Light component in LightComponents)
			{
				component.enabled = true;
			}

			//Enable UI
			foreach (Canvas ca in canvas) {
				ca.gameObject.SetActive (true);
			}

			if (xueguan) {
				xueguan.SetActive (true);
			}

			if (myCanvas) {
				myCanvas.SetActive (true);
			}

			if (touchRotate) {
				touchRotate.enabled = true;
			}

			//GeneralARUIController.inst.OnTrackingFound (this);

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
		}


		virtual protected void OnTrackingLost()
		{
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
			Light[] LightComponents = GetComponentsInChildren<Light>(true);
			Canvas[] canvas = GetComponentsInChildren<Canvas> (true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			// Disable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = false;
			}

			// Disable light:
			foreach (Light component in LightComponents)
			{
				component.enabled = false;
			}

			//Disable UI
			foreach (Canvas ca in canvas) {
				ca.gameObject.SetActive (false);
			}

			if (myCanvas) {
				myCanvas.SetActive (false);
			}

			if (xueguan) {
				xueguan.SetActive (false);
			}

			if (touchRotate) {
				touchRotate.enabled = false;
			}

			//GeneralARUIController.inst.OnTrackingLost (this);

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}

		#endregion // PRIVATE_METHODS
	}
}
