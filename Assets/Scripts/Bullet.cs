using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class Bullet : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float speed = 10f;
    public int damage = 3;
    bool alreadyDestroyed = false;
    public GameObject spawnParent;
    public float delayToShootMax = 0f;
    private float delayTimer;
    public SoundEffects ShootSound;
    public bool HasSound = true;

    void Start()
    {
        LeanTween.delayedCall(4f, DestroySelf);
        delayTimer = delayToShootMax;
        if (HasSound)
        {
            LeanTween.delayedCall(delayToShootMax, () => AudioManager.instance.PlaySound(ShootSound));
        }
        
    }

    void Update()
    {
        if(delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
        }
        else
        {
            transform.position += transform.up * Time.deltaTime * speed;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("hit: " + collision.gameObject.name);

        if(spawnParent == collision.gameObject)
        {
            return;
        }
        else
        {
            Damageable d = collision.gameObject.GetComponent<Damageable>();
            if(d != null)
            {
                d.StartDamage(damage);
            }
        }
        
        DestroySelf();
    }

    private void DestroySelf()
    {
        if (alreadyDestroyed) return;
        alreadyDestroyed = true;
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        Destroy(gameObject, 0.01f);
    }
}
