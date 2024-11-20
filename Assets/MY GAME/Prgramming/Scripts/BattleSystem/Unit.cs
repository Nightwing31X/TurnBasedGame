using GameDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

namespace TurnBase
{
    public class Unit : MonoBehaviour
    {
        public SavePlayerData _playerData;

        public string unitName;
        public string unitDescription;
        public string unitAction;
        public int unitLevel;
        public int damage;
        public int maxHealth;
        public float currentHealth;


        private void Start()
        {
            _playerData = GameObject.Find("PlayerManager").GetComponent<SavePlayerData>();
        }

        public void SetUpDataForBattle()
        {
            unitName = _playerData.usernameREF;
            unitLevel = _playerData.levelREF;
            maxHealth = _playerData.maxHealthREF;
            currentHealth = _playerData.currentHealthREF;
        }

        // When during TakeDamage pass a damage value in for calculations
        public bool TakeDamage(int damage)
        {
            // Current health is affected by damage amount
            currentHealth -= damage;
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
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
}
