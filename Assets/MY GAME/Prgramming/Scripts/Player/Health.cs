using GameDev;
using TurnBase;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public SavePlayerData _playerData;
    public GameObject playerObject;
    public Unit _playerUnit;

    public float maxHealth;
    public float currentHealth;

    [SerializeField] private Image _playHealthBar;
    [SerializeField] private Text _playHealthText;
    [SerializeField] private Image _battleHealthBar;
    [SerializeField] private Text _battleHealthText;

    private void Awake()
    {
        _playerData = GetComponent<SavePlayerData>();

        if (_playerData.maleREF)
        {
            playerObject = GameObject.Find("MalePlayer");
            _playerUnit = playerObject.GetComponent<Unit>();
        }
        else
        {
            playerObject = GameObject.Find("FemalePlayer");
            _playerUnit = playerObject.GetComponent<Unit>();
        }
    }

    public void UpdatePlayersHealth(bool value)
    {
        if (value)
        {
            _playerUnit.SetUpPlayerDataForBattle(); //? Have to put this here so that the health elements would get updated
        }
        maxHealth = _playerUnit.maxHealth;
        currentHealth = _playerUnit.currentHealth;

        _playHealthText.text = $"{currentHealth}/{maxHealth}";
        _playHealthBar.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);

        _battleHealthText.text = $"{currentHealth}/{maxHealth}";
        _battleHealthBar.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }
}
