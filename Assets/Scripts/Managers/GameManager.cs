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
	[SerializeField] private GameObject FinishLinePrefab;

	public event Action<GameState> GameStateUpdated;
	public Transform StartTransform { get; set; }
	public Vector3 EndPosition { get; set; }

	private GameState State;

	public float StartTime;
	public float EndTime;
	public int Penalties;

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
		//print("Before: " + State + " / After: " + NewState);
		State = NewState;
		if (GameStateUpdated != null) {
			GameStateUpdated.Invoke(State);
		}
	}
	public void OnRoadBuildingCompleted(Transform StartTransform, Vector3 EndPosition) {
		this.EndPosition = EndPosition;
		this.StartTransform = StartTransform;
		Instantiate(FinishLinePrefab, EndPosition, Quaternion.identity);
		SetGameState(GameState.ObstaclePlacing);
	}
	public void OnObstaclePlacementCompleted() {
		SetGameState(GameState.WaitingToStart);
	}

	public void OnCountdownEnded() {
		StartTime = Time.time;
		SetGameState(GameState.Racing);
		FindObjectOfType<ExperienceManager>().OnRaceStarted();
	}

	public void OnRaceEnded() {
		EndTime = Time.time;
		SetGameState(GameState.Returning);
	}

	public void ResetCar() {
		GameObject go = FindObjectOfType<VehicleController>().gameObject;
		go.transform.rotation = StartTransform.rotation;
		go.transform.position = StartTransform.position;
	}
}
