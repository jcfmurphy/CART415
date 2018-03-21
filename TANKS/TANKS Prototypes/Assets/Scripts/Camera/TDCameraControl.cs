using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCameraControl : MonoBehaviour {

	public GameObject m_Tank;
	public GameObject m_Cube;
	public CubeSide m_CubeSide;
	public float CameraDistance = 25f;

	private Camera m_Camera;
	private float m_RoundDelay = 3f;
	private float m_StartSize = 30f;
	private float m_PlaySize = 15f;
	private Vector3 m_StartPosition = new Vector3(0f, 0f, -25f);
	private bool m_RoundOver = false;
	private Vector3 m_TargetPosition;
	private Quaternion m_OffsetRotation;
	private Quaternion m_TargetRotation;
	private Transform m_Transform;
	private float m_MoveSpeed = 5.0f;
	private float m_RotateSpeed = 0.2f;

	// Use this for initialization
	void Start () {
		m_Transform = this.gameObject.transform;
		m_OffsetRotation = Quaternion.AngleAxis (90f, m_CubeSide.m_CameraRotateVector);
		m_Camera = GetComponent<Camera> ();

		StartCoroutine (StartCamera ());
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (!m_RoundOver) {
			MoveCamera ();
			RotateCamera ();
		}
	}


	void MoveCamera() {
		m_TargetPosition = m_Tank.transform.position.normalized * CameraDistance;

		m_TargetPosition = m_OffsetRotation * m_TargetPosition;

		m_Transform.position = Vector3.Slerp(m_Transform.position, m_TargetPosition, Time.deltaTime * m_MoveSpeed);
	}


	void RotateCamera() {
		Vector3 tempUp = Vector3.Slerp (m_Transform.up, m_Tank.transform.forward, m_RotateSpeed);
			
		m_Transform.LookAt (m_Cube.transform, tempUp);
	}

	public void SetCubeSide(CubeSide cubeside) {
		m_CubeSide = cubeside;
		m_OffsetRotation = Quaternion.AngleAxis (90f, m_CubeSide.m_CameraRotateVector);
	}

	IEnumerator StartCamera() {
		float timePassed = -0.5f;

		while (m_Camera.orthographicSize > m_PlaySize) {
			timePassed += Time.deltaTime;
			m_Camera.orthographicSize = Mathf.Lerp (m_StartSize, m_PlaySize, timePassed / m_RoundDelay);

			yield return null;
		}
	}

	IEnumerator EndCamera() {
		float timePassed = 0f;
		Vector3 endUp = transform.up;

		while (timePassed < m_RoundDelay) {
			timePassed += Time.deltaTime;
			m_Camera.orthographicSize = Mathf.Lerp (m_PlaySize, m_StartSize, timePassed / m_RoundDelay);

			m_Transform.position = Vector3.Slerp (m_TargetPosition, m_StartPosition, timePassed / m_RoundDelay).normalized * CameraDistance;
			m_Transform.LookAt (m_Cube.transform, Vector3.Slerp(endUp, Vector3.up, timePassed / m_RoundDelay));

			yield return null;
		}
	}

	public void EndRound() {
		m_RoundOver = true;
		StartCoroutine (EndCamera ());
	}
}
