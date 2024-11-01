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

        public Text dialogueText;

        public bool meleeRange;
        public bool playerPicked;
        public bool playerChoice;

        [Header("Player")]
        //public GameObject playerPrefab;
        [SerializeField] private GameObject battleCameraMale;
        [SerializeField] private GameObject battleCameraFemale;
        [SerializeField] private GameObject playerHUDContainer;
        [SerializeField] private GameObject BattleHUDContainer;
        [SerializeField] BattleHUD playerHUD;
        //public Transform playerBattleStation;
        public Unit playerUnit;
        [Header("Enemy")]
        // public GameObject enemyPrefab;
        [SerializeField] private GameObject battleCameraEnemy;
        public GameObject enemy;
        //public Transform enemyBattleStation;
        public Unit enemyUnit;
        public BattleHUD enemyHUD;

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
            BattleHUDContainer.SetActive(false);
            Debug.Log("Not in a Battle...");
            battleState = BattleStates.NotInBattle;
            // enemy = enemyPrefab;
            if (enemy == null)
            {
                enemy = GameObject.FindWithTag("Enemy");             
            }
        }
        public void BattleChoice()
        {
            // Debug.Log("Battle Choice; means popup should be here for if its the player's turn...");
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
                playerChoice = fromWho;
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
            playerPicked = true;
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", true);
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", false);
            // BattleChoicePopupContainer.SetActive(true);
            NotInBattle();
        }


        public void YesBattleChoicePlayer()
        {
            playerPicked = true;
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", true);
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", false);
            playerHUDContainer.SetActive(false);
            BattleHUDContainer.SetActive(true);
            BattleHUDContainer.transform.Find("BattlePlayerHUD").GetComponent<Animator>().SetBool("PlayerInfoOpen", true);
            BattleHUDContainer.transform.Find("All Buttons").GetComponent<Animator>().SetBool("Show", true);
            StartBattle();
        }


        public void NoBattleChoicePlayer()
        {
            HideBattleChoice();
            Debug.Log("Player choose not to fight...");
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

                //battleCameraMale = GameObject.Find("MalePlayerBattleCamera");
                if (playerChoice)
                {
                    Camera.main.gameObject.SetActive(false);
                    battleCameraMale.SetActive(true);
                    battleCameraMale.GetComponent<Animator>().SetBool("Player", true);
                }

            }
            else
            {
                //battleCameraFemale = GameObject.Find("FemalePlayerBattleCamera");
                if (playerChoice)
                {
                    Camera.main.gameObject.SetActive(false);
                    battleCameraFemale.SetActive(true);
                    battleCameraMale.GetComponent<Animator>().SetBool("Player", true);
                }
            }


            battleCameraEnemy = GameObject.Find("EnemyBattleCamera");

            //GameObject player = Instantiate(playerPrefab);
            //GameObject player = Instantiate(playerPrefab, playerBattleStation);
            // playerUnit = player.GetComponent<Unit>();
            //GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);

            // enemy = Instantiate(enemyPrefab);
            // enemyUnit = enemy.GetComponent<Unit>();
            //dialogueText.text = $"{enemyUnit.unitDescription} {enemyUnit.unitName} {enemyUnit.unitAction}...";
            yield return new WaitForSeconds(2f);
            Debug.Log("Animation should play...");
            //battleState = BattleStates.PlayerTurn;
            //PlayerTurn();
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