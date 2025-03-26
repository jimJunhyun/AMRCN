using Unity.Netcode;
using UnityEngine;

public class Attacker : NetworkBehaviour, INetworkSerializable
{
	public string attackCode;

    public float attackDmg;

	public float lifetime;

	public float moveDist;

	//계수같은거에따라스케일링되는값으로변경할예정

    Vector2 movePow;
    float curLifetime;


	

    void DoMove()
    {
        movePow = transform.right * (moveDist / lifetime);

		GameManager.instance.loggerTemp.text += "MOVEOREOROEROER";
    }


	public override void OnNetworkSpawn() //아마풀링을한다면바꾸겠지요
	{
		base.OnNetworkSpawn();

		DoMove();
		curLifetime = 0;
	}


	private void Update()
	{
		curLifetime += Time.deltaTime;
        if (curLifetime > lifetime)
        {
            //RemoveAttackHitBoxServerRpc();
			
        }
	}

	private void FixedUpdate()
	{
		if(moveDist > 0)
        {
            transform.position += (Vector3)movePow * Time.fixedDeltaTime;
        }
	}

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		serializer.SerializeValue(ref attackDmg);
		serializer.SerializeValue(ref moveDist);
		serializer.SerializeValue(ref lifetime);
		serializer.SerializeValue(ref movePow);
		serializer.SerializeValue(ref curLifetime);
	}

	//[Rpc(SendTo.Server)]
	//internal void CreateBulletServerRpc( float dmg, float rng, float lTime)
	//{
	//	(obj = GetComponent<NetworkObject>()).Spawn(true);
	//	GameManager.instance.loggerTemp.text += "BltCalll\n";
	//	InitInfo(dmg, rng, lTime);
	//}



	//[Rpc(SendTo.Server, RequireOwnership = false)]
	//   public void RemoveAttackHitBoxServerRpc()
	//   {
	//       obj.Despawn(true);
	//   }


	//???
}
