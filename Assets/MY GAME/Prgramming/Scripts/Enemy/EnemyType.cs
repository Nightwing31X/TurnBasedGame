using System.Collections;
using System.Collections.Generic;
using TurnBase;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(EnemyInteract), typeof(EnemyMovement))]
public class EnemyType : MonoBehaviour
{
    public Enemy enemyType;
    public EnemyUnit enemyUnit;
    public EnemyHealth enemyHealth;
    public int meleeDamageREF;
    public int rangeDamageREF;
    public int maxHealthREF;
    public int currentHealthREF;

    public void DefineNames()
    {
        //Debug.Log("Yep running the enemy details...");
        BattleSystem.instance.enemyNameText.text = enemyType.enemyName;
        BattleSystem.instance.enemyDescriptionText.text = enemyType.description;
        BattleSystem.instance.enemyIconImage.texture = enemyType.artwork;
        //Debug.Log("Finished reading all the details...");

        meleeDamageREF = enemyType.meleeDamage;
        rangeDamageREF = enemyType.rangeDamage;
        maxHealthREF = enemyType.maxHealth;
        currentHealthREF = enemyType.currentHealth;

        enemyUnit = GetComponent<EnemyUnit>();
        enemyUnit.SetUpEnemyDataForBattle();
        
        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.UpdateEnemyHealth();
    }

    // void Awake()
    // {
    // }
    // void Start()
    // {
    //     enemyUnit.SetUpEnemyDataForBattle();
    // }
}
