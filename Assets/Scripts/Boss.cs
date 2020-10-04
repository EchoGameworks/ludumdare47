using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Damageable
{
    public GameObject Player;
    public float ActionTimer;
    public float ActionTimerMax = 5f;

    public int ContactDamage = 7;
    public bool isFighting = false;
    UIManager uiManager;

    public ParticleSystem deathParticles;
    public RectTransform WinInstructions;
    public GameObject ReturnPortal;
    public Portal OriginalPortal;
    

    void Start()
    {
        MainColor = SpriteRend.color;
        ReturnPortal.transform.localScale = Vector3.zero;
        WinInstructions.localScale = Vector3.zero;
        uiManager = GameManager.instance.uiManager;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartFight()
    {
        //Boss Enter

        Health = HealthMax;
        TakeDamage();
        //Start
        
        LeanTween.delayedCall(1f, CommenceFight);
    }

    public void StopFight()
    {
        isFighting = false;
    }

    public void CommenceFight()
    {
        isFighting = true;
        GameManager.instance.StartTimer();
    }

    public override void TakeDamage()
    {
        uiManager.UpdateBossHealth(Health / (float)HealthMax);
    }

    public void UpdateActionTimer()
    {
        base.InvulnerableTimerUpdate();
        if (ActionTimer > 0f && isFighting)
        {
            ActionTimer -= Time.deltaTime;
        }
        else if(isFighting)
        {
            int r = Random.Range(1, 4);
            switch (r)
            {
                case 1:
                    Action1();
                    break;
                case 2:
                    Action2();
                    break;
                case 3:
                    Action3();
                    break;
                case 4:
                    Action4();
                    break;
            }
            ActionTimer = ActionTimerMax;
        }
    }

    public virtual void Action1()
    {

    }

    public virtual void Action2()
    {

    }

    public virtual void Action3()
    {

    }

    public virtual void Action4()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            print("contact damage");
            Player.GetComponent<PlayerController>().StartDamage(ContactDamage);
        }
    }

    //public override void Die()
    //{

    //} 
}
