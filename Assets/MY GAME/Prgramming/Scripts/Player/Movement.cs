using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class Movement : MonoBehaviour
{
   #region Newer version rotate the player with buttons

    GameObject PlayerCharacter;
    Vector3 currentPOS;
    public float rotationDirection = 90f;
    public float bigRotationDirection = 180;
    public float moveDistance = 3f;
    [SerializeField] private bool _turnedRight;
    [SerializeField] private bool _turnedLeft;
    [SerializeField] private bool _facingForward;
    [SerializeField] private bool _facingBackward;
    [SerializeField] private bool _wallInFront;
    [SerializeField] private bool _enemyInFront;

    private void Start()
    {
        PlayerCharacter = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        _wallInFront = GetComponent<Interact>().wallHit;
        _enemyInFront = GetComponent<Interact>().enemyHit;
    }

    public void Move()
    {
        if (!_wallInFront || !_enemyInFront)
        {
            if (_turnedRight)
            {
                transform.position += new Vector3(moveDistance, 0f, 0f);
            }
            else if (_turnedLeft)
            {
                transform.position += new Vector3(-moveDistance, 0f, 0f);
            }
            else if (_facingForward)
            {
                transform.position += new Vector3(0f, 0f, moveDistance);
            }
            else if (_facingBackward)
            {
                transform.position += new Vector3(0f, 0f, -moveDistance);
            }
        }
        else if (_wallInFront)
        {
            Debug.Log("I cannot move, a wall is in front of me...");
        }
        else if (_enemyInFront)
        {
            Debug.Log("Enemy is in front... I can attack or run away...");
        }

    }

    public void LeftTurn()
    {
        transform.Rotate(0, -rotationDirection, 0);
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
        transform.Rotate(0, rotationDirection, 0);
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
        transform.Rotate(0, bigRotationDirection, 0);
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


    #endregion
}
