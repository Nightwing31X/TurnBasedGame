using GameDev;
using UnityEngine;

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
        public float currentHealth;
        public int maxHealth;
        public int healValue;
        public int meleeDamage;
        public int rangeDamage;


        private void Start()
        {
            _playerManager = GameObject.Find("PlayerManager");
            _playerData = _playerManager.GetComponent<SavePlayerData>();
        }

        public void SetUpPlayerDataForBattle()
        {
            unitName = _playerData.usernameREF;
            unitLevel = _playerData.levelREF;
            currentHealth = _playerData.currentHealthREF;
            maxHealth = _playerData.maxHealthREF;

            healValue = _playerData.healValueREF;

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
        public void Heal()
        {
            currentHealth += healValue;
            _playerManager.GetComponent<Health>().UpdatePlayersHealth(false);
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
}
