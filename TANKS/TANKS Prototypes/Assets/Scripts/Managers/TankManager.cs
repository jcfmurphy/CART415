using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[Serializable]
public class TankManager
{

	public bool m_IsAITank = false;
	public State m_StartState;
    public Color m_PlayerColor;            
    public Transform m_SpawnPoint;         
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;                     
	[HideInInspector] public List<Transform> m_WayPointList;

    protected TankMovement m_Movement;       
	protected TankShooting m_Shooting;
	protected GameObject m_CanvasGameObject;
	protected StateController m_StateController;


	public void Setup(List<Transform> wayPointList)
	{
		if (m_IsAITank)
		{
			UnityEngine.Object.Destroy(m_Instance.gameObject.GetComponent<TankMovement>());
			m_Instance.GetComponent<Rigidbody>().isKinematic = false;
			m_Instance.GetComponent<StateController>().currentState = m_StartState;
			SetupAI(wayPointList);
		}
		else
		{
			UnityEngine.Object.Destroy(m_Instance.GetComponent<NavMeshAgent>());
			UnityEngine.Object.Destroy(m_Instance.GetComponent<StateController>());
			SetupPlayerTank();
		}
	}

	public virtual void SetupAI(List<Transform> wayPointList)
	{
		m_StateController = m_Instance.GetComponent<StateController> ();
		m_StateController.SetupAI (true, wayPointList);

		m_Shooting = m_Instance.GetComponent<TankShooting> ();
		m_Shooting.m_PlayerNumber = m_PlayerNumber;

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


	public virtual void SetupPlayerTank ()
	{
		// Get references to the components.

		m_Movement = m_Instance.GetComponent<TankMovement> ();
		m_Shooting = m_Instance.GetComponent<TankShooting> ();
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


	public void DisableControl ()
	{
		if (m_Movement != null)
			m_Movement.enabled = false;

		if (m_StateController != null)
			m_StateController.enabled = false;

		if (m_Shooting != null)
			m_Shooting.enabled = false;

		m_CanvasGameObject.SetActive (false);
	}


    public void EnableControl()
    {
		if (m_Movement != null)
			m_Movement.enabled = true;

		if (m_StateController != null)
			m_StateController.enabled = true;

		if (m_Shooting != null)
			m_Shooting.enabled = true;

		m_CanvasGameObject.SetActive (true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
