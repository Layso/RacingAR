using PathCreation;
using PathCreation.Examples;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PathCreator))]
[RequireComponent(typeof(RoadMeshCreator))]
public class RoadBuilder : MonoBehaviour {
	[SerializeField] private PathCreator PathCreator;
	[SerializeField] private GameObject RoadNodePrefab;
	[SerializeField] private GameObject RoadBuilderButtonsPrefab;

	private List<Transform> RoadNodes;
	private ExperienceManager ExperienceManager;
	private RoadBuilderButtons Buttons;

	private float RotationAngle;
	private List<Vector3> LoadTrackPositions;

	void Start() {
		RotationAngle = 0;
		LoadTrackPositions = null;
		RoadNodes = new List<Transform>();
		ExperienceManager = FindObjectOfType<ExperienceManager>();

		FindObjectOfType<GameManager>().TrackLoaded += this.OnTrackLoaded;
		FindObjectOfType<GameScreenController>().SliderValueChanged += this.OnSliderValueChanged;
		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null && RoadBuilderButtonsPrefab != null) {
			RectTransform buttonsGameObject = Instantiate(RoadBuilderButtonsPrefab, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
			buttonsGameObject.SetParent(canvas.transform);
			buttonsGameObject.anchoredPosition = Vector2.zero;

			Buttons = buttonsGameObject.GetComponent<RoadBuilderButtons>();
			if (Buttons != null) {
				Buttons.AddButton.onClick.AddListener(OnAddNodeButtonClicked);
				Buttons.RemoveButton.onClick.AddListener(OnRemoveNodeButtonClicked);
				Buttons.GenerateButton.onClick.AddListener(OnGenerateNodeButtonClicked);
			}
		}
	}

	private void OnSliderValueChanged(float Value) {
		RotationAngle = Value;
	}

	private void Update() {
		if (LoadTrackPositions != null) {
			if (ExperienceManager.GetWorldPosition(new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2), out Vector3 WorldPoint)) {
				foreach (Transform transform in RoadNodes) {
					Destroy(transform.gameObject);
				}

				RoadNodes.Clear();
				foreach (Vector3 position in LoadTrackPositions) {
					Transform transform = Instantiate(RoadNodePrefab, position + WorldPoint, Quaternion.identity).transform;
					transform.RotateAround(WorldPoint, Vector3.up, RotationAngle);
					RoadNodes.Add(transform);
				}
			}
		}
	}

	private void OnTrackLoaded(List<Vector3> NodeList) {
		LoadTrackPositions = NodeList;		
	}

	private void OnAddNodeButtonClicked() {
#if UNITY_EDITOR
		int listNumber = 3;
		List<Vector3> list = null;

		switch (listNumber) {
			case 0:
				list = new List<Vector3>() { new Vector3(0, 0, -10), new Vector3(90, 0, 10), new Vector3(180, 0, -10), new Vector3(270, 0, 10)};
				break;

			case 1:
				list = new List<Vector3>() { new Vector3(-0.2f, -0.9f, 0.9f), new Vector3(1.2f, -1.3f, 2), new Vector3(0.9f, -1.3f, 0.7f), new Vector3(1.6f, -1.4f, 0)};
				break;

			case 2:
				list = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(90, 0, 1), new Vector3(180, 0, 0), new Vector3(270, 0, -1) };
				break;

			case 3:
				list = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(1, 0, 1), new Vector3(2, 0, 0), new Vector3(3, 0, -1) };
				break;
		}

		RoadNodes.Add(Instantiate(RoadNodePrefab, list[RoadNodes.Count], Quaternion.identity).transform);
#else
		if (ExperienceManager.GetWorldPosition(new Vector2(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2), out Vector3 WorldPoint)) {
				RoadNodes.Add(Instantiate(RoadNodePrefab, WorldPoint, Quaternion.identity).transform);
			}
#endif
	}

	private void OnRemoveNodeButtonClicked() {
		if (RoadNodes.Count > 0) {
			Transform node = RoadNodes[RoadNodes.Count - 1];
			RoadNodes.Remove(node);
			Destroy(node.gameObject);
		}
	}

	private void OnGenerateNodeButtonClicked() {
		if (PathCreator != null) {
			LoadTrackPositions = null;
			PathCreator.bezierPath = new BezierPath(RoadNodes, space: PathSpace.xyz);
			FindObjectOfType<RoadMeshCreator>().TriggerUpdate();
			GameObject.Find("Road Mesh Holder").AddComponent<MeshCollider>();

			RoadNodes.ForEach((node) => node.gameObject.SetActive(false));

			Destroy(Buttons.gameObject);
			FindObjectOfType<GameManager>().OnRoadBuildingCompleted(RoadNodes);
		}
	}

	private GameObject GetStartTransform() {
		GameObject obj = new GameObject();
		Transform first = RoadNodes[0];
		Transform second = RoadNodes[1];
		float offset = 0.05f;

		obj.transform.position = first.position;
		obj.transform.rotation = Quaternion.LookRotation(second.position - first.position, Vector3.up);
		obj.transform.position += (second.position - first.position).normalized * offset;

		return obj;
	}
}
