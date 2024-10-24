using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public bool male;
    public int level;
}

