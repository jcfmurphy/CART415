using UnityEngine;
using UnityEngine.UI;

public class TDPlayerTankHealth : TankHealth
{

    public override void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
		m_CurrentHealth -= 0.5f * amount;

		SetHealthUI ();

		if (m_CurrentHealth <= 0f && !m_Dead) {
			OnDeath ();
		}
    }

}