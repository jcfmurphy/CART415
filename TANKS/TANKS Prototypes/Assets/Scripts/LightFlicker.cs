using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

	private Light m_Light;
	private Color m_OriginalColor;
	private float m_NoiseScale = 1f;
	private float m_RandomOffset;

	// Use this for initialization
	void Start () {
		m_Light = GetComponent<Light> ();
		m_OriginalColor = m_Light.color;
		m_RandomOffset = Random.Range (0, 1000);
	}
	
	// Update is called once per frame
	void Update () {
		float offset = 0.75f + ((Mathf.PerlinNoise ((Time.time + m_RandomOffset) * m_NoiseScale, 0f) - 0.5f) * 0.75f);

		m_Light.color = m_OriginalColor * offset;
	}
}
