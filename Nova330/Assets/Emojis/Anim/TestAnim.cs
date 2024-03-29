using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    public int index;
    public Animator aniL;
    public Animator aniR;

    public string emotionState = "EmotionState";


    // Update is called once per frame
    void Update()
    {
        SetAnim(aniL, aniR);
    }

    void SetAnim(Animator l, Animator r)
    {
        l.SetInteger(emotionState, index);
        r.SetInteger(emotionState, index);
    }
}
