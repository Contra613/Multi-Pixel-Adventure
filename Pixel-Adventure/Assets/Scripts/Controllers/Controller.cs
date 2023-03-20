using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using static Define;

public class Controller : MonoBehaviour
{
	public int Id { get; set; }

	[SerializeField]
	public float _speed = 5.0f;

	protected bool _updated = false;

	PositionInfo _positionInfo = new PositionInfo();
	public PositionInfo PosInfo
	{
		get { return _positionInfo; }
		set
		{
			if (_positionInfo.Equals(value))
				return;

			CellPos = new Vector3Int(value.PosX, value.PosY, 0);
			State = value.State;
			Dir = value.MoveDir;
		}
	}

	public Vector3Int CellPos
	{
		get
		{
			return new Vector3Int(PosInfo.PosX, PosInfo.PosY, 0);
		}

		set
		{
			if (PosInfo.PosX == value.x && PosInfo.PosY == value.y)
				return;

			PosInfo.PosX = value.x;
			PosInfo.PosY = value.y;
			_updated = true;
		}
	}

	protected Animator _animator;
	protected SpriteRenderer _sprite;

	public virtual PlayerState State
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

	protected virtual void UpdateAnimation()
	{
		if (State == PlayerState.Idle)
		{
			switch (_lastDir)
			{
				case MoveDir.Up:
					_animator.Play("idle_up");
					_sprite.flipX = false;
					break;
				case MoveDir.Down:
					_animator.Play("idle_down");
					_sprite.flipX = false;
					break;
				case MoveDir.Left:
					_animator.Play("idle_side");
					_sprite.flipX = false;
					break;
				case MoveDir.Right:
					_animator.Play("idle_side");
					_sprite.flipX = true;
					break;
			}
		}
		else if (State == PlayerState.Moving)
		{
			switch (Dir)
			{
				case MoveDir.Up:
					_animator.Play("walk_up");
					_sprite.flipX = false;
					break;
				case MoveDir.Down:
					_animator.Play("walk_down");
					_sprite.flipX = false;
					break;
				case MoveDir.Left:
					_animator.Play("walk_side");
					_sprite.flipX = false;
					break;
				case MoveDir.Right:
					_animator.Play("walk_side");
					_sprite.flipX = true;
					break;
			}
		}
		else
		{

		}
	}

	void Start()
	{
		Init();
	}

	void Update()
	{
		UpdateController();
	}

	protected virtual void Init()
	{
		_animator = GetComponent<Animator>();
		_sprite = GetComponent<SpriteRenderer>();
		Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
		transform.position = pos;

		State = PlayerState.Idle;
		Dir = MoveDir.None;
		CellPos = new Vector3Int(0, 0, 0);
		UpdateAnimation();
	}

	protected virtual void UpdateController()
	{
		switch (State)
		{
			case PlayerState.Idle:
				UpdateIdle();
				break;
			case PlayerState.Moving:
				UpdateMoving();
				break;
		}
	}

	protected virtual void UpdateIdle()
	{
	}

	// ������ �̵��ϴ� ���� ó��
	protected virtual void UpdateMoving()
	{
		Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
		Vector3 moveDir = destPos - transform.position;

		// ���� ���� üũ

		// ���� �̵� ó��
		float dist = moveDir.magnitude;
		if (dist < _speed * Time.deltaTime)
		{
			transform.position = destPos;
			MoveToNextPos();
		}
		else
		{
			transform.position += moveDir.normalized * _speed * Time.deltaTime;
			State = PlayerState.Moving;
		}
	}

	protected virtual void MoveToNextPos()
	{

	}

	protected virtual void UpdateSkill()
	{

	}

	protected virtual void UpdateDead()
	{

	}

	public virtual void OnDamaged()
	{

	}
}
