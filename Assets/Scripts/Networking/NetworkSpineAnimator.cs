using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CharacterDirection
{
	None = -1,

	Left,
	Right,
}

public enum AnimationAction
{
	None = -1,

	Idle,
	Run,
	Walk,
	Attack,
	Skill1,
	Skill2,
	Skill3,
	Skill4,
}

public class NetworkSpineAnimator : NetworkBehaviour
{
	const string IDLEL = "idle_l";
	const string IDLER = "idle_r";

	const string RUNL = "run_l";
	const string RUNR = "run_r";

	const string WALKL = "walk_l";
	const string WALKR = "walk_r";

	const string ATTACKL = "attack_l";
	const string ATTACKR = "attack_r";

	const string SKILL1L = "skill1_l";
	const string SKILL1R = "skill1_r";

	const string SKILL2L = "skill2_l";
	const string SKILL2R = "skill2_r";

	const string SKILL3L = "skill3_l";
	const string SKILL3R = "skill3_r";

	const string SKILL4L = "skill4_l";
	const string SKILL4R = "skill4_r";



	public CharacterDirection direction;
	AnimationAction action;

    SkeletonAnimation skAnim;
	
	public void InitAnim()
	{
		skAnim = GetComponentInChildren<SkeletonAnimation>();

		SetAnimState(AnimationAction.Idle, CharacterDirection.Left);
	}


	public void SetAnimState(AnimationAction act, CharacterDirection dir = CharacterDirection.None)
	{
		action = act;
		if(dir != CharacterDirection.None)
		{
			direction = dir;
		}

		PlayAnimServerRpc(action, direction);
	}


	[ServerRpc(RequireOwnership = false)]
	public void PlayAnimServerRpc(AnimationAction act, CharacterDirection dir)
	{
		action = act;
		direction = dir;

		SwitchAnimations();

		PlayAnimClientRpc(action, direction);
	}

	[ClientRpc]
	public void PlayAnimClientRpc(AnimationAction act, CharacterDirection dir)
	{
		action = act;
		direction = dir;

		SwitchAnimations();
	}

	void SwitchAnimations()
	{
		switch (action)
		{
			case AnimationAction.Idle:
				if (direction == CharacterDirection.Left)
				{
					
					skAnim.AnimationState.SetAnimation(0, IDLEL, true);
				}
				else
				{
					skAnim.AnimationState.SetAnimation(0, IDLER, true);
				}
				break;
			case AnimationAction.Run:
				if (direction == CharacterDirection.Left)
				{
					skAnim.AnimationState.SetAnimation(0, RUNL, true);
				}
				else
				{
					skAnim.AnimationState.SetAnimation(0, RUNR, true);
				}
				break;
			case AnimationAction.Walk:
				if (direction == CharacterDirection.Left)
				{
					skAnim.AnimationState.SetAnimation(0, WALKL, true);
				}
				else
				{
					skAnim.AnimationState.SetAnimation(0, WALKR, true);
				}
				break;
			case AnimationAction.Attack:
				if (direction == CharacterDirection.Left)
				{
					skAnim.AnimationState.SetAnimation(0, ATTACKL, false);
					skAnim.AnimationState.AddAnimation(0, IDLEL, true, 0);
				}
				else
				{
					skAnim.AnimationState.SetAnimation(0, ATTACKR, false);
					skAnim.AnimationState.AddAnimation(0, IDLER, true, 0);
				}
				break;
			case AnimationAction.Skill1:
				if (direction == CharacterDirection.Left)
				{
					skAnim.AnimationState.SetAnimation(0, SKILL1L, false);
					skAnim.AnimationState.AddAnimation(0, IDLEL, true, 0);
				}
				else
				{
					skAnim.AnimationState.SetAnimation(0, SKILL1R, false);
					skAnim.AnimationState.AddAnimation(0, IDLER, true, 0);
				}
				break;
			case AnimationAction.Skill2:
				if (direction == CharacterDirection.Left)
				{
					skAnim.AnimationState.SetAnimation(0, SKILL2L, false);
					skAnim.AnimationState.AddAnimation(0, IDLEL, true, 0);
				}
				else
				{
					skAnim.AnimationState.SetAnimation(0, SKILL2R, false);
					skAnim.AnimationState.AddAnimation(0, IDLER, true, 0);
				}
				break;
			case AnimationAction.Skill3:
				if (direction == CharacterDirection.Left)
				{
					skAnim.AnimationState.SetAnimation(0, SKILL3L, false);
					skAnim.AnimationState.AddAnimation(0, IDLEL, true, 0);
				}
				else
				{
					skAnim.AnimationState.SetAnimation(0, SKILL3R, false);
					skAnim.AnimationState.AddAnimation(0, IDLER, true, 0);
				}
				break;
			case AnimationAction.Skill4:
				if (direction == CharacterDirection.Left)
				{
					skAnim.AnimationState.SetAnimation(0, SKILL4L, false);
					skAnim.AnimationState.AddAnimation(0, IDLEL, true, 0);
				}
				else
				{
					skAnim.AnimationState.SetAnimation(0, SKILL4R, false);
					skAnim.AnimationState.AddAnimation(0, IDLER, true, 0);
				}
				break;
			default:
				break;
		}

		//GameManager.instance.loggerTemp.text += "Action : " + action.ToString() + '\n';
	}
}
