using GameDev;
using Interactions;
using Player;
using System.Collections;
using System.Collections.Generic;
using TurnBase;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Player
{
    [AddComponentMenu("GameDev/Player/First Person Movement")]
    public class Movement : MonoBehaviour
    {
        GameObject _player;
        Animator _playerAnim;
        [SerializeField] private float _rotationDirection = 90f;
        [SerializeField] private float _bigRotationDirection = 180;
        [SerializeField] private GameObject _walkPosition;
        [SerializeField] private GameObject _behidePosition;
        [SerializeField] private bool _turnedRight;
        [SerializeField] private bool _turnedLeft;
        [SerializeField] private bool _facingForward;
        [SerializeField] private bool _facingBackward;
        [SerializeField] private bool _wallInFront;
        [SerializeField] private bool _enemyInFront;
        [SerializeField] private bool _enemyInFrontRange;
        // [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _targetForward;
        [SerializeField] private Vector3 _targetBehide;
        [SerializeField] private float _speed = 1;
        [SerializeField] private float _stoppingDistance = 0.1f; // A small value for how close is considered "reached"

        public bool isMoving;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _playerAnim = _player.GetComponent<Animator>();
            _targetForward = _walkPosition.transform.position;
            _targetBehide = _behidePosition.transform.position;
        }

        void Update()
        {
            //if (BattleSystem.instance.battleState == BattleStates.NotInBattle || BattleSystem.instance.battleState == BattleStates.BattleChoice)
            if (GameManager.instance.state != GameStates.Pause)
                {
                if (_playerAnim.speed == 0)
                {
                    _playerAnim.speed = 1;
                }
                _wallInFront = GetComponent<Interact>().wallHit;
                _enemyInFront = GetComponent<Interact>().enemyFront;
                _enemyInFrontRange = GetComponent<Interact>().enemyFrontRange;


                if (isMoving)
                {
                    if (BattleSystem.instance.battleState == BattleStates.NotInBattle || BattleSystem.instance.battleState == BattleStates.BattleChoice)
                    {
                        // Move towards the _target
                        _playerAnim.SetBool("Walk", true);
                        _playerAnim.SetBool("Idle", false);
                        transform.position = Vector3.MoveTowards(transform.position, _targetForward, _speed * Time.deltaTime);
                        // Check if the object has reached the _target
                        if (Vector3.Distance(transform.position, _targetForward) <= _stoppingDistance)
                        {
                            // Snap the player to the target position so things stay even 
                            transform.position = _targetForward;
                            // Object has reached the _target
                            Debug.Log("target reached!");

                            _playerAnim.SetBool("Idle", true);
                            _playerAnim.SetBool("Walk", false);

                            isMoving = false;
                        
                            StartCoroutine(CheckWalls());
                        }
                    }
                    else
                    {
                        if (BattleSystem.instance.battleState == BattleStates.PlayerTurn)
                        {
                            BattleMove();
                        }
                    }
                }
            }
            else
            {
                _playerAnim.speed = 0;
            }
        }

        IEnumerator CheckWalls()
        {
            yield return new WaitForSeconds(0.1f);
            if (!_wallInFront)
            {
                _targetForward = _walkPosition.transform.position;
                _targetBehide = _behidePosition.transform.position;
            }
            else
            {
                Debug.Log("I cannot move Object, a wall is in front of Player...");
            }
        }

        public void BattleMove()
        {
            //isMoving = true;
            if (_enemyInFront)
            {
                //? Then need to move the player in that direction - call the movement script to do that
                Debug.Log("Player can only run backwards into Range attacks...");
                // Then run the backwardPOS
                // Move towards the _target
                _playerAnim.SetBool("Walk", true);
                _playerAnim.SetBool("Idle", false);
                transform.position = Vector3.MoveTowards(transform.position, _targetBehide, _speed * Time.deltaTime);
                // Check if the object has reached the _target
                if (Vector3.Distance(transform.position, _targetBehide) <= _stoppingDistance)
                {
                    // Snap the player to the target position so things stay even 
                    transform.position = _targetBehide;
                    // Object has reached the _target
                    Debug.Log("target reached!");

                    _playerAnim.SetBool("Idle", true);
                    _playerAnim.SetBool("Walk", false);

                    isMoving = false;

                    StartCoroutine(CheckWalls());
                }
            }
            else
            {
                //? Then need to move the player in that direction - call the movement script to do that
                Debug.Log("Player can only run forwards into Melee attacks...");

                // Then run the forwardPOS
                // Move towards the _target
                _playerAnim.SetBool("Walk", true);
                _playerAnim.SetBool("Idle", false);
                transform.position = Vector3.MoveTowards(transform.position, _targetForward, _speed * Time.deltaTime);
                // Check if the object has reached the _target
                if (Vector3.Distance(transform.position, _targetForward) <= _stoppingDistance)
                {
                    // Snap the player to the target position so things stay even 
                    transform.position = _targetForward;
                    // Object has reached the _target
                    Debug.Log("target reached!");

                    _playerAnim.SetBool("Idle", true);
                    _playerAnim.SetBool("Walk", false);

                    isMoving = false;

                    StartCoroutine(CheckWalls());
                }
            }
        }

        public void Move()
        {
            if (_player != null)
            {
                if (!_wallInFront)
                {
                    if (!_enemyInFront)
                    {
                        isMoving = true;
                    }
                    else
                    {
                        if (_enemyInFrontRange)
                        {
                            isMoving = true;
                        }
                        else
                        {
                            Debug.Log("Enemy is in right in-front... I cannot move...");
                        }
                    }
                }
                else
                {
                    Debug.Log("I cannot move, a wall is in front of me...");
                }
            }
        }

        public void LeftTurn()
        {
            transform.Rotate(0, -_rotationDirection, 0);
            if (_player != null)
            {
                StartCoroutine(CheckWalls());
            }
            if (_turnedRight)
            {
                _facingForward = true;
                _turnedRight = false;
            }
            else if (_facingForward)
            {
                _turnedLeft = true;
                _facingForward = false;
                _turnedRight = false;
            }
            else if (_turnedLeft)
            {
                _facingBackward = true;
                _turnedLeft = false;
            }
            else if (_facingBackward)
            {
                _turnedRight = true;
                _facingBackward = false;
            }
        }

        public void RightTurn()
        {
            transform.Rotate(0, _rotationDirection, 0);
            if (_player != null)
            {
                StartCoroutine(CheckWalls());
            }
            if (_turnedLeft)
            {
                _facingForward = true;
                _turnedLeft = false;
            }
            else if (_facingForward)
            {
                _turnedRight = true;
                _facingForward = false;
                _turnedLeft = false;
            }
            else if (_turnedRight)
            {
                _facingBackward = true;
                _turnedRight = false;
            }
            else if (_facingBackward)
            {
                _turnedLeft = true;
                _facingBackward = false;
            }
        }

        public void BigTurn()
        {
            transform.Rotate(0, _bigRotationDirection, 0);
            if (_player != null)
            {
                StartCoroutine(CheckWalls());
            }
            if (_facingForward)
            {
                _facingBackward = true;
                _facingForward = false;
            }
            else if (_turnedRight)
            {
                _turnedLeft = true;
                _turnedRight = false;
            }
            else if (_turnedLeft)
            {
                _turnedRight = true;
                _turnedLeft = false;
            }
            else if (_facingBackward)
            {
                _facingForward = true;
                _facingBackward = false;
            }
        }
    }
}
