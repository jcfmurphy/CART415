using UnityEngine;

public class TDShellExplosion : ShellExplosion
{
    
	public LayerMask m_ShellPortalMask;
	public Transform m_SpriteTransform;

	private float m_MovementSpeed = 20f;
	private Rigidbody m_RigidBody;
	private Transform m_Transform;

	protected override void Start()
	{
		Destroy(gameObject, 1f);
		m_RigidBody = GetComponent<Rigidbody>();
		m_Transform = GetComponent<Transform> ();
	}


	protected  void FixedUpdate() {
		Vector3 movement = m_Transform.forward * m_MovementSpeed * Time.deltaTime;

		m_RigidBody.MovePosition (m_RigidBody.position + movement);
	}


	protected override void OnTriggerEnter(Collider other)
    {
		if (m_TankMask.value == 1 << other.gameObject.layer) {
			// Find all the tanks in an area around the shell and damage them.
			Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);

			for (int i = 0; i < colliders.Length; i++) {
				Rigidbody targetRigidbody = colliders [i].GetComponent<Rigidbody> ();

				if (!targetRigidbody) {
					continue;
				}

				targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

				TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

				if (!targetHealth) {
					continue;
				}

				float damage = CalculateDamage (targetRigidbody.position);

				targetHealth.TakeDamage (damage);
			}

			m_ExplosionParticles.transform.parent = null;

			m_ExplosionParticles.Play ();

			m_ExplosionAudio.Play ();

			Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);

			Destroy (gameObject);

		} else if (m_ShellPortalMask.value == 1 << other.gameObject.layer) {

			Vector3 rotationAxis = other.gameObject.transform.right;

			transform.RotateAround (m_SpriteTransform.position, rotationAxis, 90f);

		}

    }
}