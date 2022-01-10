using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ObstacleType {
	Cone,
	Roadblock
}

public class ObstaclePlacerButtons : MonoBehaviour {
	public event Action<ObstacleType> ObstacleChanged;

	[SerializeField] private Image ConeBackground;
	[SerializeField] private Image RoadblockBackground;
	[SerializeField] private Color SelectedColor;
	[SerializeField] private Color NotSelectedColor;

	public Button PlaceButton;
	public Button RemoveButton;
	public Button NextButton;

	private void Start() {
		OnConeSelected();
	}

	public void OnConeSelected() {
		ConeBackground.color = SelectedColor;
		RoadblockBackground.color = NotSelectedColor;
		if (ObstacleChanged != null) {
			ObstacleChanged.Invoke(ObstacleType.Cone);
		}
	}
	public void OnRoadblockSelected() {
		RoadblockBackground.color = SelectedColor;
		ConeBackground.color = NotSelectedColor;
		if (ObstacleChanged != null) {
			ObstacleChanged.Invoke(ObstacleType.Roadblock);
		}
	}
}
