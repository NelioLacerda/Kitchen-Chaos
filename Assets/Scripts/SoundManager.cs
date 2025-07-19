using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    
    public static SoundManager Instance { get; private set; }
    
    [SerializeField] private AudiClipRefsSO audiClipRefsSO;
    
    private float volume = 1f;
    
    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeDeliveredSuccess += InstanceOnOnRecipeDeliveredSuccess;
        DeliveryManager.Instance.OnRecipeDeliveredFailed += InstanceOnOnRecipeDeliveredFailed;
        CuttingCounter.OnAnyCut += CuttingCounterOnOnAnyCut;
        Player.Instance.OnPickedSomething += InstanceOnOnPickedSomething;
        BaseCounter.OnAnyObjectPlaceHere += BaseCounterOnOnAnyObjectPlaceHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounterOnOnAnyObjectTrashed;
    }

    private void TrashCounterOnOnAnyObjectTrashed(object sender, EventArgs e)
    {
        var trashCounter = sender as TrashCounter;
        PlaySound(audiClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounterOnOnAnyObjectPlaceHere(object sender, EventArgs e)
    {
        var anyCounter = sender as BaseCounter;
        PlaySound(audiClipRefsSO.objectDrop, anyCounter.transform.position);
    }

    private void InstanceOnOnPickedSomething(object sender, EventArgs e)
    {
        PlaySound(audiClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounterOnOnAnyCut(object sender, EventArgs e)
    {
        var cuttingCounter = sender as CuttingCounter;
        PlaySound(audiClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void InstanceOnOnRecipeDeliveredFailed(object sender, EventArgs e)
    {
        PlaySound(audiClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void InstanceOnOnRecipeDeliveredSuccess(object sender, EventArgs e)
    {
        PlaySound(audiClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }
    
    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, volumeMultiplier);
    }
    
    public void PlayFootstepSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audiClipRefsSO.footstep, position, volume);
    }
    
    public void PlayCountdownSound()
    {
        PlaySound(audiClipRefsSO.warning, Vector3.zero);
    }
    
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audiClipRefsSO.warning, position);
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;   
    }
}