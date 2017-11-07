using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameCon :SingleObject<GameCon> {

    public event Action BeforeSceneUnload;
    public event Action AfterSceneLoad;

    public string StartScene = "Game";


    static PlayerCharacter _player;

    public static PlayerCharacter Player
    {
        get
        {
            if (_player==null)
            {
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
            }
            return _player;
        }
    }

    static FadeControl _fader;
    public static FadeControl Fader
    {
        get
        {
            if (_fader==null)
            {
                _fader = GameObject.FindObjectOfType<FadeControl>();
            }
            return _fader;
        }
    }
    private IEnumerator Start()
    {
        //加载初始场景并等待加载完成
        yield return StartCoroutine(LoadSceneAndSetActive(StartScene));

        StartCoroutine(Fader.Fade(0f));
    }

    public void ReloadScene()
    {

        LoadScence(SceneManager.GetActiveScene().name);
    }


    public void LoadScence(string sceneName)
    {
        if (!Fader.isFading)
        {
            
            StartCoroutine(FadeAndeSwitchScences(sceneName));
            Debug.Log("==========================================");
        }
    }
    IEnumerator FadeAndeSwitchScences(string sceneName)
    {
        //淡出场景 
        
        yield return StartCoroutine(Fader.Fade(1f));
        
        if (BeforeSceneUnload!=null)
        {
            BeforeSceneUnload();
        }
        

        yield return StartCoroutine(Fader.Fade(0f));
        //卸载当前场景
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //加载新的场景
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        
        if (AfterSceneLoad != null)
            AfterSceneLoad();

    }
    IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);

        Scene newlyLoadedScence = SceneManager.GetSceneAt(SceneManager.sceneCount-1);
        SceneManager.SetActiveScene(newlyLoadedScence);
    }


}
