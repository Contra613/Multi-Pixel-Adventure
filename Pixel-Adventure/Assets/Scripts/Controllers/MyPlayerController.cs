using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MyPlayerController : PlayerController
{
    [SerializeField]
    protected int _cameraFocus = -10;


    // 1. State, Dir, CellPos
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        base.UpdateController();

        switch (State)
        {
            case PlayerState.Idle:
                GetDirInput();
                break;
            case PlayerState.Jumping:
                GetDirInput();
                break;
            case PlayerState.Moving:
                GetDirInput();
                break;
        }


        if (Input.GetKey(KeyCode.Escape))
            State = PlayerState.Die;
    }

    /*void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y + 2, _cameraFocus);
    }*/

    protected override void Idle()
    {
        base.Idle();
    }

    void GetDirInput()
    {
        /*if (Input.GetKey(KeyCode.Z))
        {
            Dir = MoveDir.Up;
        }*/

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            Dir = MoveDir.None;
        }

        CheckUpdatedFlag();
    }

    /*protected override void Moving()
    {
        base.Moving();

        CellPos = new Vector3(transform.position.x, transform.position.y, 0);

        CheckUpdatedFlag();
        PlayerState prevState = State;
        Vector3 prevCellPos = CellPos;


        CellPos = new Vector3(transform.position.x, transform.position.y, 0);

        if (State != PlayerState.Moving)
            State = PlayerState.Idle;

        if (prevState != State || CellPos != prevCellPos)
        {
            C_Move movePacket = new C_Move();
            movePacket.PosInfo = PosInfo;
            Managers.Network.Send(movePacket);
        }

    }*/


    // 움직임이 있으면 C_Move Packet을 Server에 전송
    void CheckUpdatedFlag()
    {
        if (_updated)
        {
            C_Move movePacket = new C_Move();
            movePacket.PosInfo = PosInfo;
            Managers.Network.Send(movePacket);
            _updated = false;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            _isGrounded = true;
            _isJumping = false;
            _jumpCount = 0;
        }
    }

}
