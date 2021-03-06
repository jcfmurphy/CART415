using UnityEngine;

public class TDTankMovement : TankMovement
{

	private Transform m_Transform;

	protected override void Start()
	{
		base.Start ();

		m_Transform = GetComponent<Transform> ();
	}

	protected override void Update()
	{
		// Store the player's input and make sure the audio for the engine is playing.

		m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

		EngineAudio ();

	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate ();

		m_Rigidbody.AddForce (m_Transform.up * -9.8f);

		m_Rigidbody.angularVelocity = Vector3.zero;
	}


}