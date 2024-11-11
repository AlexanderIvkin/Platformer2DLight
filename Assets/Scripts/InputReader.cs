using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const string Horizontal = "Horizontal";

    private bool _isJump;
    private bool _isDig;

    private KeyCode _jumpButton = KeyCode.Space;
    private KeyCode _digButton = KeyCode.Q;

    public float Direction { get; private set; }

    private void Update()
    {
        ReadJumpButton();
        ReadMoveButton();
        ReadDigButton();
    }

    public bool GetIsJump() => GetBoolAsTrigger(ref _isJump);

    public bool GetIsDig() => GetBoolAsTrigger(ref _isDig);

    private void ReadJumpButton()
    {
        if (Input.GetKeyUp(_jumpButton))
        {
            _isJump = true;
        }
    }

    private void ReadMoveButton()
    {
        Direction = Input.GetAxis(Horizontal);
    }

    private void ReadDigButton()
    {
        if (Input.GetKeyDown(_digButton))
        {
            _isDig = true;
        }
    }

    private bool GetBoolAsTrigger(ref bool value)
    {
        bool localValue = value;
        value = false;

        return localValue;
    }
}
