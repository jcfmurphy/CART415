using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITankCounter : MonoBehaviour {

	public int m_TanksLeft = 10;

	public void RemoveTank() {
		GameObject tankImage = gameObject.transform.Find ("TankCounter" + m_TanksLeft).gameObject;
		tankImage.SetActive (false);

		m_TanksLeft--;
	}
}
