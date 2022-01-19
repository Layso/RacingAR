using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {
	Initializing,
	RoadBuilding,
	ObstaclePlacing,
	WaitingToStart,
	Racing,
	Returning
}

public class GameManager : MonoBehaviour {
	[SerializeField] private Slider RotationSlider;
	[SerializeField] private GameObject FinishLinePrefab;

	public event Action<GameState> GameStateUpdated;
	public event Action<List<Vector3>> TrackLoaded;
	public Transform StartTransform { get; set; }
	public Vector3 EndPosition { get; set; }

	private GameState State;
	public List<Transform> CurrentRoadNodes;

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

	private void Start() {
		RotationSlider.gameObject.SetActive(false);
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


	private GameObject GetStartTransform(List<Transform> RoadNodes) {
		GameObject obj = new GameObject();
		Transform first = RoadNodes[0];
		Transform second = RoadNodes[1];
		float offset = 0.05f;

		obj.transform.position = first.position;
		obj.transform.rotation = Quaternion.LookRotation(second.position - first.position, Vector3.up);
		obj.transform.position += (second.position - first.position).normalized * offset;

		return obj;
	}

	public void OnRoadBuildingCompleted(List<Transform> RoadNodes) {
		CurrentRoadNodes = RoadNodes;
		this.EndPosition = CurrentRoadNodes[CurrentRoadNodes.Count - 1].position;
		this.StartTransform = GetStartTransform(CurrentRoadNodes).transform;
		Instantiate(FinishLinePrefab, EndPosition, Quaternion.identity);
		SetGameState(GameState.ObstaclePlacing);
		FindObjectOfType<ExperienceManager>().OnRaceStarted();
		RotationSlider.value = 0;
		RotationSlider.gameObject.SetActive(true);
	}

	public void OnObstaclePlacementCompleted() {
		SetGameState(GameState.WaitingToStart);
		RotationSlider.gameObject.SetActive(false);
	}

	public void OnCountdownEnded() {
		StartTime = Time.time;
		SetGameState(GameState.Racing);
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

	public void TryLoadTrack(List<Vector3> NodePositions) {
		if (State == GameState.RoadBuilding) {
			RotationSlider.value = 0;
			RotationSlider.gameObject.SetActive(true);
			TrackLoaded.Invoke(NodePositions);
		}
	}
}
