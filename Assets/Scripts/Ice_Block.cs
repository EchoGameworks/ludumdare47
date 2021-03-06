﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class Ice_Block : MonoBehaviour
{
    public float delayDestroyMin = 8f;
    public float delayDestroyMax = 15f;
    private bool alreadyDestroyed = false;
    public ParticleSystem particleSystem;
    public SoundEffects DestroySound;

    void Start()
    {
        LeanTween.delayedCall(Random.Range(delayDestroyMin, delayDestroyMax), DestroySelf);
    }

    private void DestroySelf()
    {
        if (alreadyDestroyed) return;
        alreadyDestroyed = true;
        AudioManager.instance.PlaySound(DestroySound);
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        Destroy(gameObject, 0.01f);
    }
}
