using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCone : MonoBehaviour {
	private void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "Vehicle") {
			++FindObjectOfType<GameManager>().Penalties;
			Destroy(this);
		}
	}
}
