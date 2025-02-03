using GameDev;
using System.Collections;
using System.Collections.Generic;
using TurnBase;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    // public EnemyType _enemyData;
    // public GameObject enemyObject;
    public EnemyUnit _enemyUnit;
    public float maxHealth;
    public float currentHealth;
    public bool enemyUI;

    [SerializeField] private Image _enemyHealthBar;
    [SerializeField] private Text _enemyHealthText;
    [SerializeField] private Canvas _canvas;
    private void Start()
    {
        // _enemyData = GetComponent<EnemyType>();
        _enemyUnit = GetComponent<EnemyUnit>();
        // _canvas.GetComponent<Canvas>().rootCanvas.worldCamera = Camera.main;
        // SetEnemyHealth();
        ToggleEnemyUIHealth(true);
        // _enemyHealthText.enabled = false;
    }

    private void ToggleEnemyUIHealth(bool value)
    {
        _enemyHealthBar.enabled = value;
        // _enemyHealthText.enabled = value;
        enemyUI = value;
    }


    // public void SetEnemyHealth()
    // {
    //     maxHealth = _enemyUnit.maxHealth;
    //     currentHealth = _enemyUnit.currentHealth;
    //     _enemyHealthBar.fillAmount = Mathf.Clamp01(currentHealth/maxHealth);
    // }

    // public void INVDataUpdate()
    // {
    //     _enemyUnit.SetUpPlayerDataForBattle();
    // }

    public void UpdateEnemyHealth()
    {
        // maxHealth = _enemyData.maxHealthREF;
        // currentHealth = _enemyData.currentHealthREF;
        // if (value)
        // {
        //     _enemyUnit.SetUpEnemyDataForBattle(); //? Have to put this here so that the health elements would get updated
        // }
        maxHealth = _enemyUnit.maxHealth;
        currentHealth = _enemyUnit.currentHealth;
        //_canvas.GetComponent<Canvas>().rootCanvas.worldCamera = Camera.main;

        _enemyHealthText.text = $"{currentHealth}/{maxHealth}";
        _enemyHealthBar.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        // if (value)
        // {
        //     ToggleEnemyUIHealth(true);
        // }
    }
}
