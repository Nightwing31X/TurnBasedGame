using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bag Item", menuName = "BagItem")]
public class Item : ScriptableObject
{
    public new string name;
    public string description;
    public RenderTexture artwork;
    public bool isWeapon;
    public bool isPoition;
}