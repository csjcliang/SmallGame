using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public enum BulletSpawnMode
{
    //子弹的两种模式：朝着玩家方向发射，沿着固定轨迹走
    TowardsPlayer, OnTrack
}

public enum TrackMoveMode
{
    //固定轨迹移动的三种方式：轨迹不动，轨迹也一定范围移动（终点动），轨迹转圈圈（终点转圈圈）
    NoMove, Range, Circle
}

/// <summary>
/// 发射的子弹要么朝着玩家要么沿着固定轨迹
/// </summary>
public class BulletDistributor : MonoBehaviour {

    // Use this for initialization
    public Transform Player;//绑定玩家
    public BulletSpawnMode SpawnMode;//给定发射模式
    
    public float ActivationRadius = 60f;//作用范围半径？
    public float StartDelay = 0f;//延迟?
    public bool IsIgnoreActivationRadius = false;//无视半径？

    [Header("Bullet properties")]
    public GameObject OrangeBulletPrefab;//绑定两种子弹
    public GameObject PurpleBulletPrefab;

    [Range(0.02f, 1.0f)]
    public float FireCooldown = 0.5f;//绑定冷却时间

    [Range(10f, 70f)]
    public float BulletSpeed = 2f;//绑定子弹速度

    public bool IsBulletsIgnoreWalls = false;//无视墙

    [Header("Bullet follow track properties")]
    public LineTrack TrackToSpawnOn;//绑定直线轨迹
    public bool IsTrackHiddenInEditor = true;//隐藏轨迹
    public TrackMoveMode MoveMode = TrackMoveMode.NoMove;//绑定轨迹模式
    [Range(0.0f, 30.0f)]
    public float TrackMoveRangeHorizontal = 1.0f; //轨迹终点的移动时横向范围
    [Range(0.0f, 30.0f)]
    public float TrackMoveRangeVertical = 1.0f; //轨迹终点的移动时纵向范围
    [Range(5.0f, 60.0f)]
    public float TrackCircleRadius = 30.0f; //轨迹终点的移动时半径范围
    [Range(0.0f, 3.0f)]
    public float TrackMoveSpeed = 3.0f;//轨迹终点的移动速度

    public float TrackStartT = 0.0f;//轨迹终点的起始点

    [Header("1 wave bullet properties")]
    public float TotalCount;//设置发射的两种子弹的比例
    public float NumberOfOrangeBullets;

    //privates
    private float coolDownTimer = 0;
    private float index = 0;
    private Vector3 sineTrackOriginalPos;
    private float circleTrackT = 0.0f;
    private float onTrackBulletSpeedIncrement = 3f;
    private float trackSine = 0.0f;

    void Start () {

        if (SpawnMode == BulletSpawnMode.OnTrack)
            //如果按轨迹走又不给轨迹就报个错
            Assert.IsNotNull<LineTrack>(TrackToSpawnOn, "BulletDistributor > Bullets trying to spawn on a track but no track is assigned.");
            
        //cache of the original second point, which will be modified later on
        if (SpawnMode == BulletSpawnMode.OnTrack)
        {
            //从轨迹里面拿到目标位置，或者转圈圈的话拿转圈圈起始点
            sineTrackOriginalPos = TrackToSpawnOn.ShapePoints[1];
            circleTrackT = TrackStartT;
        }
    }


    void OnDrawGizmos()
    {
        //用来展示那条线的
        if (this.transform.hasChanged && TrackToSpawnOn != null)
        {
            TrackToSpawnOn.ShapePoints[0] = transform.position;
            TrackToSpawnOn.IsHiddenInEditor = IsTrackHiddenInEditor;
        }
    }

    // Update is called once per frame
    void Update () {

        if (Player == null)
            return;

        if (StartDelay > 0)
        {
            StartDelay -= Time.deltaTime;
            return;
        }

        //超过作用范围就不起作用
        bool isTooFar = Vector3.Distance(Player.transform.position, this.transform.position) >= ActivationRadius;
        if (isTooFar && !IsIgnoreActivationRadius)
            return;

        //冷却时间到就自己发一个子弹
        coolDownTimer -= Time.deltaTime;
        if(coolDownTimer <= 0)
        {
            ShootBullet();
            coolDownTimer = 1 - FireCooldown;
            //通过index来控制发的球球
            index++;

            if (index >= TotalCount)
                index = 0;
        }

        if (SpawnMode == BulletSpawnMode.OnTrack)
        {
            TrackToSpawnOn.ShapePoints[0] = transform.position;//轨迹起点是自己

            if (MoveMode == TrackMoveMode.Range)
            {
                //变化轨迹终点（不过没想懂为什么要sin）
                float valSine = Mathf.Sin(trackSine);

                TrackToSpawnOn.ShapePoints[1] = new Vector3(sineTrackOriginalPos.x + valSine * TrackMoveRangeHorizontal, sineTrackOriginalPos.y, sineTrackOriginalPos.z + valSine * TrackMoveRangeVertical);
                trackSine += Time.deltaTime * TrackMoveSpeed;
            }

            if (MoveMode == TrackMoveMode.Circle)
            {
                //如果是圆就围着自己绕圈圈
                Vector3 point = CalculateCirclePoint(circleTrackT, TrackCircleRadius, transform.position.x, transform.position.z);
                TrackToSpawnOn.ShapePoints[1] = point;

                circleTrackT += Time.deltaTime * TrackMoveSpeed;
            }
            //end position
        }
    }

    public void ShootBullet()
    {
        GameObject gO;
        //两种子弹
        if(index < NumberOfOrangeBullets)
             gO = Utils.InstantiateSafe(OrangeBulletPrefab, transform.position);
        else
            gO = Utils.InstantiateSafe(PurpleBulletPrefab, transform.position);

        //如果是跟着轨迹走
        if (SpawnMode == BulletSpawnMode.OnTrack)
        {
            //on bullet object - Patroller（PatrolEnemy->运动轨迹管理组件）
            PatrolEnemy e = gO.AddComponent<PatrolEnemy>();
            e.Track = TrackToSpawnOn;//添加轨迹
            e.SetTrajectory(0.0f);//Trajectory是衡量在轨迹上的进度
            e.MoveSpeed = BulletSpeed * onTrackBulletSpeedIncrement;//子弹的移动速度（加上轨道修正？）
            e.IsNoReverse = true;
            e.IsDestroyAtEnd = true;

            Bullet b = gO.GetComponent<Bullet>();
            //b.Speed = BulletSpeed; //doesn't do anything because bullets aren't moving on their own.
            b.IsImmortal = true;//没有持续时间
        }

        if(SpawnMode == BulletSpawnMode.TowardsPlayer)
        {
            //朝着玩家就简单多了直接朝着终点move
            Vector3 playerDirection = Player.position - this.transform.position;
            Bullet b = gO.GetComponent<Bullet>();
            b.Speed = BulletSpeed;

            if (IsBulletsIgnoreWalls)
                b.IsIgnoreWall = true;
            //move towards player
            b.Move(playerDirection.normalized);
         }

        //parent, can be optimized 找群，方便管理
        string name = this.gameObject.name + " bullet list";
        GameObject groupObject = GameObject.Find(name);

        if (groupObject == null)
            groupObject = new GameObject(name);

        gO.transform.parent = groupObject.transform;

    }

    public Vector3 CalculateCirclePoint(float angleInRadians, float rad, float xCenter, float yCenter)
    {
        float x, y;
        x = Mathf.Cos(angleInRadians) * rad + xCenter;
        y = Mathf.Sin(angleInRadians) * rad + yCenter;
        return new Vector3(x, 0, y);
    }
}
