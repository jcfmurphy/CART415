using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/TDAttack")]
public class TDAttackAction : Action {

	public override void Act (StateController controller)
	{
		Attack (controller);	
	}

	private void Attack(StateController controller)
	{
		RaycastHit hit;

		if (Physics.SphereCast (controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.attackRange)
			&& hit.collider.gameObject.name == "3DTank(Clone)") {
			controller.tankShooting.Fire(controller.enemyStats.attackRate);
		}
	}
}
