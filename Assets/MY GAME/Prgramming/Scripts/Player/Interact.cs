using System;
using System.Collections;
using TMPro;
using TurnBase;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

namespace Player
{
    [AddComponentMenu("GameDev/Player/First Person Interact")]
    public class Interact : MonoBehaviour
    {
        [SerializeField] private bool _debugErrors;
        private Movement _playerMove;
        [SerializeField] private bool _playerPick = false;
        //public GUIStyle crossHair, tooltip;
        public LayerMask Layers;
        public string interactionLayer;
        public string attackLayer;
        [Tooltip("Toggle on to print console messages from this component.")]
        [SerializeField] private bool _checkControllerInput;
        [SerializeField] private bool _forceControllerInput;
        [Tooltip("The distance that the player can reach interactions."), SerializeField, Range(0, 100)] private float distance = 2f;
        [SerializeField, Range(0, 100)] private float attackRadiusDistance = 4f;

        [Header("Wall Hit Checks")]
        public bool wallFrontHit = false;
        public bool wallBehideHit = false;
        [Header("Enemy Checks")]
        public bool enemyFront = false;
        public bool enemyFrontRange = false;
        public bool meleeDistance = false;
        public bool enemyRight = false;
        public bool enemyLeft = false;
        public bool enemyBack = false;

        // public bool showToolTip = false;
        //public string action, button, instruction;
        // public bool pickUpObj;
        // public bool attackToolTip;
        [Header("Buttons")]
        public GameObject firstButton;
        public GameObject keyboardPickUpText; //# Text to pickup things
        public GameObject keyboardAttackText;
        public GameObject controllerPickUpText; //# Text to pickup things
        public GameObject controllerAttackText;

        #region Reference to the player cameras - for the raycast 
        [Header("Cameras for Raycast")]
        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private GameObject _rightCamera;
        [SerializeField] private GameObject _leftCamera;
        [SerializeField] private GameObject _behideCamera;
        #endregion


        void Start()
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            _rightCamera = GameObject.FindGameObjectWithTag("RightCamera");
            _leftCamera = GameObject.FindGameObjectWithTag("LeftCamera");
            _behideCamera = GameObject.FindGameObjectWithTag("BehideCamera");
            _playerMove = GetComponent<Movement>();
            SelectObjectUI();
        }

        public void SelectObjectUI()
        {
            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            EventSystem.current.SetSelectedGameObject(firstButton);
        }

        private void Update()
        {
            if (BattleSystem.instance.battleState == BattleStates.NotInBattle || BattleSystem.instance.battleState == BattleStates.BattleChoice)
            {
                //Debug.LogWarning("Not in battle");
                //_forceControllerInput = InputHandler.instance.forceController;
                //_checkControllerInput = InputHandler.instance.onController;

                #region Raycast for the Forward view
                // create a ray (a Ray is ?? a beam, line that comes into contact with colliders)
                Ray interactRayForward;
                // this ray shoots forward from the center of the camera
                interactRayForward = _mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                // if (_debugErrors)
                // {
                //     Debug.DrawRay(interactRayForward.origin, transform.forward * distance, Color.green); // Forward side
                //     // Debug.DrawRay(interactRayForward.origin, transform.right * distance, Color.green); // Right side
                //     // Debug.DrawRay(interactRayForward.origin, -transform.forward * distance, Color.green); // -transform.forward = Backward side
                //     // Debug.DrawRay(interactRayForward.origin, -transform.right * distance, Color.green); //-transform.right = Left side
                // }
                // create hit info (this holds the info for the stuff we interact with) 
                RaycastHit hitInfoForward;
                // if this physics ray that gets cast in a direction hits a object within our distance and or layer
                if (Physics.Raycast(interactRayForward, out hitInfoForward, distance, Layers /*This part here is the layer its optional*/ ))
                {
                    if (_debugErrors)
                    {
                        Debug.DrawRay(transform.position, transform.forward * distance, Color.yellow, 0.5f);
                    }
                    # region Detect the attack layer (Enemy Layer) - Forward Player view
                    if (hitInfoForward.transform.gameObject.layer == LayerMask.NameToLayer(attackLayer))
                    {
                        if (_debugErrors)
                        {
                            Debug.Log($"Player - Enemy is in melee distance to fight!");
                        }
                        enemyFront = true;
                        enemyFrontRange = false;
                        BattleSystem.instance.meleeRange = meleeDistance;
                        _playerPick = BattleSystem.instance.playerPicked;
                        if (!_playerPick)
                        {
                            DisplayBattleChoicePopup();
                        }
                        else
                        {
                            if (!meleeDistance && _playerPick)
                            {
                                DisplayBattleChoicePopup();
                            }
                        }
                        meleeDistance = true;
                        // showToolTip = true;
                    }
                    # endregion
                }
                else
                {
                    enemyFront = false;
                }
                #endregion

                #region Raycast for the Front side view - Range Attack
                // if (_debugErrors)
                // {
                //     Debug.DrawRay(interactRayForward.origin, transform.forward * attackRadiusDistance, Color.magenta); // Forward side
                // }

                if (Physics.Raycast(interactRayForward, out hitInfoForward, attackRadiusDistance, Layers /*This part here is the layer its optional*/ ))
                {
                    if (!enemyFront)
                    {
                        if (hitInfoForward.transform.gameObject.layer == LayerMask.NameToLayer(attackLayer))
                        {
                            if (_debugErrors)
                            {
                                Debug.Log($"Player - Enemy is in-front; can do range attacks");
                            }

                            enemyFrontRange = true;
                            meleeDistance = false;
                            BattleSystem.instance.meleeRange = meleeDistance;
                            _playerPick = BattleSystem.instance.playerPicked;
                            enemyRight = false;
                            enemyLeft = false;
                            enemyBack = false;
                            if (!_playerPick)
                            {
                                DisplayBattleChoicePopup();
                            }
                        }
                    }
                }
                else
                {
                    enemyFrontRange = false;
                    meleeDistance = false;
                    BattleSystem.instance.meleeRange = meleeDistance;
                    _playerPick = false;
                    BattleSystem.instance.playerPicked = _playerPick;
                }
                #endregion

                # region Raycast Right side 
                # region Raycast for the Right side view
                // this ray shoots forward from the center of the camera (Right)
                Ray interactRayRight;
                // this ray shoots forward from the center of the camera (Right)
                interactRayRight = _rightCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                // if (_debugErrors)
                // {
                //     // Debug.DrawRay(interactRayRight.origin, transform.forward * distance, Color.green); // Forward side
                //     Debug.DrawRay(interactRayRight.origin, transform.right * attackRadiusDistance, Color.magenta); // Right side
                //     // Debug.DrawRay(interactRayRight.origin, -transform.forward * distance, Color.green); // -transform.forward = Backward side
                //     // Debug.DrawRay(interactRayRight.origin, -transform.right * distance, Color.green); //-transform.right = Left side
                // }
                // create hit info (this holds the info for the stuff we interact with) 
                RaycastHit hitInfoRight;

                if (Physics.Raycast(interactRayRight, out hitInfoRight, attackRadiusDistance, Layers /*This part here is the layer its optional*/ ))
                {
                    if (hitInfoRight.transform.gameObject.layer == LayerMask.NameToLayer(attackLayer))
                    {
                        if (_debugErrors)
                        {
                            Debug.Log($"Player - Enemy is on right side");
                        }
                        enemyFront = false;
                        enemyFrontRange = false;
                        enemyRight = true;
                        enemyLeft = false;
                        enemyBack = false;
                    }
                }
                else
                {
                    enemyRight = false;
                    // meleeDistance = false;
                    // BattleSystem.instance.meleeRange = meleeDistance;
                    // _playerPick = false;
                    // BattleSystem.instance.playerPickedNo = _playerPick;
                    // showToolTip = false;
                    // attackToolTip = false;
                    //keyboardPickUpText.SetActive(false); //# Pickup text turns off
                    //keyboardAttackText.SetActive(false);
                    //controllerPickUpText.SetActive(false);
                    //controllerAttackText.SetActive(false);
                }
                #endregion

                #endregion

                # region Raycast Left side
                # region Raycast for the Left side view
                Ray interactRayLeft;
                // this ray shoots forward from the center of the camera (Left)
                interactRayLeft = _leftCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                // if (_debugErrors)
                // {
                //     Debug.DrawRay(interactRayLeft.origin, -transform.right * attackRadiusDistance, Color.magenta); // Left side
                // }
                // create hit info (this holds the info for the stuff we interact with) 
                RaycastHit hitInfoLeft;

                if (Physics.Raycast(interactRayLeft, out hitInfoLeft, attackRadiusDistance, Layers /*This part here is the layer its optional*/ ))
                {
                    if (hitInfoLeft.transform.gameObject.layer == LayerMask.NameToLayer(attackLayer))
                    {
                        if (_debugErrors)
                        {
                            Debug.Log($"Player - Enemy is on left side");
                        }

                        enemyFront = false;
                        enemyFrontRange = false;
                        enemyRight = false;
                        enemyLeft = true;
                        enemyBack = false;

                        // showToolTip = true;
                        // attackToolTip = false;
                        // OnGUI(); // Displays out ToolTip
                    }
                }
                else
                {
                    enemyLeft = false;
                    // showToolTip = false;
                    // attackToolTip = false;
                    //keyboardPickUpText.SetActive(false); //# Pickup text turns off
                    //keyboardAttackText.SetActive(false);
                    //controllerPickUpText.SetActive(false);
                    //controllerAttackText.SetActive(false);
                }
                #endregion
                #endregion

                # region Raycast Behide side
                # region Raycast for the Behide side view
                Ray interactRayBehide;
                // this ray shoots forward from the center of the camera (Behide)
                interactRayBehide = _behideCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                if (_debugErrors)
                {
                    Debug.DrawRay(interactRayBehide.origin, -transform.forward * attackRadiusDistance, Color.magenta); // Behide side
                }
                // create hit info (this holds the info for the stuff we interact with) 
                RaycastHit hitInfoBehide;

                if (Physics.Raycast(interactRayBehide, out hitInfoBehide, attackRadiusDistance, Layers /*This part here is the layer its optional*/ ))
                {
                    if (hitInfoBehide.transform.gameObject.layer == LayerMask.NameToLayer(attackLayer))
                    {
                        if (_debugErrors)
                        {
                            Debug.Log($"Player - Enemy is on behide.");
                        }

                        enemyFront = false;
                        enemyFrontRange = false;
                        enemyRight = false;
                        enemyLeft = false;
                        enemyBack = true;

                        // showToolTip = true;
                        // attackToolTip = false;
                        // OnGUI(); // Displays out ToolTip
                    }
                }
                else
                {
                    enemyBack = false;
                    // showToolTip = false;
                    // attackToolTip = false;
                    //keyboardPickUpText.SetActive(false); //# Pickup text turns off
                    //keyboardAttackText.SetActive(false);
                    //controllerPickUpText.SetActive(false);
                    //controllerAttackText.SetActive(false);
                }
                #endregion
                #endregion
            }
        }

        void DisplayBattleChoicePopup()
        {
            if (_forceControllerInput) // If the player forced controller Icons to appear
            {
                // keyboardIcon.SetActive(false);
                // controllerIcon.SetActive(true);
                BattleSystem.instance.ShowBattleChoice(true);
            }
            else
            {
                if (_checkControllerInput) // If the player is using/changed to a controller during the game
                {
                    // keyboardIcon.SetActive(false);
                    // controllerIcon.SetActive(true);
                    BattleSystem.instance.ShowBattleChoice(true);
                }
                else // If it isn't anything above then they must be using keyboard - Since xbox doesn't work
                {
                    // keyboardIcon.SetActive(true);
                    // controllerIcon.SetActive(false);
                    BattleSystem.instance.ShowBattleChoice(true);
                }
            }
        }




        // void OnGUI()
        // {
        //if (showToolTip)
        //{
        //    if (pickUpObj)
        //    {
        //        if (_forceControllerInput)
        //        {
        //            keyboardAttackText.SetActive(false);
        //            keyboardPickUpText.SetActive(false);
        //            if (!attackToolTip)
        //            {
        //                controllerAttackText.SetActive(false);
        //                controllerPickUpText.SetActive(true);
        //            }
        //            else
        //            {
        //                controllerPickUpText.SetActive(false);
        //                controllerAttackText.SetActive(true);
        //            }
        //        }
        //        else
        //        {
        //            if (_checkControllerInput)
        //            {
        //                if (!attackToolTip)
        //                {
        //                    keyboardAttackText.SetActive(false);
        //                    keyboardPickUpText.SetActive(false);

        //                    controllerAttackText.SetActive(false);
        //                    controllerPickUpText.SetActive(true);
        //                }
        //                else
        //                {
        //                    keyboardAttackText.SetActive(false);
        //                    keyboardPickUpText.SetActive(false);

        //                    controllerPickUpText.SetActive(false);
        //                    controllerAttackText.SetActive(true);
        //                }
        //            }
        //            else
        //            {
        //                if (!attackToolTip)
        //                {
        //                    controllerPickUpText.SetActive(false);
        //                    controllerAttackText.SetActive(false);

        //                    keyboardAttackText.SetActive(false);
        //                    keyboardPickUpText.SetActive(true); //# Pickup text turns on
        //                }
        //                else
        //                {
        //                    controllerPickUpText.SetActive(false);
        //                    controllerAttackText.SetActive(false);

        //                    keyboardPickUpText.SetActive(false);
        //                    keyboardAttackText.SetActive(true);
        //                }
        //            }
        //        }
        //    }
        //}
    }
    // }
}