using System.Collections;
using TurnBase;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameDev
{
    [AddComponentMenu("GameDev/Game Manager")]
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameStates state = GameStates.PlayerTurn;
        public bool isPlayerDead = false;
        public bool inCutscene = false;
        public bool inPause = false;
        public bool inINV = false;
        public string currentGameState;
        public Unit playerUnit;
        [SerializeField] private GameObject playerObject;


        [Header("GameOver")]
        [SerializeField] private GameObject GameOverContainer;
        [SerializeField] private Text GameOverText;
        [SerializeField] private Text GameOverDefeatedText;
        //[SerializeField] private float gameOverStateDuration = 3f;

        [Header("Credit")]
        [SerializeField] private float timeToCredit = 3f;
        [SerializeField] private float CreditDuration = 3f;
        [SerializeField] private GameObject CreditContainer;


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

            if (GameOverContainer != null)
            {
                Debug.LogWarning("NEED TO PUT IN THE GAMEOVERCONTAINER MENU!!!");
            }
            else
            {
                GameOverContainer.SetActive(false);
            }

            if (CreditContainer == null)
            {
                Debug.LogWarning("NEED TO PUT IN THE CREDITCONTAINER MENU!!!");
            }
            else
            {
                CreditContainer.SetActive(false);
            }

            OnPlayerTurn();
        }

        void Start()
        {
            if (PlayerCharacterManager.Instance.male)
            {
                playerObject = GameObject.Find("MalePlayer");
            }
            else
            {
                playerObject = GameObject.Find("FemalePlayer");
            }
            playerUnit = playerObject.GetComponent<Unit>();
        }

        public void CheckCurrentStat()
        {
            if (state == GameStates.PlayerTurn)
            {
                OnPlayerTurn();
            }
            if (state == GameStates.EnemyTurn)
            {
                OnEnemyTurn();
            }
            else if (state == GameStates.Pause)
            {
                OnPause();
            }
            else if (state == GameStates.Menu)
            {
                OnMenu();
            }
            else if (state == GameStates.Death)
            {
                OnDeath();
            }
            else if (state == GameStates.EndGame)
            {
                OnEndGame();
            }
            else
            {
                Debug.LogWarning("GAME MANAGER - THIS SHOULD NOT SHOW AS CURRENT STATE!!!");
            }
        }

        public void OnPlayerTurn()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = true;
            state = GameStates.PlayerTurn;
            currentGameState = "PlayerTurn";
        }
        public void OnEnemyTurn()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = true;
            state = GameStates.EnemyTurn;
            currentGameState = "EnemyTurn";
        }
        public void OnPause()
        {
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            // if (InputHandler.instance.forceController)
            // {
            //     Cursor.visible = false;
            // }
            // if (InputHandler.instance.onController)
            // {
            //     Cursor.visible = false;
            // }
            // if (InputHandler.instance.onKeyboard)
            // {
            //     Cursor.visible = true;
            // }
            state = GameStates.Pause;
            currentGameState = "Pause";
        }
        public void OnMenu()
        {
            //Cursor.lockState = CursorLockMode.Confined;
            //Cursor.visible = true;
            // if (InputHandler.instance.forceController)
            // {
            //     Cursor.visible = false;
            // }
            // if (InputHandler.instance.onController)
            // {
            //     Cursor.visible = false;
            // }
            // if (InputHandler.instance.onKeyboard)
            // {
            //     Cursor.visible = true;
            // }
            state = GameStates.Menu;
            currentGameState = "Menu";
        }
        public void OnDeath()
        {
            // Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            // if (!InputHandler.instance.forceController)
            // {
            //     Cursor.visible = false;
            // }
            // if (InputHandler.instance.onController)
            // {
            //     Cursor.visible = false;
            // }
            // if (InputHandler.instance.onKeyboard)
            // {
            //     Cursor.visible = true;
            // }
            GameOverContainer.SetActive(true);
            GameOverText.text = $"{playerUnit.unitName} has Died!";
            GameOverDefeatedText.text = $"Defeated by {BattleSystem.instance.enemyNameText.text}";
            isPlayerDead = true;
            state = GameStates.Death;
            currentGameState = "Death";
        }

        public void DefeatedEnemy()
        {
            OnPlayerTurn();
        }

        public void OnEndGame()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = true;
            //Cursor.visible = false;
            state = GameStates.EndGame;
            currentGameState = "EndGame";
        }

        private IEnumerator GameWonSequence(bool restartLevel)
        {
            // instance.OnEndGame();
            OnEndGame();
            yield return new WaitForSeconds(timeToCredit);
            CreditContainer.SetActive(true);
            yield return new WaitForSeconds(CreditDuration);
            if (restartLevel)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }

        public void TriggerWinState(bool restartLevel)
        {
            StartCoroutine(GameWonSequence(restartLevel));
        }

        public void TriggerNextLevelState()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public enum GameStates
    {
        PlayerTurn, EnemyTurn, Pause, Menu, Death, EndGame
    }
}
