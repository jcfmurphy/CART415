using UnityEngine;
using UnityEngine.UI;

public class TDTankShooting : TankShooting
{
 
	protected override void Update() {
		if (Input.GetButtonDown (m_FireButton)) {
			Fire ();
		}
	}

	public override void Fire ()
	{
		m_Fired = true;

		Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation);

		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();
	}
}