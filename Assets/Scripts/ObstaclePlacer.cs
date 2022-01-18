using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstaclePlacer : MonoBehaviour {
	[SerializeField] private GameObject ObstaclePlacerButtonsPrefab;
	[SerializeField] private GameObject ConePrefab;
	[SerializeField] private GameObject RoadblockPrefab;
	[SerializeField] private Text DebugText;

	private ObstaclePlacerButtons Buttons;
	private GameObject SelectedPrefab;

	private List<GameObject> Placements;

	void Start() {
		Placements = new List<GameObject>();
		FindObjectOfType<GameManager>().GameStateUpdated += this.OnGameStateUpdated;
	}

	private void OnGameStateUpdated(GameState State) {
		if (State == GameState.ObstaclePlacing) {
			Canvas canvas = FindObjectOfType<Canvas>();
			if (canvas != null && ObstaclePlacerButtonsPrefab != null) {
				RectTransform buttonsGameObject = Instantiate(ObstaclePlacerButtonsPrefab, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
				buttonsGameObject.SetParent(canvas.transform);
				buttonsGameObject.anchoredPosition = Vector2.zero;

				Buttons = buttonsGameObject.GetComponent<ObstaclePlacerButtons>();
				if (Buttons != null) {
					Buttons.ObstacleChanged += this.OnObstacleChanged;
					Buttons.PlaceButton.onClick.AddListener(OnPlaceButtonClicked);
					Buttons.RemoveButton.onClick.AddListener(OnRemoveButtonClicked);
					Buttons.NextButton.onClick.AddListener(OnNextButtonClicked);
				}
			}
		}
	}

	private void OnObstacleChanged(ObstacleType Type) {
		switch (Type) {
			case ObstacleType.Cone:
				SelectedPrefab = ConePrefab;
				break;
			case ObstacleType.Roadblock:
				SelectedPrefab = RoadblockPrefab;
				break;
		}
	}

	private void OnPlaceButtonClicked() {
		if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0)), out RaycastHit Hit)) {
			Vector3 position = Hit.point;			
			if (SelectedPrefab == ConePrefab) {
				position += new Vector3(0,0.005f,0);
			} else {
				position += new Vector3(0, 0.015f, 0);
			}

			Placements.Add(Instantiate(SelectedPrefab, position, Quaternion.FromToRotation(Vector3.up, Hit.normal)));
		}
	}


	private void OnRemoveButtonClicked() {
		if (Placements.Count > 0) {
			GameObject obstacle = Placements[Placements.Count-1];
			Placements.Remove(obstacle);
			Destroy(obstacle);
		}
	}

	private void OnNextButtonClicked() {
		Destroy(Buttons.gameObject);
		FindObjectOfType<GameManager>().OnObstaclePlacementCompleted();
	}
}