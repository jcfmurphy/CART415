using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpedBool : MonoBehaviour {

	private bool m_Warped;

	// Use this for initialization
	void Start () {
		m_Warped = false;
	}

	void Update() {
		if (m_Warped) {
			m_Warped = false;
		}
	}
	
	public void SetWarped (bool hasWarped) {
		m_Warped = hasWarped;
	}

	public bool GetWarped() {
		return m_Warped;
	}
		
}
