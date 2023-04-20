using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

//一条直线段
public class LineTrack : Track {

    public float pointOnLine = 0.5f;

    public override void CalculateStartPoints()
    {
        ClearPoints();//基类方法清空点
        //加的是两个默认点一条线段
        AddPoint(transform.position - new Vector3(15, 0, 0));
        AddPoint(transform.position + new Vector3(15, 0, 0));

    }
    //这里为了展示曲线而取点函数
    public void CalculateStartPoints(int xOffset, int zOffset)
    {
        ClearPoints();//基类方法清空点
        //加的是两个自定义点一条线段
        AddPoint(transform.position - new Vector3(xOffset, 0, zOffset));
        AddPoint(transform.position + new Vector3(xOffset, 0, zOffset));

    }

    public void CenterTransform()
    {
        //拿0.5即拿线段中点
        transform.position = GetPointOnTrack(0.5f);
    }
    //这里是实际运动要用的取点函数
    public override Vector3 GetPointOnTrack(float n)
    {
        //通过插值，从起点到终点，得到中间部分的点的坐标
        Vector3 result = Vector3.Lerp(GetPointAtIndex(0), GetPointAtIndex(Count() - 1), n);  //ins't great when working with multiple points
        return result;
    }

    //Adds a new point on the collection based on the previous point
    public void AddNewLine()
    {
        //如果要加新线段，就是取一个末端点的坐标值，然后加坐标值(只有默认值）加进去
        Vector3 point = GetPointAtIndex(Count() - 1);

        point += new Vector3(3, 0, 3);
        AddPoint(point);
    }

    public void RemoveLast()
    {
        RemovePoint(Count() - 1);
    }
}
