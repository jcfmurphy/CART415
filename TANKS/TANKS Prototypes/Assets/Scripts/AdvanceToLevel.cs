using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvanceToLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("LoadTheLevel", 0.1f);
	}

	void LoadTheLevel () {
		SceneManager.LoadScene (1);
	}
}
