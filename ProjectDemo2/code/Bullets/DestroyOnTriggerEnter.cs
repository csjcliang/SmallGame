using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Destroys the attached gameobject when the trigger has a certain tag (set in the AffectedByTag), and optionally calls a callback function to trigger an event.
/// </summary>
public class DestroyOnTriggerEnter : MonoBehaviour {

    // Use this for initialization
    public int Health = 1;
    public List<string> AffectedByTag;//里面写着碰撞时能产生有效作用的类（tag）

    public bool IsTriggerStayAlive;//启用后子弹撞到就会没掉
    public bool IsInvincible = false;//无敌
    public bool BlinkOnTakeDamage = false;//受伤时闪烁一下展示受伤的红色
    public Color TakeDamageColor;

    private Color defaultColor;
    Renderer RenderComponent;
    //这个就和那边的渲染受伤时的颜色一样
    public UnityEvent OnDestroyCallback;//外部传进来的函数

    void Start()
    {
        if (BlinkOnTakeDamage)
        {
            RenderComponent = GetComponentInChildren<Renderer>();

            if(RenderComponent != null)
                defaultColor = RenderComponent.material.color;
            //取了那个渲染的组件以及颜色
        }

        if (IsInvincible)
            Health = 999999;
    }

    void OnTriggerEnter(Collider col)
    {
        //发生碰撞时
        //如果碰撞的东西的tag（类名？）在列表里，就是被打了，扣一血
        //Prefabs里面可以看到，每个物体都可以设置tag，因此可以根据tag来找碰撞物
        if (AffectedByTag.Contains(col.gameObject.tag))
        {
            TakeDamage(1);

            if (!IsTriggerStayAlive)
                Destroy(col.gameObject); //打开了那个开关，被撞到的东西就会没了
        }

    }

    void TakeDamage(int value)
    {
        if(Health > 1 && BlinkOnTakeDamage && RenderComponent != null)
            StartCoroutine(damageFlash());//StartCoroutine协同函数运行，
                               //使用这个是因为，要实现闪烁，这里的处理是延迟变色。（定时）
                               //而使用计时功能停顿等待时，不能锁住主线程卡住游戏。
                               //unity就可以使用开启一个新线程的功能，可以在那个线程自由暂停而不会影响主进程。
        //扣血
        Health -= value;
        //没血了
        if (Health <= 0)
        {
            //我没了
            Destroy(this.gameObject);
            //执行回调函数
            if (OnDestroyCallback != null)
                OnDestroyCallback.Invoke();
        }

    }

    IEnumerator damageFlash()
    {
        //闪烁处理：变色，等待，变回去
        RenderComponent.material.color = TakeDamageColor;
        yield return new WaitForSeconds(0.1f);//协进程停顿
        RenderComponent.material.color = defaultColor;
    }
}
