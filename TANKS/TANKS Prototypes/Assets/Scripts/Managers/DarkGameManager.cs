﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class DarkGameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;           
    public CameraControl m_CameraControl;   
    public Text m_MessageText;              
    public GameObject m_TankPrefab;         
    public DarkTankManager[] m_Tanks;
	public Light m_ShotLight;


    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
    private DarkTankManager m_RoundWinner;
    private DarkTankManager m_GameWinner;
	private int m_ShooterNumber;
	private float m_ShotTimer;
	private float m_ShotLimit = 6f;
	private List<Light> m_Lights;

    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        SetCameraTargets();

		m_ShooterNumber = 0;

		m_Lights = new List<Light>();

        StartCoroutine(GameLoop());
    }
		

	private void Update() {
		m_ShotTimer += Time.deltaTime;

		if (m_ShotTimer >= m_ShotLimit) {
			SwitchActiveShooter();
		}

		if (Input.GetButtonDown ("Cancel")) {
			SceneManager.LoadScene(0);
		}
	}

    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
		ResetAllTanks ();
		DisableTankControl ();

		m_CameraControl.SetStartPositionAndSize ();

		m_RoundNumber++;

		if (m_RoundNumber != 1) {
			m_MessageText.text = "ROUND " + m_RoundNumber;
		}

		yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
		EnableTankControl ();
		SwitchActiveShooter ();
		m_ShotTimer = 0f;

		m_MessageText.text = string.Empty;

		while (!OneTankLeft()) {
        	yield return null;
		}
    }


    private IEnumerator RoundEnding()
    {
		DisableTankControl ();

		m_RoundWinner = null;

		m_RoundWinner = GetRoundWinner ();

		if (m_RoundWinner != null) {
			m_RoundWinner.m_Wins++;
		}

		m_GameWinner = GetGameWinner ();

		string message = EndMessage ();
		m_MessageText.text = message;

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private DarkTankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private DarkTankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }

	public void SwitchActiveShooter()
	{
		m_ShooterNumber++;
		if (m_ShooterNumber >= m_Tanks.Length) {
			m_ShooterNumber = 0;
		}

		for (int i = 0; i < m_Tanks.Length; i++)
		{
			m_Tanks [i].DeactivateShooter ();
		}

		m_Tanks [m_ShooterNumber].ActivateShooter ();

		m_ShotTimer = 0f;
	}

	public void DropLight(Vector3 lightPos, Quaternion lightRot) {

		m_Lights.Add(Instantiate (m_ShotLight, lightPos, lightRot));

		while (m_Lights.Count > 6) {
			Light destroyLight = m_Lights [0];
			m_Lights.Remove (destroyLight);
			Destroy (destroyLight.gameObject);
		}
	}
}