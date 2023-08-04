using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController
{
    public Action OnMessageChanged;
    public Action OnDialogStarted;
    public Action OnDialogFinished;

    private List<IDialogMessage> messages = new();
    private DialogSetup setup = null;
    private int currentMessageIdx;
    private DialogQueue dialogQueue;

    static private DialogController instance = null;

    public bool IsEnabled
    {
        get;
        private set;
    }

    public DialogSetup CurrentDialog
    {
        get
        {
            return setup;
        }
    }

    public string CurrentMessage
    {
        get
        {
            return messages[currentMessageIdx].Setup.LocalizedMessage.GetLocalizedString();
        }
    }

    public Sprite CurrentAvatar
    {
        get
        {
            return messages[currentMessageIdx].Setup.Avatar;
        }
    }

    public bool CurrentNextButtonEnabled
    {
        get
        {
            return messages[currentMessageIdx].Setup.NextButtonEnabled;
        }
    }

    public bool CurrentPreviousButtonEnabled
    {
        get
        {
            return messages[currentMessageIdx].Setup.PrevButtonEnabled;
        }
    }

    static public DialogController GetInstance()
    {
        if (instance == null)
        {
            instance = new();
        }
        return instance;
    }

    private DialogController()
    {
        dialogQueue = new();
        dialogQueue.ReadyForDialog += OnReadyForNextDialog;
    }

    public void CancelAllRequests()
    {
        IsEnabled = false;
        dialogQueue.Clear();
        this.setup = null;
        messages.Clear();
    }

    public void OpenDialogRequest(DialogSetup setup)
    {
        dialogQueue.AddRequest(setup);        
    }

    private void OnReadyForNextDialog(DialogSetup setup)
    {
        OpenDialog(setup);
    }

    private void OpenDialog(DialogSetup setup)
    {
        this.setup = setup;
        IsEnabled = true;
        InitializeDialog(setup);        
        currentMessageIdx = 0;
        messages[currentMessageIdx].Enable();
        SendDialogStartedEvent();
    }

    public void NextMessage()
    {
        if (!IsEnabled)
        { 
            return;
        }

        if (currentMessageIdx == setup.Messages.Count - 1)
        {
            if (messages.Count <= currentMessageIdx)
            {
                return;
            }
            CloseDialog();            
        }
        else
        {
            messages[currentMessageIdx].Disable();
            currentMessageIdx++;
            messages[currentMessageIdx].Enable();
            OnMessageChanged?.Invoke();
        }
    }

    public void PreviousMessage()
    {
        if (!IsEnabled)
        {
            return;
        }

        if (currentMessageIdx > 0)
        {
            messages[currentMessageIdx].Disable();
            currentMessageIdx--;
            messages[currentMessageIdx].Enable();
            OnMessageChanged?.Invoke();
        }
    }

    public void CloseDialog()
    {
        if (messages.Count <= currentMessageIdx)
        {
            return;
        }
        messages[currentMessageIdx].Disable();
        SendDialogFinishedEvent();
        DestroyDialog();
        dialogQueue.CloseDialog();
        currentMessageIdx = 0;
        IsEnabled = false;
    }

    public void Update(float deltaTime)
    {
        if (IsEnabled)
        {            
            messages[currentMessageIdx].Update(deltaTime);
        }
        dialogQueue.Update(deltaTime);
    }

    private void InitializeDialog(DialogSetup setup)
    {
        foreach (var message in setup.Messages)
        {
            var dialog = new DialogMessage(message);
            messages.Add(dialog);
            dialog.MessageFinishedByTime += OnMessageFinishByTime;
        }
    }

    private void DestroyDialog()
    {
        messages.Clear();
    }

    private void OnMessageFinishByTime()
    {
        NextMessage();
    }

    private void SendDialogStartedEvent()
    {
        OnDialogStarted?.Invoke();
    }

    private void SendDialogFinishedEvent()
    {
        OnDialogFinished?.Invoke();
    }
}
