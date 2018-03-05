using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Follow")]
public class FollowAction : Action {

	[HideInInspector] public Transform followTarget;

	public override void Act (StateController controller)
	{
		Follow (controller);
	}

	private void Follow (StateController controller) {

		GameObject followObject = GameObject.Find ("Sound King Tank(Clone)");

		if (followObject != null) {
			followTarget = followObject.transform;

			controller.navMeshAgent.destination = followTarget.position;
			controller.navMeshAgent.isStopped = false;
		}
	}
}
