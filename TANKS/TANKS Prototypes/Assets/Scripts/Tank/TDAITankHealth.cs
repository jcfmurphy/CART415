using UnityEngine;
using UnityEngine.UI;

public class TDAITankHealth : TankHealth
{
  
	protected override void OnDeath()
    {
		base.OnDeath ();

		RemoveTank ();
	}


	protected void RemoveTank() {
		AITankCounter tankCounter = GameObject.Find ("MessageCanvas").GetComponent<AITankCounter> ();
		tankCounter.RemoveTank ();
	}
}