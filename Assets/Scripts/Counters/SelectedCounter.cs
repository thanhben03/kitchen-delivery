using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounter : MonoBehaviour
{
    public BaseCounter clearCounter;
    public GameObject[] visualGameObjectArr;
    void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == clearCounter)
        {
            Debug.Log("SELECTED COUNTER LINE 18");
            Show();
        } else
        {
            Debug.Log("SELECTED COUNTER LINE 22");

            Hide();
        }
    }

    private void Show()
    {
        foreach(GameObject visualGameObject in visualGameObjectArr)
        {
            visualGameObject.SetActive(true);

        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArr)
        {
            visualGameObject.SetActive(false);

        }
    }
}
