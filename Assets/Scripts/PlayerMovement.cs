using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public PlayerController controller;
    public PlayerController controller;

    private float _horizontalMove;
    private bool _jump = false;
    private bool _crouch = false;
    private bool _jumpHigh = false;

    [SerializeField]private float _speed;

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _speed;

        if(Input.GetKeyDown(KeyCode.W))
        {
            _jump = true;
        }
        if(Input.GetKey(KeyCode.W))
        {
            _jumpHigh = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            _jump = false;
            _jumpHigh = false;
        }

            if (Input.GetKeyDown(KeyCode.S))
        { 
            _crouch = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            _crouch = false;
        }

    }

    private void FixedUpdate()
    {
        controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump, _jumpHigh);
        _jump = false;
    }



}
