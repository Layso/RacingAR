using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
	public Vector2 JoystickPosition { get; private set; }

	private bool IsPointerDown;
	private bool IsPointerEntered;
	[SerializeField] private RectTransform JoystickBackground;
	[SerializeField] private RectTransform JoystickButton;

	void Start() {
		IsPointerDown = false;
		IsPointerEntered = false;
	}

	private void Update() {
		if (IsPointerEntered && IsPointerDown) {
			JoystickButton.position = Input.mousePosition;
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
		IsPointerDown = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		IsPointerDown = false;
	}
}
