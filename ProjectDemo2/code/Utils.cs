using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

//就是设置Transform的坐标
public static class TransformExtensions
    {

        public static void SetX(this Transform transform, float x)
        {
            Vector3 newPosition =
               new Vector3(x, transform.position.y, transform.position.z);

            transform.position = newPosition;
        }

        public static void SetY(this Transform transform, float y)
        {
            Vector3 newPosition =
               new Vector3(transform.position.x, y, transform.position.z);

            transform.position = newPosition;
        }

        public static void SetZ(this Transform transform, float z)
        {
            Vector3 newPosition =
               new Vector3(transform.position.x, transform.position.y, z);

            transform.position = newPosition;
        }

        public static void SetPosition(this Transform transform, float x, float y, float z)
        {
            Vector3 newPosition =
               new Vector3(x, y, z);

            transform.position = newPosition;
        }
    }

class Utils
{
    //初始化
    public static GameObject InstantiateSafe(GameObject gO, Vector3 position)
    {
        if(gO == null)
        {
            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;

            UnityEngine.Debug.LogError(string.Format("Can't load prefab at {0}.{1}", type, name));
            UnityEngine.Debug.Break();
        }

        GameObject result = (GameObject)GameObject.Instantiate(gO, position, Quaternion.identity);
        return result;
    }
    //加载
    public static GameObject LoadGameObjectSafe(string filenameFromResources)
    {
        GameObject GO = Resources.Load<GameObject>(filenameFromResources.Trim());

        if (GO == null)
        {
            //report error better
            UnityEngine.Debug.LogError("Can't load GameObject " + filenameFromResources);
            UnityEngine.Debug.Break();
        }

        return GO;
    }
}

