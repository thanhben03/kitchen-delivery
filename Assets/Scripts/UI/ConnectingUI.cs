using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame += KitchenGameManager_OnTryingToJoinGame;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameManager_OnFailedToJoinGame;
        Hide();
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame -= KitchenGameManager_OnTryingToJoinGame;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameManager_OnFailedToJoinGame;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void KitchenGameManager_OnTryingToJoinGame()
    {
        Show();
    }

    private void KitchenGameManager_OnFailedToJoinGame(string message)
    {
        Hide();
    }
}
