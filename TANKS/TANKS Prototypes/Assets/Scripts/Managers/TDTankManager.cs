using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class TDTankManager : TankManager
{
	public override void SetupPlayerTank ()
	{
		// Get references to the components.

		m_Movement = m_Instance.GetComponent<TDTankMovement> ();
		m_Shooting = m_Instance.GetComponent<TDTankShooting> ();
		m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;

		// Set the player numbers to be consistent across the scripts.
		m_Movement.m_PlayerNumber = m_PlayerNumber;
		m_Shooting.m_PlayerNumber = m_PlayerNumber;

	}

	public override void SetupAI(List<Transform> wayPointList)
	{
		m_StateController = m_Instance.GetComponent<StateController> ();
		m_StateController.SetupAI (true, wayPointList);
	}

	public void FixedUpdate() {
		KeepOnCube ();
	}

	public void KeepOnCube() {

		Vector3 tankPosition = m_Instance.transform.position;

		if (tankPosition.x > 10.1f || tankPosition.x < -10.1f || tankPosition.y > 10.1f || tankPosition.y < -10.1f || tankPosition.z > 10.1f || tankPosition.z < -10.1f) {
			float newX = Mathf.Clamp (tankPosition.x, -10f, 10f);
			float newY = Mathf.Clamp (tankPosition.y, -10f, 10f);
			float newZ = Mathf.Clamp (tankPosition.z, -10f, 10f);

			Vector3 newPos = new Vector3 (newX, newY, newZ);

			m_Instance.transform.position = newPos;

			Debug.Log ("kept on cube");
		}
	}
}
