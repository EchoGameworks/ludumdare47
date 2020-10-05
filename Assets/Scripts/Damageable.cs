using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using static AudioManager;

public class Damageable : MonoBehaviour
{
    //public UnitTypes UnitType;
    public int Health;
    public int HealthMax;
    public bool HasSound = true;
    public SoundEffects HurtSound;
    public SoundEffects DeathSound;
    public float invulnerableTimer;
    public float invulnerableTimerMax;
    public bool IsVulnerable = true;
    public SpriteRenderer SpriteRend;
    int flashID;
    public Color MainColor;

    public void ResetHealth()
    {
        Health = HealthMax;
        TakeDamage();
    }

    public virtual void InvulnerableTimerUpdate()
    {
        if (invulnerableTimer > 0f)
        {
            invulnerableTimer -= Time.deltaTime;
        }
        else
        {
            LeanTween.cancel(flashID);
            SpriteRend.color = MainColor;
        }
    }

    public void StartDamage(int dmg)
    {
        if (IsVulnerable && invulnerableTimer <= 0f)
        {
            flashID = LeanTween.value(gameObject, val => SpriteRend.color = val,
                MainColor,
                new Color(1f, 0.2f, 0.2f, MainColor.a), 0.1f)
                .setLoopPingPong(100).id;
            invulnerableTimer = invulnerableTimerMax;
            Health -= dmg;
            TakeDamage();
            if (Health <= 0)
            {
                Die();
            }
            else
            {
                if (HasSound)
                {
                    //print("playing dmg sound");
                    AudioManager.instance.PlaySound(HurtSound);
                }

            }
            
        }
    }

    public virtual void TakeDamage()
    {

    }

    public virtual void Die()
    {

    }

}
