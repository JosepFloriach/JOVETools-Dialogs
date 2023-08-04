using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DialogControllerTests
{
    private DialogSetup dialogSetup1;
    private DialogSetup dialogSetup2;

    private int dialogStartedReceivedCount;
    private int dialogClosedReceivedCount;
    private int messageChangedReceivedCount;

    [SetUp]
    public void Setup()
    {
        IDialogMessage message1 = Substitute.For<IDialogMessage>();
        IDialogMessage message2 = Substitute.For<IDialogMessage>();

        MessageSetup messageSetup1 = new();
        MessageSetup messageSetup2 = new();

        dialogStartedReceivedCount = 0;
        dialogSetup1 = new();
        dialogSetup1.Messages.Add(messageSetup1);
        dialogSetup1.Messages.Add(messageSetup2);

        dialogSetup2 = new();
        dialogSetup2.Messages.Add(messageSetup1);
        dialogSetup2.Messages.Add(messageSetup2);

        DialogController.GetInstance().OnDialogStarted += OnDialogStarted;
    }

    [TearDown]
    public void TearDown()
    {
        DialogController.GetInstance().CancelAllRequests();
        DialogController.GetInstance().OnDialogStarted -= OnDialogStarted;
    }

    [Test]
    public void CallInstanceTwice_Expect_SameObject()
    {
        DialogController controller1 = DialogController.GetInstance();
        DialogController controller2 = DialogController.GetInstance();

        Assert.AreSame(controller1, controller2);
    }

    [UnityTest]
    public IEnumerator OpenDialogRequest_Expect_DialogControllerEnabled()
    {
        DialogController.GetInstance().OpenDialogRequest(dialogSetup1);
        yield return UpdateControllerForSeconds(0.0f);
        Assert.IsTrue(DialogController.GetInstance().IsEnabled);
    }

    [UnityTest]
    public IEnumerator OpenDialogRequest_Expect_DialogOpened()
    {
        DialogController.GetInstance().OpenDialogRequest(dialogSetup1);
        yield return UpdateControllerForSeconds(0.0f);
        Assert.AreEqual(1, dialogStartedReceivedCount);
    }

    [UnityTest]
    public IEnumerator OpenTwoDialogRequest_Expect_OneDialogOpenedThenAnotherOne()
    {
        DialogController.GetInstance().OpenDialogRequest(dialogSetup1);
        DialogController.GetInstance().OpenDialogRequest(dialogSetup2);
        yield return UpdateControllerForSeconds(0.0f);
        Assert.AreEqual(1, dialogStartedReceivedCount);
        DialogController.GetInstance().CloseDialog();
        yield return UpdateControllerForSeconds(1.0f);        
        Assert.AreEqual(2, dialogStartedReceivedCount);
    }

    private void OnDialogStarted()
    {
        dialogStartedReceivedCount++;
    }

    private IEnumerator UpdateControllerForSeconds(float seconds)
    {
        float currentTime = 0.0f;
        while (currentTime <= seconds)
        {
            float deltaTime = Time.deltaTime;
            currentTime += deltaTime;
            DialogController.GetInstance().Update(deltaTime);
            yield return null;
        }
    }
}
