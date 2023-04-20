using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


//这里是那个墙，当指定的游戏物体没有了（列表空了），这道墙就开了
public class DestroyOnObjectRemoval : MonoBehaviour {

    public List<GameObject> ObjectList;//指定的游戏物体
    public UnityEvent OnDestroyCallback;

	void Update () {

        //cool
        ObjectList.RemoveAll(obj => obj == null);

        if (ObjectList.Count == 0)
        {
            Debug.Log("Removing " + name + " because tracking list is empty.");

            Destroy(this.gameObject);

            if (OnDestroyCallback != null)
                OnDestroyCallback.Invoke();

        }
	
	}
}
