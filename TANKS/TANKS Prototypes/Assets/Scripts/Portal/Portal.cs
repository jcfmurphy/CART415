﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour {

	public Portal m_LinkedPortal;
	public CubeSide m_CubeSide;
	public GameObject m_GhostPrefab;

	private Color m_StartColor;
	private Color m_StartEmission;
	private Color m_GlowColor;
	private Color m_GlowEmission;
	private float m_GlowTimer = 0;
	private bool m_Glowing = false;
	private Transform m_Transform;
	private Transform m_LinkedTransform;
	private MeshRenderer m_Renderer;
	private TDCameraControl m_CameraControl;


	// Use this for initialization
	void Start () {
		m_Transform = this.gameObject.transform;
		m_LinkedTransform = m_LinkedPortal.gameObject.transform;
		m_Renderer = GetComponent<MeshRenderer> ();
		m_GlowColor = new Color(0.6588f, 0.2353f, 0.3176f, 0.4353f);
		m_GlowEmission = new Color (0.5f, 0.0f, 0.0f);
		m_StartColor = new Color(0.2353f, 0.6588f, 0.6588f, 0.4353f);
		m_StartEmission = new Color (0.0f, 0.0f, 0.0f);
		m_CameraControl = Camera.main.GetComponent<TDCameraControl>();
	}


	void Update() {
		if (m_Glowing) {
			m_GlowTimer += Time.deltaTime;

			m_Renderer.material.color = Color.Lerp (m_GlowColor, m_StartColor, m_GlowTimer);
			m_Renderer.material.SetColor("_EmissionColor", Color.Lerp(m_GlowEmission, m_StartEmission, m_GlowTimer * 0.5f));

			if (m_GlowTimer >= 2.0f) {
				m_GlowTimer = 0f;
				m_Glowing = false;
			}
		}
	}


	void OnTriggerEnter (Collider tankCollider) {

		if (tankCollider.gameObject.tag == "PortalCollider") {

			if (tankCollider.transform.parent.name == "3DTank(Clone)") {

				Warp (tankCollider);

			} else {

				WarpAI (tankCollider);

			}

		}
	}


	void Warp(Collider tankCollider) {
		
		WarpedBool hasWarped = tankCollider.GetComponent<WarpedBool> ();

		if (!hasWarped.GetWarped ()) {

			Transform tankTransform = tankCollider.gameObject.transform.parent.transform;

			Instantiate (m_GhostPrefab, tankTransform.position, tankTransform.rotation);

			Vector3 positionOffset = tankTransform.position - m_Transform.position;

			positionOffset = Quaternion.AngleAxis (90f, m_Transform.up) * positionOffset;

			tankTransform.position = m_LinkedTransform.position + positionOffset;

			tankTransform.RotateAround (tankTransform.position, m_Transform.up, 90f);

			tankTransform.position += m_Transform.up;

			hasWarped.SetWarped (true);

			Glow ();

			m_LinkedPortal.Glow();

			m_CameraControl.SetCubeSide (m_LinkedPortal.m_CubeSide);

		}
	}


	void WarpAI(Collider tankCollider) {

		WarpedBool hasWarped = tankCollider.GetComponent<WarpedBool> ();

		if (!hasWarped.GetWarped ()) {

			Transform tankTransform = tankCollider.gameObject.transform.parent.transform;

			Instantiate (m_GhostPrefab, tankTransform.position, tankTransform.rotation);

			NavMeshAgent tankAgent = tankCollider.gameObject.transform.parent.GetComponent<NavMeshAgent> ();

			StateController tempController = tankCollider.gameObject.transform.parent.GetComponent<StateController> ();

			tempController.SetCubeSide (m_LinkedPortal.m_CubeSide);

			Vector3 newPosition = tankTransform.position - m_Transform.position;

			newPosition = Quaternion.AngleAxis (90f, m_Transform.up) * newPosition;

			newPosition = newPosition + m_LinkedTransform.position + m_Transform.up;

			tankAgent.Warp (newPosition);

			tankTransform.RotateAround (tankTransform.position, m_Transform.up, 90f);

			hasWarped.SetWarped (true);

			Glow ();

			m_LinkedPortal.Glow();

		}
	}
		

	void Glow() {
		m_Renderer.material.color = m_GlowColor;
		m_Renderer.material.SetColor("_EmissionColor", m_GlowEmission);
		m_Glowing = true;
		m_GlowTimer = 0.0f;
	}
}
