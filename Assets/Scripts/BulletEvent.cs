using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEvent : EventData
{
    public Vector2 MouseScreenPos;

    public BulletEvent(float time, Vector2 mouseScreenPos)
    {
        TimeTriggered = time;
        MouseScreenPos = mouseScreenPos;
    }
}
