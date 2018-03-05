using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour {

	private Quaternion m_TargetRotation;
	private float m_RotateSpeed = 10f;
	private Transform m_Transform;


	// Use this for initialization
	private void Start () {
		m_Transform = this.gameObject.GetComponent<Transform> ();
		m_TargetRotation = Quaternion.LookRotation(Vector3.forward);
	}
	
	// Update is called once per frame
	private void Update () {
		if (Input.GetKeyDown ("1")) {
			SetTarget(Vector3.forward);
		} else if (Input.GetKeyDown ("2")) {
			SetTarget(Vector3.left);
		} else if (Input.GetKeyDown ("3")) {
			SetTarget(Vector3.back);
		} else if (Input.GetKeyDown ("4")) {
			SetTarget(Vector3.right);
		} else if (Input.GetKeyDown ("5")) {
			SetTarget(Vector3.up);
		} else if (Input.GetKeyDown ("6")) {
			SetTarget(Vector3.down);
		}
	}

	private void FixedUpdate() {
		m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, m_TargetRotation, Time.deltaTime * m_RotateSpeed);
	}

	public void SetTarget(Vector3 targetVector) {
		m_TargetRotation = Quaternion.LookRotation(targetVector);
	}
}
