using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// GameManager which holds global data and which won't be destroyed between scenes (by default), has a number of public functions which can be called from other classes and as callbacks set in the inspector (see DestroyOnTrigger for example)
/// </summary>
public class GameManager : MonoBehaviour {

    static GameManager instance;
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            var k = GameObject.Find("GameManager").GetComponent<GameManager>();
            instance = k;

            if (instance.IsDebug)
                Debug.Log("GameManager accessed first time from GetInstance");

        }
        return instance;
    }

    public bool IsDebug = true;

    [Header("Core")]
    static public string currentstate = "level0-1";
    public bool IsWon = false;
    public bool IsGameOver = false;
    public bool IsMouseCursorHidden = false;
    public bool IsDontDestroyBetweenScenes = false;

    [Header("Overlay panel to show win/lose")]
    public Image FadeImage;
    public Text PanelText;
    public float Alpha;
    public int FadeMax = 120;
    public float FadeSpeed = 0.05f;

    [Header("Particles")]
    public GameObject ParticleSystemPrefab;

    private float t = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Cursor.visible = !IsMouseCursorHidden;
            Cursor.lockState = CursorLockMode.Confined;

            //CurrentGameState = GameState.None;

            //don't destroy between scenes
            if (IsDontDestroyBetweenScenes)
                DontDestroyOnLoad(gameObject);

            Debug.Log("GameManager initialized");

        }
    }


    public void Update()
    {
        if (IsWon || IsGameOver)
        {
            t += FadeSpeed * Time.deltaTime;
            Alpha = Mathf.Lerp(Alpha, FadeMax, t);

            float aValue = Alpha / 255;
            FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, aValue);
            PanelText.color = new Color(PanelText.color.r, PanelText.color.g, PanelText.color.b, aValue);

            //TODO improve
            if (Alpha >= FadeMax - 5) {
                if(IsGameOver)
                LoadLevel("Menu");
                if (IsWon)
                Nextstate();
            }
        }

    }

    public void WinGame()
    {
        IsWon = true;
        PanelText.text = "HACKING COMPLETE";
        //do more
    }

    public void LoseGame()
    {
        Debug.Log("LOSE");
        IsGameOver = true;
        PanelText.text = "HACKING FAILURE";
    }

    //Called from enemies and player, set in the editor as a UnityEvent
    public void UnitsChanged()
    {
        Debug.Log("Units in scene changed");

        StartCoroutine(checkUnitChange());
    }

    IEnumerator checkUnitChange()
    {
        yield return 0; //TODO: waits one frame, set optional delay?

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var player = GameObject.FindGameObjectWithTag("Player");

        if (enemies.Length == 0)
            WinGame();
        if (player == null)
            LoseGame();
    }

    public void LoadLevel(string name)
    {
        Application.LoadLevel(name.Trim());
        SoundManager.GetInstance().PlayConfirmSound();
        currentstate = name;

        //reset global values
        IsWon = false;
        IsGameOver = false;
        t = 0;
    }

    public void EndGame()
    {
        #if UNITY_EDITOR
                Debug.Break();
        #else
         Application.Quit();
        #endif
    }

    public void SpawnParticles(Vector3 position)
    {
        Utils.InstantiateSafe(ParticleSystemPrefab, position);
    }

    
    public void Nextstate()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentstate = scene.name;
        Debug.Log(currentstate);
        if (currentstate == "level0-1") { LoadLevel("level0-2");return; }
        if (currentstate == "level0-2") { LoadLevel("level0-3"); return; }
        if (currentstate == "level0-3") { LoadLevel("level0-4"); return; }
        if (currentstate == "level0-4") { LoadLevel("level0-5"); return; }
        if (currentstate == "level0-5") { LoadLevel("level0-6"); return; }
        if (currentstate == "level0-6") { LoadLevel("level0-7"); return; }
        if (currentstate == "level0-7") { LoadLevel("level0-8"); return; }
        if (currentstate == "level0-8") { LoadLevel("level0-9"); return; }
        if (currentstate == "level0-9") { LoadLevel("level0-10"); return; }
        if (currentstate == "level0-10") { LoadLevel("level0-11"); return; }
        if (currentstate == "level0-11") { LoadLevel("level0-12"); return; }
        if (currentstate == "level0-12") { LoadLevel("level1-1"); return; }
        if (currentstate == "level1-1") { LoadLevel("level1-2"); return; }
        if (currentstate == "level1-2") { LoadLevel("level1-3"); return; }
        if (currentstate == "level1-3") { LoadLevel("level1-4"); return; }
        if (currentstate == "level1-4") { LoadLevel("level1-5"); return; }
        if (currentstate == "level1-5") { LoadLevel("level1-6"); return; }
        if (currentstate == "level1-6") { LoadLevel("Menu"); return; }
    }
}
