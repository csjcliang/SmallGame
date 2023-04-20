using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    public MenuUI MainMenu; //主菜单
    public MenuUI ModeMenu; //关卡选择菜单
 
    [Header("Background")]
    public GameObject MenuBackgrounds;

    private MenuUI currentMenu;
    private Vector2 activeMenuPosition;

    private const float MENU_TRANSITION_DURATION = .5f; //菜单切换持续时间
    /// <summary>
    /// Measured in pixels
    /// </summary>
    private const float MENU_DISTANCE = 50;
    /// <summary>
    /// Measured in unity units
    /// </summary>        
    private const float BACKGROUNDS_DISTANCE = 50;
    private const int MAIN_MENU_INDEX = 0;
    private const int NORMAL_MODE_INDEX = 1;
    private const int REWIND_MODE_INDEX = 2;
    private const int OPTIONS_MENU_INDEX = 3;

    #region Monos
    private void Awake()
    {
        Setup();
    }

    private void Update()
    {
        //返回主菜单
        if (Input.GetButtonDown("Back") && currentMenu != MainMenu)
        {
            GoToMainMenu();
        }
    }
    #endregion

    #region API
    public void Setup()
    {
        Cursor.visible = false; //隐藏鼠标

        if (MainMenu)
        {
            //当前为主菜单
            currentMenu = MainMenu;
            activeMenuPosition = currentMenu.transform.position;
            ModeMenu.transform.position = activeMenuPosition + new Vector2(MENU_DISTANCE, 0);
        }

    }

    /// <summary>
    /// Go to mode selection menu
    /// </summary>
    //进入关卡选择菜单
    public void GoToModeMenu()
    {
        //初始化相关
        currentMenu.DeselectAll();
        currentMenu.enabled = false;
        currentMenu.FadeButtons(false, MENU_TRANSITION_DURATION);
        currentMenu.transform.DOMoveX(activeMenuPosition.x - MENU_DISTANCE, MENU_TRANSITION_DURATION);
        //当前为关卡选择菜单
        currentMenu = ModeMenu;
        //自动选择第一个按钮
        currentMenu.SelectFirst();
        currentMenu.FadeButtons(true, MENU_TRANSITION_DURATION);
        currentMenu.transform.DOMoveX(UnityEngine.Screen.width*7/20, MENU_TRANSITION_DURATION).onComplete += () => currentMenu.enabled = true;

        //BACKGROUNDS
        MenuBackgrounds.transform.DOMoveY(BACKGROUNDS_DISTANCE * NORMAL_MODE_INDEX, MENU_TRANSITION_DURATION);

    }

    /// <summary>
    /// Go to main menu
    /// </summary>
    //跳转主菜单
    public void GoToMainMenu()
    {
        //初始化相关
        currentMenu.DeselectAll();
        currentMenu.enabled = false;
        currentMenu.FadeButtons(false, MENU_TRANSITION_DURATION);
        currentMenu.transform.DOMoveX(activeMenuPosition.x + MENU_DISTANCE, MENU_TRANSITION_DURATION);
        //当前为主菜单
        currentMenu = MainMenu;

        currentMenu.FadeButtons(true, MENU_TRANSITION_DURATION);
        currentMenu.SelectFirst();
        currentMenu.transform.DOMoveX(activeMenuPosition.x, MENU_TRANSITION_DURATION).onComplete += () => currentMenu.enabled = true;

        //BACKGROUNDS
        MenuBackgrounds.transform.DOMoveY(BACKGROUNDS_DISTANCE * MAIN_MENU_INDEX, MENU_TRANSITION_DURATION);

    }

    //动态背景1
    public void BackgroundForNormalMode()
    {
        MenuBackgrounds.transform.DOMoveY(BACKGROUNDS_DISTANCE * NORMAL_MODE_INDEX, MENU_TRANSITION_DURATION);
    }

    //动态背景2
    public void BackgroundForRewindMode()
    {
        MenuBackgrounds.transform.DOMoveY(BACKGROUNDS_DISTANCE * REWIND_MODE_INDEX, MENU_TRANSITION_DURATION);
    }

    //加载场景
    public void LoadScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }
    //退出游戏
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}