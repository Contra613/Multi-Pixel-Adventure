using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
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

    protected virtual void Init()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
}
