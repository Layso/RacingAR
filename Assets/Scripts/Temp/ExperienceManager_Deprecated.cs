using Niantic.ARDK.AR;
using Niantic.ARDK.AR.Configuration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class ExperienceManager_Deprecated : MonoBehaviour {
	IARSession Session;

	void Start() {
#if PLATFORM_ANDROID
		if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
			PermissionCallbacks callbacks = new PermissionCallbacks();
			callbacks.PermissionDenied += OnPermissionDenied;
			callbacks.PermissionGranted += OnPermissionGranted;
			callbacks.PermissionDeniedAndDontAskAgain += OnPermissionDenied;
			Permission.RequestUserPermission(Permission.Camera, callbacks);
		} else {
			OnPermissionGranted(string.Empty);
		}
#endif
	}

	private void OnPermissionGranted(string obj) {
		IARWorldTrackingConfiguration config = ARWorldTrackingConfigurationFactory.Create();
		config.WorldAlignment = WorldAlignment.Gravity;
		config.IsLightEstimationEnabled = true;
		config.PlaneDetection = PlaneDetection.Vertical;
		config.IsAutoFocusEnabled = true;
		config.IsDepthEnabled = false;
		config.IsSharedExperienceEnabled = true;

		Session = ARSessionFactory.Create();
		Session.FrameUpdated += OnFrameUpdated;
		Session.Run(config);
	}

	private void OnFrameUpdated(Niantic.ARDK.AR.ARSessionEventArgs.FrameUpdatedArgs args) {
		//throw new System.NotImplementedException();
	}

	private void OnPermissionDenied(string obj) {
		Application.Quit();
	}
}
