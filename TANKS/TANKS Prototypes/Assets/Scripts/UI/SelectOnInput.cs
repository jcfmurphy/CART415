using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

	public EventSystem m_EventSystem;
	public GameObject m_SelectedObject;

	private bool m_ButtonSelected = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw ("Vertical1") != 0 && m_ButtonSelected == false) {
			m_EventSystem.SetSelectedGameObject (m_SelectedObject);
			m_ButtonSelected = true;
		}
	}

	private void OnDisable() {
		m_ButtonSelected = false;
	}
}
