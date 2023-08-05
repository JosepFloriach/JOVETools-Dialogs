using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a command that can be executed through the console.
/// </summary>
public interface IConsoleCommand
{
    /// <summary>
    /// Provides the command name. This is the name that needs to be written in the console 
    /// when calling this command.
    /// </summary>
    /// <returns>String representing the command name.</returns>
    string GetCommandName();

    /// <summary>
    /// Provides a description for the command. This description must provide as well a description 
    /// of each parameter needed for the command, as well for the optional ones.
    /// </summary>
    /// <returns>String represeting the whole help description</returns>
    string GetHelp();

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="arguments">String array with each of the provided arguments (optional ones should be at the end)</param>
    /// <returns>True if the command has been executed succesfuly. False otherwise.</returns>
    bool Execute(params string[] arguments);
}
