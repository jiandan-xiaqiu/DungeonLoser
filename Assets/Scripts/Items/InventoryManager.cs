using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string itemId, int quantity = 1)
    {
        if (items.ContainsKey(itemId))
        {
            items[itemId] += quantity;
        }
        else
        {
            items.Add(itemId, quantity);
        }
    }

    public bool RemoveItem(string itemId, int quantity = 1)
    {
        if (!items.ContainsKey(itemId) || items[itemId] < quantity)
        {
            return false;
        }

        items[itemId] -= quantity;
        if (items[itemId] <= 0)
        {
            items.Remove(itemId);
        }
        return true;
    }

    public int GetItemCount(string itemId)
    {
        return items.ContainsKey(itemId) ? items[itemId] : 0;
    }
}