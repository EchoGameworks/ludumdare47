using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEvent : EventData
{
    public DropEvent(float time)
    {
        TimeTriggered = time;
    }
}
