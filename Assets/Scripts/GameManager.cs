using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Controls Controls;
    public UIManager uiManager;
    public GameObject StartCamera;
    public List<GameObject> InitCameras;

    public GameObject PlayerGO;
    public Transform RespawnLocation;
    public CameraTransition RespawnTransition;

    public PlayerController playerController;
    public bool IsTesting = false;

    public bool RunTimer = false;

    public float Timer;
    private float TimerMax = 30f;


    public Transform Spawn_PlayerStart;

    public GameObject Icetopus;
    private Icetopus icetopusLogic;
    public Transform SpawnLocation_Icetopus;

    public GameObject Volture;
    public Transform SpawnLocation_Volture;

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
        icetopusLogic = Icetopus.GetComponent<Icetopus>();
        if (!IsTesting)
        {
            foreach (GameObject go in InitCameras)
            {
                go.SetActive(false);
            }
            StartCamera.SetActive(true);
            PlayerGO.transform.position = Spawn_PlayerStart.position;
        }
        else
        {
            Respawn();
        }
        

    }

    public void ResetFullGame()
    {

    }

    private void Update()
    {
        if (RunTimer)
        {
            if(Timer > 0)
            {
                Timer -= Time.deltaTime;
            }
            else
            {
                Timer = TimerMax;
                print("spawn char");
            }
            uiManager.UpdateTimer(Timer);
        }
    }

    public void StartTimer()
    {
        RunTimer = true;
    }

    public void StopTimer()
    {
        RunTimer = false;
    }

    public void ResetTimer()
    {
        RunTimer = false;
        Timer = TimerMax;
    }

    public void InitBoss()
    {
        ResetTimer();
        uiManager.Timer_PopIn();
        uiManager.BossHealth_PopIn();
    }

    public void Respawn()
    {
        icetopusLogic.StopFight();

        StopTimer();
        uiManager.Timer_PopOut();
        playerController.ResetHealth();
        uiManager.BossHealth_PopOut();
        PlayerGO.transform.position = RespawnLocation.position;
        RespawnTransition.RespawnReset();
    }

    public void SetupBoss(SkillTypes bossType)
    {
        ResetTimer();
        uiManager.Timer_PopIn();
        uiManager.BossHealth_PopIn();
        switch (bossType)
        {
            case SkillTypes.Ice:
                Icetopus.transform.position = SpawnLocation_Icetopus.position;
                Icetopus bossLogic = Icetopus.GetComponent<Icetopus>();
                bossLogic.StartFight();
                break;
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

    private void OnEnable()
    {
        Controls.Player.Enable();
    }

    private void OnDisable()
    {
        Controls.Player.Disable();
    }
}
