using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogQueue
{
    public event Action<DialogSetup> ReadyForDialog;
    
    private Queue<DialogSetup> requests = new();

    private bool openNextInstantly;
    private bool readyForNext;
    private float timeBetweenMessages;
    private float timeSinceClosed;

    public DialogQueue()
    {
        readyForNext = true;
        openNextInstantly = true;
        timeBetweenMessages = 1.0f;
    }

    public void Clear()
    {
        requests.Clear();
    }

    public void AddRequest(DialogSetup setup)
    {
        requests.Enqueue(setup);
    }

    public void CloseDialog()
    {
        readyForNext = true;
        timeSinceClosed = 0.0f;
        openNextInstantly = requests.Count == 0;
    }

    public void Update(float deltaTime)
    {
        if (readyForNext && requests.Count > 0)
        {
            timeSinceClosed += deltaTime;
            if (timeSinceClosed >= timeBetweenMessages || openNextInstantly)
            {
                ReadyForDialog?.Invoke(requests.Peek());
                requests.Dequeue();
                readyForNext = false;
                timeSinceClosed = 0.0f;
            }
        }
    }
}
