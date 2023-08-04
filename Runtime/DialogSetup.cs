using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogSetup
{
    public bool TriggerOnEnable;
    public bool TriggerOnStart;
    public bool TriggerJustOnce;
    public List<MessageSetup> Messages = new();
}
