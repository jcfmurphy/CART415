﻿using UnityEngine;

public class DarkShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;       
	public Light m_ShotLight;
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              
	private DarkGameManager m_GameManager;

    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);

		GameObject GameManagerObject = GameObject.Find ("GameManager");
		m_GameManager = GameManagerObject.GetComponent<DarkGameManager> ();
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
		Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

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

		Vector3 posOffset = transform.rotation * Vector3.back * 2.0f;

		Vector3 lightPosition = new Vector3(transform.position.x, 1.0f, transform.position.z);

		lightPosition += posOffset;

		m_GameManager.DropLight (lightPosition, transform.rotation);

		Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);

		Destroy (gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
		Vector3 explosionToTarget = targetPosition - transform.position;

		float explosionDistance = explosionToTarget.magnitude;

		float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

		float damage = relativeDistance * m_MaxDamage;

		damage = Mathf.Max (0f, damage);

		return damage;
    }
}