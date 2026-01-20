using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjTrashed += TrashCounter_OnAnyObjTrashed;
    }

    private void TrashCounter_OnAnyObjTrashed(TrashCounter obj)
    {
        Debug.Log("GET EVENT TRASHED");
        PlaySound(audioClipRefsSO.trash, obj.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(BaseCounter obj)
    {
        PlaySound(audioClipRefsSO.objectDrop, obj.transform.position);
    }

    private void Player_OnPickedSomething(Player player)
    {
        PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(CuttingCounter obj)
    {
        Debug.Log("GET EVENT CUTTING");
        PlaySound(audioClipRefsSO.chop, obj.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed()
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess()
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClip, Vector3 position, float volume = 1f)
    {
        GameObject soundGO = new GameObject("Sound");
        soundGO.transform.position = position;

        AudioSource audioSource = soundGO.AddComponent<AudioSource>();
        audioSource.clip = audioClip[Random.Range(0, audioClip.Length)];
        audioSource.volume = volume;

        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 5f;
        audioSource.maxDistance = 30f;

        audioSource.Play();
        Destroy(soundGO, audioSource.clip.length);
    }

    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
}
