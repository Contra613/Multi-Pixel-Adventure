using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Controller : MonoBehaviour
{
    public int ID { get; set; }

    protected bool _updated = false;

    PositionInfo _positionInfo = new PositionInfo();
    public PositionInfo PosInfo
    {
        get { return _positionInfo; }
        set
        {
            if (_positionInfo.Equals(value))
                return;

            _positionInfo = value;
        }
    }

    public Vector3 CellPos
    {
        get { return new Vector3(PosInfo.PosX, PosInfo.PosY, 0); }
        set
        {
            PosInfo.PosX = value.x;
            PosInfo.PosY = value.y;

            _updated = true;
        }
    }

    public PlayerState State
    {
        get { return PosInfo.State; }
        set
        {
            if (PosInfo.State == value)
                return;

            PosInfo.State = value;
            UpdateAnimation();
            _updated = true;
        }
    }

    protected MoveDir _lastDir = MoveDir.Down;
    public MoveDir Dir
    {
        get { return PosInfo.MoveDir; }
        set
        {
            if (PosInfo.MoveDir == value)
                return;

            PosInfo.MoveDir = value;
            if (value != MoveDir.None)
                _lastDir = value;

            UpdateAnimation();
            _updated = true;
        }
    }

    [SerializeField]
    protected float _moveSpeed = 15.0f;
    [SerializeField]
    protected float _jumpForce = 6.5f;

    protected int _jumpCount = 0;

    protected bool _isJumping = false;
    protected bool _isGrounded = false;

    protected SpriteRenderer _sprite;
    protected CapsuleCollider2D _collider;
    protected Rigidbody2D _rigid;
    protected Animator _animator;

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
        //UpdateAnimation();
    }

    protected virtual void Init()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();


        UpdateAnimation();
    }

    protected virtual void UpdateController() 
    {
        switch (State)
        {
            case PlayerState.Idle:
                Idle();
                break;
            case PlayerState.Moving:
                Moveing();
                break;
            case PlayerState.Jumping:
                Jumping();
                break;
            case PlayerState.Falling:
                Falling();
                break;
            case PlayerState.Die:
                Die();
                break;
        }
    }

    protected virtual void Idle()
    {

    }

    protected virtual void Moveing()
    {

    }
    protected virtual void Jumping()
    {

    }
    protected virtual void Falling()
    {

    }

    protected virtual void Die()
    {

    }

    protected virtual void UpdateAnimation()
    {
        /*switch (_state)
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
        }*/

        if (State == PlayerState.Idle && _isJumping == false)
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
        else if (State == PlayerState.Jumping)
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
        else if (State == PlayerState.Falling)
        {
            switch (Dir)
            {
                case MoveDir.Left:
                    _animator.Play("FALL");
                    _sprite.flipX = true;
                    break;

                case MoveDir.Right:
                    _animator.Play("FALL");
                    _sprite.flipX = false;
                    break;
            }
        }
        else if (State == PlayerState.Die)
        {
            _animator.Play("DIE");
        }

    }
}
