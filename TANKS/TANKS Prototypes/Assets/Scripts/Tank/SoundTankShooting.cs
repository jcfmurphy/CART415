using UnityEngine;
using UnityEngine.UI;

public class SoundTankShooting : TankShooting
{
  
	public GameObject m_CrosshairCanvas;

	private float m_ImpactTime = 1.0398f;
	private float m_Scale = 3.0f;
	private float m_CanvasHeight = -1.6f;

	protected override void Update()
	{
		if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired) {
			m_CurrentLaunchForce = m_MaxLaunchForce;
			Fire ();
		} else if (Input.GetButtonDown (m_FireButton)) {
			m_Fired = false;
			m_CurrentLaunchForce = m_MinLaunchForce;

			m_ShootingAudio.clip = m_ChargingClip;
			m_ShootingAudio.Play ();
		} else if (Input.GetButton (m_FireButton) && !m_Fired) {
			m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
		} else if (Input.GetButtonUp (m_FireButton) && !m_Fired) {
			Fire ();
		}

		SetCrosshairs ();
	}


	private void SetCrosshairs() {
		float crosshairDistance = m_CurrentLaunchForce * m_ImpactTime / m_Scale;

		Vector3 crosshairPosition = new Vector3 (0f, m_CanvasHeight, crosshairDistance);

		m_CrosshairCanvas.transform.localPosition = crosshairPosition;
	}

}