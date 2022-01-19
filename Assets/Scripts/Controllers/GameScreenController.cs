using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScreenController : MonoBehaviour {
	public event Action<float> SliderValueChanged;
	[SerializeField] private Button MenuButton;
	[SerializeField] private GameObject MenuContent;


	void Start() {
		GameManager manager = FindObjectOfType<GameManager>();
		if (manager != null) {
			manager.GameStateUpdated += this.OnGameStateUpdated;
		}
	}

	private void OnGameStateUpdated(GameState State) {
		if (State == GameState.Returning) {
			MenuButton.gameObject.SetActive(false);
			MenuContent.gameObject.SetActive(false);
		}
	}

	public void OnMenuButtonPressed() {
		MenuContent.SetActive(!MenuContent.activeSelf);
	}


	public void BackToMainMenu() {
		FindObjectOfType<ExperienceManager>().StopExperience();
		SceneManager.LoadScene("MainMenu");
	}

	public void SaveTrack() {
		SaveLoadManager.Instance.SaveLatestTrack(FindObjectOfType<GameManager>().CurrentRoadNodes);
	}

	public void LoadTrack() {
		if (SaveLoadManager.Instance.LoadLatestTrack(out TrackSaveRecord record)) {
			Vector3 first = record.RoadNodes[0];
			List<Vector3> positions = new List<Vector3>();
			foreach (Vector3 position in record.RoadNodes) {
				positions.Add(position - first);
			}

			FindObjectOfType<GameManager>().TryLoadTrack(positions);
		} else {
			print ("Nothing to load");
		}
	}

	public void OnSliderValueChanged(float Value) {
		SliderValueChanged.Invoke(Value);
	}
}
