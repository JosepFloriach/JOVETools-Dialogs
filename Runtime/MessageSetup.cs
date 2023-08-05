using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

[Serializable]
public class MessageSetup
{
    public Sprite Avatar;
    public LocalizedString LocalizedMessage;

    public bool NextButtonEnabled;
    public bool PrevButtonEnabled;

    public bool TriggerNextMessageByTime;
    public float Time;

    public List<UnityEvent> CallbacksOnOpen = new();
    public List<UnityEvent> CallbacksOnClose = new();
}
