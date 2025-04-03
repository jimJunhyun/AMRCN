
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.Netcode;

public class PickCharacter : MonoBehaviour
{
    List<BaseCharacter> charList;
    Dictionary<string, Transform> charNameObjectPair;

    const string CHARDATAPATH = "Data/Character/";

    public void LoadCharList() //선택창이든신이든처음들어갈때진행하겠지아마도
    {
        charList = new List<BaseCharacter>(Resources.LoadAll<BaseCharacter>(CHARDATAPATH));
        charNameObjectPair = new Dictionary<string, Transform>();
        for (int i = 0; i < charList.Count; i++)
        {
            charNameObjectPair.Add(charList[i].charName, charList[i].characterPrefab);
        }
    }

    public void Pick(int idx)
    {
        
        NetworkObject local = NetworkManager.Singleton.LocalClient.PlayerObject;
        Debug.Log(local.name + " is Player character!");
        local.GetComponent<PlayerActControl>().PickCharacter(charList[idx]);
    }

    public Transform GetCharacterPrefab(string name)
    {
        if (charNameObjectPair.ContainsKey(name))
            return charNameObjectPair[name];
        return null;
    }
}
