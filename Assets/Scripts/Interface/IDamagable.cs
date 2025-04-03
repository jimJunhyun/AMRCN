using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void Die();
	public void TakeDamage(int amt);
	public void Heal(int amt, bool overheal = false);
}
