using UnityEngine;
using TMPro;

public class CoinsController : MonoBehaviour
{
    public static CoinsController Instance;
    public int coins;
    public delegate void CurrencyChanged();
    public event CurrencyChanged OnCoinsChanged;
    private const string CoinsKey = "Coins";
    [SerializeField] private TextMeshProUGUI coinsText;
   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AddCoins(1000);
        LoadCurrencyData();
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveCurrencyData();
        OnCoinsChanged?.Invoke();
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            SaveCurrencyData();
            OnCoinsChanged?.Invoke();
            UpdateUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough coins");
            return false;
        }
    }
    private void SaveCurrencyData()
    {
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
    }

    private void LoadCurrencyData()
    {
        coins = PlayerPrefs.GetInt(CoinsKey, 0);
    }

    public int GetCoins()
    {
        return coins;
    }

    private void UpdateUI()
    {
        coinsText.text = coins.ToString();
    }
}
