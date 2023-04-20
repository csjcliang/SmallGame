using UnityEngine;
using System.Collections;
using UnityEngine.Events;

//这里是那个护屁股的那个，后面的那个没了自己就没了
public class RemoveWhenChildIsGone : MonoBehaviour {

    public Transform ChildToTrack;
    public UnityEvent OnDestroyCallback;

	void Start () {
	
	}
	
	void Update () {

        if (ChildToTrack == null)
        {
            Destroy(this.gameObject);

            if (OnDestroyCallback != null)
                OnDestroyCallback.Invoke();
        }
	
	}
}
