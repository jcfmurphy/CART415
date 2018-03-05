using UnityEngine;

public class SoundShellExplosion : ShellExplosion
{

	protected override void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
		m_MaxDamage = 300f;
    }


}