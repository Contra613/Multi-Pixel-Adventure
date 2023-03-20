using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : Controller
{
	protected Coroutine _coSkill;
	protected bool _rangedSkill = false;

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

	protected override void UpdateController()
	{
		base.UpdateController();
	}

	protected override void UpdateIdle()
	{
		// 이동 상태로 갈지 확인
		if (Dir != MoveDir.None)
		{
			State = PlayerState.Moving;
			return;
		}
	}

}



