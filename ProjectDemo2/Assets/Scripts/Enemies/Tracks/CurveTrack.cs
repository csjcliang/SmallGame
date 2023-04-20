using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Represents a curved bezier path in the editor
/// 贝塞尔曲线Bézier curve算法
/// 这里是三阶贝塞尔曲线，两个固定点，两个控制点
/// 通过插值来得到曲线
/// </summary>
public class CurveTrack : Track {

    public Vector3 StartPoint;//p0
    public Vector3 StartControlPoint; //p1
    public Vector3 EndPoint; //p3
    public Vector3 EndControlPoint; //p2

    public int InterpolationSteps = 20;//线段细分

    //这里为了展示曲线而取点函数
    public override void CalculateStartPoints()
    {
        ClearPoints();

        float step = 1.0f / (InterpolationSteps - 1); //if interstep = 5 [ 1 / 5 = 0.2, but steps should be 0.25 to be correct!!!]

        for (int i = 0; i < InterpolationSteps; ++i)
        {
            var t = i * step;
            var point = GetPointOnTrack(t);
            AddPoint(point);
        }
    }

    public void ResetOnTransform()
    {
        //return; //TODO fix
        StartPoint = this.transform.position;
        EndPoint = this.transform.position + new Vector3(15.0f, 0, 0);

        StartControlPoint = StartPoint + new Vector3(7.0f, 0.0f, -7.0f);
        EndControlPoint = EndPoint - new Vector3(7.0f, 0.0f, -7.0f);

        CalculateStartPoints();

    }

    //这里是实际运动要用的取点函数
    public override Vector3 GetPointOnTrack(float t)
    {
        //see wikipedia
        //贝塞尔曲线的插值算法，得到中间点
        var r2 = Mathf.Pow((1 - t), 3) * StartPoint;
        r2 += 3 * Mathf.Pow((1 - t), 2) * t * StartControlPoint;
        r2 += 3 * (1 - t) * Mathf.Pow(t, 2) * EndControlPoint;
        r2 += Mathf.Pow(t, 3) * EndPoint;

        return r2;
    }

    //这个是可视化画出来轨迹
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if (Count() == 0)
            CalculateStartPoints();

        DrawPointLines();
        DrawVertexPoints();

        Gizmos.color = Color.gray;
        //Line to control points
        Gizmos.DrawLine(StartPoint, StartControlPoint);
        Gizmos.DrawLine(EndPoint, EndControlPoint);

        float wireSmallRadius = 0.1f;

        //Control points
        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(StartControlPoint, wireSmallRadius);
        Gizmos.DrawWireSphere(EndControlPoint, wireSmallRadius);
    }
}
