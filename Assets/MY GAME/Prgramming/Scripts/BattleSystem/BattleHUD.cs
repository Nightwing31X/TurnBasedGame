using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace TurnBase
{
    public class BattleHUD : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text levelText;
        [SerializeField] private Text healthText;
        [SerializeField] private Image healthBar;
        public void SetHUD(Unit unit)
        {
            nameText.text = unit.unitName;
            levelText.text = unit.unitLevel.ToString();
            healthText.text = $"{unit.currentHealth}/{unit.maxHealth}";
            SetHealth(unit);
        }
        public void SetHealth(Unit unit)
        {
            healthBar.fillAmount = Mathf.Clamp01(unit.currentHealth/unit.maxHealth);
        }
    }
}