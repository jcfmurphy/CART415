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
}
