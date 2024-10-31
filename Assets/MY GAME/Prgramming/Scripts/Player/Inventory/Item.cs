using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bag Item", menuName = "BagItem")]
public class Item : ScriptableObject
{
    public new string name;
    public string description;
    public RenderTexture artwork;
    public float baseRarity; // e.g., from 0 to 1, where higher is rarer
    public bool isWeapon;
    public bool isPoition;
}