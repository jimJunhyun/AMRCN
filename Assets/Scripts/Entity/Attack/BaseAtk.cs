
using UnityEngine;

public class BaseAtk : ScriptableObject
{

	public Attacker relatedAtk;

	public float coolDown;

	public virtual void DoAttack()
	{
		//???
	}
}
