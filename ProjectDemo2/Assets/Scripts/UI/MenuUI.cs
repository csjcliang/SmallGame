using UnityEngine;

//菜单UI
public class MenuUI : MonoBehaviour
{
    public enum NavigationType { Vertical, Horizontal } //菜单选择方向
    public bool autoSelectFirstOne = true; //自动选择第一个
    public bool fadeOutOnSetup; //淡出效果
    public bool disableOnSetup; //初始化时不显示
    public NavigationType navigationType;

    ButtonUI[] customButtons; //自定义按钮
    ButtonUI currentlySelectedButton; //当前选择按钮
    int currentlySelectedButtonIndex; //当前选择按钮索引
    bool selectable;
    float key;

    #region Monos
    private void Awake()
    {
        Setup(); //初始化
    }

    private void Update()
    {
        if(key==0)
            selectable = true;

        switch (navigationType)
        {
            case NavigationType.Vertical:
                key = Input.GetAxisRaw("Vertical");
                break;
            case NavigationType.Horizontal:
                key = Input.GetAxisRaw("Horizontal");
                break;
        }

        if (!selectable)
            return;

        if (key == 1)
            ChangeSelection(1);
        else if (key == -1)
            ChangeSelection(-1);

        //确认选择
        if (Input.GetButtonDown("Confirm"))
            currentlySelectedButton.Click();
    }
    #endregion

    void ChangeSelection(int _directionSign)
    {
        selectable = false;

        switch (Mathf.Sign(_directionSign))
        {
            case -1:
                if (currentlySelectedButtonIndex == customButtons.Length - 1)
                    currentlySelectedButtonIndex = 0; //最后一个往下选择就到第一个
                else
                    currentlySelectedButtonIndex++; //选择下一个
                break;
            case 1:
                if (currentlySelectedButtonIndex == 0)
                    currentlySelectedButtonIndex = customButtons.Length - 1; //第一个往上选择就到最后一个
                else
                    currentlySelectedButtonIndex--; //选择上一个
                break;
        }

        currentlySelectedButton?.Deselect();
        currentlySelectedButton = customButtons[currentlySelectedButtonIndex];
        currentlySelectedButton.Select();
    }

    #region API
    public void Setup()
    {
        customButtons = GetComponentsInChildren<ButtonUI>();

        foreach (ButtonUI button in customButtons)
        {
            button.Setup(); //初始化按钮
        }

        if (autoSelectFirstOne)
        {
            currentlySelectedButton = customButtons[0]; //选择第一个按钮
            currentlySelectedButton.Select();
        }

        if (fadeOutOnSetup)
            FadeButtons(false, 0);

        enabled = !disableOnSetup;
    }
    //按钮消失
    public void FadeButtons(bool _fadeValue, float _duration, System.Action _callback = null)
    {
        for (int i = 0; i < customButtons.Length; i++)
        {
            if (i == 0 && _callback != null)
                customButtons[i].FadeAll(_fadeValue, _duration, _callback);
            else
                customButtons[i].FadeAll(_fadeValue, _duration);
        }
    }
    //取消选择
    public void DeselectAll()
    {
        foreach (ButtonUI button in customButtons)
        {
            button.Deselect();
        }
    }

    //选择第一个按钮
    public void SelectFirst()
    {
        DeselectAll();

        currentlySelectedButtonIndex = 0;
        currentlySelectedButton = customButtons[currentlySelectedButtonIndex];
        currentlySelectedButton.Select();
    }
    #endregion
}