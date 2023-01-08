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
        base.Idle();

        if (Input.anyKey == true)
        {
            _state = PlayerState.Moving;
        }
    }

    protected override void Move()
    {
        base.Move();


        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * Time.deltaTime * _moveSpeed;
            _sprite.flipX = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * Time.deltaTime * _moveSpeed;
            _sprite.flipX = true;
        }

        if (Input.GetKey(KeyCode.Z) && _jumpCount < 1)
        {
            _isJumping = true;

            _jumpCount++;
            _rigid.velocity = Vector2.zero;
            _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        _state = PlayerState.Idle;
    }

    protected override void Die()
    {
        base.Die();

        GameObject player = GameObject.Find("MyPlayer");
        GameObject.Destroy(player, 1f);
    }

}
