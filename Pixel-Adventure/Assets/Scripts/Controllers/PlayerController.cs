using Google.Protobuf.Protocol;
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
    }

    protected override void UpdateController()
    {
        base.UpdateController();
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
    }

    protected override void Idle()
    {
        if (Dir != MoveDir.None)
        {
            State = PlayerState.Moving;
            return;
        }
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


