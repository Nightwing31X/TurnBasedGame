using Player;
using UnityEngine;

public class CheckMovement : MonoBehaviour
{
    [SerializeField] bool WalkPosition;
    [SerializeField] bool BackPosition;
    [SerializeField] GameObject playerObject;

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
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "WalkPosition")
        {
            WalkPosition = true;
            playerObject.GetComponent<Movement>()._wallInFront = WalkPosition;
        }

        if (other.tag == "BackPosition")
        {
            BackPosition = true;
            playerObject.GetComponent<Movement>()._wallInBehide = BackPosition;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "WalkPosition")
        {
            WalkPosition = false;
            playerObject.GetComponent<Movement>()._wallInFront = WalkPosition;
        }

        if (other.tag == "BackPosition")
        {
            BackPosition = false;
            playerObject.GetComponent<Movement>()._wallInBehide = BackPosition;
        }
    }
}
