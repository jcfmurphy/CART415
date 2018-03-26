using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

	private Light m_Light;
	private Color m_OriginalColor;
	private float m_NoiseScale = 1f;

	// Use this for initialization
	void Start () {
		m_Light = GetComponent<Light> ();
		m_OriginalColor = m_Light.color;
	}
	
	// Update is called once per frame
	void Update () {
		float offset = 1f + ((Mathf.PerlinNoise (Time.time * m_NoiseScale, 0f) - 0.5f) * 0.5f);

		m_Light.color = m_OriginalColor * offset;
	}
}
