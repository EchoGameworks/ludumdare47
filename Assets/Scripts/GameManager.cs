using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Controls Controls;


    public int TotalPotionsCount = 0;
    public int CollectedPotionsCount;

    public PlayerController playerController;

    public int CurrentLevel = 1;
    public bool IsTesting = false;

    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Controls = new Controls();

#if !UNITY_EDITOR
        IsTesting = false;
#endif
    }

    private void Start()
    {
        //if (!IsTesting)
        //{
        //    SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        //}
        //else
        //{
        //    PlayTestSceneCurrentLevel();
        //}
    }

    //public void ResetFullGame()
    //{        
    //    SceneManager.UnloadSceneAsync(CurrentLevel);
        
    //    SceneManager.LoadSceneAsync(CurrentLevel, LoadSceneMode.Additive);
    //    AudioManager.instance.MixToForward();
    //}

    public void PlayTestSceneCurrentLevel()
    {
        Scene[] loadedScenes = GetLoadedScenes();
        for (int i = 0; i < loadedScenes.Length; i++)
        {
            if (loadedScenes[i].buildIndex != 0)
            {
                CurrentLevel = loadedScenes[i].buildIndex;
                return;
            }
        }
    }

    public Scene[] GetLoadedScenes()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }
        return loadedScenes;
    }

    public void LoadNextLevel()
    {
        //SceneManager.UnloadSceneAsync(CurrentLevel);
        //CurrentLevel++;
        //if (CurrentLevel >= SceneManager.sceneCountInBuildSettings)
        //{
        //    CurrentLevel = 1;
        //    MainMenu.instance.MenuNumber = 4;
        //    MainMenu.instance.UpdateMenu();
        //}
        //SceneManager.LoadSceneAsync(CurrentLevel, LoadSceneMode.Additive);
        //AudioManager.instance.MixToForward();
    }

    public void ReloadLevel()
    {
        //SceneManager.UnloadSceneAsync(CurrentLevel);
        //SceneManager.LoadSceneAsync(CurrentLevel, LoadSceneMode.Additive);
        //AudioManager.instance.MixToForward();
    }

    private void OnEnable()
    {
        Controls.Player.Enable();
        //Inputs.Menu.Enable();
    }

    private void OnDisable()
    {
        Controls.Player.Disable();
        //Inputs.Menu.Disable();
    }
}
