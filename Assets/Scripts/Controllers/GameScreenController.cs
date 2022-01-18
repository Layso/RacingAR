using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScreenController : MonoBehaviour {
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
}
