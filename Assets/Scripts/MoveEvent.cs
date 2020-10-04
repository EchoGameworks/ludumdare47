using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEvent : EventData
{
    public float MoveData;

    public MoveEvent (float time, float moveData)
    {
        TimeTriggered = time;
        MoveData = moveData;
    }
}
