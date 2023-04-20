using UnityEngine;
using System.Collections;

public class ParticleAutoDestroy : MonoBehaviour {

    private ParticleSystem ps;
    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                //粒子效果结束后，把粒子效果实例给没掉
                Destroy(this.gameObject);
            }
        }
    }
}
