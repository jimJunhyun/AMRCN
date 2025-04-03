
using UnityEngine;

public class BaseAtk : ScriptableObject
{

	public Attacker relatedAtk;

	public float coolDown;

	public int atkSocketIdx;

	public virtual void DoAttack(ulong callerId)
	{
		//???
	}
}
