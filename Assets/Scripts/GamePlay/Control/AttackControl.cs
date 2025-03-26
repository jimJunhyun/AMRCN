
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class SkillSlot
{
    public float passedCool;
    float acceleration = 1;


    BaseAtk boundSkill;

    public void PassTime()
    {
        if(boundSkill != null && boundSkill.coolDown > passedCool)
        {
            passedCool += Time.deltaTime * acceleration;
        }
    }

	public bool Use()
	{
		if (Usable)
		{
			boundSkill.DoAttack();
			passedCool = 0;
		}
		return Usable;
	}

    internal bool Usable
    {
        get => passedCool >= boundSkill.coolDown;
    }

	public SkillSlot(BaseAtk atk)
	{
		passedCool = 0;
		boundSkill = atk;
		acceleration = 1;
	}
}


public class AttackControl : NetworkBehaviour
{
	public BaseAtk normAtk;
	public BaseAtk skill1;
	public BaseAtk skill2;
	public BaseAtk skill3;
	public BaseAtk skill4;

	//아마얘네를캐릭터정보에넣고
	//그걸연결하는식으로
	//그캐릭터정보에서 이거랑 플레이어컨트롤 등등

	internal SkillSlot normalSlot;

    internal SkillSlot slot1;
    internal SkillSlot slot2;
    internal SkillSlot slot3;
    internal SkillSlot slot4;

	//일단최대치가4개인게반확정이고 스킬이2개에다 궁1개 비슷한 경우도 있을수있기때문에따로하는게차라리더나을덧,,


	private void Awake()
	{
		normalSlot = new SkillSlot(normAtk);
		slot1 = new SkillSlot(skill1);
		slot2 = new SkillSlot(skill2);
		slot3 = new SkillSlot(skill3);
		slot4 = new SkillSlot(skill4);

	}

	public void OnPressNormalAtk(InputAction.CallbackContext context) //LClick
	{
		if (context.performed)
		{
			normalSlot?.Use();
		}
	}


	public void OnPressSkill1(InputAction.CallbackContext context) //U
	{
		if (context.performed)
		{
			slot1?.Use();
		}
	}

	public void OnPressSkill2(InputAction.CallbackContext context)//I
	{
		if (context.performed)
		{
			slot2?.Use();
		}
	}

	public void OnPressSkill3(InputAction.CallbackContext context)//O
	{
		if (context.performed)
		{
			slot3?.Use();
		}
	}
	public void OnPressSkill4(InputAction.CallbackContext context)//P
	{
		if (context.performed)
		{
			slot4?.Use();
		}
	}


	private void Update()
	{
		normalSlot.PassTime();
        slot1.PassTime();
        slot2.PassTime();
        slot3.PassTime();
        slot4.PassTime();
        
	}
}
