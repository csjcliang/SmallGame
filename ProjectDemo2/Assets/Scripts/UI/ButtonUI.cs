using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

//按钮UI
public class ButtonUI : MonoBehaviour
{
    public UnityEvent OnSelection; //选择按钮

    Button button;
    TextMeshProUGUI text;

    Vector3 originalScale; //选择前大小
    Color originalColor; //选择后颜色
    bool selected; //是否选择

    const float SELECTION_SCALE_MULTIPLIER = 1.3f; //选择后放大倍数
    const float CLICK_SCALE_MULTIPLIER = 1.5f; //点击后放大倍数
    const float SELECTION_ANIMATION_DURATION = .3f; //选择动画持续时间
    const float CLICK_ANIMATION_DURATION = .2f; //点击动画持续时间

    #region API
    //初始化
    public void Setup()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        originalScale = transform.localScale;
        originalColor = button.image.color;
        FadeImage(false, 0);
    }

    //选择按钮
    public void Select()
    {
        button.image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        FadeImage(true, .1f);
        transform.DOPunchScale(originalScale * SELECTION_SCALE_MULTIPLIER, SELECTION_ANIMATION_DURATION, 0, .7f); //来回缩放的效果
        selected = true;
        OnSelection.Invoke(); //响应
    }

    //取消选择
    public void Deselect()
    {
        FadeImage(false, .1f);
        transform.DOScale(originalScale, SELECTION_ANIMATION_DURATION); //恢复原来大小
        selected = false;
    }

    //点击按钮
    public void Click()
    {
        transform.DOPunchScale(originalScale * CLICK_SCALE_MULTIPLIER, CLICK_ANIMATION_DURATION, 0, .7f); //来回缩放的效果
        transform.DOScale(originalScale * SELECTION_SCALE_MULTIPLIER, CLICK_ANIMATION_DURATION);
        button.onClick.Invoke(); //调用方法
    }

    //按钮消失
    public void FadeAll(bool _fadeValue, float _duration, System.Action _callback = null)
    {
        if (selected)
            button.image.DOFade(_fadeValue == true ? 1 : 0, _duration);
        text.DOFade(_fadeValue == true ? 1 : 0, _duration).onComplete += () => _callback?.Invoke(); //回调函数
    }

    //按钮背景消失
    public void FadeImage(bool _fadeValue, float _duration, System.Action _callback = null)
    {
        button.image.DOFade(_fadeValue == true ? 1 : 0, _duration).onComplete += () => _callback?.Invoke();
    }
    #endregion
}