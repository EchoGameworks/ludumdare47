using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPlatform : MonoBehaviour
{
    PlatformEffector2D pf2d;

    void Start()
    {
        pf2d = GetComponent<PlatformEffector2D>();
    }

   
    void Update()
    {
        
    }

    public void TogglePlatformDirection()
    {
        pf2d.surfaceArc = 0f;
        LeanTween.delayedCall(0.3f, () => pf2d.surfaceArc = 180f);
    }
}
