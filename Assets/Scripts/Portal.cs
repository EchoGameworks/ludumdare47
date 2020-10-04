using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public SkillTypes BossType;
    public bool IsComplete;
    public GameObject GoToCamera;
    public Transform SpawnPosition;
    private GameObject PlayerGO;
    private GameManager gameManager;

    public GameObject CompleteFillGO;
    public GameObject AwayFromCamera;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        PlayerGO = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsComplete)
        {
            EnterPortal();
        }
    }

    private void EnterPortal()
    {
        PlayerGO.transform.position = new Vector3(SpawnPosition.position.x, SpawnPosition.position.y, 0f);
        GoToCamera.SetActive(true);
        gameManager.SetupBoss(BossType);
        if(AwayFromCamera != null)
        {
            AwayFromCamera.SetActive(false);
        }
        
        //gameManager.ResetTimer();
    }

    public void CompletePortal()
    {
        IsComplete = true;
        CompleteFillGO.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
