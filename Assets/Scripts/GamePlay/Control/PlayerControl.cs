using System.ComponentModel;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : NetworkBehaviour
{
	//보통캐릭터별로같을거
    public float jumpDel = 1;


	public int jumpCount = 2;


	float moveSpd;
	float jumpPow;

	Vector2 moveDir = Vector2.zero;
	Vector2 powerDir = Vector2.zero;
	Vector2 inputDir = Vector2.zero;

	float jumpCool = 0;

	int curJump = 0;

	NetworkSpineAnimator spineAnimator;

	Foot footPos;
	PlayerInput input;

	BaseCharacter picked;

	NetworkObject avatar;

	public void OnMove(InputAction.CallbackContext context)
	{
		//GameManager.instance.logger.text += "Move : " + OwnerClientId + '\n';

		inputDir = context.ReadValue<Vector2>();
        if(inputDir.x > GameManager.MOVETHRESHOLD)
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

	public void OnPressSpace(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Jump();
		}
	}

	public void OnMouseDelta(InputAction.CallbackContext context)
	{

	}

	void MoveRight()
	{
		if (!avatar)
			return;
		
		spineAnimator.SetAnimState(AnimationAction.Run, CharacterDirection.Right);
		
		moveDir.x = 1;
	}
	void MoveLeft()
	{
		if (!avatar)
			return;
		spineAnimator.SetAnimState(AnimationAction.Run, CharacterDirection.Left);
		
		moveDir.x = -1;
	}
	void MoveReset()
	{
		if (!avatar)
			return;
		spineAnimator.SetAnimState(AnimationAction.Idle);
		
		moveDir.x = 0;
	}

	bool Jump()
	{
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

	void Gravitate()
	{
		if(!footPos.isGrounded)
		{
			powerDir.y -= GameManager.GRAVITY * Time.fixedDeltaTime;
		}
		else if (footPos.isGrounded && powerDir.y <= 0)
		{
			powerDir.y = 0;
			curJump = 0;
			jumpCool = 0;
		}
	}



	void Interact() //아마상호작용도있겠지
	{

	}


	internal void PickCharacter(BaseCharacter actor)
	{
		picked = actor;
		moveSpd = picked.moveSpd;
		jumpPow = picked.jumpPow;

		SpawnCharacterServerRpc(picked);
	}


	[ServerRpc]
	internal void SpawnCharacterServerRpc(BaseCharacter actor)
	{
		picked = actor;
		moveSpd = picked.moveSpd;
		jumpPow = picked.jumpPow;

		avatar = Instantiate(GameManager.instance.picker.GetCharacterPrefab(actor.charName)).GetComponent<NetworkObject>();
		avatar.transform.position = transform.position;
		avatar.Spawn(true);
		avatar.transform.SetParent(transform);

		spineAnimator = avatar.GetComponent<NetworkSpineAnimator>();
		spineAnimator.InitAnim();

		SetAvatarClientRpc();

		//GameManager.instance.loggerTemp.text += "SPAWNED & CLIENTRPC CALLED";
	}


	[ClientRpc]
	internal void SetAvatarClientRpc()
	{
		//GameManager.instance.loggerTemp.text += "CLIENTRPC RECEIVED";

		avatar = GetComponentInChildren<NetworkSpineAnimator>().GetComponent<NetworkObject>();
		spineAnimator = avatar.GetComponent<NetworkSpineAnimator>();

		spineAnimator.InitAnim();
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
   //     if (avatar)
   //     {
			//Debug.Log(avatar.transform.localPosition);
   //     }
	}

	private void FixedUpdate()
	{
		//if (!IsOwner)
		//	return;

		footPos.CalcGrounded();

		Gravitate();
			
		transform.position += (Vector3)((moveDir * moveSpd) + powerDir) * Time.fixedDeltaTime;
	}
}
