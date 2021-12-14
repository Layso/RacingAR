using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
	public Vector2 JoystickPosition { get; private set; }

	[SerializeField] private RectTransform JoystickButton;
	[SerializeField] private RectTransform JoystickBackground;

	private int PointerIndex;
	private bool IsPointerDown;
	private bool IsPointerEntered;

	public Text DebugText;

	void Start() {
		IsPointerDown = false;
		IsPointerEntered = false;
	}

	private void Update() {
		if (IsPointerEntered && IsPointerDown) {
			JoystickButton.position = Input.GetTouch(PointerIndex).position;
			JoystickPosition = JoystickButton.anchoredPosition.normalized;
			float radius = JoystickBackground.rect.width / 2;
			if (JoystickButton.anchoredPosition.magnitude > radius) {
				JoystickButton.anchoredPosition = JoystickButton.anchoredPosition.normalized * radius;
			}
		} else {
			JoystickButton.anchoredPosition = Vector2.zero;
			JoystickPosition = JoystickButton.anchoredPosition.normalized;
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		IsPointerEntered = true;
	}

	public void OnPointerExit(PointerEventData eventData) {
		IsPointerEntered = false;
	}

	public void OnPointerDown(PointerEventData eventData) {
		PointerIndex = eventData.pointerId;
		IsPointerDown = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		IsPointerDown = false;
	}
}
