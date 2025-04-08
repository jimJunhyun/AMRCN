using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControl : BaseControlModule, IDamagable
{
	int maxHp;
	int maxSp;


    int curHp;
    int curSp;

	bool immune = false;

	int immuneFrame = 0;

	public override void OnPickedCharacterChange(BaseCharacter newPicked)
	{
		base.OnPickedCharacterChange(newPicked);

		maxHp = newPicked.maxHp;
		maxSp = newPicked.maxSp;

		curHp = maxHp;
		curSp= maxSp;
	}

	internal override void RefreshResource()
	{
		base.RefreshResource();

		curHp = maxHp;
		curSp = maxSp;
	}

	public void SetImmune(int frame) //새로고침 방식 || 연장 방식
	{
		immune = true;
		immuneFrame += frame;
	}

	public void Heal(int amt, bool overHeal = false)
	{
		if(amt >= 0)
		{
			curHp += amt;

			if(!overHeal)
			{
				curHp = Mathf.Clamp(curHp, 0, maxHp);
			}
		}
	}

	public void TakeDamage(int amt)
	{
        if(immune)
			return;

        if (amt > 0)
		{
			curHp -= amt;

			if(curHp < 0)
			{
				Die();
			}
		}
	}

	public void Die()
	{
		Debug.Log("죽었소.");
		act.DisableEverything();
	}

	private void Update()
	{
		if(immune && immuneFrame > 0)
		{
			immuneFrame -= 1;
		}

		if(immuneFrame < 0)
			immune = false;
	}

}
