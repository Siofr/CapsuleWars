using TMPro;
using Unity.Services.Multiplayer;
using UnityEngine;

public class SessionInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text sessionNameText;
    [SerializeField] private TMP_Text passwordProtectedText;
    [SerializeField] private TMP_Text playerCountText;

    public void FillSessionDetails(ISession sessionInfo)
    {
        string sessionName = sessionInfo.Name;
        bool passwordProtected = sessionInfo.HasPassword;
        int maxPlayerCount = sessionInfo.MaxPlayers;
        int currentPlayerCount = maxPlayerCount - sessionInfo.AvailableSlots;

        sessionNameText.text += sessionName;
        passwordProtectedText.text += passwordProtected ? "True" : "False";
        playerCountText.text += $"{currentPlayerCount} / {maxPlayerCount}";
    }
}
