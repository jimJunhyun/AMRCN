using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;


[CreateAssetMenu]
public class BaseSpawnAtk : BaseAtk
{


    public override void DoAttack(ulong callerId)
    {
        base.DoAttack(callerId);

        //GameManager.instance.loggerTemp.text = $"{relatedAtk == null}";

        GameManager.instance.attackManager.AttackSpawnCallServerRpc(relatedAtk.attackCode, callerId, atkSocketIdx);
    }


	
}
