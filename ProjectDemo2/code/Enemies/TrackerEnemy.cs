using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackerEnemy : MonoBehaviour {

    public float RotateSpeed = 80.0f;//这个东西会朝着玩家（保护屁股），转速不要太快才能被玩家绕圈圈

    public GameObject TrackTarget;//躲着玩家
    private static List<TrackerEnemy> TrackerEnemies = new List<TrackerEnemy>();

    public Vector3 Velocity;

    // Use this for initialization
    void Start () {
        TrackerEnemies.Add(this);
    }
	
	// Update is called once per frame
	void Update () {

        if (TrackTarget == null)
            return; 

        float x = 0f;
        float y = 0f;

        //direction
        var thisPos = Camera.main.WorldToViewportPoint(this.transform.position); //0-1
        var playerScreen = Camera.main.WorldToViewportPoint(TrackTarget.transform.position); //0-1

        //point from player to target
        Vector3 dir = playerScreen - thisPos;
        x = dir.x;
        y = dir.y;
        //这个代码就这里主要负责转弯，给一个朝向的方向向量
        RotateTowards(new Vector3(x, 0, y));
    }


    void RotateTowards(Vector3 direction)
    {
        //和玩家的那个转弯一样
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, RotateSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
