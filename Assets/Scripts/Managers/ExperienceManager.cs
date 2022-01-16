using Niantic.ARDK.AR;
using Niantic.ARDK.AR.Anchors;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour {
	[SerializeField] private GameObject PlanePrefab;

	private bool RaceStarted;
	private IARSession Session;
	private Dictionary<Guid, GameObject> PlaneLookup;


	void Start() {
		RaceStarted = false;
		Session = ARSessionFactory.Create();
		PlaneLookup = new Dictionary<Guid, GameObject>();

		Session.AnchorsAdded += this.OnAnchorsAdded;
		Session.AnchorsUpdated += this.OnAnchorsUpdated;
		Session.AnchorsMerged += this.OnAnchorsMerged;
		Session.AnchorsRemoved += this.OnAnchorsRemoved;

		var config = ARWorldTrackingConfigurationFactory.Create();
		config.WorldAlignment = WorldAlignment.Gravity;
		config.IsLightEstimationEnabled = true;
		config.PlaneDetection = PlaneDetection.Horizontal | PlaneDetection.Vertical;
		config.IsAutoFocusEnabled = true;
		config.IsDepthEnabled = false;
		config.IsSharedExperienceEnabled = true;

		Session.Run(config);
	}

	private void OnAnchorsRemoved(AnchorsArgs args) {
		if (!RaceStarted) {
			foreach (var anchor in args.Anchors) {
				if (anchor.AnchorType == AnchorType.Plane) {
					Destroy(PlaneLookup[anchor.Identifier]);
					PlaneLookup.Remove(anchor.Identifier);
				}
			}
		}
	}

	private void OnAnchorsMerged(AnchorsMergedArgs args) {
		//throw new System.NotImplementedException();
	}

	private void OnAnchorsUpdated(AnchorsArgs args) {
		if (!RaceStarted) {
			foreach (IARPlaneAnchor anchor in args.Anchors) {
				if (PlaneLookup.TryGetValue(anchor.Identifier, out GameObject plane)) {
					plane.transform.position = anchor.Transform.ToPosition();
					plane.transform.rotation = anchor.Transform.ToRotation();
					plane.transform.localScale = anchor.Extent;
				}
			}
		}
	}

	private void OnAnchorsAdded(AnchorsArgs args) {
		if (!RaceStarted) {
			foreach (IARPlaneAnchor anchor in args.Anchors) {
				if (anchor.AnchorType == AnchorType.Plane) {
					var plane = Instantiate(PlanePrefab);
					PlaneLookup.Add(anchor.Identifier, plane);

					plane.transform.position = anchor.Transform.ToPosition();
					plane.transform.rotation = anchor.Transform.ToRotation();
					plane.transform.localScale = anchor.Extent;
				}
			}
		}
	}

	public bool GetWorldPosition(Vector2 ScreenPoint, out Vector3 WorldPoint) {
		WorldPoint = Vector3.zero;

		if (Session != null) {
			IARFrame frame = Session.CurrentFrame;
			var results = frame.HitTest(Camera.main.pixelWidth, Camera.main.pixelHeight, ScreenPoint, ARHitTestResultType.ExistingPlane | ARHitTestResultType.ExistingPlaneUsingExtent | ARHitTestResultType.ExistingPlaneUsingGeometry);
			if (results.Count > 0) {
				IARHitTestResult result = results[0];
				WorldPoint = result.WorldTransform.ToPosition();
				return true;
			}
		}

		return false;
	}

	public void OnRaceStarted() {
		RaceStarted = true;
		foreach (GameObject plane in PlaneLookup.Values) {
			Destroy(plane);
		}
		PlaneLookup.Clear();
	}
}