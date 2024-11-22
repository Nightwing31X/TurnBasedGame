using System.Collections;
using System.Collections.Generic;
using GameDev;
using TurnBase;
using UnityEngine;

public class RandomEnemySpawn : MonoBehaviour
{
    // [SerializeField, Tooltip("How many items do you want to spawn in that 'Grid'.")]
    private int _spawnCount = 1;

    [SerializeField, Tooltip("Where the enemy will spawn - As children.")]
    private Transform[] _spawnLocations;

    [SerializeField, Tooltip("Enemy which get spawned.")]

    private GameObject[] enemiesToSpawn;
    private SavePlayerData _savePlayerData;
    [SerializeField, Tooltip("Player's current XP level to affect item rarity.")]
    private int playerXP;

    private void Start()
    {
        _savePlayerData = GameObject.Find("PlayerManager").GetComponent<SavePlayerData>();
        playerXP = _savePlayerData.levelREF;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        playerXP = _savePlayerData.levelREF;

        // Make sure the spawn locations match the spawn count
        if (_spawnLocations.Length < _spawnCount)
        {
            Debug.LogWarning("Not enough spawn locations for the requested spawn count.");
            return;
        }

        // Shuffle spawn locations to randomise placement
        List<Transform> shuffledLocations = new List<Transform>(_spawnLocations);
        Shuffle(shuffledLocations);

        // Randomise the number of items to spawn (from 0 to _spawnCount)
        // int spawnCount = Random.Range(0, _spawnCount + 1);  // This will be a number between 0 and _spawnCount


        int spawnedItems = 0;

        // Don't spawn more items than the available spawn count
        while (spawnedItems < _spawnCount)
        {
            // Select an item based on rarity and player XP
            GameObject itemToSpawn = GetRandomItemBasedOnProbability();

            if (itemToSpawn == null) continue;

            // Get spawn position of the spawn location
            Vector3 spawnPosition = shuffledLocations[spawnedItems].position;

            // Log spawn position and prefab Y value
            Debug.Log($"Spawn Location: {spawnPosition}, Prefab Y value: {itemToSpawn.transform.position.y}");
            Debug.Log(spawnPosition.y);
            //spawnPosition.y = itemToSpawn.transform.localPosition.y;

            // Spawn the item at the spawn location with the current Y value of the prefab
            GameObject spawnedItem = Instantiate(itemToSpawn, spawnPosition, Quaternion.identity, shuffledLocations[spawnedItems].transform);
            //spawnedItem.transform.position = itemToSpawn.transform.position;
            // Log the final position of the instantiated object
            Debug.Log($"Spawned Item Position: {spawnedItem.transform.position}");
            Debug.Log($"Spawned Item Y Position: {spawnedItem.transform.position.y}");

            // BattleSystem.instance.enemy = itemToSpawn; 
            BattleSystem.instance.EnemyCam(itemToSpawn.GetComponent<EnemyType>().enemyType.enemyName);
            spawnedItems++;
        }

        // Debug message for the number of items spawned
        if (spawnedItems == 0)
        {
            Debug.Log("No items spawned this time.");
        }
        else
        {
            Debug.Log(spawnedItems + " items spawned.");
        }
    }



    private GameObject GetRandomItemBasedOnProbability()
    {
        List<(GameObject, float)> weightedItems = new List<(GameObject, float)>();
        float baseRarity;
        foreach (var itemData in enemiesToSpawn)
        {
            // Determine base rarity and player XP relationship
            // try
            // {
            baseRarity = itemData.GetComponent<EnemyType>().enemyType.baseRarity;
            // }
            // catch
            // {
            //     baseRarity = itemData.GetComponentInChildren<ItemPickup>().item.baseRarity;
            // }

            // High chance for matching or lower base rarities
            float spawnChance = 0f;

            if (baseRarity <= playerXP)
            {
                // Items with base rarity equal to or less than player XP have a higher chance
                spawnChance = Mathf.Clamp01(1f - (0.1f * (playerXP - baseRarity))); // Decrease chance with higher rarity
            }
            else
            {
                // Items with base rarity higher than player XP have a very low chance
                spawnChance = Mathf.Clamp01(0.05f / (baseRarity - playerXP)); // Less likely as rarity increases
            }

            // Add to weighted items if the random chance is met
            if (Random.value < spawnChance)
            {
                // weightedItems.Add((itemData.itemPrefab, spawnChance));
                weightedItems.Add((itemData.gameObject, spawnChance));
            }
        }

        if (weightedItems.Count > 0)
        {
            int randomIndex = Random.Range(0, weightedItems.Count);
            return weightedItems[randomIndex].Item1;
        }
        return null;
    }


    // Utility method to shuffle spawn locations
    private void Shuffle<placeHolder>(IList<placeHolder> list)
    {
        int remainingElements = list.Count; // Number of elements left to shuffle
        while (remainingElements > 1)
        {
            remainingElements--; // Decrease the count of remaining elements
            int randomIndex = Random.Range(0, remainingElements + 1); // Random index from 0 to remainingElements
            placeHolder tempValue = list[randomIndex]; // Store the value at the random index
            list[randomIndex] = list[remainingElements]; // Move the last remaining element to the random index
            list[remainingElements] = tempValue; // Place the stored value into the last position
        }
    }
}