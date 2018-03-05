using UnityEngine;
using UnityEngine.UI;

public class SoundTankHealth : TankHealth
{
	private bool m_InContact;
	public Color[] m_FlashColors = new Color[2];
	private Color m_StartColor;
	MeshRenderer[] m_Renderers;

	void Start() {
		m_Renderers = this.gameObject.GetComponentsInChildren<MeshRenderer> ();
		m_StartColor = m_Renderers [0].material.color;
	}

	void OnCollisionEnter(Collision collisionInfo) {
		if (collisionInfo.gameObject.name == "Sound King Tank(Clone)") {
			m_InContact = true;
		}
	}

	void OnCollisionExit(Collision collisionInfo) {
		if (collisionInfo.gameObject.name == "Sound King Tank(Clone)") {
			m_InContact = false;
			for (int i = 0; i < m_Renderers.Length; i++)
			{
				m_Renderers[i].material.color = m_StartColor;
			}
		}
	}

	public void Update() {
		if (m_InContact) {
			float damage = Time.deltaTime * 100;
			TakeDamage (damage);

			Color tempColor = m_FlashColors[Random.Range(0, m_FlashColors.Length)];
			for (int i = 0; i < m_Renderers.Length; i++)
			{
				m_Renderers[i].material.color = tempColor;
			}
		}
	}
}