using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SessionNameInputUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button continueButton;

    public string SessionName;

    [SerializeField] private UnityEvent OnNameSaved;

    void Start()
    {
        StartInputField();
        SetupContinueButton();
    }

    void SetupContinueButton()
    {
        if (continueButton == null) return;

        continueButton.onClick.AddListener(ConnectToHost);
    }

    void StartInputField()
    {
        if (inputField == null ) return;

        SetSessionName(inputField.text);
    }

    public void SetSessionName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void ConnectToHost()
    {
        SessionName = inputField.text;

        if (SessionName == null) return;

        // Try Connect to the Game
    }
}
