using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class SoundGameManager : MonoBehaviour
{

	public float m_StartDelay = 3f;         
	public float m_EndDelay = 3f; 
	public float m_SpawnDelay = 3f;
	public int m_SpawnsRemaining = 30;
	public Text m_MessageText;              
	public GameObject m_CitizenPrefab;
	public GameObject m_KingPrefab;
	public Transform m_KingSpawnPoint;
	public List<Transform> m_SpawnPoints;
	public Color[] m_CitizenColors = new Color[3];
	public SoundCameraControl m_CameraControl;
	public AudioSource m_ViveLaFrance;

	public float m_SpawnTimer = 0f;
	protected Vector3 m_CheckBoxSize = new Vector3(1.25f, 1.25f, 1.25f);
	protected SoundTankManager[] m_CitizenTanks;
	protected SoundTankManager m_KingTank;       
	protected WaitForSeconds m_StartWait;     
	protected WaitForSeconds m_EndWait;       
	protected float m_AudioTime = -2.5f;

	protected void Start()
	{
		m_StartWait = new WaitForSeconds(m_StartDelay);
		m_EndWait = new WaitForSeconds(m_EndDelay);

		m_KingTank = new SoundTankManager ();
		m_KingTank.m_PlayerNumber = 1;
		m_KingTank.m_Instance = 
			Instantiate(m_KingPrefab, m_KingSpawnPoint.position, m_KingSpawnPoint.rotation) as GameObject;
		m_KingTank.m_IsAITank = false;
		m_KingTank.m_PlayerColor = new Color (0.2275f, 0.2275f, 0.2275f);
		m_KingTank.SetupKingTank ();

		m_CameraControl.m_Target = m_KingTank.m_Instance.transform;

		m_CitizenTanks = new SoundTankManager[m_SpawnsRemaining];
		for (int i = 0; i < m_CitizenTanks.Length; i++) {
			m_CitizenTanks[i] = new SoundTankManager();
		}

		StartCoroutine(GameLoop());
	}


	protected void Update() {
		m_SpawnTimer += Time.deltaTime;

		if (m_SpawnTimer >= m_SpawnDelay && m_SpawnsRemaining > 0) {
			SpawnTank ();
		}

		m_AudioTime += Time.deltaTime;
	}


	protected void SpawnTank()
	{
		Transform tempSpawnPoint = GetSpawnPoint ();

		if (tempSpawnPoint != null) {
			m_CitizenTanks [m_SpawnsRemaining - 1].m_Instance = 
			Instantiate (m_CitizenPrefab, tempSpawnPoint.position, tempSpawnPoint.rotation) as GameObject;
			m_CitizenTanks [m_SpawnsRemaining - 1].m_IsAITank = true;

			int randColor = Random.Range (0, 3);
			m_CitizenTanks [m_SpawnsRemaining - 1].m_PlayerColor = m_CitizenColors [randColor];
			m_CitizenTanks [m_SpawnsRemaining - 1].SetupAI (m_SpawnPoints);

			AudioSource tempSource = m_CitizenTanks [m_SpawnsRemaining - 1].m_Instance.GetComponent<AudioSource> ();
			tempSource.time = m_AudioTime;

			m_SpawnsRemaining -= 1;
			m_SpawnTimer = 0f;
		}
	}


	protected Transform GetSpawnPoint() {
		List<Transform> tempSpawnPoints = new List<Transform>();

		for (int i = 0; i < m_SpawnPoints.Count; i++) {
			Vector3 checkLocation = new Vector3 (m_SpawnPoints [i].transform.position.x, m_SpawnPoints [i].transform.position.y + 1.3f, m_SpawnPoints [i].transform.position.z);

			if (!Physics.CheckBox(checkLocation, m_CheckBoxSize, m_SpawnPoints[i].transform.rotation)) {
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

		m_ViveLaFrance.PlayDelayed (1f);

		yield return m_EndWait;
	}


	protected bool RoundOver() {
		if (!m_KingTank.m_Instance.activeSelf) {
			return true;
		} else if (m_SpawnsRemaining > 0) {
			return false;
		} else {

			int numTanksLeft = 0;

			for (int i = 0; i < m_CitizenTanks.Length; i++) {
				if (m_CitizenTanks [i].m_Instance.activeSelf)
					numTanksLeft++;
			}

			return numTanksLeft < 1;
		}
	}


	protected string EndMessage()
	{
		string message;

		if (m_KingTank.m_Instance.activeSelf) {
			message = "YOU DEFEATED THE REVOLUTIONARIES!";
		} else {
			message = "THE KING IS DEAD!";
		}

		return message;
	}
		
	protected void EnableTankControl()
	{
		m_KingTank.EnableControl();
	}


	protected void DisableTankControl()
	{
		m_KingTank.DisableControl();
	}
}