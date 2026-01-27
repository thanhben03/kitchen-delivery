using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectSingleUI : MonoBehaviour
{
    [SerializeField] private int colorId;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectedGameObject;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.Instance.ChangePlayerColor(colorId);
        });
    }
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;

        Color c = KitchenGameMultiplayer.Instance.GetPlayerColor(colorId);
        c.a = 1f;

        image.color = c;

        UpdateIsSelected();
    }
    private void KitchenGameMultiplayer_OnPlayerDataNetworkListChanged()
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if (KitchenGameMultiplayer.Instance.GetPlayerData().colorId == colorId)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenGameMultiplayer_OnPlayerDataNetworkListChanged;

    }
}
