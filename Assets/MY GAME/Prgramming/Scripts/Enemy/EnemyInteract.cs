using GameDev;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyInteract : MonoBehaviour
{
    //public GUIStyle crossHair, tooltip;
    public LayerMask Layers;
    public string playerLayer;
    public string wallLayer;
    [Tooltip("Toggle on to print console messages from this component.")]
    [SerializeField] private bool _debug;
    [SerializeField] private bool _hasRan;
    [Tooltip("The distance that the player can reach interactions."), SerializeField, Range(0, 100)] private float distance = 2f;
    [SerializeField, Range(0, 100)] private float attackRadiusDistance = 6f;

    public bool wallHit = false;
    public bool playerFront = false;
    public bool playerRight = false;
    public bool enemyLeft = false;
    public bool enemyBack = false;

    #region Reference to the player cameras - for the raycast 
    [Header("Cameras for Raycast")]
    [SerializeField] private Transform _frontCameraEnemy;
    [SerializeField] private Transform _rightCameraEnemy;
    [SerializeField] private Transform _leftCameraEnemy;
    [SerializeField] private Transform _behideCameraEnemy;
    #endregion


    void Start()
    {
        _frontCameraEnemy = transform.Find("FrontCameraEnemy");
        _rightCameraEnemy = transform.Find("RightCameraEnemy");
        _leftCameraEnemy = transform.Find("LeftCameraEnemy");
        _behideCameraEnemy = transform.Find("BehideCameraEnemy");
    }

    private void Update()
    {
        //_forceControllerInput = InputHandler.instance.forceController;
        //_checkControllerInput = InputHandler.instance.onController;
        if (GameManager.instance.state == GameStates.Pause)
        {
            this.GetComponent<Animator>().enabled = false;
        }
        else
        {
            this.GetComponent<Animator>().enabled = true;
        }


        #region Raycast for the Forward view
        // create a ray (a Ray is ?? a beam, line that comes into contact with colliders)
        Ray interactRayForward;
        // this ray shoots forward from the center of the camera
        interactRayForward = _frontCameraEnemy.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (_debug)
        {
            Debug.DrawRay(interactRayForward.origin, transform.forward * distance, Color.green); // Forward side
            // Debug.DrawRay(interactRayForward.origin, transform.right * distance, Color.green); // Right side
            // Debug.DrawRay(interactRayForward.origin, -transform.forward * distance, Color.green); // -transform.forward = Backward side
            // Debug.DrawRay(interactRayForward.origin, -transform.right * distance, Color.green); //-transform.right = Left side
        }
        // create hit info (this holds the info for the stuff we interact with) 
        RaycastHit hitInfoForward;
        // if this physics ray that gets cast in a direction hits a object within our distance and or layer
        if (Physics.Raycast(interactRayForward, out hitInfoForward, distance, Layers /*This part here is the layer its optional*/ ))
        {
            if (_debug)
            {
                Debug.DrawRay(transform.position, transform.forward * distance, Color.yellow, 0.5f);
            }
            //# region Detect the interact layer (Interaction Layer)
            //if (hitInfoForward.transform.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            //{
            //    if (_debug)
            //    {
            //        if (!_hasRan)
            //        {
            //            Debug.Log($"Hit Layer = {playerLayer}");
            //            _hasRan = true;
            //        }
            //    }
            //    //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
            //    //OnGUI(); // Displays out ToolTip
            //    if (Input.GetButtonDown("Interaction"))
            //    {
            //        if (hitInfoForward.collider.TryGetComponent(out IInteractable interactableObject))
            //        {
            //            interactableObject.Interact();
            //        }
            //    }
            //}
            //# endregion
            # region Detect the attack layer (Player Layer) - Forward Enemy view
            if (hitInfoForward.transform.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            {
                if (_debug)
                {
                    if (!_hasRan)
                    {
                        Debug.Log($"Enemy - Player is in range to fight!");
                        _hasRan = true;
                    }
                }
                playerFront = true;
                wallHit = false;
                //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
                //OnGUI(); // Displays out ToolTip
            }
            # endregion
            # region Detect the wall layer (Wall Layer)
            if (hitInfoForward.transform.gameObject.layer == LayerMask.NameToLayer(wallLayer))
            {
                if (_debug)
                {
                    if (!_hasRan)
                    {
                        Debug.Log($"Enemy - Wall is infront.");
                        _hasRan = true;
                    }
                }
                wallHit = true;
                playerFront = false;
                //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
                //OnGUI(); // Displays out ToolTip
            }
            # endregion
        }
        else
        {
            wallHit = false;
            playerFront = false;
        }
        #endregion


        # region Raycast for the Front side view)
        if (_debug)
        {
            Debug.DrawRay(interactRayForward.origin, transform.forward * attackRadiusDistance, Color.red); // Right side
        }

        if (Physics.Raycast(interactRayForward, out hitInfoForward, attackRadiusDistance, Layers /*This part here is the layer its optional*/ ))
        {
            if (hitInfoForward.transform.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            {
                if (_debug)
                {
                    if (!_hasRan)
                    {
                        Debug.Log($"Enemy - Player is infront; can do range attacks");
                        _hasRan = true;
                    }
                }
                playerFront = true;
                playerRight = false;
                enemyLeft = false;
                enemyBack = false;
                wallHit = false;
                //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
                //OnGUI(); // Displays out ToolTip
            }
        }
        else
        {
            playerFront = false;
            //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
            //OnGUI(); // Displays out ToolTip
        }
        #endregion
        # region Raycast for the Right side view
        // this ray shoots forward from the center of the camera (Right)
        Ray interactRayRight;
        // this ray shoots forward from the center of the camera (Right)
        interactRayRight = _rightCameraEnemy.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (_debug)
        {
            // Debug.DrawRay(interactRayRight.origin, transform.forward * distance, Color.green); // Forward side
            Debug.DrawRay(interactRayRight.origin, transform.right * attackRadiusDistance, Color.red); // Right side
            // Debug.DrawRay(interactRayRight.origin, -transform.forward * distance, Color.green); // -transform.forward = Backward side
            // Debug.DrawRay(interactRayRight.origin, -transform.right * distance, Color.green); //-transform.right = Left side
        }
        // create hit info (this holds the info for the stuff we interact with) 
        RaycastHit hitInfoRight;

        if (Physics.Raycast(interactRayRight, out hitInfoRight, attackRadiusDistance, Layers /*This part here is the layer its optional*/ ))
        {
            if (hitInfoRight.transform.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            {
                if (_debug)
                {
                    if (!_hasRan)
                    {
                        Debug.Log($"Enemy - Player is on right side");
                        _hasRan = true;
                    }
                }
                playerFront = false;
                playerRight = true;
                enemyLeft = false;
                enemyBack = false;
                //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
                //OnGUI(); // Displays out ToolTip
            }
        }
        else
        {
            playerRight = false;
            //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
            //OnGUI(); // Displays out ToolTip
        }
        #endregion
        # region Raycast for the Left side view
        Ray interactRayLeft;
        // this ray shoots forward from the center of the camera (Right)
        interactRayLeft = _leftCameraEnemy.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (_debug)
        {
            // Debug.DrawRay(interactRayRight.origin, transform.forward * distance, Color.green); // Forward side
            Debug.DrawRay(interactRayLeft.origin, -transform.right * attackRadiusDistance, Color.red); // Right side
            // Debug.DrawRay(interactRayRight.origin, -transform.forward * distance, Color.green); // -transform.forward = Backward side
            // Debug.DrawRay(interactRayRight.origin, -transform.right * distance, Color.green); //-transform.right = Left side
        }
        // create hit info (this holds the info for the stuff we interact with) 
        RaycastHit hitInfoLeft;

        if (Physics.Raycast(interactRayLeft, out hitInfoLeft, attackRadiusDistance, Layers /*This part here is the layer its optional*/ ))
        {
            if (hitInfoLeft.transform.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            {
                if (_debug)
                {
                    if (!_hasRan)
                    {
                        Debug.Log($"Enemy - Player is on left side");
                        _hasRan = true;
                    }
                }
                playerFront = false;
                playerRight = false;
                enemyLeft = true;
                enemyBack = false;
                //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
                //OnGUI(); // Displays out ToolTip
            }
        }
        else
        {
            enemyLeft = false;
            //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
            //OnGUI(); // Displays out ToolTip
        }
        #endregion
        # region Raycast for the Behide side view
        Ray interactRayBehide;
        // this ray shoots forward from the center of the camera (Right)
        interactRayBehide = _behideCameraEnemy.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (_debug)
        {
            // Debug.DrawRay(interactRayRight.origin, transform.forward * distance, Color.green); // Forward side
            Debug.DrawRay(interactRayBehide.origin, -transform.forward * attackRadiusDistance, Color.red); // Right side
            // Debug.DrawRay(interactRayRight.origin, -transform.forward * distance, Color.green); // -transform.forward = Backward side
            // Debug.DrawRay(interactRayRight.origin, -transform.right * distance, Color.green); //-transform.right = Left side
        }
        // create hit info (this holds the info for the stuff we interact with) 
        RaycastHit hitInfoBehide;

        if (Physics.Raycast(interactRayBehide, out hitInfoBehide, attackRadiusDistance, Layers /*This part here is the layer its optional*/ ))
        {
            if (hitInfoBehide.transform.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            {
                if (_debug)
                {
                    if (!_hasRan)
                    {
                        Debug.Log($"Enemy - Player is on behide.");
                        _hasRan = true;
                    }
                }

                playerFront = false;
                playerRight = false;
                enemyLeft = false;
                enemyBack = true;
                //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
                //OnGUI(); // Displays out ToolTip
            }
        }
        else
        {
            enemyBack = false;
            //! PUT A FUNCTION WHICH WILL THEN CHECK IF IT SHOULD ATTACK OR MOVE, ETC.
            //OnGUI(); // Displays out ToolTip
        }
        #endregion


    }
    //void OnGUI()
    //{
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
    //}
}