using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;


[CreateAssetMenu]
public class BaseRangedAtk : BaseAtk
{

    public Transform origination; //이걸어케찾느냐................

    public override void DoAttack()
    {
        base.DoAttack();

        //GameManager.instance.loggerTemp.text = $"{relatedAtk == null}";



        GameManager.instance.attackManager.AttackSpawnCallServerRpc(relatedAtk.attackCode);
    }


	
}
