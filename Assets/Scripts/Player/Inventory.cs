using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public List<ItemInstance> weapons = new(); //All weapons player has in inventory
    public List<ItemInstance> consumables = new(); //Other items (potions and such)
}
