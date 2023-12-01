using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private bool _facingRight = true;
    [SerializeField] private bool _grounded = false;

    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpTime;
    private float _jumpTimeCounter;
    private float _checkGroundRadius = 0.1f;
    private float _checkCeilingRadius = 0.1f;

    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private Collider2D _crouchDisableCollider;


    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool _isCrouching = false;

    private void Awake()
    {
        //_transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }


    private void FixedUpdate()
    {
        bool isGrounded = _grounded;
        _grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _checkGroundRadius, _whatIsGround);
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject != gameObject)
            {
                _grounded = true;
                if (!isGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump, bool jumpHigh)
    {
        if (!crouch)
        {
            if (Physics2D.OverlapCircle(_ceilingCheck.position, _checkCeilingRadius, _whatIsGround))
            {
                crouch = true;
            }
        }

        if(crouch)
        {
            if(!_isCrouching)
            {
                _isCrouching = true;
                OnCrouchEvent.Invoke(true);
            }

            move *= _crouchSpeed;

            if(_crouchDisableCollider != null)
                _crouchDisableCollider.enabled = false;
        }
        else
        {
            if (_crouchDisableCollider != null)
                _crouchDisableCollider.enabled = true;

            if(_isCrouching)
            {
                _isCrouching = false;
                OnCrouchEvent.Invoke(false);
            }
        }

        Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
        _rigidbody.velocity = targetVelocity;

        if (move < 0 && _facingRight)
        {
            Flip();
        }
        else if (move > 0 && !_facingRight)
        {
            Flip();
        }

        if(_grounded && jump)
        {
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
            _jumpTimeCounter = _jumpTime;
            _grounded = false;
        }

        
        if (jumpHigh)
        {
            if (_jumpTimeCounter > 0)
            {
                _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
                _jumpTimeCounter -= Time.deltaTime;
                _grounded = false;
            }
            else jumpHigh = false;
        }

        
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

}
