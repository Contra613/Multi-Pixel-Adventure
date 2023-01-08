using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : Controller
{
    [SerializeField]
    protected int _cameraFocus = -10;

    protected PlayerState _state = PlayerState.Idle;

    protected override void Init()
    {
        base.Init();

        UpdateAnimation();
    }

    protected override void UpdateController()
    {
        switch (_state)
        {
            case PlayerState.Idle:
                Idle();
                break;
            case PlayerState.Moving:
                Move();
                break;
            case PlayerState.Die:
                Die();
                break;
        }
    }

    protected override void UpdateAnimation()
    {
        switch (_state)
        {
            case PlayerState.Idle:
                if(_isJumping == false) 
                    _animator.Play("IDLE");

                if (_isJumping == true && _rigid.velocity.y <= 0)
                    _animator.Play("FALL");
                break;
            case PlayerState.Moving:
                if (_isJumping == false)
                    _animator.Play("RUN");
                
                if(_rigid.velocity.y > 0)
                    _animator.Play("JUMP");
                else if(_isJumping == true && _rigid.velocity.y <= 0)
                    _animator.Play("FALL");
                break;
            case PlayerState.Die:
                _animator.Play("DIE");
                break;
        }
    }

    protected virtual void Idle()
    {
       
    }

    protected virtual void Move()
    {
       
    }

    protected virtual void Die()
    {   
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.contacts[0].normal.y >0.7f)
        {
            _isGrounded = true;
            _isJumping = false;
            _jumpCount = 0;
        }
    }
}


