using UnityEngine;
using System.Collections;

/// <summary>
/// Handles player movement, defaults to controller input when a controller is connected (configured for a PlayStation 4 controller)
/// </summary>
public class PlayerMove : MonoBehaviour {

    public float MoveSpeed = 10.0f;
    public float RotateSpeed = 80.0f;

    private bool isMoving = false;
    private bool isRotating = false;
    
    public bool UseMouseKeyboard = true;// 手柄支持

    Vector3 Velocity;
    private Vector3 rotateDirection;

    void Start()
    {
        //手柄检测支持
        int joyCount = Input.GetJoystickNames().Length;
        Debug.Log(joyCount + " controller(s) detected.");

        if (joyCount > 0)
            UseMouseKeyboard = false;
    }

    void FixedUpdate()
    {
        LeftStickInput();//键盘控制
        RightStickInput();//没有用的

        RotateOnClick();//点击鼠标他也转

        if(rotateDirection != Vector3.zero)
            //在这里做的旋转
            transform.rotation = Quaternion.LookRotation(rotateDirection);
    }

    // 手柄输入控制（事实上并没有采用）
    void RightStickInput()
    {
        float x = Input.GetAxis("HorizontalRight");
        float y = Input.GetAxis("VerticalRight");

        #region old unused player in direction of mouse cursor code
        /*
        //use mouse instead
        if (UseMouseKeyboard)
        {

            var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition); //0-1
            var playerScreen = Camera.main.WorldToViewportPoint(this.transform.position); //0-1

            //point from player to target
            Vector3 dir = mousePos - playerScreen;
            x = dir.x;
            y = dir.y;

        }
        */
        #endregion

        if (Mathf.Abs(x) + Mathf.Abs(y) > 0.1f)
        {
            Vector3 rot = new Vector3(x, 0, y);
            RotateTowards(rot);
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }
        

    }

    void RotateOnClick()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition); //0-1
            var playerScreen = Camera.main.WorldToViewportPoint(this.transform.position); //0-1
            //获取鼠标点击的坐标
            //获取自己的坐标
            //坐标差得方向向量
            //point from player to target
            Vector3 dir = mousePos - playerScreen;

            Vector3 rot = new Vector3(dir.x, 0, dir.y);
            RotateTowards(rot);
        }
    }

    // 键盘输入控制
    void LeftStickInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        //获取到的是[0,1]的值（似乎是自带加速度，按住一个键的时候，值从0开始递增直到1）
        if (x == 0 && y == 0)
        {
            //如果检测到两轴都是没有输入
            isMoving = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;//速度的设置作用于Rigidbody，直接设为0，实现不按键时停下来
            return;
        }

        if(!isMoving)
        {
            //rotate once
            //RotateTowards(new Vector3(x, 0, y));
            isMoving = true;
        }

        //rotation
        
        Vector3 rot = new Vector3(x, 0, y);
        //(x, 0, y)这个向量本身是指向要去的方向，因为输入上自带加速度类似的机制，因此转向的时候可以有缓慢转向的缓冲
        //例如：本来是向上走的（1，0，0），这时候按下另一个左右的方向键
        //（1，0，0.1）->（1，0，0.2）->（1，0，0.5）->（1，0，1）
        //这样子就有转向上的过渡了
        RotateTowards(rot);
        Velocity = new Vector3(x * MoveSpeed * Time.deltaTime * 100, 0, y * MoveSpeed * Time.deltaTime * 100);

        //consider doing this in FixedUpdate
        GetComponent<Rigidbody>().velocity = Velocity;
    }

    void RotateTowards(Vector3 direction)
    {
        if (rotateDirection == direction)//不变就退出了
            return;
        //Time.deltaTime是为了适配各个电脑配置的不同的帧数
        /*Vector3.RotateTowards四个参数
           current	要改变的向量（这里是物体的朝向）
           target	目标方向
           maxRadiansDelta	The maximum angle in radians allowed for this rotation.（最大转动幅度角）
           maxMagnitudeDelta	The maximum allowed change in vector magnitude for this rotation.（？）*/
        rotateDirection = Vector3.RotateTowards(transform.forward, direction, RotateSpeed * Time.deltaTime, 0.0f);
        //transform似乎可以直接用，直接关联至这个对应的实体
        //transform.forward默认绑定z轴指向 （因此游戏里是x-z轴平面）
    }
}
