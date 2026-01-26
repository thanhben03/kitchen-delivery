using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour
{
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameGameButton;

    private void Awake()
    {
        createGameButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
        });

        joinGameGameButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.Instance.StartClient();
        });
    }
}
