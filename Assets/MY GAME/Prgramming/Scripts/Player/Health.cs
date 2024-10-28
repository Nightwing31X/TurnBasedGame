using GameDev;
using System.Collections;
using System.Collections.Generic;
using TurnBase;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public SavePlayerData _playerData;

    public int maxHealth;
    public int currentHealth;
    

    [SerializeField] private Text PlayHealthContainer;
    [SerializeField] private Text AttackHealthContainer;

    private void Awake()
    {
        _playerData = GetComponent<SavePlayerData>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayersHealth()
    {


        maxHealth = _playerData.maxHealthREF;
        currentHealth = _playerData.currentHealthREF;

        if (GameManager.instance.state == GameStates.Menu)
        { 
            PlayHealthContainer.text = $"{currentHealth.ToString()}/{maxHealth.ToString()}";
            Debug.Log("Hello");
        }
    }
}
