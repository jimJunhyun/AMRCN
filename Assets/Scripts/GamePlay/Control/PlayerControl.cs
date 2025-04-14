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

	Vector2 prevPower = Vector2.zero;

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

			if (!functioning || dashing)
				return;

			if (curJump < jumpCount)
			{
				if (jumpCool <= 0)
				{
					JumpServerRpc();
				}
			}
		}
	}

	public void OnPressDash(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			if (!functioning)
				return;

			if (curDashCool <= 0)
			{
				DashServerRpc();

			}
		}
	}

	internal void SwitchMoveDir() //얘도 RPC로 해야할까?
	{
		if (!functioning)
			return;

		MoveServerRpc(inputDir);
	}

	void MoveRight()
	{
		if (!dashing)
		{
			direction = CharacterDirection.Right;

			act.anim.SetAnimState(AnimationAction.Run, direction);
		}

		moveDir = Vector2.right;
	}
	void MoveLeft()
	{

		if (!dashing)
		{
			direction = CharacterDirection.Left;

			act.anim.SetAnimState(AnimationAction.Run, direction);
		}

		moveDir = Vector2.left;
	}
	void MoveReset()
	{
		if (!dashing)
		{
			act.anim.SetAnimState(AnimationAction.Idle);
		}

		moveDir = Vector2.zero;

	}


	[ServerRpc]
	void MoveServerRpc(Vector2 input)
	{
		inputDir = input;
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

		RefreshValuesOnClient();
	}

	//[ClientRpc]
	//void MoveClientRpc(Vector2 input)
	//{
	//	if (inputDir.x > GameManager.MOVETHRESHOLD)
	//	{
	//		MoveRight();
	//	}
	//	else if (inputDir.x < -GameManager.MOVETHRESHOLD)
	//	{
	//		MoveLeft();
	//	}
	//	else
	//	{
	//		MoveReset();
	//	}
	//}

	[ServerRpc]
	void JumpServerRpc()
	{
		jumpCool = jumpDel;
		curJump += 1;

		powerDir.y = jumpPow;

		RefreshValuesOnClient();
	}

	//[ClientRpc]
	//void JumpClientRpc()
	//{
	//	jumpCool = jumpDel;
	//	curJump += 1;

	//	powerDir.y = jumpPow;
	//}

	[ServerRpc]
	void DashServerRpc()
	{

		curDashCool = dashCool;
		curDashTime = dashTime;


		if (direction == CharacterDirection.Right)
			dashDir.x += dashPow;
		else
			dashDir.x -= dashPow;
		dashing = true;

		act.health.SetImmune(15);
	}

	//[ClientRpc]
	//void DashClientRpc()
	//{
	//	curDashCool = dashCool;
	//	curDashTime = dashTime;


	//	if (direction == CharacterDirection.Right)
	//		dashDir.x += dashPow;
	//	else
	//		dashDir.x -= dashPow;
	//	dashing = true;
	//}

	[ServerRpc]
	void DashStopServerRpc()
	{
		curDashTime = 0;

		dashing = false;
		if (direction == CharacterDirection.Right)
			dashDir.x -= dashPow;
		else
			dashDir.x += dashPow;

		SwitchMoveDir();

		RefreshValuesOnClient();
	}

	//[ClientRpc]
	//void DashStopClientRpc()
	//{
	//	curDashTime = 0;

	//	dashing = false;
	//	if (direction == CharacterDirection.Right)
	//		dashDir.x -= dashPow;
	//	else
	//		dashDir.x += dashPow;

	//	SwitchMoveDir();
	//}

	[ServerRpc]
	void AdjustMoveServerRpc(Vector2 move)
	{
		moveDir = move;
		RefreshValuesOnClient();
	}

	[ServerRpc]
	void HardSetPowerServerRpc(Vector2 pow)
	{
		powerDir = pow;
		RefreshValuesOnClient();
	}

	[ServerRpc]
	void SoftSetPowerServerRpc(Vector2 pow)
	{
		if(pow.x != 0)
			powerDir.x = pow.x;
		if (pow.y != 0)
			powerDir.y = pow.y;
		RefreshValuesOnClient();
	}

	[ServerRpc]
	void AddPowerServerRpc(Vector2 pow)
	{
		powerDir += pow;
		RefreshValuesOnClient();
	}


	void RefreshValuesOnClient()
	{
		RefreshValuesClientRpc(inputDir, moveDir, powerDir, curJump, jumpCool, dashing, dashDir, curDashTime, curDashCool, direction);
	}

	[ClientRpc]
	void RefreshValuesClientRpc(Vector2 input, Vector2 move, Vector2 pow, int cJump, float jCool, bool dash, Vector2 dDir, float cDashTime, float cDashCool, CharacterDirection dir)
	{
		inputDir = input;
		moveDir = move;
		powerDir = pow;
		curJump = cJump;
		jumpCool = jCool;
		dashing = dash;
		dashDir = dDir;
		curDashTime = cDashTime;
		curDashCool = cDashCool;
		direction = dir;
	}


	void Gravitate()
	{
		if (dashing)
			return;

		if (powerDir.y <= 0 && footPos.isGrounded)
		{
			powerDir.y = 0;
			curJump = 0;
			jumpCool = 0;

			transform.position = footPos.sampledHit.point + Vector2.up;

		}
		else
		{
			powerDir.y -= GameManager.GRAVITY * Time.fixedDeltaTime;
		}

		RefreshValuesOnClient(); //동작은 할거임, 근데 비효율적. 아마도 옵저버 비슷한 느낌으로 바꾸거나, 상태 변화가 감지되었을때만 이걸 해주는 식으로 바꿀 듯 싶다.
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
		if (!IsServer)
			return;

		if (jumpCool > 0)
		{
			jumpCool -= Time.deltaTime;
			if(jumpCool <= 0)
			{
				jumpCool = 0;
				RefreshValuesOnClient();
			}
		}
		if(!dashing && curDashCool > 0)
		{
			curDashCool -= Time.deltaTime;
			if (curDashCool <= 0)
			{
				curDashCool = 0;
				RefreshValuesOnClient();
			}
		}
		if (dashing)
        {
            curDashTime -= Time.deltaTime;
			if(curDashTime <= 0)
			{
				DashStopServerRpc();
			}
        }
        //     if (avatar)
        //     {
        //Debug.Log(avatar.transform.localPosition);
        //     }
    }


	//로컬에서 위치가 바뀌긴 하는데
	//서버에 위치를 받아오는게 먼저라서
	//계속덮어씌워짐된다는추측 (확인함)


	private void FixedUpdate()
	{
		if (!IsServer)
			return;


		footPos.CalcGrounded();

		Gravitate();

		//GameManager.instance.loggerTemp.text = $"(({moveDir} * {moveSpd}) + {powerDir}) * {Time.fixedDeltaTime} = {((Vector3)((moveDir * moveSpd) + powerDir) * Time.fixedDeltaTime).ToString()}";

		if (dashing)
			transform.position += (Vector3)(dashDir) * Time.fixedDeltaTime;
		else
			transform.position += (Vector3)((moveDir * moveSpd) + powerDir) * Time.fixedDeltaTime;

		
	}
}
