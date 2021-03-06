using UnityEngine;
using UnityEngine.UI;

public class SoundTankShooting : TankShooting
{
  
	public GameObject m_CrosshairCanvas;
	public LineRenderer m_Trajectory;

	private float m_ImpactTime = 1.0197f;
	private float m_Scale = 3.0f;
	private float m_CanvasHeight = -1.6f;
	private float m_Gravity = 9.81f;


	protected override void Start()
	{
		base.Start ();
		DeactivateTargeting ();
	}


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

			ActivateTargeting ();
			SetCrosshairs ();
			SetTrajectory ();
		} else if (Input.GetButton (m_FireButton) && !m_Fired) {
			m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
			SetCrosshairs ();
			SetTrajectory ();
		} else if (Input.GetButtonUp (m_FireButton) && !m_Fired) {
			Fire ();
		}

		if (!m_Fired && !Input.GetKey(KeyCode.Space)) {
			DeactivateTargeting ();
		}
	}


	public override void Fire() {
		base.Fire ();
		DeactivateTargeting ();
	}


	private void SetCrosshairs() {
		float crosshairDistance = m_CurrentLaunchForce * m_ImpactTime / m_Scale;

		Vector3 crosshairPosition = new Vector3 (0f, m_CanvasHeight, crosshairDistance);

		m_CrosshairCanvas.transform.localPosition = crosshairPosition;
	}
		
	private void SetTrajectory() {
		for (int i = 0; i < m_Trajectory.positionCount; i++) {
			float timePos = ((float)i / m_Trajectory.positionCount) * m_ImpactTime; 
			float yPos = (-0.5f * m_Gravity * timePos * timePos) / m_Scale;
			float zPos = timePos * m_CurrentLaunchForce / m_Scale;

			Vector3 pos = new Vector3 (0f, yPos, zPos);
			m_Trajectory.SetPosition (i, pos);
		}
	}


	private void ActivateTargeting() {
		m_CrosshairCanvas.SetActive (true);
		m_Trajectory.enabled = true;
	}

	private void DeactivateTargeting() {
		m_CrosshairCanvas.SetActive (false);
		m_Trajectory.enabled = false;
	}
}