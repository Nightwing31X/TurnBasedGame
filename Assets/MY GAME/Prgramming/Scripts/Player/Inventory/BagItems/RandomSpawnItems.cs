using System.Collections;
using System.Collections.Generic;
using GameDev;
using Unity.VisualScripting;
using UnityEngine;

public class RandomSpawnItems : MonoBehaviour
{
    [SerializeField, Tooltip("How many items do you want to spawn in that 'Grid'.")]
    private int _spawnCount;

    [SerializeField, Tooltip("Where the items will spawn - As children.")]
    private Transform[] _spawnLocations;

    [SerializeField, Tooltip("Items which get spawned.")]

    // private SpawnableItemData[] itemsToSpawn;
    private GameObject[] itemsToSpawn;
    private SavePlayerData _savePlayerData;
    [SerializeField, Tooltip("Player's current XP level to affect item rarity.")]
    private int playerXP;

    private void Start()
    {
        _savePlayerData = GameObject.Find("PlayerManager").GetComponent<SavePlayerData>();
        playerXP = _savePlayerData.levelREF;
        SpawnItems();
    }

    private void SpawnItems()
    {
        playerXP = _savePlayerData.levelREF;

        if (_spawnLocations.Length < _spawnCount)
        {
            Debug.LogWarning("Not enough spawn locations for the requested spawn count.");
            return;
        }

        // Sort spawn locations and shuffle them to randomise placement
        List<Transform> shuffledLocations = new List<Transform>(_spawnLocations);
        Shuffle(shuffledLocations);

        int spawnedItems = 0;

        while (spawnedItems < _spawnCount)
        {
            // Select an item based on rarity and player XP
            GameObject itemToSpawn = GetRandomItemBasedOnProbability();
            if (itemToSpawn == null) continue;

            Instantiate(itemToSpawn, shuffledLocations[spawnedItems].position, Quaternion.identity, shuffledLocations[spawnedItems]);
            spawnedItems++;
        }
    }

    private GameObject GetRandomItemBasedOnProbability()
    {
        List<(GameObject, float)> weightedItems = new List<(GameObject, float)>();

        foreach (var itemData in itemsToSpawn)
        {
            // Determine base rarity and player XP relationship
            float baseRarity = itemData.GetComponent<ItemPickup>().item.baseRarity;

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