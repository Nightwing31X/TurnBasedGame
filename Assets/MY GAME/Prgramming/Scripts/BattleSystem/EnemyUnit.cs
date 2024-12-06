using GameDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBase
{
    public class EnemyUnit : MonoBehaviour
    {
        public GameObject _enemyObject;
        public EnemyType _enemyData;
        public string unitName;
        public string unitDescription;
        public string unitAction;
        // public int unitLevel;
        public int meleeDamageREF;
        public int rangeDamageREF;
        public int maxHealth;
        public float currentHealth;


        public void SetUpEnemyDataForBattle()
        {
            _enemyObject = GameObject.FindWithTag("Enemy");
            _enemyData = _enemyObject.GetComponent<EnemyType>();
            unitName = _enemyData.name;
            // unitLevel = _enemyData.baseRarity;
            meleeDamageREF = _enemyData.meleeDamageREF;
            rangeDamageREF = _enemyData.rangeDamageREF;

            maxHealth = _enemyData.maxHealthREF;
            currentHealth = _enemyData.currentHealthREF;

            
        }

        // When during TakeDamage pass a damage value in for calculations
        public bool TakeDamage(int damage)
        {
            // Current health is affected by damage amount
            currentHealth -= damage;
            _enemyObject.GetComponent<EnemyHealth>().UpdateEnemyHealth();
            Debug.Log("Need to add a health bar above the enemy");
            // _playerManager.GetComponent<Health>().UpdatePlayersHealth();
            if (currentHealth <= 0)
            {
                // Say that kills us
                return true;
            }
            else
            {
                // Else say it didn't kill us
                return false;
            }
        }
        public void Heal(int amount)
        {
            currentHealth += amount;
            _enemyObject.GetComponent<EnemyHealth>().UpdateEnemyHealth();
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
}
