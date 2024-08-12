using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScaner : MonoBehaviour
{
    private const string Horizontal = "Horizontal";

    public event Action<float> Moved;
    public event Action Jumped;
    public event Action Attacked;
    public event Action Digging;

    private KeyCode _jumpButton = KeyCode.Space;
    private KeyCode _digButton = KeyCode.Q;
    private KeyCode _useButton = KeyCode.E;

    private void Update()
    {
        ReadJumpButton();
        ReadMoveButton();
        ReadUseButton();
        ReadDigButton();
    }

    private void ReadJumpButton()
    {
        if (Input.GetKeyUp(_jumpButton))
        {
            Jumped?.Invoke();
        }
    }

    private void ReadMoveButton()
    {
        float direction = Input.GetAxis(Horizontal);

        if (direction != 0)
        {
            Moved?.Invoke(Input.GetAxis(Horizontal));
        }
    }

    private void ReadUseButton()
    {
        if (Input.GetKeyDown(_useButton))
        {
            Attacked?.Invoke();
        }
    }

    private void ReadDigButton()
    {
        if (Input.GetKeyDown(_digButton))
        {
            Digging?.Invoke();
        }
    }
}
