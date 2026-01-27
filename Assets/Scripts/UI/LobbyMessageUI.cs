using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;


    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameManager_OnFailedToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_OnJoinStarted;
        KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_OnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed += KitchenGameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameManager_OnFailedToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobbyStarted -= KitchenGameLobby_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed -= KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted -= KitchenGameLobby_OnJoinStarted;
        KitchenGameLobby.Instance.OnJoinFailed -= KitchenGameLobby_OnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed -= KitchenGameLobby_OnQuickJoinFailed;

    }

    private void KitchenGameLobby_OnQuickJoinFailed()
    {
        ShowMessage("Could not find a lobby to Quick Join !");
    }

    private void KitchenGameLobby_OnJoinFailed()
    {
        ShowMessage("Failed to join Lobby !");
    }

    private void KitchenGameLobby_OnJoinStarted()
    {
        ShowMessage("Joining Lobby...");
    }

    private void KitchenGameLobby_OnCreateLobbyFailed()
    {
        ShowMessage("Failed to create Lobby !");
    }

    private void KitchenGameLobby_OnCreateLobbyStarted()
    {
        ShowMessage("Creating Lobby....");
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;

    }

    

    private void KitchenGameManager_OnFailedToJoinGame(string message)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect !");
        } else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
