using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SkillTypes CurrentBoss;
    public static GameManager instance;
    public Controls Controls;
    public UIManager uiManager;
    public GameObject StartCamera;
    public List<GameObject> InitCameras;

    public GameObject prefab_Player;
    public Transform clone_SpawnLocation;

    public GameObject PlayerGO;
    public Transform RespawnLocation;
    public CameraTransition RespawnTransition;

    public PlayerController playerController;
    public bool IsTesting = false;

    public bool RunTimer = false;

    public List<GameObject> cloneList;
    public float Timer;
    private float TimerMax = 5f;

    public List<EventData> listEvents;

    public Transform Spawn_PlayerStart;

    public GameObject Icetopus;
    private Icetopus icetopusLogic;
    public Transform SpawnLocation_Icetopus;

    public GameObject BurnataurGO;
    public Transform SpawnLocation_Burnataur;

    public GameObject Volture;
    public Transform SpawnLocation_Volture;

    public GameObject UroGO;
    public Transform SpawnLocation_Uro;
    public Portal Portal_Uro;
    int bossDefeatNum = 0;
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
        listEvents = new List<EventData>();
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

    public void AddToQueue(EventData ed)
    {
        listEvents.Add(ed);
        //print("New Event: " + ed.GetType() + " | Event Count: " + listEvents.Count);
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
                GameObject cloneGO = Instantiate(prefab_Player, null);
                cloneList.Add(cloneGO);
                PlayerController clonePC = cloneGO.GetComponent<PlayerController>();
                clonePC.ConfigureClone();
                SpriteRenderer cloneSR = cloneGO.GetComponent<SpriteRenderer>();
                clonePC.MainColor = new Color(0.55f, 0.55f, 0.55f, 0.75f);
                cloneSR.color = clonePC.MainColor;
                cloneGO.transform.position = clone_SpawnLocation.position;
                //print("spawn char");
            }
            uiManager.UpdateTimer(Timer);
        }
    }

    public void ClearBossArea()
    {
        foreach(GameObject go in cloneList)
        {
            Destroy(go);
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
        ClearQueue();
        ResetTimer();
        uiManager.Timer_PopIn();
        uiManager.BossHealth_PopIn();
    }

    public void ClearQueue()
    {
        cloneList = new List<GameObject>();
        listEvents = new List<EventData>();
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

    public void SetupBoss(SkillTypes bossType, Transform spawnLocation)
    {
        CurrentBoss = bossType;
        clone_SpawnLocation = spawnLocation;
        InitBoss();
        switch (bossType)
        {
            case SkillTypes.Ice:
                Icetopus.transform.position = SpawnLocation_Icetopus.position;
                Icetopus bossLogic = Icetopus.GetComponent<Icetopus>();
                bossLogic.StartFight();
                break;
            case SkillTypes.Fire:
                BurnataurGO.transform.position = SpawnLocation_Burnataur.position;
                Burnataur burn_bossLogic = BurnataurGO.GetComponent<Burnataur>();
                burn_bossLogic.StartFight();
                burn_bossLogic.firstAction = true;
                break;
            case SkillTypes.Normal:
                UroGO.transform.position = SpawnLocation_Uro.position;
                Uro uro_bossLogic = UroGO.GetComponent<Uro>();
                uro_bossLogic.StartFight();
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

    public void BossDefeated()
    {
        bossDefeatNum++;
        AudioManager.instance.MixSongs();
        ClearBossArea();
        if (bossDefeatNum > 1)
        {
            Portal_Uro.OpenPortal();
        }

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
