using PathCreation;
using PathCreation.Examples;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PathCreator))]
[RequireComponent(typeof(RoadMeshCreator))]
public class RoadBuilder : MonoBehaviour {
	[SerializeField] private GameObject RoadNodePrefab;
	[SerializeField] private GameObject RoadBuilderButtonsPrefab;

	private PathCreator PathCreator;
	private List<Transform> RoadNodes;
	private ExperienceManager ExperienceManager;

	void Start() {
		RoadNodes = new List<Transform>();
		PathCreator = GetComponent<PathCreator>();
		ExperienceManager = FindObjectOfType<ExperienceManager>();

		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null && RoadBuilderButtonsPrefab != null) {
			RectTransform buttonsGameObject = Instantiate(RoadBuilderButtonsPrefab, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
			buttonsGameObject.SetParent(canvas.transform);
			buttonsGameObject.anchoredPosition = Vector2.zero;

			RoadBuilderButtons buttons = buttonsGameObject.GetComponent<RoadBuilderButtons>();
			if (buttons != null) {
				buttons.AddButton.onClick.AddListener(OnAddNodeButtonClicked);
				buttons.RemoveButton.onClick.AddListener(OnRemoveNodeButtonClicked);
				buttons.GenerateButton.onClick.AddListener(OnGenerateNodeButtonClicked);
			}
		}
	}

	private void OnAddNodeButtonClicked() {
		if (ExperienceManager.GetWorldPosition(new Vector2(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2), out Vector3 WorldPoint)) {
			RoadNodes.Add(Instantiate(RoadNodePrefab, WorldPoint, Quaternion.identity).transform);
		}

		/*
		var list = new List<Vector3>() {
			new Vector3(0,0,0),
			new Vector3(90,0,0),
			new Vector3(90,90,0),
			new Vector3(180,180,0)
		};

		RoadNodes.Add(Instantiate(RoadNodePrefab, list[RoadNodes.Count], Quaternion.identity).transform);
		*/
	}

	private void OnRemoveNodeButtonClicked() {
		if (RoadNodes.Count > 0) {
			Transform node = RoadNodes[RoadNodes.Count-1];
			RoadNodes.Remove(node);
			Destroy(node.gameObject);
		}
	}

	private void OnGenerateNodeButtonClicked() {
		if (PathCreator != null) {
			PathCreator.bezierPath = new BezierPath(RoadNodes, false, PathSpace.xyz);
			RoadNodes.ForEach((node) => Destroy(node.gameObject));
			RoadNodes.Clear();
		}
	}
}
