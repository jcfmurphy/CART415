using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class TDGameManager : MonoBehaviour
{

	public float m_StartDelay = 3f;         
	public float m_EndDelay = 3f; 
	public int m_SpawnsRemaining = 10;
	public Text m_MessageText;              
	public GameObject m_AIPrefab;
	public GameObject m_PlayerPrefab;
	public List<Transform> m_SpawnPoints;
	public TDCameraControl m_CameraControl;

	private float m_SpawnDelay = 6f;
	protected float m_SpawnTimer = 0f;
	protected Vector3 m_CheckBoxSize = new Vector3(1.25f, 1.25f, 1.25f);
	protected TDTankManager[] m_AITanks;
	protected TDTankManager m_PlayerTank;       
	protected WaitForSeconds m_StartWait;     
	protected WaitForSeconds m_EndWait;       

	protected void Start()
	{
		m_StartWait = new WaitForSeconds(m_StartDelay);
		m_EndWait = new WaitForSeconds(m_EndDelay);

		m_PlayerTank = new TDTankManager ();
		m_PlayerTank.m_PlayerNumber = 1;
		m_PlayerTank.m_Instance = 
			Instantiate(m_PlayerPrefab, m_SpawnPoints[0].transform.position, m_SpawnPoints[0].transform.rotation) as GameObject;
		m_PlayerTank.m_IsAITank = false;
		m_PlayerTank.SetupPlayerTank ();

		m_CameraControl.m_Tank = m_PlayerTank.m_Instance;

		m_AITanks = new TDTankManager[m_SpawnsRemaining];
		for (int i = 0; i < m_AITanks.Length; i++) {
			m_AITanks[i] = new TDTankManager();
		}

		StartCoroutine(GameLoop());
	}


	protected void Update() {
		m_SpawnTimer += Time.deltaTime;

		if (m_SpawnTimer >= m_SpawnDelay && m_SpawnsRemaining > 0) {
			SpawnTank ();
		}
	}


	protected void SpawnTank()
	{
		Transform tempSpawnPoint = GetSpawnPoint ();

		if (tempSpawnPoint != null) {
			m_AITanks [m_SpawnsRemaining - 1].m_Instance = 
			Instantiate (m_AIPrefab, tempSpawnPoint.position, tempSpawnPoint.rotation) as GameObject;
			m_AITanks [m_SpawnsRemaining - 1].m_IsAITank = true;

			m_AITanks [m_SpawnsRemaining - 1].SetupAI (m_SpawnPoints);

			m_SpawnsRemaining -= 1;
			m_SpawnTimer = 0f;
		}
	}


	protected Transform GetSpawnPoint() {
		List<Transform> tempSpawnPoints = new List<Transform>();

		for (int i = 0; i < m_SpawnPoints.Count; i++) {
			Transform spawnTransform = m_SpawnPoints [i].transform;
			Vector3 checkLocation = spawnTransform.position + 1.3f * spawnTransform.up;

			if (!Physics.CheckBox(checkLocation, m_CheckBoxSize, spawnTransform.rotation)) {
				tempSpawnPoints.Add (m_SpawnPoints [i]);
			}
		}

		if (tempSpawnPoints.Count > 0) {
			int spawnRandom = Random.Range (0, tempSpawnPoints.Count);
			return tempSpawnPoints [spawnRandom];
		} else {
			return null;
		}
	}


	protected IEnumerator GameLoop()
	{
		yield return StartCoroutine(RoundStarting());
		yield return StartCoroutine(RoundPlaying());
		yield return StartCoroutine(RoundEnding());

		SceneManager.LoadScene(0);
	}


	protected IEnumerator RoundStarting()
	{
		DisableTankControl ();

		yield return m_StartWait;
	}


	protected IEnumerator RoundPlaying()
	{
		EnableTankControl ();

		m_MessageText.text = string.Empty;

		while (!RoundOver()) {
			yield return null;
		}
	}
		
	protected IEnumerator RoundEnding()
	{
		string message = EndMessage ();
		m_MessageText.text = message;

		DisableTankControl ();

		yield return m_EndWait;
	}


	protected bool RoundOver() {
		if (!m_PlayerTank.m_Instance.activeSelf) {
			return true;
		} else if (m_SpawnsRemaining > 0) {
			return false;
		} else {

			int numTanksLeft = 0;

			for (int i = 0; i < m_AITanks.Length; i++) {
				if (m_AITanks [i].m_Instance.activeSelf)
					numTanksLeft++;
			}

			return numTanksLeft < 1;
		}
	}


	protected string EndMessage()
	{
		string message;

		if (m_PlayerTank.m_Instance.activeSelf) {
			message = "YOU WIN!";
		} else {
			message = "YOU HAVE BEEN DEFEATED!";
		}

		return message;
	}
		
	protected void EnableTankControl()
	{
		m_PlayerTank.EnableControl();

		//for (int i = 0; i < m_AITanks.Length; i++) {
		//	m_AITanks [i].EnableControl ();
		//}
	}


	protected void DisableTankControl()
	{
		m_PlayerTank.DisableControl();

		//for (int i = 0; i < m_AITanks.Length; i++) {
		//	m_AITanks [i].DisableControl ();
		//}
	}
}