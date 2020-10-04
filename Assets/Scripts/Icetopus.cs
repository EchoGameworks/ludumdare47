using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class Icetopus : Boss
{
    public GameObject prefab_IceDagger;
    public GameObject prefab_IceOrb;
    public Transform cleanupTransform;
    int moveID;
    public SoundEffects IceDaggers_Spawn;

    public List<Transform> MovePoints;

    private void Update()
    {
        base.UpdateActionTimer();
    }

    public override void Action1()
    {
        //print("A1");
        Action4();
        GameObject goOrb = Instantiate(prefab_IceOrb, cleanupTransform);
        goOrb.transform.position = this.transform.position;
        Ice_Homing orbHoming = goOrb.GetComponent<Ice_Homing>();
        orbHoming.Target = Player;
    }

    public override void Action2()
    {

        float spawnDistance = 3f;
        float spreadAngle = 120f;
        int spawnCount = 6;
        float incrementChange = spreadAngle / spawnCount;
        float startAngle = spreadAngle / 2f;

        float incrementAngle = 0f;
        Vector3 moveDir = Player.transform.position - this.transform.position;
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        angle += Random.Range(-10, 10);
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject daggerGO = Instantiate(prefab_IceDagger, cleanupTransform);
            daggerGO.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f - spreadAngle + startAngle + incrementAngle);
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            daggerGO.transform.position = this.transform.position + q * Vector3.right * spawnDistance;
            incrementAngle += incrementChange;
        }
        AudioManager.instance.PlaySound(IceDaggers_Spawn);
    }

    public override void Action3()
    {
        Action2();
        Action4();
    }

    public override void Action4()
    {
        if (moveID != 0)
        {
            if (LeanTween.isTweening(moveID)) return;
        }

        int r = Random.Range(0, MovePoints.Count - 1);
        moveID = LeanTween.move(gameObject, MovePoints[r], 5f).id;
        //print("moveID: " + moveID);
    }

    public override void Die()
    {
        base.Die();
        //foreach (Transform t in cleanupTransform)
        //{
        //    GameObject.Destroy(t.gameObject);
        //}
        AudioManager.instance.PlaySound(DeathSound);
        LeanTween.scale(gameObject, Vector3.zero, 1f).setEaseInOutCirc();
        GameManager.instance.BossDefeated();
        if (deathParticles != null)
        {
            deathParticles.Play();
        }
        StopFight();
        transform.position = new Vector3(-1000f, -1000f, -100f);
        Player.GetComponent<PlayerController>().DefeatedBoss();
        GameManager.instance.StopTimer();
        OriginalPortal.CompletePortal();
        LeanTween.value(gameObject, val => WinInstructions.localScale = val,
            WinInstructions.localScale, new Vector3(1f, 1f, 1f), 0.3f)
            .setEaseInOutCubic();
        LeanTween.value(gameObject, val => ReturnPortal.transform.localScale = val,
            ReturnPortal.transform.localScale, new Vector3(1f, 1f, 1f), 0.3f)
            .setEaseInOutCubic().setDelay(1f);
    }
}
