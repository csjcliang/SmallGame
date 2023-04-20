using UnityEngine;
using System.Collections;

public class PlayerHealthIndicator : MonoBehaviour {

    // Use this for initialization
    public Color HealthTwoLeft = new Color(0.6f, 0.3f, 0.3f);//剩两点血时的颜色
    public Color HealthOneLeft = new Color(0.8f, 0.0f, 0.0f);//剩一点血时的颜色

    public Renderer RenderComponent;//拉过来设置的一个mesh render，绑定的是player的实体图形
	
	void Update () {

        int h = GetComponent<DestroyOnTriggerEnter>().Health;//从别的地方获取血量

        if(h == 2)
            RenderComponent.material.color = HealthTwoLeft;

        if (h == 1)
            RenderComponent.material.color = HealthOneLeft;

    }
}
