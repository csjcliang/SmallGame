using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 轨迹基类，包括画图，列点
/// Base class for tracks containing drawing methods, list of points, abstract methods that derived classes should implement,...
/// </summary>
public abstract class Track : MonoBehaviour {

    //自带或者计算出来
    public List<Vector3> ShapePoints = new List<Vector3>(); //preferred to use methods below, but accessible if needed
    public float EditorVectorSize = 0.1f;

    public abstract Vector3 GetPointOnTrack(float n);//要给出拿点的方法
    public abstract void CalculateStartPoints();//要给出计算开始点的方法

    public bool IsHiddenInEditor = false;


    //这个是可视化画出来轨迹
    public void DrawVertexPoints()
    {
        if (IsHiddenInEditor)
            return;

        //point spheres
        Gizmos.color = Color.green;
        ShapePoints.ForEach(p => Gizmos.DrawWireSphere(p, EditorVectorSize));//递归画点
    }

    public void DrawPointLines()
    {
        if (IsHiddenInEditor)
            return;

        for (int i = 0; i < Count() - 1; ++i)
        {
            Gizmos.DrawLine(GetPointAtIndex(i), GetPointAtIndex(i + 1));//递归画线段
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        DrawPointLines();
        DrawVertexPoints();
    }

    public void AddPoint(Vector3 point)
    {
        ShapePoints.Add(point);
    }

    public void RemovePoint(int index)
    {
        if (index < ShapePoints.Count)
            ShapePoints.RemoveAt(index);
    }

    public void ClearPoints()
    {
        ShapePoints.Clear();
    }

    public Vector3 GetPointAtIndex(int i)
    {
        return ShapePoints[i];
    }

    public int Count()
    {
        return ShapePoints.Count;
    }

    public void SetAllY(float value)
    {
        for (int i = 0; i < ShapePoints.Count; i++)
        {
            ShapePoints[i] = new Vector3(ShapePoints[i].x, value, ShapePoints[i].z);
        }
    }



}
