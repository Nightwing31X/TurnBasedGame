using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Old version - Could move the player though I had to set the location
    //public GameObject _player;

    //public Transform currentPostion;
    //public Transform nextPostion;

    //public float moveSpeed;
    //private Vector3 current;
    //private Vector3 target;
    //private float sinTime;

    //void Start()
    //{
    //    current = currentPostion.transform.position;
    //    target = nextPostion.transform.position;
    //    transform.position = current;
    //    _player = GameObject.FindWithTag("Player");
    //}

    //void Update()
    //{
    //    if (transform.position != target)
    //    {
    //        sinTime += Time.deltaTime * moveSpeed;
    //        sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
    //        float t = Evaluate(sinTime);
    //        transform.position = Vector3.Lerp(current, target, t);
    //    }

    //    Swap();
    //}

    //public void Swap()
    //{
    //    if (current.z != target.y)
    //    {
    //        return;
    //    }

    //    Vector3 t = current;
    //    current.z = target.y;
    //    target = t;
    //    sinTime = 0;
    //}


    //public float Evaluate(float x)
    //{
    //    return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) * 05f;
    //}
    #endregion

    #region Newer version rotate the player with buttons

    GameObject PlayerCharacter;
    Vector3 currentPOS;
    public float rotationDirection = 90f;
    public float moveDistance = 3f;
    [SerializeField] private bool _turnedRight;
    [SerializeField] private bool _turnedLeft;
    [SerializeField] private bool _facingForward;
    [SerializeField] private bool _facingBackward;

    private void Start()
    {
        PlayerCharacter = GameObject.FindWithTag("Player");
    }

    public void Move()
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



    #endregion
}
