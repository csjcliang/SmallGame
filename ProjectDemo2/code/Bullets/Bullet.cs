using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float Speed = 20;//速度
    public float duration = 5;//存留时间
    public bool IsMoving = false;
    public bool IsIgnoreWall = false;
    public bool IsImmortal = false;//是否有持续时间
    public Vector3 MoveDirection;


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (IsMoving)
        {
            //Transform.Translate，经典移动函数，参数是Vector3的方向向量然后速度和帧数适配
            transform.Translate(MoveDirection * Speed * Time.deltaTime);

        }

        if (IsImmortal)
            return;

        duration -= Time.deltaTime;//持续时间

        if (duration <= 0)
            Destroy(this.gameObject);

    }

    public void Move(Vector3 direction)//调用这个函数赋值方向然后才会动
    {
        MoveDirection = direction;
        IsMoving = true;
    }

    public void Move(Vector3 direction, float speed)//重载，可以把速度也改了
    {
        MoveDirection = direction;
        Speed = speed;
        IsMoving = true;
    }


    void OnTriggerEnter(Collider col)//发生碰撞时
    {
        //如果子弹是玩家发射的。撞到东西之后在子弹当前的地方放一个粒子效果
        if(this.tag == "PlayerBullet" && col.name != "Player")
           GameManager.GetInstance().SpawnParticles(transform.position);

        //撞到墙了，子弹没了
        if (col.gameObject.tag == "Wall" && !IsIgnoreWall)
        {
            Destroy(this.gameObject);
        }

        //如果我是橙色子弹，撞到玩家了，我没了
        if (col.gameObject.tag == "PlayerBullet" && this.tag == "OrangeEnemyBullet")
        {
            Destroy(this.gameObject);
        }

        //如果我是玩家子弹，我撞到了橙色子弹，我没了
        if (col.gameObject.tag == "OrangeEnemyBullet" && this.tag == "PlayerBullet")
        {
            Destroy(this.gameObject);
        }



    }
}
