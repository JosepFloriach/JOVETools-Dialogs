using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class DialogMessageTests
{
    MessageSetup setup;
    DialogMessage message;

    UnityEvent onOpenEvent;
    UnityEvent onCloseEvent;

    bool openCallBackCalled;
    bool closeCallBackCalled;
    bool startCallBackCalled;
    bool finishCallBackCalled;

    [SetUp]
    public void SetUp()
    {
        setup = new();

        onOpenEvent = new UnityEvent();
        onCloseEvent = new UnityEvent();        
        onOpenEvent.AddListener(OnOpen);
        onCloseEvent.AddListener(OnClose);

        message = new DialogMessage(setup);

        openCallBackCalled = false;
        closeCallBackCalled = false;
        startCallBackCalled = false;
        finishCallBackCalled = false;
    }

    [TearDown]
    public void TearDown()
    {
        onOpenEvent.RemoveListener(OnOpen);
        onOpenEvent.RemoveListener(OnClose);
        setup.CallbacksOnOpen.Clear();
        setup.CallbacksOnClose.Clear();

        message.MessageFinishedByTime -= OnFinishByTime;
    }

    [Test]
    public void OnEnable_Expect_OnOpenCallback()
    {
        setup.CallbacksOnOpen.Add(onOpenEvent);
        message.Enable();
        Assert.IsTrue(openCallBackCalled);
        Assert.IsFalse(closeCallBackCalled);
    }

    [Test]
    public void OnEnable_Expect_NoCallBacks()
    {
        message.Enable();
        Assert.IsFalse(openCallBackCalled);
        Assert.IsFalse(closeCallBackCalled);
    }

    [Test]
    public void OnDisable_Expect_OnCloseCallback()
    {
        setup.CallbacksOnClose.Add(onCloseEvent);
        message.Enable();
        message.Disable();
        Assert.IsTrue(closeCallBackCalled);
        Assert.IsFalse(openCallBackCalled);
    }

    [Test]
    public void OnDisable_Expect_NoCallbacks()
    {
        message.Enable();
        message.Disable();
        Assert.IsFalse(closeCallBackCalled);
        Assert.IsFalse(openCallBackCalled);
    }

    [UnityTest]
    public IEnumerator TimedMessage_Expect_FinishedMessageAfterSeconds()
    {
        message.MessageFinishedByTime += OnFinishByTime;
        setup.TriggerNextMessageByTime = true;
        setup.Time = 1.0f;
        message.Enable();
        float time = 0.0f;

        while(time < 1.0f)
        {
            float delta = Time.deltaTime;
            message.Update(delta);
            time += delta;
            yield return null;
        }
        Assert.IsTrue(finishCallBackCalled);
    }

    [UnityTest]
    public IEnumerator NonTimedMessage_Expect_NonFinishedMessageAfterSeconds()
    {
        message.MessageFinishedByTime += OnFinishByTime;
        setup.TriggerNextMessageByTime = false;
        setup.Time = 1.0f;
        message.Enable();
        float time = 0.0f;

        while (time < 1.0f)
        {
            float delta = Time.deltaTime;
            message.Update(delta);
            time += delta;
            yield return null;
        }
        Assert.IsFalse(finishCallBackCalled);
    }

    private void OnFinishByTime()
    {
        finishCallBackCalled = true;
    }

    private void OnOpen()
    {
        openCallBackCalled = true;
    }

    private void OnClose()
    {
        closeCallBackCalled = true;
    }

}
