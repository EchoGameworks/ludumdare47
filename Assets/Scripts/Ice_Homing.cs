using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Homing : MonoBehaviour
{
    public GameObject Target;
    public GameObject prefab_IceBlock;
    public GameObject spawnParent;

    private bool alreadyDestroyed = false;
    public ParticleSystem particleSystem;
    public float speed;
    public int damage = 2;
    public int numSpawnBlocks = 3;
    public float spawnRange = 3f;
    public float delayExplode = 10f;

    void Start()
    {
        LeanTween.delayedCall(delayExplode, Explode);
    }

    // Update is called once per frame
    void Update()
    {
        if(Target != null)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), Target.transform.position, 1f * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (spawnParent != null)
        {
            if (spawnParent == collision.gameObject)
            {
                return;
            }
            else
            {
                Explode();
                Damageable d = collision.gameObject.GetComponent<Damageable>();
                if (d != null)
                {
                    d.StartDamage(damage);
                }
            }
        }
    }

    private void Explode()
    {
        for(int i = 0; i < numSpawnBlocks; i++)
        {
            GameObject iceGO = Instantiate(prefab_IceBlock, null);
            Vector2 v = Random.insideUnitCircle * spawnRange;
            iceGO.transform.position = new Vector3(this.transform.position.x + v.x, this.transform.position.y + v.y, 0f);
        }
        DestroySelf();
    }

    private void DestroySelf(bool isSilent = false)
    {
        if (alreadyDestroyed) return;
        alreadyDestroyed = true;
        if (particleSystem != null && !isSilent)
        {
            particleSystem.Play();
        }
        Destroy(gameObject, 0.01f);
    }
}
