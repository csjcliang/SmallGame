using UnityEngine;
using System.Collections;


public enum EnemymoveMode
{
    //更新：敌人的移动方式：固定轨迹，不动，朝着玩家
    Nomove, OnTrack, ToPlayer
}

public class PatrolEnemy : MonoBehaviour {

    public EnemymoveMode MoveMode = EnemymoveMode.OnTrack;
    public Track Track;
    public Transform Player;//绑定玩家
    public bool move = true;
    [Range(1f, 180.0f)]
    public float MoveSpeed = 20;
    public float rotateSpeed = 20;
    public bool IsNoReverse;
    private float trajectory = 0.0f;
    public bool IsDestroyAtEnd = false;
    public bool IsIgnoreY = false;
    private float moveDirection = 1;


	void FixedUpdate () {
        if (MoveMode == EnemymoveMode.OnTrack) { 
        MoveEnemy();//只管动
	}
        if(MoveMode == EnemymoveMode.ToPlayer)
        {

            if(Player!=null){
            this.transform.rotation = Quaternion.Slerp(
                 this.transform.rotation,
                 Quaternion.LookRotation(Player.position - this.transform.position),
                 rotateSpeed * Time.deltaTime
            );

            this.transform.position += this.transform.forward * MoveSpeed * Time.deltaTime;


            //Vector3 playerDirection = Player.position - this.transform.position;
            //transform.Translate(playerDirection.normalized * MoveSpeed * Time.deltaTime);
            }
        }
    }

    void MoveEnemy()
    {

        //feeds in a value from 0 -> 1 and back to GetPointOnTrack
        //trajectory衡量进度，如果到底了看看掉不掉头
        if (!IsNoReverse)
       { 
            if (trajectory <= 0.01f || trajectory >= 0.99f)
            {
                trajectory = Mathf.Clamp(trajectory, 0.01f, 0.99f);
                moveDirection *= -1;
            }
        }

        trajectory += Time.deltaTime * (MoveSpeed / 100) * moveDirection;
        //Track的方法，得到下一阶段的目标点
        Vector3 position = Track.GetPointOnTrack(trajectory);

        if(IsIgnoreY)
            position.y = this.transform.position.y;
        //移过去了
        this.transform.position = position;

        if(trajectory > 1.0f && IsDestroyAtEnd)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetTrajectory(float t)
    {
        trajectory = t;
    }
}
