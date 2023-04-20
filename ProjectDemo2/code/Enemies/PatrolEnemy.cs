using UnityEngine;
using System.Collections;

public class PatrolEnemy : MonoBehaviour {

    public Track Track;

    [Range(10f, 180.0f)]
    public float MoveSpeed = 20;
    public bool IsNoReverse;
    private float trajectory = 0.0f;
    public bool IsDestroyAtEnd = false;
    public bool IsIgnoreY = false;
    private float moveDirection = 1;


	void FixedUpdate () {
        MoveEnemy();//只管动
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
