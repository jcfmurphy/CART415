using UnityEngine;

public class SoundCameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                              
    [HideInInspector] public Transform m_Target;                              
    private Vector3 m_MoveVelocity;                        

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
		transform.position = Vector3.SmoothDamp(transform.position, m_Target.position, ref m_MoveVelocity, m_DampTime);
    }
		
}