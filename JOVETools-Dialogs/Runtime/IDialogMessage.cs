using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogMessage
{   
    MessageSetup Setup { get; }
    void Enable();
    void Disable();
    void Update(float deltaTime);
}
