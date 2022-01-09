using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using System;
using UnityEngine;

public enum GameState {
	Initializing,
	RoadBuilding,
	ObstaclePlacing,
	Racing,
	Returning
}

public class GameManager : MonoBehaviour {
	//public delegate GameState GameStateUpdated();
	public event Action<GameState> GameStateUpdated;

	private GameState State;
	//private GameScreenController Controller;

	private void Awake() {
		SetGameState(GameState.Initializing);
		//Controller = FindObjectOfType<GameScreenController>();
		ARSessionFactory.SessionInitialized += this.OnExperienceInitialized;
	}

	private void OnExperienceInitialized(AnyARSessionInitializedArgs args) {
		SetGameState(GameState.ObstaclePlacing);
	}

	void Update() {

	}

	private void SetGameState(GameState NewState) {
		State = NewState;
		if (GameStateUpdated != null) {
			GameStateUpdated.Invoke(State);
		}
	}
}
