using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour {
	[SerializeField] private int CountdownStart;
	[SerializeField] private Text CountdownText;
	[SerializeField] private Text ChronometerText;
	[SerializeField] private Button ResetButton;
	[SerializeField] private GameObject VehiclePrefab;
	[SerializeField] private Joystick SteeringJoystick;
	[SerializeField] private Joystick AccelerationJoystick;

	private GameManager Manager;

	void Start() {
		ResetButton.gameObject.SetActive(false);
		ChronometerText.gameObject.SetActive(false);
		CountdownText.text = string.Empty;
		Manager = FindObjectOfType<GameManager>();
		Manager.GameStateUpdated += this.OnGameStateUpdated;
	}

	private void OnGameStateUpdated(GameState State) {
		if (State == GameState.WaitingToStart) {
			StartCoroutine(Countdown());
		} else if (State == GameState.Racing) {
			StartCoroutine(StartRace());
		} else if (State == GameState.Returning) {
			ResetButton.gameObject.SetActive(false);
			ChronometerText.gameObject.SetActive(false);
			SteeringJoystick.gameObject.SetActive(false);
			AccelerationJoystick.gameObject.SetActive(false);
		}
	}

	void Update() {
		ChronometerText.text = TimeSpan.FromSeconds(Time.time - Manager.StartTime).ToString("mm\\:ss\\.fff");
	}

	private IEnumerator Countdown() {
#if UNITY_EDITOR
		yield return null;
#else
		int current = Mathf.Max(CountdownStart, 0);
		while (current > 0) {
			CountdownText.text = current.ToString();
			yield return new WaitForSeconds(1);
			--current;
		}
#endif

		FindObjectOfType<GameManager>().OnCountdownEnded();
	}

	private IEnumerator StartRace() {
		Transform startTransform = FindObjectOfType<GameManager>().StartTransform;
		VehicleController vehicle = Instantiate(VehiclePrefab, startTransform.position, startTransform.rotation).GetComponent<VehicleController>();

		ResetButton.gameObject.SetActive(true);
		ChronometerText.gameObject.SetActive(true);
		SteeringJoystick.gameObject.SetActive(true);
		AccelerationJoystick.gameObject.SetActive(true);
		vehicle.SetJoysticks(SteeringJoystick, AccelerationJoystick);
		
		CountdownText.text = "Start!";
		yield return new WaitForSeconds(1);
		CountdownText.text = string.Empty;
	}
}
