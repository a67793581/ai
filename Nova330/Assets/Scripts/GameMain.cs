using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMain : MonoBehaviour
{
    public EventTrigger eventTrigger;
    public Transform emoji;
    public AudioSource AudioSource => emoji.GetComponent<AudioSource>();
    public List<AudioClip> audioClips;
    private Animator[] _emojiAnimators;
    private static readonly int Emotion = Animator.StringToHash("EmotionState");
    private GUILayoutOption[] _options = { GUILayout.Width(200), GUILayout.Height(100)};
    [SerializeField]
    private Dictionary<int, string> _dictionary;
    private void Refresh()
    {
        _options = new [] { GUILayout.Width(400), GUILayout.Height(100)};
        _dictionary = new Dictionary<int, string>
        {
            {0,"idle"},
            {1,"happy"},
            {2,"love"},
            {3,"faint"},
            {4,"sae"},
            {5,"angry"},
            {6,"sleepy"},
        };
    }

    private void Start()
    {
        Refresh();
        _emojiAnimators = emoji.GetComponentsInChildren<Animator>();
        
        GetComponent<ShakeDetector>().unityEvent.AddListener(() =>
        {
            // Debug.LogError("震动检测");
            this.ChangeToAnima(3);
        });
        foreach (EventTriggerType item in Enum.GetValues(typeof(EventTriggerType)))
        {
            var triggerEvent = new EventTrigger.TriggerEvent();
            triggerEvent.AddListener((eventData) =>
            {
                switch (item)
                {
                    case EventTriggerType.PointerEnter:
                    case EventTriggerType.PointerExit:
                    case EventTriggerType.PointerDown:
                    case EventTriggerType.PointerUp:
                        break;
                    case EventTriggerType.PointerClick:
                        Debug.LogError("====>" + item);
                        break;
                    case EventTriggerType.Drag:
                        Debug.LogError("====>" + item);
                        this.ChangeToAnima(1);
                        break;
                    case EventTriggerType.Drop:
                    case EventTriggerType.Scroll:
                    case EventTriggerType.UpdateSelected:
                    case EventTriggerType.Select:
                    case EventTriggerType.Deselect:
                    case EventTriggerType.Move:
                    case EventTriggerType.InitializePotentialDrag:
                    case EventTriggerType.BeginDrag:
                    case EventTriggerType.EndDrag:
                    case EventTriggerType.Submit:
                    case EventTriggerType.Cancel:
                        break;
                }
            });
            var entry = new EventTrigger.Entry
            {
                eventID = item,
                callback = triggerEvent
            };
            eventTrigger.triggers.Add(entry);
        }
    }

    // private void Update()
    // {
    //     switch (Screen.orientation)
    //     {
    //         case ScreenOrientation.LandscapeLeft:
    //             Camera.main.transform.localPosition = Vector3.forward * -1;
    //             break;
    //         case ScreenOrientation.LandscapeRight:
    //             Camera.main.transform.localPosition = Vector3.forward * -1;
    //             break;
    //         default:
    //             Camera.main.transform.localPosition = Vector3.forward * -3;
    //             break;
    //         // 添加其他方向的处理逻辑
    //     }
    // }
    
    private void ChangeToAnima(int index)
    {
        foreach (var animator in _emojiAnimators)
        {
            animator.SetInteger(Emotion, index);
        }
        AudioSource.clip = audioClips.Find((clip) => clip.name == _dictionary[index]);
        AudioSource.Play();
    }

    private void OnGUI()
    {
        GUI.skin.button.fontSize = 30;
        if (GUILayout.Button("刷新", _options))
        {
            Refresh();
        }
        if (GUILayout.Button("待机", _options))
        {
            this.ChangeToAnima(0);
        }
        if (GUILayout.Button("抚摸\n(开心)", _options))
        {
            this.ChangeToAnima(1);
        }
        if (GUILayout.Button("点击\n(摇晃)", _options))
        {
            this.ChangeToAnima(3);
        }
        if (GUILayout.Button("陀螺仪检测\n(生气)", _options))
        {
            this.ChangeToAnima(5);
        }
        if (GUILayout.Button("喜欢\n(爱心)", _options))
        {
            this.ChangeToAnima(2);
        }
        if (GUILayout.Button("观察主人\n(为什么伤心啦)", _options))
        {
            this.ChangeToAnima(6);
        }
        if (GUILayout.Button("睡觉", _options))
        {
            this.ChangeToAnima(4);
        }
    }
}
