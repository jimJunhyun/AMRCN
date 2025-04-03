
using Spine.Unity;
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
		bool res = Usable;
		if (Usable)
		{
			boundSkill.DoAttack(NetworkManager.Singleton.LocalClientId);
			passedCool = 0;
		}
		return res;
	}

	internal void ResetCool()
	{
		if(boundSkill != null)
		{
			passedCool = boundSkill.coolDown;
		}
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


public class AttackControl : BaseControlModule
{
	public BaseAtk normAtk;
	public BaseAtk skill1;
	public BaseAtk skill2;
	public BaseAtk skill3;
	public BaseAtk skill4;

	//�Ƹ���׸�ĳ�����������ְ�
	//�װɿ����ϴ½�����
	//��ĳ������������ �̰Ŷ� �÷��̾���Ʈ�� ���

	internal SkillSlot normalSlot;

    internal SkillSlot slot1;
    internal SkillSlot slot2;
    internal SkillSlot slot3;
    internal SkillSlot slot4;

	//�ִ� 4���ΰ� ��Ȯ���̰� ��ų2����1�� ���� ��쵵 �������ֱ⶧���� �����ϴ°� ���� �� ���� ���̶����.

	internal List<BoneFollower> atkPoses;

	internal IntegerModifier attackDamageMod;

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
		if(!functioning)
			return;
		if (context.performed)
		{

			if (normalSlot != null && normalSlot.Use())
			{
				act.anim.SetAnimState(AnimationAction.Attack);
			}
		}
	}


	public void OnPressSkill1(InputAction.CallbackContext context) //U
	{
		if (!functioning)
			return;
		if (context.performed)
		{
			if (slot1 != null && slot1.Use())
			{
				act.anim.SetAnimState(AnimationAction.Skill1);
			}
		}
	}

	public void OnPressSkill2(InputAction.CallbackContext context)//I
	{
		if (!functioning)
			return;
		if (context.performed)
		{
			if (slot2 != null && slot2.Use())
			{
				act.anim.SetAnimState(AnimationAction.Skill2);
			}
		}
	}

	public void OnPressSkill3(InputAction.CallbackContext context)//O
	{
		if (!functioning)
			return;
		if (context.performed)
		{
			if (slot3 != null && slot3.Use())
			{
				act.anim.SetAnimState(AnimationAction.Skill3);
			}
		}
	}
	public void OnPressSkill4(InputAction.CallbackContext context)//P
	{
		if (!functioning)
			return;
		if (context.performed)
		{
			if (slot4 != null && slot4.Use())
			{
				act.anim.SetAnimState(AnimationAction.Skill4);
			}
		}
	}


	public override void OnPickedCharacterChange(BaseCharacter newPicked)
	{
		atkPoses = new List<BoneFollower>(GetComponentsInChildren<BoneFollower>());


		//ĳ���ͽ�ų��ü�����ֱ�.
	}

	internal override void RefreshResource()
	{
		base.RefreshResource();

		normalSlot.ResetCool();
		slot1.ResetCool();
		slot2.ResetCool();
		slot3.ResetCool();
		slot4.ResetCool();
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
