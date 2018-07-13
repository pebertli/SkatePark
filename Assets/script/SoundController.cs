using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundController : MonoBehaviour, IPointerClickHandler {

    public Button sound;
    public Sprite soundOn;
    public Sprite soundOff;
    private float lastSoundVolume;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = lastSoundVolume;
            sound.image.sprite = soundOn;
        }
        else
        {
            lastSoundVolume = AudioListener.volume;
            AudioListener.volume = 0;
            sound.image.sprite = soundOff;
        }
    }
   
}
