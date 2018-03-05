using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SoundTankManager : TankManager
{
	public override void SetupPlayerTank ()
	{
		// Get references to the components.

		m_Movement = m_Instance.GetComponent<SoundTankMovement> ();
		m_Shooting = m_Instance.GetComponent<SoundTankShooting> ();
		m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;

		// Set the player numbers to be consistent across the scripts.
		m_Movement.m_PlayerNumber = m_PlayerNumber;
		m_Shooting.m_PlayerNumber = m_PlayerNumber;

		// Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
		m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

		// Get all of the renderers of the tank.
		MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer> ();

		// Go through all the renderers...
		for (int i = 0; i < renderers.Length; i++)
		{
			// ... set their material color to the color specific to this tank.
			renderers[i].material.color = m_PlayerColor;
		}
	}

	public override void SetupAI(List<Transform> wayPointList)
	{
		m_StateController = m_Instance.GetComponent<StateController> ();
		m_StateController.SetupAI (true, wayPointList);

		m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;
		m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

		// Get all of the renderers of the tank.
		MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer> ();

		// Go through all the renderers...
		for (int i = 0; i < renderers.Length; i++)
		{
			// ... set their material color to the color specific to this tank.
			renderers[i].material.color = m_PlayerColor;
		}
	}

	public void SetupKingTank ()
	{
		// Get references to the components.

		m_Movement = m_Instance.GetComponent<SoundTankMovement> ();
		m_Shooting = m_Instance.GetComponent<SoundTankShooting> ();
		m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;

		// Set the player numbers to be consistent across the scripts.
		m_Movement.m_PlayerNumber = m_PlayerNumber;
		m_Shooting.m_PlayerNumber = m_PlayerNumber;

		// Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
		m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

	}
}
