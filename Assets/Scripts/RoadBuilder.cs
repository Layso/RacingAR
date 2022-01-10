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

	void Start() {
		RoadNodes = new List<Transform>();
		ExperienceManager = FindObjectOfType<ExperienceManager>();

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

	private void OnAddNodeButtonClicked() {
		bool test = false;

		if (!test) {
			if (ExperienceManager.GetWorldPosition(new Vector2(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2), out Vector3 WorldPoint)) {
				RoadNodes.Add(Instantiate(RoadNodePrefab, WorldPoint, Quaternion.identity).transform);
			}
		} else {
			/*		
			var list = new List<Vector3>() {
				new Vector3(0,0,0),
				new Vector3(90,0,0),
				new Vector3(90,90,0),
				new Vector3(180,180,0)
			};
			*/

			var list = new List<Vector3>() {
				new Vector3(-0.2f, -0.9f, 0.9f),
				new Vector3(1.2f, -1.3f, 2),
				new Vector3(0.9f, -1.3f, 0.7f),
				new Vector3(1.6f, -1.4f, 0)
			};

			RoadNodes.Add(Instantiate(RoadNodePrefab, list[RoadNodes.Count], Quaternion.identity).transform);
		}
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
			PathCreator.bezierPath = new BezierPath(RoadNodes, space:PathSpace.xyz);
			FindObjectOfType<RoadMeshCreator>().TriggerUpdate();
			RoadNodes.ForEach((node) => Destroy(node.gameObject));
			RoadNodes.Clear();
			Destroy(Buttons.gameObject);
			FindObjectOfType<GameManager>().OnRoadBuildingCompleted();
		}
	}
}
