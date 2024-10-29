using GameDev;
using Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyType _enemyType;
    public GameObject _enemy;
    string _enemyName;
    Animator _enemyAnim;
    [SerializeField] private float _rotationDirection = 90f;
    [SerializeField] private float _bigRotationDirection = 180;
    [SerializeField] private GameObject _walkPosition;
    [SerializeField] private bool _turnedRight;
    [SerializeField] private bool _turnedLeft;
    [SerializeField] private bool _facingForward;
    [SerializeField] private bool _facingBackward;
    [SerializeField] private bool _wallInFront;
    [SerializeField] private bool _enemyInFront;
    // [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _target;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _stoppingDistance = 0.1f; // A small value for how close is considered "reached"
    public bool isMoving;

    private void Awake()
    {
        _enemyType = GetComponent<EnemyType>();
        _enemyName = _enemyType.enemy.enemyName;
        Debug.Log(_enemyName);
        _enemy = GameObject.Find(_enemyName);
    }

    private void Start()
    {
        _enemyAnim = _enemy.GetComponent<Animator>();
        _target = _walkPosition.transform.position;
    }

    void Update()
    {
        if (GameManager.instance.state == GameStates.EnemyTurn)
        {
            if (_enemyAnim.speed == 0)
            {
                _enemyAnim.speed = 1;
            }
            _wallInFront = GetComponent<EnemyInteract>().wallHit;
            _enemyInFront = GetComponent<EnemyInteract>().playerFront;

            if (isMoving)
            {
                // Move towards the _target
                _enemyAnim.SetBool("Walk", true);
                _enemyAnim.SetBool("Idle", false);
                transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
                // Check if the object has reached the _target
                if (Vector3.Distance(transform.position, _target) <= _stoppingDistance)
                {
                    // Snap the player to the target position so things stay even 
                    transform.position = _target;
                    // Object has reached the _target
                    Debug.Log("_target reached!");
                    _enemyAnim.SetBool("Idle", true);
                    _enemyAnim.SetBool("Walk", false);
                    isMoving = false;
                    StartCoroutine(CheckWalls());
                }
            }
        }
        else
        {
            _enemyAnim.speed = 0;
        }
    }

    IEnumerator CheckWalls()
    {
        yield return new WaitForSeconds(0.1f);
        if (!_wallInFront)
        {
            _target = _walkPosition.transform.position;
        }
        else
        {
            Debug.Log("I cannot move Object, a wall is in front of Player...");
        }
    }


    public void Move()
    {
        if (_enemy != null)
        {
            if (!_wallInFront)
            {
                if (!_enemyInFront)
                {
                    isMoving = true;
                }
                else
                {
                    Debug.Log("Enemy is in front... I can attack or run away...");
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
        if (_enemy != null)
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
        if (_enemy != null)
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
        if (_enemy != null)
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