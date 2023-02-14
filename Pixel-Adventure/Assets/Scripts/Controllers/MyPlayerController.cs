using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MyPlayerController : PlayerController
{
    
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        base.UpdateController();

        if (Input.GetKey(KeyCode.Escape))
            _state = PlayerState.Die;
    }

    /*void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y + 2, _cameraFocus);
    }*/

    protected override void Idle()
    {
        if (Input.anyKey == true)
        {
            State = PlayerState.Moving;
        }
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * Time.deltaTime * _moveSpeed;
            Dir = MoveDir.Right;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * Time.deltaTime * _moveSpeed;
            Dir = MoveDir.Left;
        }

        if (Input.GetKey(KeyCode.Z) && _jumpCount < 1)
        {
            _isJumping = true;

            _jumpCount++;
            _rigid.velocity = Vector2.zero;
            _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

    }

    protected override void Moveing()
    {
        /*PlayerState prevState = State;
        Vector3 prevCellPos = CellPos;*/

        GetInput();

        CellPos = new Vector3(transform.position.x, transform.position.y, 0);

        /*if(State != PlayerState.Moving)
            _state = PlayerState.Idle;*/

        /*if (prevState != State || CellPos != prevCellPos)
        {
            C_Move movePacket = new C_Move();
            movePacket.PosInfo = PosInfo;
            Managers.Network.Send(movePacket);
        }*/

        CheckUpdatedFlag();
    }

    protected override void Jumping()
    {
        GetInput();

        if (_rigid.velocity.y <= 0)
            State = PlayerState.Falling;
    }

    protected override void Falling()
    {
        GetInput();

        if (_isGrounded == true && _isJumping == false)
            State = PlayerState.Idle;
    }


    protected override void Die()
    {
        GameObject player = GameObject.Find("MyPlayer");
        GameObject.Destroy(player, 1f);
    }

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
}
