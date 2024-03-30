using UnityEngine;

public class GameMain : MonoBehaviour
{
    private Animator[] _emojiAnimators;
    private static readonly int Emotion = Animator.StringToHash("EmotionState");
    private GUILayoutOption[] _options = { GUILayout.Width(200), GUILayout.Height(100)};

    private void OnEnable()
    {
        _options = new [] { GUILayout.Width(200), GUILayout.Height(100)};
    }

    private void Start()
    {
        _emojiAnimators = GameObject.Find("Emoji").GetComponentsInChildren<Animator>();
    }

    private void OnGUI()
    {
        GUI.skin.button.fontSize = 20;
        if (GUILayout.Button("视觉反馈", _options))
        {
            foreach (var animator in _emojiAnimators)
            {
                animator.SetInteger(Emotion, 0);
            }
        }
        if (GUILayout.Button("抚摸(开心)", _options))
        {
            foreach (var animator in _emojiAnimators)
            {
                animator.SetInteger(Emotion, 1);
            }
        }
        if (GUILayout.Button("点击(摇晃)", _options))
        {
            foreach (var animator in _emojiAnimators)
            {
                animator.SetInteger(Emotion, 2);
            }
        }
        if (GUILayout.Button("陀螺仪检测(头晕/生气)", _options))
        {
            foreach (var animator in _emojiAnimators)
            {
                animator.SetInteger(Emotion, 3);
            }
        }
        if (GUILayout.Button("说话反馈(拍照/唱歌/画图/视频)", _options))
        {
            foreach (var animator in _emojiAnimators)
            {
                animator.SetInteger(Emotion, 3);
            }
        }
        if (GUILayout.Button("观察主人(为什么伤心啦)", _options))
        {
            foreach (var animator in _emojiAnimators)
            {
                animator.SetInteger(Emotion, 0);
            }
        }
    }
}
