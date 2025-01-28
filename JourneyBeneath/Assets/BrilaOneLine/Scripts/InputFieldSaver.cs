using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InputFieldSaver : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI startName;
    private const string SavedTextKey = "SavedInputText";

    public ShopController shopController;
    private void Start()
    {
        LoadText();
        shopController.LoadShopData();
    }

   
    public void OnTextChanged()
    {
        SaveText(inputField.text);
        LoadText();
    }

   
    private void SaveText(string text)
    {
        PlayerPrefs.SetString(SavedTextKey, text);
        PlayerPrefs.Save();
    }

   
    private void LoadText()
    {
        if (PlayerPrefs.HasKey(SavedTextKey))
        {
            string savedText = PlayerPrefs.GetString(SavedTextKey);
            inputField.text = $"{savedText}";
            startName.text = $"Hello {savedText}!";
        }
        else
        {
            inputField.text = "Enter a name";
            startName.text = $"Hello!";
        }
    }
}
