using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class LootPool : ScriptableObject
{
    public List<ItemData> loot = new List<ItemData>(); //All possible loot (weapons & consumables)
}
