using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : Controller
{
    [SerializeField]
    protected int _cameraFocus = -10;

    protected float _hp = 10;

    PlayerState _state = PlayerState.Idle;

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        if (Input.GetKey(KeyCode.Escape))
            _state = PlayerState.Die;


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

    void Idle()
    {
        if (Input.anyKey == true)
        {
            _state = PlayerState.Moving;
        }
        else if(_isJumping == false)
        {
            _animator.Play("IDLE");
        }
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * Time.deltaTime * _moveSpeed;
            _sprite.flipX = false;

            if(_isJumping == false)
                _animator.Play("RUN");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * Time.deltaTime * _moveSpeed;
            _sprite.flipX = true;

            if (_isJumping == false)
                _animator.Play("RUN");
        }
        
        if (Input.GetKey(KeyCode.Z) && _jumpCount < 1)
        {
            _isJumping = true;

            _jumpCount++;
            _rigid.velocity = Vector2.zero;
            _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

            if (_rigid.velocity.y > 0)
                _animator.Play("JUMP");
        }
        else if (_isJumping == true && _rigid.velocity.y < 0)
            _animator.Play("FALL");


        _state = PlayerState.Idle;
    }

    void Die()
    {   
        _animator.Play("DIE");
        
        GameObject player = GameObject.Find("Player");
        GameObject.Destroy(player, 1f);
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


