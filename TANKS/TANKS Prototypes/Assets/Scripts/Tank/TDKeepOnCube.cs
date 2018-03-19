using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDKeepOnCube : MonoBehaviour {

	private Transform m_Transform;

	public void Start()
	{
		m_Transform = GetComponent<Transform> ();
	}

	void FixedUpdate() {
		KeepOnCube ();
	}

	public void KeepOnCube() {

		Vector3 tankPosition = m_Transform.position;

		if (tankPosition.x > 10.1f || tankPosition.x < -10.1f || tankPosition.y > 10.1f || tankPosition.y < -10.1f || tankPosition.z > 10.1f || tankPosition.z < -10.1f) {
			float newX = Mathf.Clamp (tankPosition.x, -10f, 10f);
			float newY = Mathf.Clamp (tankPosition.y, -10f, 10f);
			float newZ = Mathf.Clamp (tankPosition.z, -10f, 10f);

			Vector3 newPos = new Vector3 (newX, newY, newZ);

			m_Transform.position = newPos;
		}
	}
}
