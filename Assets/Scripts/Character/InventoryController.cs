using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private Dictionary<string, GameObject> inventoryItems = new Dictionary<string, GameObject>();
    private int currentItemIndex = 0;

    public GameObject meleeItemSlot;
    public GameObject weaponItemSlot;
    public GameObject specialItemSlot;

    void Awake()
    {
        inventoryItems.Add("melee", null);
        inventoryItems.Add("weapon", null);
        inventoryItems.Add("special", null);
    }

    public void AddItem(string itemType, GameObject item)
    {
        if (inventoryItems.ContainsKey(itemType))
        {
            inventoryItems[itemType] = item;
        }
    }

    public GameObject GetCurrentItem()
    {
        switch (currentItemIndex)
        {
            case 0:
                return inventoryItems["melee"];
            case 1:
                return inventoryItems["weapon"];
            case 2:
                return inventoryItems["special"];
            default:
                return null;
        }
    }

    public void NextItem()
    {
        currentItemIndex = (currentItemIndex + 1) % 3;
    }

    public void PrevItem()
    {
        currentItemIndex = (currentItemIndex + 2) % 3;
    }
}
