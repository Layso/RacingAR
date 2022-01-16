using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
	[SerializeField] private string GameSceneName;
	public void OnPlayClicked() {
		#if PLATFORM_ANDROID
		if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
			PermissionCallbacks callbacks = new PermissionCallbacks();
			callbacks.PermissionDenied += this.OnCameraPermissionDenied;
			callbacks.PermissionGranted += this.OnCameraPermissionGranted;
			callbacks.PermissionDeniedAndDontAskAgain += this.OnCameraPermissionDenied;

			Permission.RequestUserPermission(Permission.Camera, callbacks);
		} else {
			OnCameraPermissionGranted(null);
		}
		#else
		OnCameraPermissionGranted(null);
		#endif
	}

	private void OnCameraPermissionDenied(string obj) {
		OnPlayClicked();
	}

	private void OnCameraPermissionGranted(string obj) {
		if (!string.Empty.Equals(GameSceneName)) {
			SceneManager.LoadScene(GameSceneName);
		}
	}

	public void OpenDebugMenu() {
		SceneManager.LoadScene("Test");
	}
}
