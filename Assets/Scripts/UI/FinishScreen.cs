using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishScreen : MonoBehaviour {
	[SerializeField] private Text FinishTime;
	[SerializeField] private Text Penalties;
	[SerializeField] private Text TotalTime;
	[SerializeField] private GameObject Display;

	GameManager Manager;

	void Start() {
		Display.SetActive(false);
		Manager = FindObjectOfType<GameManager>();
		Manager.GameStateUpdated += this.OnGameStateUpdated;
	}

	private void OnGameStateUpdated(GameState State) {
		if (State == GameState.Returning) {
			UpdateDisplay();
			Display.SetActive(true);
		}
	}

	public void OnReturnButtonPressed() {
		FindObjectOfType<GameScreenController>().BackToMainMenu();
	}

	private void UpdateDisplay() {
		TimeSpan finishTime = TimeSpan.FromSeconds(Manager.EndTime - Manager.StartTime);
		TimeSpan penalties = TimeSpan.FromSeconds(Manager.Penalties * 5.0f);
		TimeSpan totalTime = finishTime+ penalties;

		string format = "mm\\:ss\\.fff";
		FinishTime.text = finishTime.ToString(format);
		Penalties.text = penalties.ToString(format);
		TotalTime.text = totalTime.ToString(format);
	}
}
