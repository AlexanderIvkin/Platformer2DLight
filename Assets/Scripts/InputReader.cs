using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const string Horizontal = "Horizontal";

    public event Action<float> Moved;
    public event Action Jumped;
    public event Action Digging;

    private KeyCode _jumpButton = KeyCode.Space;
    private KeyCode _digButton = KeyCode.Q;

    private void Update()
    {
        ReadJumpButton();
        ReadMoveButton();
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

    private void ReadDigButton()
    {
        if (Input.GetKeyDown(_digButton))
        {
            Digging?.Invoke();
        }
    }
}
