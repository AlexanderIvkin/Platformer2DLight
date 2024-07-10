using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class AdvancedPlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _jumpForce = 8.0f;
    [SerializeField] private float _slopeForce = 5.0f;
    [SerializeField] private float _slopeRayLength = 1.5f;
    [SerializeField] private float _rotationSpeed = 300.0f;
    [SerializeField] private InputScaner _scaner;

    private CharacterController _controller;
    private Vector3 _moveDirection = Vector3.zero;

    private void OnEnable()
    {
        _scaner.Moved += SetMoveDirection;
        _scaner.Jumped += Jump;
    }

    private void OnDisable()
    {
        _scaner.Moved -= SetMoveDirection;
        _scaner.Jumped -= Jump;
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_controller.isGrounded)
        {
            _controller.Move(_moveDirection * Time.deltaTime);
        }

    }

    private void SetMoveDirection(float direction)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
        inputDirection = transform.TransformDirection(inputDirection);
        _moveDirection = inputDirection * _speed;
    }

    private void Jump()
    {
        _moveDirection.y = _jumpForce;
    }

    private void Slope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _slopeRayLength) == false)
        {
            return;
        }

        if (Vector3.Angle(hit.normal, Vector3.up) > _controller.slopeLimit)
        {
            _moveDirection.x += (1f - hit.normal.y) * hit.normal.x * _slopeForce;
            _moveDirection.z += (1f - hit.normal.y) * hit.normal.z * _slopeForce;
            _moveDirection.y -= _slopeForce;
        }
    }

    private void Flip()
    {
        float facedRightFactor = -1.0f;


    }
}
