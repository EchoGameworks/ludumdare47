using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uro : Boss
{
    public Transform cleanupTransform;
    public GameObject prefab_Poison;
    public GameObject prefab_Slither;

    public List<Transform> MovePoints;
    public List<Transform> SlitherPoints;
    int moveID;
    public bool firstAction = true;

    private void Update()
    {
        base.UpdateActionTimer();
    }

    public override void Action1()
    {
        //Poison
        float spreadAngle = 180f;
        int spawnCount = 3;
        float incrementChange = spreadAngle / spawnCount;
        float startAngle = spreadAngle / 2f;

        //float incrementAngle = 0f;
        Vector3 moveDir = Player.transform.position - this.transform.position;
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        angle += Random.Range(-90, 90);
        for (int i = 0; i < spawnCount; i++)
        {
            float rDist = Random.Range(1f, 8f);
            GameObject poisonGO = Instantiate(prefab_Poison, cleanupTransform);
            //poisonGO.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f - spreadAngle + startAngle + incrementAngle);
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            poisonGO.transform.position = this.transform.position + q * Vector3.right * rDist;
            //incrementAngle += incrementChange;
        }
    }

    public override void Action2()
    {
        Action1();
        Action1();
        Action3();
    }

    public override void Action3()
    {
        //Shoot Snakes
        if (moveID != 0)
        {
            if (LeanTween.isTweening(moveID)) return;
        }

        int r = Random.Range(0, MovePoints.Count - 1);
        var seq = LeanTween.sequence();
        moveID = seq.id;
        seq.append(LeanTween.scale(gameObject, Vector3.zero, 0.4f));
        seq.append(2f);
        seq.append(() => gameObject.transform.position = MovePoints[r].position);
        seq.append(LeanTween.scale(gameObject, Vector3.one, 0.4f));

        r = Random.Range(0, SlitherPoints.Count - 1);
        GameObject sliterGO = Instantiate(prefab_Slither, null);
        prefab_Slither.transform.position = SlitherPoints[r].position;
        prefab_Slither.transform.rotation = SlitherPoints[r].rotation;
    }

    public override void Action4()
    {

        if (moveID != 0)
        {
            if (LeanTween.isTweening(moveID)) return;
        }

        int r = Random.Range(0, MovePoints.Count - 1);
        var seq = LeanTween.sequence();
        moveID = seq.id;
        seq.append(LeanTween.scale(gameObject, Vector3.zero, 0.4f));
        seq.append(() => LeanTween.delayedCall(0.1f, () => LeavePoison(this.transform.position)));
        seq.append(() => gameObject.transform.position = MovePoints[r].position);
        seq.append(LeanTween.scale(gameObject, Vector3.one, 0.4f));
    }

    private void LeavePoison(Vector3 v)
    {

        GameObject poisonGO = Instantiate(prefab_Poison, cleanupTransform);
        poisonGO.transform.position = v;
    }





    public override void Die()
    {
        base.Die();
        //foreach(Transform t in cleanupTransform)
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
