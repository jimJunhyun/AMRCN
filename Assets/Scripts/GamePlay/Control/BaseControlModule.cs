using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseControlModule : NetworkBehaviour
{
	public bool functioning = false;


	protected internal PlayerActControl act
	{
		get;
		set;
	}

	public virtual void OnPickedCharacterChange(BaseCharacter newPicked)
	{

	}

	internal virtual void RefreshResource()
	{

	}
}
