using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;

	protected float m_NextFireTime;
	protected string m_FireButton;         
	protected float m_CurrentLaunchForce;  
	protected float m_ChargeSpeed;         
	protected bool m_Fired;                


	protected void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


	protected void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }
    

	protected void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
		m_AimSlider.value = m_MinLaunchForce;

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

			m_AimSlider.value = m_CurrentLaunchForce;
		} else if (Input.GetButtonUp (m_FireButton) && !m_Fired) {
			Fire ();
		}
    }


	public void Fire()
    {
        // Instantiate and launch the shell.
		m_Fired = true;

		Rigidbody shellInstance = Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

		shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();

		m_CurrentLaunchForce = m_MinLaunchForce;
    }


	public void Fire (float launchForce, float fireRate)
	{
		if (Time.time > m_NextFireTime)
		{
			m_NextFireTime = Time.time + fireRate;
			// Set the fired flag so only Fire is only called once.
			m_Fired = true;

			// Create an instance of the shell and store a reference to it's rigidbody.
			Rigidbody shellInstance =
				Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

			// Set the shell's velocity to the launch force in the fire position's forward direction.
			shellInstance.velocity = launchForce * m_FireTransform.forward;

			// Change the clip to the firing clip and play it.
			m_ShootingAudio.clip = m_FireClip;
			m_ShootingAudio.Play();

			// Reset the launch force.  This is a precaution in case of missing button events.
			m_CurrentLaunchForce = m_MinLaunchForce;
		}
	}
}