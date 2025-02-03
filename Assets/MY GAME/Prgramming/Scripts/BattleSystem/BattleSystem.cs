using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using GameDev;
using Player;

namespace TurnBase
{
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField] private bool _debugErrors;
        public static BattleSystem instance;
        public BattleStates battleState = BattleStates.NotInBattle;
        public Text dialogueText;
        public bool meleeRange;
        public bool playerPicked;
        public bool playerChoice;
        [SerializeField] private bool isDead;
        [SerializeField] private bool alreadyDefined;
        private bool _blockChange;
        [Header("Action Points")]
        [SerializeField] private Text _actionPointsText;
        [SerializeField] private int _actionPoints = 3;
        [SerializeField] private int _currentActionPoints;

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
        [SerializeField] private bool _wallInFront;
        [SerializeField] private bool _wallInBehide;
        [SerializeField] private float _fleeDelay;

        [Header("Enemy")]
        // public GameObject enemyPrefab;
        [SerializeField] private GameObject _battleCameraEnemy;
        [SerializeField] private GameObject _enemyObject;
        public EnemyUnit enemyUnit;

        // private BattleHUD enemyHUD;

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
            if (_debugErrors)
            {
                Debug.Log("Not in a Battle...");
            }
            battleState = BattleStates.NotInBattle;
        }

        public void EnemyCam(string name)
        {
            _enemyObject = GameObject.Find($"{name}(Clone)"); //? This get defined on start
            _battleCameraEnemy = GameObject.Find("EnemyBattleCamera");
            _battleCameraEnemy.SetActive(false);
        }

        public void BattleChoice()
        {
            // if (_debugErrors)
            // {
            //     Debug.Log("Battle Choice; means popup should be here for if its the player's turn...");
            // }
            battleState = BattleStates.BattleChoice;
        }

        public void StartBattle()
        {
            // if (_debugErrors)
            // {
            //     Debug.Log("BattleStarted...");
            // }
            StartCoroutine(SetupBattle());
        }

        public void ShowBattleChoice(bool fromWho)
        {
            // if (_debugErrors)
            // {
            //     Debug.Log(_enemyObject);
            // }
            _enemyObject.GetComponent<EnemyType>().DefineNames(); //? Gets all the info about the enemy you are looking at
            if (fromWho) // Means it is from the players interact.cs (True) 
            {
                playerChoice = fromWho;

                BattleChoicePopupContainer.SetActive(true);
                BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", true);
                BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", false);
            }
            else // Means it is from the enemy's interact.cs (False)
            {
                if (_debugErrors)
                {
                    Debug.Log("Enemy choice for the battle");
                }
            }


            BattleChoice();
            //battleState = BattleStates.BattleChoice;
        }
        public void HideBattleChoice()
        {
            if (battleState == BattleStates.PlayerTurn)
            {
                playerPicked = true;
                playerPicked = BattleSystem.instance.playerPicked;
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
            playerPicked = BattleSystem.instance.playerPicked;
            _yesBTN.enabled = false;
            _noBTN.enabled = false;
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Hide", true);
            BattleChoicePopupContainer.GetComponent<Animator>().SetBool("Show", false);
            playerHUDContainer.transform.Find("Player HUD").GetComponent<Animator>().SetBool("Show", false);
            BattleHUDContainer.SetActive(true);
            BattleHUDContainer.transform.Find("BattlePlayerHUD").GetComponent<Animator>().SetBool("PlayerInfoOpen", true);
            BattleHUDContainer.transform.Find("All Buttons").GetComponent<Animator>().SetBool("Show", true);
            StartBattle();
            if (_debugErrors)
            {
                if (meleeRange)
                {
                    Debug.Log("Player is in melee range");
                }
                else
                {
                    Debug.Log("Player is in Range distance");
                }
            }
        }

        public void ChangePosition(bool runAway)
        {
            if (_debugErrors)
            {
                Debug.Log("This is the function which checks what position needs to be changed too");
            }
            //? Need to get the position forwards and backwards
            _playerMovement = playerObject.GetComponent<Movement>();
            _playerMovement.checkFlee = runAway;
            // Should be able to just run the function - which does all the checks for me
            _playerMovement.isMoving = true;
            //_playerMovement.BattleMove();
            //_forwardPOS = _playerMovement.walkPosition;
            //_backwardPOS = _playerMovement.behidePosition;
            //_melee = _playerMovement.enemyInFront;
        }

        public void FleeBattle() //? This runs second
        {
            BattleHUDContainer.transform.Find("BattlePlayerHUD").GetComponent<Animator>().SetBool("PlayerInfoOpen", false);
            BattleHUDContainer.transform.Find("All Buttons").GetComponent<Animator>().SetBool("Show", false);

            StartCoroutine(PlayerChangePosition(true));

            //if (PlayerCharacterManager.Instance.male)
            //{
            //    battleCameraMale.GetComponent<Animator>().SetBool("Flee", true);
            //}
            //else
            //{
            //    battleCameraFemale.GetComponent<Animator>().SetBool("Flee", true);
            //}

            //playerHUDContainer.SetActive(true);
            //BattleHUDContainer.SetActive(false);
            //mainCameraREF.SetActive(true);

            //BattleHUDContainer.transform.Find("BattlePlayerHUD").GetComponent<Animator>().SetBool("PlayerInfoOpen", true);
            //BattleHUDContainer.transform.Find("All Buttons").GetComponent<Animator>().SetBool("Show", true);
        }


        public void NoBattleChoicePlayer()
        {
            HideBattleChoice();
            if (_debugErrors)
            {
                Debug.Log("Player choose not to fight...");
            }
        }


        IEnumerator ActionPointsNeeded(int value)
        {
            Debug.Log("Inside the action points needed Enum...");
            dialogueText.text = $"You need {value} Action Points to use that function.";
            yield return new WaitForSeconds(2);
            PlayerTurn(false);
            Debug.Log("End of the action points needed enum");
        }


        void PlayerTurn(bool passingTurn)
        {
            if (passingTurn)
            {
                _currentActionPoints = _actionPoints;
                UpdateActionPoints();
                BlockState(false);
            }
            battleState = BattleStates.PlayerTurn;
            dialogueText.text = "Choose Action...";
            // if (PlayerCharacterManager.Instance.male)
            // {
            //     battleCameraMale.SetActive(true);
            //     _battleCameraEnemy.SetActive(false);
            // }
            // else
            // {
            //     battleCameraFemale.SetActive(true);
            //     _battleCameraEnemy.SetActive(false);
            // }
        }
        public void OnWalk()
        {
            dialogueText.text = $"{playerUnit.unitName} walks...";
        }

        public void onFlee()
        {
            dialogueText.text = $"{playerUnit.unitName} has ran away!";
        }

        public void OnAttack() // Player button AttackBTN
        {
            if (_currentActionPoints > 0)
            {
                if (battleState != BattleStates.PlayerTurn)
                {
                    return;
                }
                StartCoroutine(PlayerAttack());
            }
        }
        public void OnHeal()
        {
            // float _halfMaxHealthValue = playerUnit.maxHealth * 0.5f;
            // if (playerUnit.currentHealth <= _halfMaxHealthValue)
            if (playerUnit.currentHealth < playerUnit.maxHealth)
            {
                if (_currentActionPoints > 0)
                {
                    if (battleState != BattleStates.PlayerTurn)
                    {
                        return;
                    }
                    StartCoroutine(PlayerHeal());
                }
            }
            else
            {
                // dialogueText.text = $"Need to be {_halfMaxHealthValue} or below to heal.";
                dialogueText.text = $"Need to be below max health to heal.";
            }
        }
        public void OnBlock()
        {
            if (_currentActionPoints > 0)
            {
                if (battleState != BattleStates.PlayerTurn)
                {
                    return;
                }
                StartCoroutine(PlayerBlock());
            }
        }
        public void OnPosition()
        {
            WallCheck();
            if (_currentActionPoints == _actionPoints)
            {
                if (battleState == BattleStates.PlayerTurn)
                {
                    StartCoroutine(PlayerChangePosition(false));
                }
            }
            else
            {
                StartCoroutine(ActionPointsNeeded(_actionPoints));
            }
        }
        public void OnFlee()
        {
            WallCheck();
            if (_currentActionPoints == _actionPoints)
            {
                if (battleState == BattleStates.PlayerTurn)
                {
                    StartCoroutine(PlayerFlee());
                }
            }
            else
            {
                StartCoroutine(ActionPointsNeeded(_actionPoints));
            }
        }

        void EndBattle()
        {
            if (battleState == BattleStates.Win)
            {
                dialogueText.text = $"You Won the battle by defeating {enemyNameText.text}";
                GameManager.instance.DefeatedEnemy();
                StartCoroutine(PlayerFlee());
            }
            else if (battleState == BattleStates.Lose)
            {
                dialogueText.text = $"You Lost the battle and were defeated by {enemyNameText.text}";
                GameManager.instance.OnDeath();
            }
            else
            {
                if (_debugErrors)
                {
                    Debug.Log("Fled the battle...");
                }
            }
        }

        void UpdateActionPoints()
        {
            if (_currentActionPoints <= 0)
            {
                _currentActionPoints = 0;
            }
            _actionPointsText.text = $"Action Points: {_currentActionPoints.ToString()}/{_actionPoints.ToString()}";
        }

        void WallCheck()
        {
            _wallInFront = playerObject.GetComponent<Movement>()._wallInFront;
            _wallInBehide = playerObject.GetComponent<Movement>()._wallInBehide;
        }

        IEnumerator SetupBattle()
        {
            _currentActionPoints = _actionPoints;
            UpdateActionPoints();
            if (PlayerCharacterManager.Instance.male)
            {
                if (playerChoice)
                {
                    if (_debugErrors)
                    {
                        Debug.Log("Should be male");
                    }
                    if (!alreadyDefined)
                    {
                        alreadyDefined = true;
                        playerObject = GameObject.Find("MalePlayer");
                        playerUnit = playerObject.GetComponent<Unit>();
                        playerUnit.SetUpPlayerDataForBattle();

                        enemyUnit = _enemyObject.GetComponent<EnemyUnit>();
                        enemyUnit.SetUpEnemyDataForBattle();

                        playerHUD = GameObject.Find("BattleManager").GetComponent<BattleHUD>();
                        mainCameraREF = Camera.main.gameObject;
                        playerHUD.SetHUD(playerUnit);
                        mainCameraREF.SetActive(false);
                        battleCameraMale.SetActive(true);
                        battleCameraMale.GetComponent<Animator>().SetBool("Player", true);
                        PlayerTurn(false);
                    }
                    else
                    {
                        playerHUD.SetHUD(playerUnit);
                        mainCameraREF.SetActive(false);
                        battleCameraMale.SetActive(true);
                        battleCameraMale.GetComponent<Animator>().SetBool("Player", true);
                        PlayerTurn(false);
                    }
                }
            }
            else
            {
                if (playerChoice)
                {
                    if (_debugErrors)
                    {
                        Debug.Log("Should be female");
                    }
                    if (!alreadyDefined)
                    {
                        alreadyDefined = true;
                        playerObject = GameObject.Find("FemalePlayer");
                        playerUnit = playerObject.GetComponent<Unit>();
                        playerUnit.SetUpPlayerDataForBattle();

                        enemyUnit = _enemyObject.GetComponent<EnemyUnit>();
                        enemyUnit.SetUpEnemyDataForBattle();

                        playerHUD = GameObject.Find("BattleManager").GetComponent<BattleHUD>();
                        mainCameraREF = Camera.main.gameObject;
                        playerHUD.SetHUD(playerUnit);
                        mainCameraREF.SetActive(false);
                        battleCameraFemale.SetActive(true);
                        battleCameraFemale.GetComponent<Animator>().SetBool("Player", true);
                    }
                    else
                    {
                        playerHUD.SetHUD(playerUnit);
                        mainCameraREF.SetActive(false);
                        battleCameraFemale.SetActive(true);
                        battleCameraFemale.GetComponent<Animator>().SetBool("Player", true);
                    }
                }
                PlayerTurn(false);
            }

            WallCheck();
            yield return new WaitForSeconds(2f);
            if (_debugErrors)
            {
                Debug.Log("Animation should play...");
            }
            playerHUDContainer.SetActive(false);
            PlayerTurn(false);
        }
        IEnumerator PlayerAttack()
        {
            //playerObject.GetComponent<Animator>().SetTrigger("Block01");
            if (battleState == BattleStates.PlayerTurn)
            {
                if (_debugErrors)
                {
                    Debug.Log("Player has chosen to attack");
                }
                if (meleeRange)
                {
                    Debug.Log("Player did melee damage");
                    isDead = enemyUnit.TakeDamage(playerUnit.meleeDamage);
                    playerObject.GetComponent<Animator>().SetTrigger("Attack04");
                    yield return new WaitForSeconds(0.5f);
                    _enemyObject.GetComponent<Animator>().SetTrigger("Stun");
                }
                else
                {
                    if (_debugErrors)
                    {
                        Debug.Log("Range");
                    }
                    Debug.Log("Player did range damage");
                    isDead = enemyUnit.TakeDamage(playerUnit.rangeDamage);
                    playerObject.GetComponent<Animator>().SetTrigger("RangeAttack01");
                    yield return new WaitForSeconds(1.5f);
                    _enemyObject.GetComponent<Animator>().SetTrigger("Stun");
                }
                dialogueText.text = $"{playerUnit.unitName} attacks {enemyNameText.text}";
            }
            yield return new WaitForSeconds(3f);
            // if (_debugErrors)
            // {
            //     Debug.Log("Player's turn must end after this...");
            // }
            // if (PlayerCharacterManager.Instance.male)
            // {
            //     battleCameraMale.SetActive(false);
            //     _battleCameraEnemy.SetActive(true);
            // }
            // else
            // {
            //     battleCameraFemale.SetActive(false);
            //     _battleCameraEnemy.SetActive(true);
            // }
            if (isDead)
            {
                battleState = BattleStates.Win;
                EndBattle();
            }
            else
            {
                StartCoroutine(EnemyTurn());
            }
        }
        IEnumerator PlayerChangePosition(bool fromFlee) //? This will change the player's position - Range or Melee
        {
            if (_debugErrors)
            {
                Debug.Log("Change Position");
            }
            ChangePosition(fromFlee);
            if (fromFlee)
            {
                onFlee();
            }
            else
            {
                OnWalk();
            }
            yield return new WaitForSeconds(0.1f);
            // yield return new WaitForSeconds(8f);
            if (!fromFlee)
            //Debug.Log("Player is now in the Position: {newPosition} --- Use all 3 Action Points...");
            {
                StartCoroutine(EnemyTurn());
            }
            else
            {
                if (PlayerCharacterManager.Instance.male)
                {
                    battleCameraMale.GetComponent<Animator>().SetBool("Flee", true);
                }
                else
                {
                    Debug.Log("Should run!!");
                    battleCameraFemale.GetComponent<Animator>().SetBool("Flee", true);
                }
            }
        }

        IEnumerator PlayerFlee() //? This runs after the player has pressed the Flee Button (First thing to run)
        {
            if (_debugErrors)
            {
                Debug.Log("Undo the camera");
            }
            if (!_wallInBehide)
            {
                _fleeDelay = 11f;
                // FleeBattle(); //? This runs second
            }
            else
            {
                _fleeDelay = 3.1f;
                // BattleHUDContainer.transform.Find("BattlePlayerHUD").GetComponent<Animator>().SetBool("PlayerInfoOpen", false);
                // BattleHUDContainer.transform.Find("All Buttons").GetComponent<Animator>().SetBool("Show", false);
            }
            FleeBattle(); //? This runs second
            yield return new WaitForSeconds(_fleeDelay);
            if (_debugErrors)
            {
                Debug.Log("Should all be done now...");
            }
            NotInBattle();
            mainCameraREF.SetActive(true);
            playerHUDContainer.SetActive(true);
            playerHUDContainer.transform.Find("Player HUD").GetComponent<Animator>().SetBool("Show", true);
            if (PlayerCharacterManager.Instance.male)
            {
                battleCameraMale.SetActive(false);
            }
            else
            {
                battleCameraFemale.SetActive(false);
            }
            _battleCameraEnemy.SetActive(false);
            if (_debugErrors)
            {
                Debug.Log("Back to normal game - Enemy's turn - It costs 3 action points to leave...");
            }
        }

        IEnumerator PlayerHeal()
        {
            _currentActionPoints = _currentActionPoints - 1;
            UpdateActionPoints();
            playerUnit.Heal();
            playerHUD.SetHealth(playerUnit);
            dialogueText.text = $"{playerUnit.unitName} feels stronger!";
            yield return new WaitForSeconds(2f);
            if (_currentActionPoints > 0)
            {
                PlayerTurn(false);
            }
            else
            {
                if (_debugErrors)
                {
                    Debug.Log("Enemy's Turn now...");
                }
                StartCoroutine(EnemyTurn());
            }
        }

        void BlockState(bool state)
        {
            if (state)
            {
                _blockChange = true;
                playerObject.GetComponent<Animator>().SetBool("Block", _blockChange);
                dialogueText.text = $"{playerUnit.unitName} is blocking!";
            }
            else
            {
                _blockChange = false;
                playerObject.GetComponent<Animator>().SetBool("Block", _blockChange);
                dialogueText.text = $"{playerUnit.unitName} is no longer blocking.";
            }
        }
        IEnumerator PlayerBlock()
        {
            if (_debugErrors)
            {
                Debug.Log("Show the block animation");
            }
            // if (!_blockChange)
            // {
            //     _currentActionPoints = _currentActionPoints - 3;
            //     UpdateActionPoints();
            // }
            // else
            // {
            //     _currentActionPoints = _currentActionPoints - 1;
            //     UpdateActionPoints();
            // }
            // _blockChange = !_blockChange;
            BlockState(true);
            // _blockChange = true;
            // playerObject.GetComponent<Animator>().SetBool("Block", _blockChange);
            // dialogueText.text = $"{playerUnit.unitName} is blocking!";
            // if (_blockChange)
            // {
            //     dialogueText.text = $"{playerUnit.unitName} is blocking!";
            // }
            // else
            // {
            //     dialogueText.text = $"{playerUnit.unitName} is no longer blocking.";
            // }
            yield return new WaitForSeconds(2f);
            if (_debugErrors)
            {
                Debug.Log("Enemy's Turn now...");
            }
            StartCoroutine(EnemyTurn());
            // if (!_blockChange && _currentActionPoints > 0)
            // {
            //     PlayerTurn(false);
            // }
            // else
            // {
            //     if (_debugErrors)
            //     {
            //         Debug.Log("Enemy's Turn now...");
            //     }
            //     StartCoroutine(EnemyTurn());
            // }
        }
        IEnumerator EnemyTurn()
        {
            battleState = BattleStates.EnemyTurn;
            // if (PlayerCharacterManager.Instance.male)
            // {
            //     battleCameraMale.SetActive(false);
            //     _battleCameraEnemy.SetActive(true);
            // }
            // else
            // {
            //     battleCameraFemale.SetActive(false);
            //     _battleCameraEnemy.SetActive(true);
            // }
            // if (_debugErrors)
            // {
            //     Debug.Log("Need to write a check to see the players health...");
            //     Debug.Log("Need to write a check to see your own health (enemy)...");
            //     Debug.Log("Choose attack - range, melee, flee...");
            // }
            //dialogueText.text = $"{enemyNameText.text} attacks {playerUnit.unitName}";
            dialogueText.text = $"{enemyNameText.text} will do some sort of attack to {playerUnit.unitName}";


            yield return new WaitForSeconds(2f);
            if (battleState == BattleStates.EnemyTurn)
            {
                if (_debugErrors)
                {
                    Debug.Log("Enemy has chosen to attack");
                }
                if (meleeRange)
                {
                    if (_debugErrors)
                    {
                        Debug.Log("Melee");
                    }
                    _enemyObject.GetComponent<Animator>().SetTrigger("Attack01");
                    if (_blockChange) //? Deals less damage to the player
                    {
                        Debug.Log("Player was blocking; half melee damage");
                        Debug.Log(enemyUnit.meleeDamageREF);
                        isDead = playerUnit.TakeDamage(enemyUnit.meleeDamageREF / 2);
                        playerObject.GetComponent<Animator>().SetTrigger("StunBlock");
                    }
                    else //? Deals max damage to the player
                    {
                        Debug.Log("Player didn't block; max melee damage");
                        isDead = playerUnit.TakeDamage(enemyUnit.meleeDamageREF);
                        playerObject.GetComponent<Animator>().SetTrigger("Stun");
                    }
                }
                else
                {
                    if (_debugErrors)
                    {
                        Debug.Log("Range");
                    }
                    _enemyObject.GetComponent<Animator>().SetTrigger("RangeAttack01");
                    if (_blockChange) //? Deals less damage to the player
                    {
                        Debug.Log("Player was blocking; half range damage");
                        isDead = playerUnit.TakeDamage(enemyUnit.rangeDamageREF / 2);
                        playerObject.GetComponent<Animator>().SetTrigger("StunBlock");
                    }
                    else //? Deals max damage to the player
                    {
                        Debug.Log("Player didn't block; max range damage");
                        isDead = playerUnit.TakeDamage(enemyUnit.rangeDamageREF);
                        playerObject.GetComponent<Animator>().SetTrigger("Stun");
                    }
                }
                dialogueText.text = $"{enemyNameText.text} attacks {playerUnit.unitName}";
            }
            // bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
            //playerHUD.SetHealth(playerUnit);
            yield return new WaitForSeconds(2f);
            if (isDead)
            {
                battleState = BattleStates.Lose;
                EndBattle();
            }
            else
            {
                PlayerTurn(true);
            }
        }
    }
    public enum BattleStates
    {
        NotInBattle, BattleChoice, StartBattle, PlayerTurn, EnemyTurn, Win, Lose
    }
}