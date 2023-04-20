using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class TestCam : MonoBehaviour {

    public GameObject Target;
    public Vector3 Offset;
    public float LerpFactor = 0.25f;

    Vector3 targetPos;
    private float interpVelocity;
    private Vector3 originalOffset;
    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;//初始化为自己的位置
        originalOffset = Offset;//初始化原来的偏移量为0
        //如果没有绑定目标就报错
       Assert.IsNotNull<GameObject>(Target, "TestCam > No tracking target assigned.");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Target)
        {
            Vector3 targetDirection = Target.transform.position - transform.position;//目标方向向量

            interpVelocity = targetDirection.magnitude * 5f;//得到的是长度，即偏移量大小
            //targetDirection单位化后即方向，然后乘上偏移量大小
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            //Lerp 线性插值，实际的落点在当前位置与目标位置之间（考虑摄像机缩放的Offset），实现平滑视角移动
            transform.position = Vector3.Lerp(transform.position, targetPos + Offset, LerpFactor);

        }
    }

    public void SetZoomOffset(float value)//缩放
    {
        Offset.y = value;
    }

    public void ResetZoomOffset()
    {
        Offset = originalOffset;
    }
}
