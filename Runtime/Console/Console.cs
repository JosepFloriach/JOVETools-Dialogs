using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Console : MonoBehaviour
{
    public InputAction toggleConsoleAction;
    public InputAction commitCommandAction;

    private Dictionary<string, IConsoleCommand> commands = new Dictionary<string, IConsoleCommand>();
    private bool showConsole = false;
    private string input;

    private void Awake()
    {
        FillCommandList();
    }

    private void OnEnable()
    {
        toggleConsoleAction.performed += OnToggleConsole;
        commitCommandAction.performed += OnCommitCommand;
        toggleConsoleAction.Enable();
        commitCommandAction.Enable();
    }

    private void OnDisable()
    {
        toggleConsoleAction.performed -= OnToggleConsole;
        commitCommandAction.performed -= OnCommitCommand;
        toggleConsoleAction.Disable();
        commitCommandAction.Disable();
    }

    private void FillCommandList()
    {
        IConsoleCommand[] commandsArray = FindObjectsOfType<MonoBehaviour>().OfType<IConsoleCommand>().ToArray();
        foreach (IConsoleCommand command in commandsArray)
        {
            commands.Add(command.GetCommandName(), command);
        }
    }

    public void OnToggleConsole(CallbackContext context)
    {
        showConsole = !showConsole;
    }

    public void OnCommitCommand(CallbackContext context)
    {
        if (showConsole)
        {
            string[] splitCommand = input.Split(' ');
            string commandName = splitCommand[0];
            if (commands.ContainsKey(commandName))
            {
                if (commands[commandName].Execute(splitCommand))
                {
                    Debug.Log("Console command " + commandName + " executed successfuly");
                }
                else
                {
                    Debug.LogError("Console command " + commandName + " could not be executed properly");
                }
            }
            else
            {
                Debug.LogError("Console command " + commandName + " not recognized");
            }

            showConsole = !showConsole;
            input = "";
        }
    }

    private void OnGUI()
    {
        if (!showConsole)
        {
            return;
        }

        float y = 0.0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.SetNextControlName("ConsoleInputText");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        GUI.FocusControl("ConsoleInputText");

    }
}
