using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class AttackManager : NetworkBehaviour
{
	internal Dictionary<string, Attacker> atkCodeAtkPair;

	Dictionary<int, Attacker> allAtks = new Dictionary<int, Attacker>();

	public void LoadAttacks()
	{

		AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "attacks"));
		GameObject[] atks = asset.LoadAllAssets<GameObject>();

		atkCodeAtkPair = new Dictionary<string, Attacker>();

		Attacker attacker;
		foreach (var atk in atks)
		{
			attacker = atk.GetComponent<Attacker>();
			atkCodeAtkPair.Add(attacker.attackCode, attacker);
		}
		//GameManager.instance.loggerTemp.text += $"LOADED {atkCodeAtkPair.Count} ATTACKS";
	}


	[ServerRpc(RequireOwnership = false)]
	public void AttackSpawnCallServerRpc(FixedString128Bytes atkCode, ulong callerClientId, int attackSocketIdx)
	{
		Attacker atk = Instantiate(atkCodeAtkPair[atkCode.ToString()]);
		atk.SetInfo(atk.OwnerClientId, attackSocketIdx);
		atk.NetworkObject.Spawn(true);

		allAtks.Add(atk.gameObject.GetInstanceID(), atk);
	}

	[ServerRpc(RequireOwnership = false)]
	public void AttackDespawnCallServerRpc(int targetInstanceId)
	{
		allAtks[targetInstanceId].NetworkObject.Despawn(true);
		allAtks.Remove(targetInstanceId);
	}

}
