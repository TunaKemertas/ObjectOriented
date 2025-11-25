using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Heal, Weapon, Score }

public class Item : MonoBehaviour
{
    
    public string itemName = "HealthPack";
    public ItemType itemType = ItemType.Heal;
    public int value = 20;
    public bool isConsumable = true;
    public int itemID = 0;

    void Start()
    {
        
    }

    public void Use(PlayerController player)
    {
        if (player == null) return;
        ApplyEffect(player);
        if (isConsumable) Destroy(gameObject);
    }

    public void ApplyEffect(PlayerController player)
    {
        switch (itemType)
        {
            case ItemType.Heal:
                player.Heal(value);
                break;
            case ItemType.Weapon:
                player.hasWeapon = true;
                player.attackPower += value;
                break;
            case ItemType.Score:
                player.score += value;
               
                break;
        }
    }

   
    public static Item Spawn(Item prefab)
    {
        return Spawn(prefab, Vector2.zero);
    }

    public static Item Spawn(Item prefab, Vector2 pos)
    {
        if (prefab == null) return null;
        Item spawned = Instantiate(prefab, pos, Quaternion.identity);
        return spawned;
    }

    public string GetDescription()
    {
        return $"{itemName} ({itemType}) Value:{value}";
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController p = collision.GetComponent<PlayerController>();
        if (p != null)
        {
            Use(p);
        }
    }
}
