using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "ItemTest")]
public class Item : ScriptableObject
{
    public new string name;
    public string description;
    public RenderTexture artwork;
}
