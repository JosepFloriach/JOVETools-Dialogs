using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DialogQueueTests
{
    private DialogSetup dialog1;
    private DialogSetup dialog2;
    private int readyEventCount;
    private DialogQueue dialogQueue;

    [SetUp]
    public void SetUp()
    {
        dialogQueue = new();

        dialog1 = new();
        dialog2 = new();

        dialogQueue.ReadyForDialog += OnReadyReceived;

        readyEventCount = 0;
    }

    [TearDown]
    public void TearDown()
    {
        dialogQueue.Clear();
    }

    [UnityTest]
    public IEnumerator QueueIsEmpty_Expect_NoReady()
    {
        yield return UpdateQueueForSeconds(1.0f);
        Assert.AreEqual(readyEventCount, 0);
    }

    [UnityTest]
    public IEnumerator QueueHasOneRequest_Expect_OneEventCount()
    {
        dialogQueue.AddRequest(dialog1);
        // With one request, the dialog should be triggered instantly, without waiting.
        yield return UpdateQueueForSeconds(0.0f);
        Assert.AreEqual(readyEventCount, 1);
    }

    [UnityTest]
    public IEnumerator QueueHasOneRequestThenClear_Expect_NoReady()
    {
        dialogQueue.AddRequest(dialog1);
        dialogQueue.Clear();
        yield return UpdateQueueForSeconds(1.0f);
        Assert.AreEqual(readyEventCount, 0);
    }

    [UnityTest]
    public IEnumerator QueueHasTwoRequests_Expect_TwoReadys()
    {
        dialogQueue.AddRequest(dialog1);
        dialogQueue.AddRequest(dialog2);
        yield return UpdateQueueForSeconds(0.0f);
        Assert.AreEqual(readyEventCount, 1);
        dialogQueue.CloseDialog();
        Assert.AreEqual(readyEventCount, 1);
        yield return UpdateQueueForSeconds(1.1f);
        Assert.AreEqual(readyEventCount, 2);
        dialogQueue.CloseDialog();
        Assert.AreEqual(readyEventCount, 2);
    }

    [UnityTest]
    public IEnumerator QueueHastTwoSeparateRequests_Expect_TwoReadysInstantly()
    {
        dialogQueue.AddRequest(dialog1);
        yield return UpdateQueueForSeconds(0.0f);
        Assert.AreEqual(readyEventCount, 1);
        dialogQueue.CloseDialog();
        Assert.AreEqual(readyEventCount, 1);
        dialogQueue.AddRequest(dialog2);
        yield return UpdateQueueForSeconds(0.0f);
        Assert.AreEqual(readyEventCount, 2);
        dialogQueue.CloseDialog();
        Assert.AreEqual(readyEventCount, 2);
    }

    private IEnumerator UpdateQueueForSeconds(float seconds)
    {
        float currentTime = 0.0f;
        while (currentTime <= seconds)
        {
            float deltaTime = Time.deltaTime;
            currentTime += deltaTime;
            dialogQueue.Update(deltaTime);
            yield return null;
        }
    }

    private void OnReadyReceived(DialogSetup setup)
    {
        readyEventCount++;
    }

}
