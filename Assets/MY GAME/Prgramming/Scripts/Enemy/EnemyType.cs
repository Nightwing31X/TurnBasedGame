using System.Collections;
using System.Collections.Generic;
using TurnBase;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(EnemyInteract), typeof(EnemyMovement))]
public class EnemyType : MonoBehaviour
{
    public Enemy enemyType;

    public void DefineNames()
    {
        //Debug.Log("Yep running the enemy details...");
        BattleSystem.instance.enemyNameText.text = enemyType.enemyName;
        BattleSystem.instance.enemyDescriptionText.text = enemyType.description;
        BattleSystem.instance.enemyIconImage.texture = enemyType.artwork;
        //Debug.Log("Finished reading all the details...");
    }
}
