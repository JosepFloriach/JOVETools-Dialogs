using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DialogBehaviour))]
public class DialogTriggererOnCollider2DEvent : MonoBehaviour
{
    private enum EventType
    {
        OnTriggerEnter,
        OnTriggerExit
    };

    private enum Behaviour
    {
        Open,
        Close
    }

    [SerializeField] private EventType eventType;
    [SerializeField] private Behaviour behaviour;
    private DialogBehaviour dialog;
    private bool triggered = false;

    private void Awake()
    {
        dialog = GetComponent<DialogBehaviour>();
    }

    private void OnEnable()
    {
        triggered = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (eventType == EventType.OnTriggerEnter && other.tag == "Player" && !triggered)
        {
            if (behaviour == Behaviour.Open)
            {
                OpenDialog();
            }
            if (behaviour == Behaviour.Close)
            {
                CloseDialog();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (eventType == EventType.OnTriggerExit && other.tag == "Player" && !triggered)
        {
            if (behaviour == Behaviour.Open)
            {
                OpenDialog();
            }
            if (behaviour == Behaviour.Close)
            {
                CloseDialog();
            }
        }
    }

    private void OpenDialog()
    {
        DialogController.GetInstance().OpenDialogRequest(dialog.Setup);
        triggered = true;
    }

    private void CloseDialog()
    {
        DialogController.GetInstance().CloseDialog();
    }
}
