using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour {

	private static DontDestroyMusic instanceRef;

	void Awake() {

		if (instanceRef == null) {
			instanceRef = this;
			DontDestroyOnLoad (transform.gameObject);
		} else {
			DestroyImmediate (gameObject);
		}
	}
}
