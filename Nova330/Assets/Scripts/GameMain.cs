using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using OpenCvSharp.Demo;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    public AudioClip applau;
    public EventTrigger eventTrigger;
    public Transform emoji;
    public RawImage Clover;
    public AudioSource AudioSource => emoji.GetComponent<AudioSource>();
    public List<AudioClip> audioClips;
    private Animator[] _emojiAnimators;
    private static readonly int Emotion = Animator.StringToHash("EmotionState");
    private GUILayoutOption[] _options = { GUILayout.Width(200), GUILayout.Height(100)};
    [SerializeField]
    private Dictionary<int, string> dictionary;
    private void Refresh()
    {
        // _options = new [] { GUILayout.Width(350), GUILayout.Height(80)};
        _options = new [] { GUILayout.Width(0), GUILayout.Height(80)};
        dictionary = new Dictionary<int, string>
        {
            // {0,"idle"},
            {0,""},
            {1,"happy"},
            {2,"love"},
            {3,"faint"},
            {4,"sleepy"},
            {5,"angry"},
            {6,"sad"},
            {7,"curious"},
        };
    }

    bool needSee;
    private void Start()
    {
        AudioSource.clip = applau;
        AudioSource.loop = false;
        AudioSource.Play();
        
        Refresh();
        FindObjectOfType<FaceDetectorScene>().unityEvent.AddListener((pos) =>
        {
            if (needSee == false)
            {
                Debug.LogError(pos);
                emoji.position = pos;
            }
        });
        
        _emojiAnimators = emoji.GetComponentsInChildren<Animator>();
        
        GetComponent<ShakeDetector>().unityEvent.AddListener(() =>
        {
            // Debug.LogError("震动检测");
            this.ChangeToAnima(3, 3);
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
                        // Debug.LogError("====>" + item);
                        break;
                    case EventTriggerType.Drag:
                        Debug.LogError("====>" + item);
                        this.ChangeToAnima(1, 5);
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

    private Coroutine _coroutine;
    private void ChangeToAnima(int index, float sec, bool isLoop = true, float volume = 0.5f)
    {
        Clover.gameObject.SetActive(false);
        emoji.transform.DOLocalMoveX(0, .3f);
        
        eventTrigger.enabled = true;

        foreach (var animator in _emojiAnimators)
        {
            animator.SetInteger(Emotion, index);
        }
        if (string.IsNullOrEmpty(dictionary[index]) == false)
        {
            AudioSource.clip = audioClips.Find((clip) => clip.name == dictionary[index]);
            AudioSource.loop = isLoop;
            AudioSource.volume = volume;
            AudioSource.Play();

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            _coroutine = StartCoroutine(Enumerator(index, sec));
        }
        else
        {
            AudioSource.Stop();
        }
    }

    private IEnumerator Enumerator(int index, float sec)
    {
        yield return new WaitForSeconds(sec);
        if (index != 0)
        {
            ChangeToAnima(0, sec);
        }
    }

    private Coroutine _coro;
    private void ChangeToAdvAnima(string audioClipName, bool isLoop = false, float volume = .5f, Action callback = null)
    {
        Clover.gameObject.SetActive(true);
        emoji.transform.DOLocalMoveX(-.3f, .3f);

        eventTrigger.enabled = false;
        foreach (var animator in _emojiAnimators)
        {
            animator.SetInteger(Emotion, 0);
        }
        AudioSource.clip = Resources.Load<AudioClip>("Audio/" + audioClipName);
        AudioSource.loop = isLoop;
        AudioSource.volume = volume;
        AudioSource.Play();
        if (_coro != null)
        {
            StopCoroutine(_coro);
            _coro = null;
        }
        _coro = StartCoroutine(Enumerator(AudioSource.clip.length));
        IEnumerator Enumerator(float sac)
        {
            yield return new WaitForSeconds(sac);
            callback?.Invoke();
        }
    }

    private void OnGUI()
    {
        GUI.skin.button.fontSize = 30;
        if (GUILayout.Button("刷新", _options))
        {
            Refresh();
        }
        if (GUILayout.Button("待机", _options) || Input.GetKeyDown(KeyCode.D))
        {
            this.ChangeToAnima(0, 3);
        }
        if (GUILayout.Button("抚摸\n(开心)", _options) || Input.GetKeyDown(KeyCode.K))
        {
            this.ChangeToAnima(1, 3);
        }
        if (GUILayout.Button("点击\n(摇晃)", _options) || Input.GetKeyDown(KeyCode.Y))
        {
            this.ChangeToAnima(3, 3);
        }
        if (GUILayout.Button("(生气)", _options) || Input.GetKeyDown(KeyCode.S))
        {
            this.ChangeToAnima(5, 3);
        }
        if (GUILayout.Button("喜欢\n(爱心)", _options) || Input.GetKeyDown(KeyCode.A))
        {
            this.ChangeToAnima(2, 3);
        }
        if (GUILayout.Button("观察主人\n(悲伤)", _options) || Input.GetKeyDown(KeyCode.B))
        {
            this.ChangeToAnima(6, 3);
        }
        if (GUILayout.Button("疑问", _options) || Input.GetKeyDown(KeyCode.W))
        {
            this.ChangeToAnima(7, 3, false);
        }
        if (GUILayout.Button("觉", _options) || Input.GetKeyDown(KeyCode.J))
        {
            this.ChangeToAnima(4, float.MaxValue);
        }
        
        if (GUILayout.Button("奶茶", _options) || Input.GetKeyDown(KeyCode.N))
        {
            needSee = true;
            ChangeToAdvAnima("点外卖", false, 1);
        }
        if (GUILayout.Button("好的", _options) || Input.GetKeyDown(KeyCode.H))
        {
            ChangeToAdvAnima("好的", false, 1, () =>
            {
                needSee = false;
                this.ChangeToAnima(0, 3);
            });
        }
    }
}