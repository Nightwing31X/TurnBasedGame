using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public string description;
    public RenderTexture artwork;
    public int meleeDamage;
    public int rangeDamage;
    public int currentHealth;
    public int maxHealth;
    public float baseRarity; // e.g., from 0 to 1, where higher is rarer
}