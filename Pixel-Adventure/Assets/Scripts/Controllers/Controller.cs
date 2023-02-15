using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Controller : MonoBehaviour
{
    public int ID { get; set; }

    [SerializeField]
    protected float _moveSpeed = 15.0f;
    [SerializeField]
    protected float _jumpForce = 6.5f;

    protected int _jumpCount = 0;

    protected bool _isJumping = false;
    protected bool _isGrounded = false;

    protected SpriteRenderer _sprite;
    protected Rigidbody2D _rigid;
    protected Animator _animator;

    protected bool _updated = false;

    PositionInfo _positionInfo = new PositionInfo();
    public PositionInfo PosInfo
    {
        get { return _positionInfo; }
        set
        {
            if (_positionInfo.Equals(value))
                return;

            //_positionInfo = value;
            CellPos = new Vector3(value.PosX, value.PosY, 0);
            State = value.State;
            Dir = value.MoveDir;
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

    protected MoveDir _lastDir = MoveDir.None;
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

    // TODO 
    // - Fall Animation 구현
    protected virtual void UpdateAnimation()
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
            if(_isJumping == false)
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

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();

        Debug.Log(State);
        Debug.Log(Dir);
    }

    protected virtual void Init()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        State = PlayerState.Idle;
        Dir = MoveDir.None;
        CellPos = new Vector3(0, 0, 0);

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
                Moving();
                break;
            case PlayerState.Jumping:
                Jumping();
                break;
            case PlayerState.Die:
                Die();
                break;
        }
    }

    protected virtual void Idle()
    {
        Debug.Log("Idle Controller");

        if (Dir != MoveDir.None)
        {
            State = PlayerState.Moving;
            return;
        }
    }

    // 실제 움직임을 구현
    // MyPlayer와 Other Player의 움직임을 만드는 함수
    protected virtual void Moving()
    {
        Debug.Log("Moving Controller");

        if (Dir == MoveDir.Up)
        {
            _isJumping = true;

            _jumpCount++;
            _rigid.velocity = Vector2.zero;
            _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

            State = PlayerState.Jumping;
        }


        if (Dir == MoveDir.Left)
        {
            transform.position += Vector3.left * Time.deltaTime * _moveSpeed;
        }
        else if (Dir == MoveDir.Right)
        {
            transform.position += Vector3.right * Time.deltaTime * _moveSpeed;
        }

        if (Dir == MoveDir.None)
        {
            State = PlayerState.Idle;
        }

        CellPos = new Vector3(transform.position.x, transform.position.y, 0);
    }

    protected virtual void Jumping()
    {
        _isJumping = true;

        _jumpCount++;
        _rigid.velocity = Vector2.zero;
        _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

        if (_rigid.velocity.y <= 0)
            Falling();
    }

    protected virtual void Falling()
    {
        // 땅에 착지 후 움직임이 없다면 Idle
        if (_isGrounded == true && _isJumping == false && Dir == MoveDir.None)
            State = PlayerState.Idle;
        // 땅에 착지 후 움직임이 있다면 Moving
        else if (_isGrounded == true && _isJumping == false && Dir != MoveDir.None)
            State = PlayerState.Moving;

    }

    protected virtual void Die()
    {
        GameObject player = GameObject.Find("MyPlayer");
        GameObject.Destroy(player, 1f);
    }

    protected virtual void MoveToNextPos()
    {

    }


}
