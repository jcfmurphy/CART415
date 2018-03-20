using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTank : MonoBehaviour {

	private float m_Transparency = 1f;
	private float m_FadeSpeed = 0.05f;
	private SpriteRenderer m_Sprite;

	void Start() {
		m_Sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_Transparency -= m_FadeSpeed;

		if (m_Transparency <= 0f) {
			Destroy (gameObject.transform.parent.gameObject);
		}

		Color tempColor = new Color (1f, 1f, 1f, m_Transparency);

		m_Sprite.color = tempColor;
	}
}
