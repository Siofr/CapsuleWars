using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerNameInputUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button continueButton;

    public static string DisplayName;
    private const string PlayerPrefsNameKey = "PlayerName";

    [SerializeField] private UnityEvent OnNameSaved;

    void Start()
    {
        StartInputField();
        SetupContinueButton();
    }

    void SetupContinueButton()
    {
        if (continueButton == null) return;

        continueButton.onClick.AddListener(SavePlayerName);
    }

    void StartInputField()
    {
        if (inputField == null || !PlayerPrefs.HasKey(PlayerPrefsNameKey)) return;

        inputField.text = PlayerPrefs.GetString(PlayerPrefsNameKey);

        SetPlayerName(inputField.text);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        DisplayName = inputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
        OnNameSaved?.Invoke();
    }
}
