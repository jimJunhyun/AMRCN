using System;
using Unity.Netcode;
using UnityEngine;


[CreateAssetMenu]
public class BaseCharacter : ScriptableObject, INetworkSerializable
{
    public string charName;
    
    public int maxHp;
    public int maxSp;
    
    public float moveSpd;
    
    public float dashPow;
    public float dashCool;


    public float jumpPow;
    
    public float guardTime;
    public float guardCool;

    public float attackCool;

    public float ultGauge;
    public float ultChargeSpd;

    
    public Transform characterPrefab;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		serializer.SerializeValue(ref charName);

		serializer.SerializeValue(ref maxHp);
		serializer.SerializeValue(ref maxSp);

		serializer.SerializeValue(ref moveSpd);

		serializer.SerializeValue(ref dashPow);
		serializer.SerializeValue(ref dashCool);

		serializer.SerializeValue(ref jumpPow);

		serializer.SerializeValue(ref guardTime);
		serializer.SerializeValue(ref guardCool);

		serializer.SerializeValue(ref attackCool);

		serializer.SerializeValue(ref ultGauge);
		serializer.SerializeValue(ref ultChargeSpd);


	}
}
