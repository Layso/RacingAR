using UnityEngine;

public class VehicleController : MonoBehaviour {
	[SerializeField] private float MotorForce;
	[SerializeField] private float BrakeForce;
	[SerializeField] private float MaxSteeringAngle;

	[SerializeField] private Joystick SteeringJoystick;
	[SerializeField] private Joystick AccelerationJoystick;

	[SerializeField] private WheelCollider FrontLeftWheelCollider;
	[SerializeField] private WheelCollider FrontRightWheelCollider;
	[SerializeField] private WheelCollider RearLeftWheelCollider;
	[SerializeField] private WheelCollider RearRightWheelCollider;

	[SerializeField] private Transform FrontLeftWheelTransform;
	[SerializeField] private Transform FrontRightWheelTransform;
	[SerializeField] private Transform RearLeftWheelTransform;
	[SerializeField] private Transform RearRightWheelTransform;

	private float ForwardInput;
	private float SteeringInput;
	private bool IsBraking;

	private void Start() {
		FindObjectOfType<GameManager>().GameStateUpdated += this.OnGameStateUpdated;
	}

	private void OnGameStateUpdated(GameState State) {
		if (State == GameState.Returning) {
			Destroy(GetComponent<Rigidbody>());
		}
	}

	public void SetJoysticks(Joystick Steering, Joystick Acceleration) {
		SteeringJoystick = Steering;
		AccelerationJoystick = Acceleration;
	}

	private void FixedUpdate() {
		GetInput();
		HandleMotor();
		ApplyBraking();
		HandleSteering();
		UpdateWheels();
	}

	private void GetInput() {
#if UNITY_EDITOR
		if (Input.GetKey(KeyCode.W)) {
			ForwardInput = 1;
		} else if (Input.GetKey(KeyCode.S)) {
			ForwardInput = -1;
		} else {
			ForwardInput = 0;
		}

		if (Input.GetKey(KeyCode.D)) {
			SteeringInput = 1;
		} else if (Input.GetKey(KeyCode.A)) {
			SteeringInput = -1;
		} else {
			SteeringInput = 0;
		}
#else
		if (AccelerationJoystick != null && SteeringJoystick != null) {
			ForwardInput = AccelerationJoystick.Direction.y;
			SteeringInput = SteeringJoystick.Direction.x;
		}
#endif

		IsBraking = ((int)Mathf.Sign(ForwardInput) ^ (int)Mathf.Sign(FrontLeftWheelCollider.motorTorque)) < 0;
	}
	private void HandleMotor() {
		FrontLeftWheelCollider.motorTorque = ForwardInput * MotorForce;
		FrontRightWheelCollider.motorTorque = ForwardInput * MotorForce;
	}

	private void ApplyBraking() {
		FrontLeftWheelCollider.brakeTorque = IsBraking ? BrakeForce : 0;
		FrontRightWheelCollider.brakeTorque = IsBraking ? BrakeForce : 0;
		RearLeftWheelCollider.brakeTorque = IsBraking ? BrakeForce : 0;
		RearRightWheelCollider.brakeTorque = IsBraking ? BrakeForce : 0;
	}

	private void HandleSteering() {
		float currentSteering = MaxSteeringAngle * SteeringInput;
		FrontLeftWheelCollider.steerAngle = currentSteering;
		FrontRightWheelCollider.steerAngle = currentSteering;
	}

	private void UpdateWheels() {
		UpdateWheel(FrontLeftWheelTransform, FrontLeftWheelCollider);
		UpdateWheel(FrontRightWheelTransform, FrontRightWheelCollider);
		UpdateWheel(RearLeftWheelTransform, RearLeftWheelCollider);
		UpdateWheel(RearRightWheelTransform, RearRightWheelCollider);
	}

	private void UpdateWheel(Transform Transform, WheelCollider Collider) {
		Collider.GetWorldPose(out Vector3 pos, out Quaternion quat);
		Transform.position = pos;
		Transform.rotation = quat;
	}
}
