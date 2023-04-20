using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    public GameObject BulletPrefab;//绑定了玩家发射的子弹
    public Transform SpawnPosition;//玩家发射子弹的位置点
    public float FireCooldown = 1;//射击冷却时间

    private float coolDownTimer = 0;

	void Update () {

        coolDownTimer -= Time.deltaTime;//冷却时间的实现

        float rT = Input.GetAxis("RightTrigger");//手柄的获取，实际上没用

        if (Input.GetMouseButton(0))//还是从鼠标那里拿的输入
            rT = 1;
        
        if (rT > 0.95f && coolDownTimer <= 0)//射击&冷却时间小于0
        {
            //Utils类在外面的_Scripts目录下的Utils.cs文件定义
            //但是为什么这里可以没有任何声明可以直接调用呀（不会C#）
            GameObject gO = Utils.InstantiateSafe(BulletPrefab, SpawnPosition.position);//创建的是整个模型
            Bullet b = gO.GetComponent<Bullet>();//拿其中的一个组件

            //parent, can be optimized
            string name = this.gameObject.name + " bullets";//定义名字（？）
            GameObject groupObject = GameObject.Find(name);//找群（来自这个玩家发射的所有子弹？）

            if (groupObject == null)
                groupObject = new GameObject(name);

            gO.transform.parent = groupObject.transform;//定义了父亲（）

            b.Move(this.transform.forward);//这边是子弹类的方法了，传入的是一个Vector3，代表方向向量，子弹就沿着这个方向发射

            //move forward
            coolDownTimer = FireCooldown;//设计冷却

            //shoot sound
            SoundManager.GetInstance().PlayShootSound();//音效（有了接口但是并没有添加音效我们可以自己加）
        }

	}
}
