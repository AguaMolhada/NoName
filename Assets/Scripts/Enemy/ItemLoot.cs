using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemLoot : MonoBehaviour {

    [SerializeField]
    private int maxTries;
    public GameObject lootHolder;

    public List<LootItems> LootWeapons = new List<LootItems>();
    public List<LootItems> LootItems = new List<LootItems>();
    public LootItems Gold;

    public void SpawnLoot()
    {
        int i = Random.Range(0, LootWeapons.Count);
        if (Random.value <= LootWeapons[i].spawnChance)
        {
            GameObject loot = Instantiate(LootWeapons[i].lootObj, transform.position, Quaternion.identity) as GameObject;
            loot.transform.parent = GameObject.Find("Generated Map").transform;
        }

        int k = Random.Range(0, LootItems.Count);
        if(Random.value <= LootItems[k].spawnChance)
        {
            GameObject loot = Instantiate(LootItems[k].lootObj, transform.position, Quaternion.identity) as GameObject;
            loot.transform.parent = GameObject.Find("Generated Map").transform;
        }

        int j = Random.Range(Gold.MinAmmout, Gold.MaxAmmout);
        GameObject lootHold = Instantiate(lootHolder, transform.position, Quaternion.identity) as GameObject;
        for (int x = 0; x < j; x++)
        {
            lootHold.GetComponent<PickableObjects>().ammout = 0;
            GameObject loot = Instantiate(Gold.lootObj, transform.position, Quaternion.identity) as GameObject;
            loot.transform.parent = lootHold.transform;
        }
        lootHold.transform.parent = GameObject.Find("Generated Map").transform;
        lootHold.GetComponent<PickableObjects>().ammout = j;
    }

}

[System.Serializable]
public class LootItems
{

    public GameObject lootObj;
    [Range(0,1)]
    [Header("0 = 0% and 1 = 100%")]
    public float spawnChance;
    public int MinAmmout;
    public int MaxAmmout;

}
