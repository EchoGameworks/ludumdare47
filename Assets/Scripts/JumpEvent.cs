using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEvent : EventData
{
    public bool JumpData;

    public JumpEvent(float time, bool data)
    {
        TimeTriggered = time;
        JumpData = data;
    }
}
