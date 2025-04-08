using System.ComponentModel;
using Unity.Netcode;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : BaseControlModule
{
	//보통캐릭터별로같을거
    public float jumpDel = 1;


	public int jumpCount = 2;

	public float dashTime;

	internal CharacterDirection direction;


	float moveSpd;
	float jumpPow;

	float dashPow;
	float dashCool;


	Vector2 moveDir = Vector2.zero;
	Vector2 powerDir = Vector2.zero;
	Vector2 inputDir = Vector2.zero;

	Vector2 dashDir = Vector2.zero;

	float jumpCool = 0;
	float curDashCool = 0;

	float curDashTime = 0;

	int curJump = 0;

	bool dashing = false;


	Foot footPos;
	PlayerInput input;


	public void OnMove(InputAction.CallbackContext context)
	{
		//GameManager.instance.logger.text += "Move : " + OwnerClientId + '\n';

		inputDir = context.ReadValue<Vector2>();

        SwitchMoveDir();
    }

	public void OnPressJump(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Jump();
		}
	}

	public void OnPressDash(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Dash();
		}
	}

	void SwitchMoveDir()
	{
		if (inputDir.x > GameManager.MOVETHRESHOLD)
		{
			MoveRight();
		}
		else if (inputDir.x < -GameManager.MOVETHRESHOLD)
		{
			MoveLeft();
		}
		else
		{
			MoveReset();
		}
	}

	void MoveRight()
	{
		if (!functioning)
			return;

		if (!dashing)
		{
			direction = CharacterDirection.Right;
			
			act.anim.SetAnimState(AnimationAction.Run, direction);
		}
		
		moveDir.x = 1;
	}
	void MoveLeft()
	{
		if (!functioning)
			return;

		if (!dashing)
		{
			direction = CharacterDirection.Left;

			act.anim.SetAnimState(AnimationAction.Run, direction);
		}
		moveDir.x = -1;
	}
	void MoveReset()
	{
		if (!functioning)
			return;

		if (!dashing)
		{
			act.anim.SetAnimState(AnimationAction.Idle);
		}
		moveDir.x = 0;
	}

	bool Jump()
	{
		if(!functioning || dashing)
			return false;

		if(curJump < jumpCount)
		{
			if(jumpCool <= 0)
			{
				jumpCool = jumpDel;
				curJump += 1;


				powerDir.y = jumpPow;
				return true;
			}
		}
		return false;
	}

	void Dash()
	{
		if(!functioning)
			return;

		if(curDashCool <= 0)
		{
			curDashCool = dashCool;
			curDashTime = dashTime;
			if(direction == CharacterDirection.Right)
				dashDir.x += dashPow;
			else
				dashDir.x -= dashPow;
			dashing = true;

			act.health.SetImmune(15);
		}
	}

	void DashStop()
	{
		dashing = false;
		if (direction == CharacterDirection.Right)
			dashDir.x -= dashPow;
		else
			dashDir.x += dashPow;
		curDashTime = 0;

		SwitchMoveDir();
	}

	void Gravitate()
	{
		if(dashing)
			return;

		if (powerDir.y <= 0 && footPos.isGrounded)
		{
			powerDir.y = 0;
			curJump = 0;
			jumpCool = 0;

			//transform.position = footPos.sampledHit.point + Vector2.up;
			
		}
		else
		{
			powerDir.y -= GameManager.GRAVITY * Time.fixedDeltaTime;
		}
	}

	

	void Interact() //아마상호작용도있겠지
	{

	}


	public override void OnPickedCharacterChange(BaseCharacter newPicked)
	{
		base.OnPickedCharacterChange(newPicked);

		moveSpd = newPicked.moveSpd;
		jumpPow = newPicked.jumpPow;
		dashPow = newPicked.dashPow;
	}

	internal override void RefreshResource()
	{
		base.RefreshResource();

		moveDir = Vector2.zero;
		powerDir = Vector2.zero;
		inputDir = Vector2.zero;

		dashDir = Vector2.zero;

		jumpCool = 0;
		curDashCool = 0;

		curDashTime = 0;

		curJump = 0;

		dashing = false;
	}


	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		footPos = GetComponentInChildren<Foot>();

		if (!IsOwner)
		{
			input = GetComponent<PlayerInput>();
			input.enabled = false;
		}
	}

	private void OnEnable()
	{
		
	}

	void Update()
    {
		if (!IsOwner)
			return;

		if (jumpCool > 0)
		{
			jumpCool -= Time.deltaTime;
		}
		if(!dashing && curDashCool > 0)
		{
			curDashCool -= Time.deltaTime;
		}
		if (dashing)
        {
            curDashTime -= Time.deltaTime;
			if(curDashTime <= 0)
			{
				DashStop();
			}
        }
        //     if (avatar)
        //     {
        //Debug.Log(avatar.transform.localPosition);
        //     }
    }


	//내가볼때
	//로컬에서 위치가 바뀌긴 하는데
	//서버에 위치를 받아오는게 먼저라서
	//계속덮어씌워짐된다는생각
	//이게맞는듯...

	//movedir을 포함한 이것들을 수정하는부분을전부서버로넘겨야할거같다
	//그래야거기서위치를갱신해주지

	private void FixedUpdate()
	{
		if (!IsOwner)
			return;

		footPos.CalcGrounded();

		Gravitate();

		GameManager.instance.loggerTemp.text = $"(({moveDir} * {moveSpd}) + {powerDir}) * {Time.fixedDeltaTime} = {((Vector3)((moveDir * moveSpd) + powerDir) * Time.fixedDeltaTime).ToString()}";

		if (dashing)
			transform.position += (Vector3)(dashDir) * Time.fixedDeltaTime;
		else
			transform.position += (Vector3)((moveDir * moveSpd) + powerDir) * Time.fixedDeltaTime;
	}
}
