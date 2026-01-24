using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup";

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failedColor = Color.red;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;

    private Animator animator;
    private Coroutine hideCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed()
    {
        backgroundImage.color = Color.red;

        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);

        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY\nFAILED";

        RestartAutoHide();
    }

    private void DeliveryManager_OnRecipeSuccess()
    {
        backgroundImage.color = Color.green;

        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);

        iconImage.sprite = successSprite;
        messageText.text = "DELIVERY\nSUCCESS";

        RestartAutoHide();
    }

    private void RestartAutoHide()
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(AutoHide(2f));
    }

    private IEnumerator AutoHide(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
