using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using System;
using UnityEngine;

public enum GameState {
	Initializing,
	RoadBuilding,
	ObstaclePlacing,
	WaitingToStart,
	Racing,
	Returning
}

public class GameManager : MonoBehaviour {
	public event Action<GameState> GameStateUpdated;

	private GameState State;

	public GameState GetState() {
		return State;
	}

	private void Awake() {
		SetGameState(GameState.Initializing);
		ARSessionFactory.SessionInitialized += this.OnExperienceInitialized;
	}

	private void OnExperienceInitialized(AnyARSessionInitializedArgs args) {
		SetGameState(GameState.RoadBuilding);
	}

	void Update() {

	}

	private void SetGameState(GameState NewState) {
		print("Before: " + State + " / After: " + NewState);
		State = NewState;
		if (GameStateUpdated != null) {
			GameStateUpdated.Invoke(State);
		}
	}

	public void UpdateB() {

		SetGameState(State+1);
	}

	public void OnRoadBuildingCompleted() {
		SetGameState(GameState.ObstaclePlacing);
	}
	public void OnObstaclePlacementCompleted() {
		SetGameState(GameState.WaitingToStart);
	}
}
