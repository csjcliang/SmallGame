using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public enum BulletSpawnMode
{
    //子弹的两种模式：朝着玩家方向发射，沿着固定轨迹走
    //更新：新增：向四周扩散发射、追踪玩家
    //弃用OnTrack
    TowardsPlayer, Around,Trace
}

public enum TrackMoveMode
{
    //固定轨迹移动的三种方式：轨迹不动，轨迹也一定范围移动（终点动），轨迹转圈圈（终点转圈圈）
    NoMove, Range, Circle
}

public enum BubblebulletMode
{
    //对于扩散发射的子弹发射,单束里面变色，以及不同束才变色
    SingleChange,CrossChange
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
    public GameObject RedBulletPrefab;

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

    private List<Vector3> ShapePoints = new List<Vector3>();
    [Header("Bullet around Setting")]
    public int Bundlenums = 20;//发射的子弹束数目
    public BubblebulletMode bubblebulletmode = BubblebulletMode.CrossChange;


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

        if (SpawnMode == BulletSpawnMode.Around)
        {
            float x, y;

            //如果是四周发射就初始化这个数组
            float slice = 2 * Mathf.PI / Bundlenums;
            for (float i = 0; i <= Bundlenums; i++)
            {
                float angle = slice * i;

                x = Mathf.Cos(angle) * 3000 + transform.position.x;
                y = Mathf.Sin(angle) * 3000 + transform.position.z;

                ShapePoints.Add(new Vector3(x, 0, y));
            }
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

        //冷却时间到就自己发一次子弹
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

    }

    public void ShootBullet()
    {
        GameObject gO;
        if(SpawnMode == BulletSpawnMode.Trace)
        {
            gO = Utils.InstantiateSafe(RedBulletPrefab, transform.position);
            Bullet b = gO.GetComponent<Bullet>();
            b.Speed = BulletSpeed;
            b.Move(Player);
            string name = this.gameObject.name + " bullet list";
            GameObject groupObject = GameObject.Find(name);

            if (groupObject == null)
                groupObject = new GameObject(name);

            gO.transform.parent = groupObject.transform;
        }
        else if (SpawnMode == BulletSpawnMode.Around)
        {
            int color = 0;


            for (int i = 0; i < ShapePoints.Count; i++)
            {
                if (bubblebulletmode == BubblebulletMode.CrossChange)
                {
                    Debug.Log(color);
                    if (++color < NumberOfOrangeBullets) gO = Utils.InstantiateSafe(OrangeBulletPrefab, transform.position);
                    else  gO = Utils.InstantiateSafe(PurpleBulletPrefab, transform.position);

                }
                else
                {
                    if (index < NumberOfOrangeBullets) gO = Utils.InstantiateSafe(OrangeBulletPrefab, transform.position);
                    else gO = Utils.InstantiateSafe(PurpleBulletPrefab, transform.position);
                }
                Bullet b = gO.GetComponent<Bullet>();
                b.Speed = BulletSpeed;
                Vector3 dDirection = ShapePoints[i] - this.transform.position;
                b.Move(dDirection.normalized);
                string name = this.gameObject.name + " bullet list";
                GameObject groupObject = GameObject.Find(name);
                if (groupObject == null)
                    groupObject = new GameObject(name);
                gO.transform.parent = groupObject.transform;

            }
            
        }
        else { 
        
        //两种子弹
        if(index < NumberOfOrangeBullets)
             gO = Utils.InstantiateSafe(OrangeBulletPrefab, transform.position);
        else
            gO = Utils.InstantiateSafe(PurpleBulletPrefab, transform.position);



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
    }

    public Vector3 CalculateCirclePoint(float angleInRadians, float rad, float xCenter, float yCenter)
    {
        float x, y;
        x = Mathf.Cos(angleInRadians) * rad + xCenter;
        y = Mathf.Sin(angleInRadians) * rad + yCenter;
        return new Vector3(x, 0, y);
    }
}
