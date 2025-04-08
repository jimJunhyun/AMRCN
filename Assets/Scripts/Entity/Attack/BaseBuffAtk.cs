using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuffAtk : BaseAtk
{
	//버프되는 스탯 목록? => 거의 모든 상황에 대응
	//그냥 스킬마다 이런거 하나씩? => 나중가면 대책없을수도, 오히려나을수도있음(요상한메커니즘 많으면)
	public override void DoAttack(ulong callerId)
	{
		base.DoAttack(callerId);

		
	}
}
