using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    public AudioClip[] ads;
    public int index;
    public Animator aniL;
    public Animator aniR;

    public string emotionState = "EmotionState";
    public AudioSource ad;
    public bool playSwitch;

    // Update is called once per frame
    void Update()
    {
        SetAnim(aniL, aniR);
        SetAudio();
    }

    void SetAnim(Animator l, Animator r)
    {
        l.SetInteger(emotionState, index);
        r.SetInteger(emotionState, index);
    }

    void SetAudio()
    {
        if (playSwitch)
            return;
        
        ad.clip = ads[index];
        
        ad.Play();
        playSwitch = !playSwitch;
    }
}
