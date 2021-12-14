using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour {
	public static CarController Instance;

	public float AccelerationForce = 5;
	public float BreakForce = 3;
	public float DecelerationPercent = 0.99f;
	public float MaxVelocity = 50;
	public float TurnForce = 20;

	[SerializeField] private Joystick SteeringJoyStick;
	[SerializeField] private Joystick AccelerationJoystick;


	GameObject Wheel_FrontRight;
	GameObject Wheel_FrontLeft;
	GameObject Wheel_RearRight;
	GameObject Wheel_RearLeft;
	GameObject Axle_Right;
	GameObject Axle_Left;

	public Text DebugText;

	Rigidbody rb;
	bool ForwardVelocity;

	public const int WHEEL_TURN_ANGLE = 45;


	private void Awake() {
		if (Instance != null) {
			Destroy(Instance.gameObject);
		}
		
		Instance = this;
	}

	void Start() {
		rb = GetComponent<Rigidbody>();
		Axle_Left = transform.Find("Wheels/Meshes/LeftAxle").gameObject;
		Axle_Right = transform.Find("Wheels/Meshes/RightAxle").gameObject;
		Wheel_RearLeft = transform.Find("Wheels/Meshes/RearLeftWheel").gameObject;
		Wheel_RearRight = transform.Find("Wheels/Meshes/RearRightWheel").gameObject;
		Wheel_FrontLeft = transform.Find("Wheels/Meshes/LeftAxle/FrontLeftWheel").gameObject;
		Wheel_FrontRight = transform.Find("Wheels/Meshes/RightAxle/FrontRightWheel").gameObject;

		Text[] texts = FindObjectsOfType<Text>();
		foreach (Text t in texts) {
			if (t.name == "DebugText") {
				DebugText = t;
				break;
			}
		}

		Joystick[] joysticks = FindObjectsOfType<Joystick>();
		foreach (var js in joysticks) {
			if (js.name == "LeftJoystick") {
				SteeringJoyStick = js;
			} else {
				AccelerationJoystick = js;
			}
		}
	}

	void Update() {
		InputCheck();
		SpinWheels();
		ForwardVelocity = Vector3.Dot(rb.velocity, transform.forward) > 0;
		//DebugText.text = "Speed: " + rb.velocity.magnitude.ToString("0.00");
	}

	public void ManualFriction() {
		rb.velocity = rb.velocity * DecelerationPercent * Time.deltaTime;
		if (rb.velocity.magnitude < 1) {
			//rb.velocity = Vector3.zero;
		}
	}

	public void InputCheck() {
		if (SteeringJoyStick.JoystickPosition.x != 0) {
			Turn(SteeringJoyStick.JoystickPosition.x);
		} else {
			ResetTurn();
		}

		

		if (AccelerationJoystick.JoystickPosition.y != 0) {
			Move(AccelerationJoystick.JoystickPosition.y);
		} else {
			rb.velocity = rb.velocity * DecelerationPercent;
		}
	}

	private void Turn(float AxisValue) {
		transform.Rotate(new Vector3(0, TurnForce * Time.deltaTime * Mathf.Min(10, rb.velocity.magnitude) * (ForwardVelocity ? 1 : -1), 0));
		Axle_Right.transform.localEulerAngles = new Vector3(0, WHEEL_TURN_ANGLE * AxisValue, 0);
		Axle_Left.transform.localEulerAngles = new Vector3(0, WHEEL_TURN_ANGLE * AxisValue, 0);
	}

	private void Move(float AxisValue) {
		Text dt = null;
		var texts = FindObjectsOfType<Text>();
		foreach (var text in texts) {
			if (text.name == "DebugText") {
				dt = text;
				break;
			}
		}

		if (rb != null && rb.velocity.magnitude < MaxVelocity) {
			var asd = transform.forward * AccelerationForce * AxisValue;
			dt.text = asd.ToString();
			rb.AddForce(transform.forward * AccelerationForce * AxisValue, ForceMode.Acceleration);
		} else {
			dt.text = "anan";
		}
	}

	public void SpinWheels() {
		if (rb.velocity.magnitude > 0) {
			int direction = ForwardVelocity ? 1 : -1;
			Wheel_FrontRight.transform.Rotate(direction * rb.velocity.magnitude, 0, 0);
			Wheel_FrontLeft.transform.Rotate(direction * rb.velocity.magnitude, 0, 0);
			Wheel_RearRight.transform.Rotate(direction * rb.velocity.magnitude, 0, 0);
			Wheel_RearLeft.transform.Rotate(direction * rb.velocity.magnitude, 0, 0);
		}
	}

	public void MoveForward() {
		if (rb != null && rb.velocity.magnitude < MaxVelocity) {
			rb.AddForce(transform.forward * AccelerationForce, ForceMode.Acceleration);
		}
	}

	public void MoveBackward() {
		if (rb != null) {
			rb.AddForce(-transform.forward * BreakForce, ForceMode.Acceleration);
		}
	}

	public void TurnRight() {
		transform.Rotate(new Vector3(0, TurnForce * Time.deltaTime * Mathf.Min(10, rb.velocity.magnitude) * (ForwardVelocity ? 1 : -1), 0));
		Axle_Right.transform.localEulerAngles = new Vector3(0, WHEEL_TURN_ANGLE, 0);
		Axle_Left.transform.localEulerAngles = new Vector3(0, WHEEL_TURN_ANGLE, 0);
	}

	public void TurnLeft() {

		transform.Rotate(new Vector3(0, TurnForce * Time.deltaTime * Mathf.Min(10, rb.velocity.magnitude) * (ForwardVelocity ? -1 : 1), 0));
		Axle_Right.transform.localEulerAngles = new Vector3(0, -WHEEL_TURN_ANGLE, 0);
		Axle_Left.transform.localEulerAngles = new Vector3(0, -WHEEL_TURN_ANGLE, 0);
	}

	public void ResetTurn() {
		Axle_Right.transform.localEulerAngles = new Vector3(0, 0, 0);
		Axle_Left.transform.localEulerAngles = new Vector3(0, 0, 0);
	}
}
