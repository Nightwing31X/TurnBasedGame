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
        // public GameObject enemyPrefab;
        //public Transform enemyBattleStation;
        public Unit enemyUnit;
        public BattleHUD enemyHUD;

        public Text dialogueText;

        public bool meleeRange;

        public GameObject enemy;

        [Header("Object for which is being displayed in the BattleChoice")]
        public GameObject BattleChoicePopupContainer;
        public Text NamePersonDetailText;
        public Text DescriptionPersonDetailText;
        public RawImage IconPersonDetail;



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
            NotInBattle();
        }



        public void NotInBattle()
        {
            Debug.Log("Not in a Battle...");
            battleState = BattleStates.NotInBattle;
            // enemy = enemyPrefab;
            enemy = GameObject.FindWithTag("Enemy");
        }
        public void BattleChoice()
        {
            Debug.Log("Battle Choice; means popup should be here for if its the player's turn...");
            battleState = BattleStates.BattleChoice;
        }

        public void StartBattle()
        {
            Debug.Log("BattleStarted...");
            StartCoroutine(SetupBattle());
        }

        //public void OnPlayerTurn()
        //{
        //    battleState = BattleStates.PlayerTurn;
        //    Debug.Log("Play's turn to choose actions in battle");
        //}

        //public void OnEnemyTurn()
        //{
        //    battleState = BattleStates.EnemyTurn;
        //    Debug.Log("Enemy's turn to choose actions in battle");
        //}


        public void ShowBattleChoice(bool fromWho)
        {
            if (fromWho) // Means it is from the players interact.cs (True) 
            {
                NamePersonDetailText.text = enemy.GetComponent<EnemyType>().enemyType.enemyName;
                DescriptionPersonDetailText.text = enemy.GetComponent<EnemyType>().enemyType.description;
                IconPersonDetail.texture = enemy.GetComponent<EnemyType>().enemyType.artwork;


                BattleChoicePopupContainer.SetActive(true);
                BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", true);
                BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", false);
            }
            else // Means it is from the enemy's interact.cs (False)
            {
                
            }


            BattleChoice();
        }
        public void HideBattleChoice()
        {
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", true);
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", false);
            // BattleChoicePopupContainer.SetActive(true);
            NotInBattle();
        }


        public void YesBattleChoice()
        {
            if (meleeRange)
            {
                Debug.Log("Display only Melee Attacks...");
            }
            else
            {
                Debug.Log("Display only Range Attacks...");
            }
        }


        public void NoBattleChoice()
        {
            HideBattleChoice();
            Debug.Log("Person choose not to fight...");
        }



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
            if (PlayerCharacterManager.Instance.male)
            {
                playerPrefab = GameObject.Find("MalePlayer");
            }
            else
            {
                playerPrefab = GameObject.Find("FemlePlayer");
            }
            GameObject player = Instantiate(playerPrefab);
            //GameObject player = Instantiate(playerPrefab, playerBattleStation);
            // playerUnit = player.GetComponent<Unit>();
            //GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);

            // enemy = Instantiate(enemyPrefab);
            // enemyUnit = enemy.GetComponent<Unit>();
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
        NotInBattle, BattleChoice, StartBattle, PlayerTurn, EnemyTurn, Win, Lose
    }
}