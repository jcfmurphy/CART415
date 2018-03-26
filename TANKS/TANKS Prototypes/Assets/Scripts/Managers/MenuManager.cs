using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void GoToLevel(int sceneNum) {
		SceneManager.LoadScene(sceneNum);
	}

	public void QuitGame() {
		Application.Quit ();
	}
}
