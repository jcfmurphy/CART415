using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/TDFollow")]
public class TDFollowAction : Action {

	[HideInInspector] public Transform followTarget;

	private float m_TargetTimer = 3f;
	private float m_TargetTimerLimit = 2f;
	private Transform m_targetTransform;

	public override void Act (StateController controller)
	{
		Follow (controller);
	}

	private void Follow (StateController controller) {

		if (m_TargetTimer >= m_TargetTimerLimit) {
			SetTarget (controller);
		} else {
			m_TargetTimer += Time.deltaTime;
		}

		controller.navMeshAgent.destination = m_targetTransform.position;
			
	}

	private void SetTarget(StateController controller) {

		CubeSide targetSide = Camera.main.GetComponent<TDCameraControl> ().m_CubeSide;

		if (controller.m_CubeSide == targetSide) {
			GameObject followObject = GameObject.Find ("3DTank(Clone)");

			if (followObject != null) {
				m_targetTransform = followObject.transform;
			}
		} else {
			GameObject[] portals = GameObject.FindGameObjectsWithTag ("Portal");
			GameObject closestPortal = null;
			float closestDistance = 100f;

			foreach (GameObject portal in portals) {
				if (portal.GetComponent<Portal> ().m_LinkedPortal.m_CubeSide == targetSide) {
					
					float portalDistance = Vector3.Distance(controller.gameObject.transform.position, portal.transform.position);

					if (portalDistance < closestDistance) {
						closestDistance = portalDistance;
						closestPortal = portal;
					}
				}
			}

			if (closestPortal != null) {
				m_targetTransform = closestPortal.transform;
			}
		}
	}
}
