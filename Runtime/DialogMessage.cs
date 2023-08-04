using System;

public class DialogMessage: IDialogMessage
{
    public Action MessageFinishedByTime;
    
    private float currentTime = 0.0f;

    public MessageSetup Setup
    {
        get;
        private set;
    }

    public DialogMessage(MessageSetup setup)
    {
        this.Setup = setup;
    }

    public void Enable()
    {
        currentTime = 0.0f;
        InvokeOnOpenCallbacks();        
    }

    public void Disable()
    {
        InvokeOnCloseCallbacks();
    }

    public void Update(float diffTime)
    {
        if (Setup.TriggerNextMessageByTime)
        {
            currentTime += diffTime;
            if (currentTime >= Setup.Time)
            {
                SendMessageFinishedByTimeEvent();
            }
        }
    }

    private void InvokeOnOpenCallbacks()
    {
        if (Setup.CallbacksOnOpen.Count == 0)
        {
            return;
        }

        foreach (var callback in Setup.CallbacksOnOpen)
        {
            callback.Invoke();
        }  
    }

    private void InvokeOnCloseCallbacks()
    {
        if (Setup.CallbacksOnClose.Count == 0)
        {
            return;
        }
            
        foreach (var callback in Setup.CallbacksOnClose)
        {
            callback.Invoke();
        }
    }

    private void SendMessageFinishedByTimeEvent()
    {
        MessageFinishedByTime?.Invoke();
    }
}
