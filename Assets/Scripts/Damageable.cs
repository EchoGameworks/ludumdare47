using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Damageable : MonoBehaviour
{
    //public UnitTypes UnitType;
    public int Health;
    public int HealthMax;

    public float invulnerableTimer;
    public float invulnerableTimerMax;
    public bool IsVulnerable = true;

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
    }

    public void StartDamage(int dmg)
    {
        if (IsVulnerable && invulnerableTimer <= 0f)
        {
            invulnerableTimer = invulnerableTimerMax;
            Health -= dmg;
            if (Health <= 0)
            {
                Die();
            }

            TakeDamage();
        }

    }

    public virtual void TakeDamage()
    {

    }

    public virtual void Die()
    {
        print("this died");
    }

}
