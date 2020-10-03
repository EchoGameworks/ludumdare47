using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem particleSystem;
    float speed = 10f;
    bool alreadyDestroyed = false;
    public GameObject spawnParent;

    void Start()
    {
        LeanTween.delayedCall(2f, DestroySelf);
    }

    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(spawnParent != null)
        {
            if(spawnParent == collision.gameObject)
            {
                return;
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
