using UnityEngine;

public class SoundTankMovement : TankMovement
{
	public ProcSoundTank ProcSound;


	protected override void Start()
	{
		m_MovementAxisName = "Vertical" + m_PlayerNumber;
		m_TurnAxisName = "Horizontal" + m_PlayerNumber;
	}


	protected override void OnDisable ()
	{
		base.OnDisable ();
		ProcSound.Reset ();
	}
		

	protected override void Update()
	{
		// Store the player's input and make sure the audio for the engine is playing.

		m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

		ProcSound.engineOn = CheckEngine ();

		//update dust trail particle systems
		if (Mathf.Abs (m_MovementInputValue) > 0.1f || Mathf.Abs (m_TurnInputValue) > 0.1f) {
			UpdateParticleSystem (m_LeftDustTrail, true);
			UpdateParticleSystem (m_RightDustTrail, true);
		} else {
			UpdateParticleSystem (m_LeftDustTrail, false);
			UpdateParticleSystem (m_RightDustTrail, false);
		}

	}

	protected override void EngineAudio() {

	}

	protected bool CheckEngine()
	{
		// Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

		if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f) {
			return false;
		} else {
			return true;
		}
	}


}