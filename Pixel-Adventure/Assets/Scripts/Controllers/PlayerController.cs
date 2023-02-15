using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : Controller
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateAnimation()
    {
        if (State == PlayerState.Idle)
        {
            switch (_lastDir)
            {
                case MoveDir.Left:
                    _animator.Play("IDLE");
                    _sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("IDLE");
                    _sprite.flipX = false;
                    break;
            }

        }
        else if (State == PlayerState.Moving)
        {
            if (_isJumping == false)
            {
                switch (Dir)
                {
                    case MoveDir.Left:
                        _animator.Play("RUN");
                        _sprite.flipX = true;
                        break;

                    case MoveDir.Right:
                        _animator.Play("RUN");
                        _sprite.flipX = false;
                        break;
                }
            }
        }
        else if (State == PlayerState.Jumping)
        {
            if (_isJumping == true)
            {
                switch (Dir)
                {
                    case MoveDir.Left:
                        _animator.Play("JUMP");
                        _sprite.flipX = true;
                        break;

                    case MoveDir.Right:
                        _animator.Play("JUMP");
                        _sprite.flipX = false;
                        break;
                }
            }
        }
        else if (State == PlayerState.Die)
        {
            _animator.Play("DIE");
        }
    }

    protected override void UpdateController()
    {
        base.UpdateController();
    }

    protected override void Idle()
    {
        if (Dir != MoveDir.None)
        {
            State = PlayerState.Moving;
            return;
        }
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.contacts[0].normal.y >0.7f)
        {
            _isGrounded = true;
            _isJumping = false;
            _jumpCount = 0;
        }
    }
}


