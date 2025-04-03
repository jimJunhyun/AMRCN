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

	public void SetImmune() //새로고침 방식 || 연장 방식
	{
		//immune = true;
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
		if(amt > 0)
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

}
