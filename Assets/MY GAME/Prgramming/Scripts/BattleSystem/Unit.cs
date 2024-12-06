using GameDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBase
{
    public class Unit : MonoBehaviour
    {
        public GameObject _playerManager;
        public SavePlayerData _playerData;
        public string unitName;
        public string unitDescription;
        public string unitAction;
        public int unitLevel;
        public int meleeDamage;
        public int rangeDamage;
        public int maxHealth;
        public float currentHealth;


        private void Start()
        {
            _playerManager = GameObject.Find("PlayerManager");
            _playerData = _playerManager.GetComponent<SavePlayerData>();
        }

        public void SetUpPlayerDataForBattle()
        {
            unitName = _playerData.usernameREF;
            unitLevel = _playerData.levelREF;
            maxHealth = _playerData.maxHealthREF;
            currentHealth = _playerData.currentHealthREF;
            meleeDamage = _playerData.meleeDamageREF;
            rangeDamage = _playerData.rangeDamageREF;
        }

        // When during TakeDamage pass a damage value in for calculations
        public bool TakeDamage(int damage)
        {
            // Current health is affected by damage amount
            currentHealth -= damage;
            _playerManager.GetComponent<Health>().UpdatePlayersHealth(false);
            // Debug.Log(damage);
            // Debug.Log(currentHealth);
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
            _playerManager.GetComponent<Health>().UpdatePlayersHealth(false);
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
}
