using GameDev;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

namespace TurnBase
{
    public class Unit : MonoBehaviour
    {
        public RenderTexture unitIcon;
        public string unitName;
        public string unitDescription;
        public string unitAction;
        public int unitLevel;
        public int damage;
        public int maxHealth;
        public float currentHealth;


        public SavePlayerData _playerData;


        private void Awake()
        {
            unitName = _playerData.usernameREF;
            unitLevel = _playerData.levelREF;
        }

        // When durring TakeDamge pass a damge value in for calculations
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
