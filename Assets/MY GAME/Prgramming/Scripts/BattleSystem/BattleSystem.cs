using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameDev;
using Player;

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


        private bool _blockChange;

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

        [Header("Player Position REFs")]
        [SerializeField] private Movement _playerMovement;
        //[SerializeField] private GameObject _forwardPOS;
        //[SerializeField] private GameObject _backwardPOS;
        //[SerializeField] private bool _melee;
        //[SerializeField] private bool _range;

        [Header("Enemy")]
        // public GameObject enemyPrefab;
        [SerializeField] private GameObject battleCameraEnemy;
        public GameObject enemy;
        //public Transform enemyBattleStation;
        //public Unit enemyUnit;
        public BattleHUD enemyHUD;

        [Header("Object for which is being displayed in the BattleChoice")]
        public GameObject BattleChoicePopupContainer;
        public Text enemyNameText;
        public Text enemyDescriptionText;
        public RawImage enemyIconImage;



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
            Debug.Log(enemy);
            enemy.GetComponent<EnemyType>().DefineNames(); //? Gets all the info about the enemy you are looking at
            if (fromWho) // Means it is from the players interact.cs (True) 
            {
                playerChoice = fromWho;

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
            if (battleState == BattleStates.PlayerTurn)
            {
                playerPicked = true;
                _noBTN.enabled = false;
                _yesBTN.enabled = false;
                BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", true);
                BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", false);
                // BattleChoicePopupContainer.SetActive(true);
                NotInBattle();
            }
        }


        public void YesBattleChoicePlayer() //? Gets active from the yes button on the PopUp
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

        public void ChangePosition()
        {
            if (battleState == BattleStates.PlayerTurn)
            {
                Debug.Log("This is the function which checks what position needs to be changed too");
                //? Need to get the position forwards and backwards
                _playerMovement = playerObject.GetComponent<Movement>();
                // Should be able to just run the function - which does all the checks for me
                _playerMovement.isMoving = true;
                //_playerMovement.BattleMove();
                //_forwardPOS = _playerMovement.walkPosition;
                //_backwardPOS = _playerMovement.behidePosition;
                //_melee = _playerMovement.enemyInFront;
            }
            else
            {
                if (PlayerCharacterManager.Instance.male)
                {
                    battleCameraMale.SetActive(false);
                    battleCameraEnemy.SetActive(true);
                }
                else
                {
                    battleCameraFemale.SetActive(false);
                    battleCameraEnemy.SetActive(true);
                }
            }
        }

        public void FleeBattle() //? This runs second
        {
            if (battleState == BattleStates.PlayerTurn)
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
        public void OnPosition()
        {
            if (battleState != BattleStates.PlayerTurn)
            {
                return;
            }
            StartCoroutine(PlayerChangePosition());
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
                dialogueText.text = $"You Won the battle by defating {enemyNameText.text}";
            }
            else if (battleState == BattleStates.Lose)
            {
                dialogueText.text = $"You Lost the battle and were defated by {enemyNameText.text}";
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

            yield return new WaitForSeconds(2f);
            Debug.Log("Animation should play...");
            playerHUDContainer.SetActive(false);
            battleState = BattleStates.PlayerTurn;
            PlayerTurn();
        }
        IEnumerator PlayerAttack()
        {
            if (PlayerCharacterManager.Instance.male)
            {
                battleCameraMale.SetActive(false);
                battleCameraEnemy.SetActive(true);
            }
            else
            {
                battleCameraFemale.SetActive(false);
                battleCameraEnemy.SetActive(true);
            }
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

                dialogueText.text = $"{playerUnit.unitName} attacks {enemyNameText.text}";
            }
            yield return new WaitForSeconds(3f);
            Debug.Log("Player's turn must end after this...");
            //bool isDead = GameManager.instance.isPlayerDead;
            //if (isDead)
            //{
            //    battleState = BattleStates.Win
            //}

            if (!true)
            {
                battleState = BattleStates.Win;
                EndBattle();
            }
            else
            {
                battleState = BattleStates.EnemyTurn;
                StartCoroutine(EnemyTurn());
            }

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
        IEnumerator PlayerChangePosition() //? This will change the player's position - Range or Melee
        {
            Debug.Log("Change Position");
            ChangePosition();
            yield return new WaitForSeconds(10f);
            Debug.Log("Player is now in the Position: {newPosition} --- Use all 3 Action Points...");
            battleState = BattleStates.EnemyTurn;
            StartCoroutine(EnemyTurn());
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
            battleState = BattleStates.EnemyTurn;
            StartCoroutine(EnemyTurn());
            //if (!true)
            //{
            //    battleState = BattleStates.Win;
            //    EndBattle();
            //}
            //else
            //{
            //    battleState = BattleStates.EnemyTurn;
            //    StartCoroutine(EnemyTurn());
            //}
            if (PlayerCharacterManager.Instance.male)
            {
                battleCameraMale.SetActive(false);
                battleCameraEnemy.SetActive(true);
            }
            else
            {
                battleCameraFemale.SetActive(false);
                battleCameraEnemy.SetActive(true);
            }
        }
        IEnumerator PlayerBlock()
        {
            Debug.Log("Show the block animation");
            _blockChange = !_blockChange;
            playerObject.GetComponent<Animator>().SetBool("Block", _blockChange);
            dialogueText.text = $"{playerUnit.unitName} is blocking!";
            yield return new WaitForSeconds(2f);
            Debug.Log("Enemy's Turn now...");
            battleState = BattleStates.EnemyTurn;
            StartCoroutine(EnemyTurn());
            //if (!true)
            //{
            //    battleState = BattleStates.Win;
            //    EndBattle();
            //}
            //else
            //{
            //    battleState = BattleStates.EnemyTurn;
            //    StartCoroutine(EnemyTurn());
            //}
            if (PlayerCharacterManager.Instance.male)
            {
                battleCameraMale.SetActive(false);
                battleCameraEnemy.SetActive(true);
            }
            else
            {
                battleCameraFemale.SetActive(false);
                battleCameraEnemy.SetActive(true);
            }
        }
        IEnumerator EnemyTurn()
        {
            Debug.Log("Need to write a check to see the players health...");
            Debug.Log("Need to write a check to see your own health (enemey)...");
            Debug.Log("Choose attack - range, melee, flee...");
            //dialogueText.text = $"{enemyNameText.text} attacks {playerUnit.unitName}";
            dialogueText.text = $"{enemyNameText.text} will do some sort of attack to {playerUnit.unitName}";


            yield return new WaitForSeconds(2f);
            if (battleState == BattleStates.EnemyTurn)
            {
                Debug.Log("Enemy has chosen to attack");
                //if (meleeRange)
                //{
                //    Debug.Log("Melee");
                //    enemy.GetComponent<Animator>().SetTrigger("Attack04");
                //}
                //else
                //{
                //    Debug.Log("Range");
                //    enemy.GetComponent<Animator>().SetTrigger("RangeAttack01");
                //}

                dialogueText.text = $"{enemyNameText.text} attacks {playerUnit.unitName}";
            }
            //bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
            //playerHUD.SetHealth(playerUnit);
            yield return new WaitForSeconds(2f);
            if (!true)
            {
                battleState = BattleStates.Lose;
                EndBattle();
            }
            else
            {
                battleState = BattleStates.PlayerTurn;
                PlayerTurn();
            }
            //if (isDead)
            //{
            //    battleState = BattleStates.Lose;
            //    EndBattle();
            //}
            //else
            //{
            //    battleState = BattleStates.PlayerTurn;
            //    PlayerTurn();
            //}
        }
    }
    public enum BattleStates
    {
        NotInBattle, BattleChoice, StartBattle, PlayerTurn, EnemyTurn, Win, Lose
    }
}