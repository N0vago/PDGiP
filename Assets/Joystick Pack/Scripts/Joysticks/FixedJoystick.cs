using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.UI;

public class FixedJoystick : Joystick
{
    private void Update()
    {
        InputModeManager.Instance.Mobile.Horizontal = Horizontal;
    }
}