using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    public static GameManager instance;


    public const float GRAVITY = 15f;
    public const float GROUNDTHRESHOLD = 0.1f;
    public const float MOVETHRESHOLD = 0.25f;

    public const float SMALL = 0.001f;

    public const float SAMPLEPOINTDISTANCE = 1f; //발판 하나 두께 < 값 < 발판 사이 거리

    public const int GROUNDLAYER =  8;
    public const int PLAYERLAYER =  9;
    public const int ATTACKLAYER =  10;

    public PickCharacter picker;
    public AttackManager attackManager;

    public TextMeshProUGUI loggerTemp;

	private void Awake()
	{
		instance = this;

        picker = GetComponentInChildren<PickCharacter>();
        attackManager = GetComponentInChildren<AttackManager>();

        picker.LoadCharList();
        attackManager.LoadAttacks();
	}



	
}
