using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBehaviour : MonoBehaviour
{
    public DialogSetup Setup;

    private void OnEnable()
    {
        if (Setup.TriggerOnEnable)
        {
            DialogController.GetInstance().OpenDialogRequest(Setup);
        }
    }

    private void OnDestroy()
    {
        DialogController.GetInstance().CancelAllRequests();
    }

    private void Start()
    {
        if (Setup.TriggerOnStart)
        {
            DialogController.GetInstance().OpenDialogRequest(Setup);
        }
    }
	
	private void Update()
    {
        DialogController.GetInstance().Update(Time.deltaTime);
    }
}
