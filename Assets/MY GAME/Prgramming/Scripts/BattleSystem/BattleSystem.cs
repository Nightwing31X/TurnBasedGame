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

        [Header("Buttons")]
        [SerializeField] private Button _yesBTN;
        [SerializeField] private Button _noBTN;


        [Header("Player")]
        //public GameObject playerPrefab;
        [SerializeField] private GameObject mainCameraREF;
        [SerializeField] private GameObject battleCameraMale;
        [SerializeField] private GameObject battleCameraFemale;
        [SerializeField] private GameObject playerHUDContainer;
        [SerializeField] private GameObject BattleHUDContainer;
        [SerializeField] BattleHUD playerHUD;
        //public Transform playerBattleStation;
        [SerializeField] private GameObject playerObject;
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
        }


        void Start()
        {
            NotInBattle();
        }


        public void NotInBattle()
        {
            BattleHUDContainer.SetActive(false);
            _noBTN.enabled = true;
            _yesBTN.enabled = true;
            Debug.Log("Not in a Battle...");
            battleState = BattleStates.NotInBattle;
            // enemy = enemyPrefab;
            // if (enemy == null)
            // {
            //     enemy = GameObject.FindWithTag("Enemy");
            // }
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
            enemy = GameObject.FindWithTag("Enemy");
            if (fromWho) // Means it is from the players interact.cs (True) 
            {
                playerChoice = fromWho;
                enemy.GetComponent<EnemyType>().DefineNames();
                // Debug.Log(NamePersonDetailText.text);
                // NamePersonDetailText.text = enemy.GetComponent<EnemyType>().enemyType.enemyName;
                // DescriptionPersonDetailText.text = enemy.GetComponent<EnemyType>().enemyType.description;
                // IconPersonDetail.texture = enemy.GetComponent<EnemyType>().enemyType.artwork;


                BattleChoicePopupContainer.SetActive(true);
                BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", true);
                BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", false);
            }
            else // Means it is from the enemy's interact.cs (False)
            {
                Debug.Log("Enemy choice for the battle");
            }


            BattleChoice();
            //battleState = BattleStates.BattleChoice;
        }
        public void HideBattleChoice()
        {
            playerPicked = true;
            _noBTN.enabled = false;
            _yesBTN.enabled = false;
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", true);
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", false);
            // BattleChoicePopupContainer.SetActive(true);
            NotInBattle();
        }


        public void YesBattleChoicePlayer()
        {
            playerPicked = true;
            _yesBTN.enabled = false;
            _noBTN.enabled = false;
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", true);
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", false);
            playerHUDContainer.transform.Find("Player HUD").GetComponent<Animator>().SetBool("Show", false);
            BattleHUDContainer.SetActive(true);
            BattleHUDContainer.transform.Find("BattlePlayerHUD").GetComponent<Animator>().SetBool("PlayerInfoOpen", true);
            BattleHUDContainer.transform.Find("All Buttons").GetComponent<Animator>().SetBool("Show", true);
            StartBattle();
            if (meleeRange)
            {
                Debug.Log("Player is in melee range");
            }
            else
            {
                Debug.Log("Player is in Range distance");
            }
        }

        public void FleeBattle() //? This runs second
        {
            BattleHUDContainer.transform.Find("BattlePlayerHUD").GetComponent<Animator>().SetBool("PlayerInfoOpen", false);
            BattleHUDContainer.transform.Find("All Buttons").GetComponent<Animator>().SetBool("Show", false);

            if (PlayerCharacterManager.Instance.male)
            {
                battleCameraMale.GetComponent<Animator>().SetBool("Flee", true);
                //battleCameraMale.SetActive(false);
            }
            else
            {
                battleCameraFemale.GetComponent<Animator>().SetBool("Flee", true);
                //battleCameraFemale.SetActive(false);
            }

            EndBattle();
            //playerHUDContainer.SetActive(true);
            //BattleHUDContainer.SetActive(false);
            //mainCameraREF.SetActive(true);

            //BattleHUDContainer.transform.Find("BattlePlayerHUD").GetComponent<Animator>().SetBool("PlayerInfoOpen", true);
            //BattleHUDContainer.transform.Find("All Buttons").GetComponent<Animator>().SetBool("Show", true);

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
        public void OnBlock()
        {
            if (battleState != BattleStates.PlayerTurn)
            {
                return;
            }
            StartCoroutine(PlayerBlock());
        }
        public void OnFlee()
        {
            if (battleState != BattleStates.PlayerTurn)
            {
                return;
            }
            StartCoroutine(PlayerFlee());
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
            else
            {
                Debug.Log("Fled the battle...");
            }
        }
        IEnumerator SetupBattle()
        {
            if (PlayerCharacterManager.Instance.male)
            {
                Debug.Log("Should be male");
                if (playerChoice)
                {
                    playerObject = GameObject.Find("MalePlayer");
                    playerUnit = playerObject.GetComponent<Unit>();
                    playerUnit.SetUpDataForBattle();
                    playerHUD = GameObject.Find("BattleManager").GetComponent<BattleHUD>();
                    playerHUD.SetHUD(playerUnit);
                    mainCameraREF = Camera.main.gameObject;
                    mainCameraREF.SetActive(false);
                    battleCameraMale.SetActive(true);
                    battleCameraMale.GetComponent<Animator>().SetBool("Player", true);
                    battleState = BattleStates.PlayerTurn;
                }
            }
            else
            {
                if (playerChoice)
                {
                    playerObject = GameObject.Find("FemalePlayer");
                    playerUnit = playerObject.GetComponent<Unit>();
                    playerUnit.SetUpDataForBattle();
                    playerHUD = GameObject.Find("BattleManager").GetComponent<BattleHUD>();
                    playerHUD.SetHUD(playerUnit);
                    mainCameraREF = Camera.main.gameObject;
                    mainCameraREF.SetActive(false);
                    battleCameraFemale.SetActive(true);
                    battleCameraFemale.GetComponent<Animator>().SetBool("Player", true);
                    battleState = BattleStates.PlayerTurn;
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
            playerHUDContainer.SetActive(false);
            //battleState = BattleStates.PlayerTurn;
            //PlayerTurn();
        }
        IEnumerator PlayerAttack()
        {
            //playerObject.GetComponent<Animator>().SetTrigger("Block01");
            if (battleState == BattleStates.PlayerTurn)
            {
                Debug.Log("Player has chosen to attack");
                if (meleeRange)
                {
                    playerObject.GetComponent<Animator>().SetTrigger("Attack04");
                }
                else
                {
                    Debug.Log("Range");
                    playerObject.GetComponent<Animator>().SetTrigger("RangeAttack01");
                }

                dialogueText.text = $"{playerUnit.unitName} attacks {NamePersonDetailText.text}";
            }
            yield return new WaitForSeconds(3f);
            Debug.Log("Player's turn must end after this...");
            //bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
            //enemyHUD.SetHealth(enemyUnit);
            //dialogueText.text = $"{playerUnit.unitName} attacked {enemyUnit.unitName}";
            //yield return new WaitForSeconds(2f);
            //if (isDead)
            //{
            //    battleState = BattleStates.Win;
            //    EndBattle();
            //}
            //else
            //{
            //    battleState = BattleStates.EnemyTurn;
            //    StartCoroutine(EnemyTurn());
            //}
        }
        IEnumerator PlayerFlee() //? This runs after the player has pressed the Flee Button (First thing to run)
        {
            Debug.Log("Undo the camera");
            FleeBattle(); // This runs second
            yield return new WaitForSeconds(2.35f);
            NotInBattle();
            mainCameraREF.SetActive(true);
            playerHUDContainer.SetActive(true);
            playerHUDContainer.transform.Find("Player HUD").GetComponent<Animator>().SetBool("Show", true);
            //BattleHUDContainer.SetActive(false);
            if (PlayerCharacterManager.Instance.male)
            {
                battleCameraMale.SetActive(false);
            }
            else
            {
                battleCameraFemale.SetActive(false);
            }
            Debug.Log("Back to normal game - Enemy's turn - It costs 3 action points to leave...");
        }

        IEnumerator PlayerHeal()
        {
            playerUnit.Heal(2);
            playerHUD.SetHealth(playerUnit);
            dialogueText.text = $"{playerUnit.unitName} feels stronger!";
            yield return new WaitForSeconds(2f);
            Debug.Log("Enemy's Turn now...");
            //battleState = BattleStates.EnemyTurn;
            //StartCoroutine(EnemyTurn());
        }
        IEnumerator PlayerBlock()
        {
            Debug.Log("Show the block animation");
            playerObject.GetComponent<Animator>().SetBool("Block", true);
            dialogueText.text = $"{playerUnit.unitName} is blocking!";
            yield return new WaitForSeconds(2f);
            Debug.Log("Enemy's Turn now...");
            //battleState = BattleStates.EnemyTurn;
            //StartCoroutine(EnemyTurn());
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