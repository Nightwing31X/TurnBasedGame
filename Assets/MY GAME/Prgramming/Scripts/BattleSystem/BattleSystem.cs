using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameDev;

namespace TurnBase
{
    public class BattleSystem : MonoBehaviour
    {
        public static BattleSystem instance;
        public BattleStates battleState = BattleStates.NotInBattle;


        [Header("Player")]
        public GameObject playerPrefab;
        //public Transform playerBattleStation;
        public Unit playerUnit;
        public BattleHUD playerHUD;
        [Header("Enemy")]
        public GameObject enemyPrefab;
        //public Transform enemyBattleStation;
        public Unit enemyUnit;
        public BattleHUD enemyHUD;

        public Text dialogueText;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null && instance != this)
            {
                Destroy(this);
            }
        }


        //private void Start()
        //{
        //    BattleSystem = BattleStates.NotInBattle;
        //    //StartCoroutine(SetupBattle());
        //    //playerHUD.SetHUD(playerUnit);
        //    //enemyHUD.SetHUD(enemyUnit);
        //}
        void PlayerTurn()
        {
            dialogueText.text = "Choose Action..";
        }
        public void OnAttack()
        {
            if (battleState != BattleStates.PlayerTurn)
            {
                return;
            }
            StartCoroutine(PlayerAttack());
        }
        public void OnHeal()
        {
            if (battleState != BattleStates.PlayerTurn)
            {
                return;
            }
            StartCoroutine(PlayerHeal());
        }
        void EndBattle()
        {
            if (battleState == BattleStates.Win)
            {
                dialogueText.text = $"You Won the battle by defating {enemyUnit.unitDescription} {enemyUnit.unitName}";
            }
            else if (battleState == BattleStates.Lose)
            {
                dialogueText.text = $"You Lost the battle and were defated by {enemyUnit.unitDescription} {enemyUnit.unitName}";
            }
        }
        IEnumerator SetupBattle()
        {
            //GameObject player = Instantiate(playerPrefab, playerBattleStation);
            //playerUnit = player.GetComponent<Unit>();
            //GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);
            //enemyUnit = enemy.GetComponent<Unit>();
            dialogueText.text = $"{enemyUnit.unitDescription} {enemyUnit.unitName} {enemyUnit.unitAction}...";
            yield return new WaitForSeconds(2f);
            battleState = BattleStates.PlayerTurn;
            PlayerTurn();
        }
        IEnumerator PlayerAttack()
        {
            bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
            enemyHUD.SetHealth(enemyUnit);
            dialogueText.text = $"{playerUnit.unitName} attacked {enemyUnit.unitName}";
            yield return new WaitForSeconds(2f);
            if (isDead)
            {
                battleState = BattleStates.Win;
                EndBattle();
            }
            else
            {
                battleState = BattleStates.EnemyTurn;
                StartCoroutine(EnemyTurn());
            }
        }
        IEnumerator PlayerHeal()
        {
            playerUnit.Heal(2);
            playerHUD.SetHealth(playerUnit);
            dialogueText.text = $"{playerUnit.unitName} feels stronger!";
            yield return new WaitForSeconds(2f);
            battleState = BattleStates.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
        IEnumerator EnemyTurn()
        {
            dialogueText.text = $"{enemyUnit.unitName} Attacks!!!";
            yield return new WaitForSeconds(1f);
            bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
            playerHUD.SetHealth(playerUnit);
            yield return new WaitForSeconds(1);
            if (isDead)
            {
                battleState = BattleStates.Lose;
                EndBattle();
            }
            else
            {
                battleState = BattleStates.PlayerTurn;
                PlayerTurn();
            }
        }





    }
    public enum BattleStates
    {
        NotInBattle,
        StartBattle,
        PlayerTurn,
        EnemyTurn,
        Win,
        Lose
    }
}