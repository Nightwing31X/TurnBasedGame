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
        BattleSystem.instance.NamePersonDetailText.text = enemyType.enemyName;
        BattleSystem.instance.DescriptionPersonDetailText.text = enemyType.description;
        BattleSystem.instance.IconPersonDetail.texture = enemyType.artwork;
    }
}
