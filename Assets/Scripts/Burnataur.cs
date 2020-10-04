using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnataur : Boss
{
    public GameObject prefab_Fireball;
    public Transform cleanupTransform;
    public List<Transform> fireballLandSpawnLocations;
    public List<Transform> fireballFromTopLocations1;
    public List<Transform> fireballFromTopLocations2;

    public List<Transform> MovePoints;
    int moveID;
    public bool firstAction = true;

    private void Update()
    {
        base.UpdateActionTimer();
    }

    public override void Action1()
    {
        firstAction = false;
        Fireballs1();
        Action4();
    }

    public override void Action2()
    {
        firstAction = false;
        Fireballs2();
        Action4();
    }

    public override void Action3()
    {
        firstAction = false;
        Fireballs1();
        Fireballs2();
    }

    public override void Action4()
    {
        if (firstAction)
        {
            Action3();
        }
        if (moveID != 0)
        {
            if (LeanTween.isTweening(moveID)) return;
        }

        int r = Random.Range(0, MovePoints.Count - 1);
        moveID = LeanTween.move(gameObject, MovePoints[r].position + new Vector3(0f, 0.5f, 0f), 2f).setOnComplete(() => Land(r)).id;
        //print("moveID: " + moveID);
    }

    private void Land(int r)
    {
        moveID = LeanTween.move(gameObject, MovePoints[r].position, 0.3f)//.setDelay(0.3f)
            .setOnComplete(LandFireballs)
            .id;
    }

    private void LandFireballs()
    {
        foreach(Transform t in fireballLandSpawnLocations)
        {
            GameObject fireGO =  Instantiate(prefab_Fireball, cleanupTransform);
            fireGO.transform.position = t.position;
            fireGO.transform.rotation = t.rotation;
            fireGO.transform.localScale = Vector3.zero;
            LeanTween.scale(fireGO, Vector3.one, 0.3f).setEaseInOutCirc();
            //print("shoot fireball");
        }
    }

    private void Fireballs1()
    {
        foreach (Transform t in fireballFromTopLocations1)
        {
            GameObject fireGO = Instantiate(prefab_Fireball, cleanupTransform);
            fireGO.transform.position = t.position;
            fireGO.transform.rotation = t.rotation;
            fireGO.transform.localScale = Vector3.zero;
            LeanTween.scale(fireGO, Vector3.one, 0.3f).setEaseInOutCirc();
            //print("shoot fireball");
        }
    }

    private void Fireballs2()
    {
        foreach (Transform t in fireballFromTopLocations2)
        {
            GameObject fireGO = Instantiate(prefab_Fireball, cleanupTransform);
            fireGO.transform.position = t.position;
            fireGO.transform.rotation = t.rotation;
            fireGO.transform.localScale = Vector3.zero;
            LeanTween.scale(fireGO, Vector3.one, 0.3f).setEaseInOutCirc();
            //print("shoot fireball");
        }
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
