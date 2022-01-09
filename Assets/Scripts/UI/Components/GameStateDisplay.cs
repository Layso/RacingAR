using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDisplay : MonoBehaviour {
	[SerializeField] private int SlideSpeed = 1;
	[SerializeField] private Color IncomingColor;
	[SerializeField] private Color CompletedColor;
	[SerializeField] private Slider Indicator;
	[SerializeField] private List<Image> Backgrounds;

	private int CurrentIndex;


	void Start() {
		GameManager manager = FindObjectOfType<GameManager>();
		if (manager != null) {
			manager.GameStateUpdated += this.OnGameStateUpdated;
			OnGameStateUpdated(manager.GetState());
		}
	}

	private void OnGameStateUpdated(GameState State) {
		CurrentIndex = State - GameState.RoadBuilding;
		UpdateDisplay();
	}

	private void UpdateDisplay() {
		if (CurrentIndex < 0 || CurrentIndex >= Backgrounds.Count) {
			gameObject.SetActive(false);
		} else {
			gameObject.SetActive(true);
			StartCoroutine(SlideIndicator());
			StartCoroutine(ChangeColor());
		}
	}

	private IEnumerator SlideIndicator() {
		float t = 0;
		float begin = Indicator.value;
		float end = (float)(CurrentIndex*2+1) / (Backgrounds.Count*2);

		while (Indicator.value < end) {
			Indicator.value = Mathf.Lerp(begin, end, t);
			t += Time.deltaTime * SlideSpeed;
			yield return null;
		}
	}

	private IEnumerator ChangeColor() {
		float t = 0;
		Image image = Backgrounds[CurrentIndex];

		while (image.color != CompletedColor) {
			image.color = Color.Lerp(IncomingColor, CompletedColor, t);
			t += Time.deltaTime * SlideSpeed;
			yield return null;
		}
	}
}