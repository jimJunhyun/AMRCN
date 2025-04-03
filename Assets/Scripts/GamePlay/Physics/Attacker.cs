using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Attacker : NetworkBehaviour, INetworkSerializable
{
	public string attackCode;

    public int attackDmg;

	public float lifetime;

	public float moveDist;

	public bool oneTimeUse;



    Vector2 movePow;
    float curLifetime;

	CharacterDirection dir;

	ulong attackOwnerClientId;

	PlayerActControl owner;

	List<IDamagable> hits;
	

    void DoMove()
    {
		switch (dir)
		{
			case CharacterDirection.None:
				break;
			case CharacterDirection.Left:
				movePow = -transform.right * (moveDist / lifetime);
				break;
			case CharacterDirection.Right:
				movePow = transform.right * (moveDist / lifetime);
				break;
			default:
				break;
		}
    }

	public void SetInfo(ulong ownerCli, int atkSocket)
	{
		attackOwnerClientId = ownerCli;
		curLifetime = 0;

		owner = NetworkManager.Singleton.ConnectedClients[ownerCli].PlayerObject.GetComponent<PlayerActControl>();

		dir = owner.ctrl.direction;
		transform.position = owner.atk.atkPoses[atkSocket].transform.position;

		hits = new List<IDamagable>();

		DoMove();
	}


	public override void OnNetworkSpawn() //아마풀링을한다면바꾸겠지요
	{
		base.OnNetworkSpawn();

		//DoMove();
		//curLifetime = 0;
	}


	private void Update()
	{
		curLifetime += Time.deltaTime;
        if (curLifetime > lifetime)
        {
			GameManager.instance.attackManager.AttackDespawnCallServerRpc(gameObject.GetInstanceID());
			
        }
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		IDamagable hit = collision.GetComponent<IDamagable>();
		if (hit != null && !hits.Contains(hit))
		{
			hits.Add(hit);
			hit.TakeDamage(owner.atk.attackDamageMod.Modify(attackDmg));
			if (oneTimeUse)
			{
				GameManager.instance.attackManager.AttackDespawnCallServerRpc(gameObject.GetInstanceID()); //문제없나?
			}
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
