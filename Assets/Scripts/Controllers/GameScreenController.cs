using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreenController : MonoBehaviour {
	[SerializeField] private GameObject TestObject;
	private ExperienceManager ExperienceManager;

	void Start() {
		ExperienceManager = FindObjectOfType<ExperienceManager>();	
		GameManager manager = FindObjectOfType<GameManager>();
		if (manager != null) {
			manager.GameStateUpdated += this.OnGameStateUpdated;
		}
	}

	private void OnGameStateUpdated(GameState State) {
		switch (State) {
			case GameState.Initializing:
				break;
			case GameState.RoadBuilding:
				break;
			case GameState.ObstaclePlacing:
				break;
			case GameState.Racing:
				break;
			case GameState.Returning:
				break;
			default:
				break;
		}
	}

	public void OnPlaceButtonClicked() {
		if (ExperienceManager != null) {
			if (ExperienceManager.GetWorldPosition(new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2), out Vector3 WorldPoint)) {
				Instantiate(TestObject, WorldPoint, Quaternion.identity);
				print("yiha");
			}
		}
	}
}
