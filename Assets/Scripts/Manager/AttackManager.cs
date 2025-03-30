using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class AttackManager : NetworkBehaviour
{
	internal Dictionary<string, Attacker> atkCodeAtkPair;


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
	public void AttackSpawnCallServerRpc(FixedString128Bytes atkCode)
	{
		Instantiate(atkCodeAtkPair[atkCode.ToString()]).NetworkObject.Spawn(true);
	}

	//[Rpc(SendTo.Server)]
	//public void AttackDespawnCallServerRpc(string atkCode)
	//{
	//	targ.NetworkObject.Despawn(true);
	//	GameManager.instance.loggerTemp.text += "DEEESSTTRRROOOYY\n";
	//}

}
