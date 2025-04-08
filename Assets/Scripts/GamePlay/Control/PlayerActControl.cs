using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerActControl : NetworkBehaviour
{
    internal AttackControl atk;
    internal PlayerControl ctrl;
    internal HealthControl health;
    internal NetworkSpineAnimator anim;

	List<BaseControlModule> modules;

	BaseCharacter picked;

	NetworkObject avatar;



	private void Awake()
	{
		atk = GetComponent<AttackControl>();
		if(atk == null )
			atk = gameObject.AddComponent<AttackControl>();

		ctrl = GetComponent<PlayerControl>();
		if (ctrl == null )
			ctrl = gameObject.AddComponent <PlayerControl>();

		health = GetComponent<HealthControl>();
		if(health == null)
			health = gameObject.AddComponent<HealthControl>() ;

		modules = new List<BaseControlModule>();
		modules.Add(ctrl);
		modules.Add(atk);
		modules.Add(health);

		for (int i = 0; i < modules.Count; ++i)
		{
			modules[i].act = this;
		}
	}


	internal void PickCharacter(BaseCharacter actor)
	{
		picked = actor;
		

		SpawnCharacterServerRpc(picked);
	}

	internal void DisableEverything()
	{
		for (int i = 0; i < modules.Count; ++i)
		{
			modules[i].functioning = false;
		}
	}

	internal void RefreshResources()
	{
		for (int i = 0; i < modules.Count; ++i)
		{
			modules[i].RefreshResource();
		}
	}


	[ServerRpc]
	internal void SpawnCharacterServerRpc(BaseCharacter actor)
	{
		picked = actor;

		for(int i = 0; i < modules.Count; ++i)
		{
			modules[i].OnPickedCharacterChange(actor);

			modules[i].functioning = true;
		}

		avatar = Instantiate(GameManager.instance.picker.GetCharacterPrefab(actor.charName)).GetComponent<NetworkObject>();
		avatar.transform.position = transform.position;
		avatar.Spawn(true);
		avatar.transform.SetParent(transform);

		anim = avatar.GetComponent<NetworkSpineAnimator>();
		anim.InitAnim();

		SetAvatarClientRpc();

		//GameManager.instance.loggerTemp.text += "SPAWNED & CLIENTRPC CALLED";
	}


	[ClientRpc]
	internal void SetAvatarClientRpc()
	{
		//GameManager.instance.loggerTemp.text += "CLIENTRPC RECEIVED";

		

		avatar = GetComponentInChildren<NetworkSpineAnimator>().GetComponent<NetworkObject>();
		anim = avatar.GetComponent<NetworkSpineAnimator>();

		anim.InitAnim();


		for (int i = 0; i < modules.Count; ++i)
		{
			modules[i].OnPickedCharacterChange(picked);

			modules[i].functioning = true;
		}
	}
}
