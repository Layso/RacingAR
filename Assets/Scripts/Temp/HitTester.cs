// Copyright 2021 Niantic, Inc. All Rights Reserved.

using System.Collections.Generic;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.Anchors;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Logging;

using UnityEngine;
using UnityEngine.UI;

public class HitTester : MonoBehaviour {
	/// The camera used to render the scene. Used to get the center of the screen.
	public Camera Camera;

	/// The types of hit test results to filter against when performing a hit test.
	[EnumFlagAttribute]
	public ARHitTestResultType HitTestType = ARHitTestResultType.ExistingPlane;

	/// The object we will place when we get a valid hit test result!
	public GameObject PlacementObjectPf;

	public GameObject CarObject;
	public GameObject ConeObject;
	public GameObject ObstacleObject;
	public GameObject RoadObject;

	public Button CarButton;
	public Button ConeButton;
	public Button ObstacleButton;
	public Button RoadButton;

	/// A list of placed game objects to be destroyed in the OnDestroy method.
	private List<GameObject> _placedObjects = new List<GameObject>();

	/// Internal reference to the session, used to get the current frame to hit test against.
	private IARSession _session;

	public Text DebugText;

	private void Start() {
		ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
	}

	private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args) {
		_session = args.Session;
		_session.Deinitialized += OnSessionDeinitialized;
	}

	private void OnSessionDeinitialized(ARSessionDeinitializedArgs args) {
		ClearObjects();
	}

	private void OnDestroy() {
		ARSessionFactory.SessionInitialized -= OnAnyARSessionDidInitialize;

		_session = null;

		ClearObjects();
	}

	private void ClearObjects() {
		foreach (var placedObject in _placedObjects) {
			Destroy(placedObject);
		}

		_placedObjects.Clear();
	}

	public void PlaceObject() {
		DebugText.text = "hit";

		var currentFrame = _session.CurrentFrame;
		if (currentFrame == null) {
			return;
		}

		var results = currentFrame.HitTest
		(
		  Camera.pixelWidth,
		  Camera.pixelHeight,
		  new Vector2(Camera.pixelWidth / 2, Camera.pixelHeight / 2),
		  HitTestType
		);

		int count = results.Count;
		Debug.Log("Hit test results: " + count);

		if (count <= 0)
			return;

		// Get the closest result
		var result = results[0];

		var hitPosition = result.WorldTransform.ToPosition();

		// Assumes that the prefab is one unit tall and getting scene height from local scale
		// Place the object on top of the surface rather than exactly on the hit point
		// Note (Kelly): Now that vertical planes are also supported in-editor, need to be
		// more elegant about how/if to handle instantiation of the cube
		hitPosition.y += PlacementObjectPf.transform.localScale.y / 2.0f;

		_placedObjects.Add(Instantiate(PlacementObjectPf, hitPosition, Quaternion.identity));

		var anchor = result.Anchor;
		Debug.LogFormat
		(
		  "Spawning cube at {0} (anchor: {1})",
		  hitPosition.ToString("F4"),
		  anchor == null
			? "none"
			: anchor.AnchorType + " " + anchor.Identifier
		);
	}

	public void SelectCar() {
		PlacementObjectPf = CarObject;
		DebugText.text = "Car";
	}
	public void SelectCone() {
		PlacementObjectPf = ConeObject;
		DebugText.text = "Cone";
	}
	public void SelectObstacle() {
		PlacementObjectPf = ObstacleObject;
		DebugText.text = "Obstacle";
	}
	public void SelectRoad() {
		PlacementObjectPf = RoadObject;
		DebugText.text = "Road";
	}
}
