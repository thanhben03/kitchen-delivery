using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    [SerializeField] Customer customer;

    [Header("UI References")]
    public Image timerFillImage;

    [Header("Settings")]
    public float minTime = 60f;
    public float maxTime = 120f;

    private float currentTime;
    private float maxCountdownTime;      

    private bool isCounting = false;
    void Start()
    {
        StartNewCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCounting) return;

        currentTime -= Time.deltaTime;

        if (maxCountdownTime > 0)
        {
            timerFillImage.fillAmount = currentTime / maxCountdownTime;
        }

        if (currentTime <= 0)
        {
            timerFillImage.fillAmount = 0;
            isCounting = false;
            Debug.Log("Out of time");
            customer.OutOfTimeDelivery();
            // StartNewCountdown();
        }
    }

    public void StartNewCountdown()
    {
        maxCountdownTime = Random.Range(minTime, maxTime);
        currentTime = maxCountdownTime;

        if (timerFillImage != null)
        {
            timerFillImage.fillAmount = 1f;
        }

        isCounting = true;
    }
}
