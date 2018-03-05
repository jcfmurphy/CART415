using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCameraControl : MonoBehaviour {

	public GameObject m_Tank;
	public GameObject m_Cube;

	private Vector3 m_TargetPosition;
	private Quaternion m_OffsetRotation;
	private Quaternion m_TargetRotation;
	private Transform m_Transform;
	private float m_MoveSpeed = 5.0f;
	private float m_RotateSpeed = 10f;


	// Use this for initialization
	void Start () {
		m_Transform = this.gameObject.transform;
		m_OffsetRotation = Quaternion.AngleAxis (90f, Vector3.forward);
	}


	// Update is called once per frame
	void FixedUpdate () {
		MoveCamera ();
		RotateCamera ();
	}


	void MoveCamera() {
		m_TargetPosition = m_Tank.transform.position.normalized * 30f;

		m_TargetPosition = m_OffsetRotation * m_TargetPosition;

		m_Transform.position = Vector3.Slerp(m_Transform.position, m_TargetPosition, Time.deltaTime * m_MoveSpeed);
	}


	void RotateCamera() {
		Vector3 tempUp = Vector3.Slerp (m_Transform.up, m_Tank.transform.forward, m_RotateSpeed);
			
		m_Transform.LookAt (m_Cube.transform, tempUp);
	}
}
