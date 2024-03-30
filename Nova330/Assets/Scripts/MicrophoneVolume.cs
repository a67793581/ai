using System.Collections;
using UnityEngine;

public class MicrophoneVolume : MonoBehaviour
{
    // 麦克风名称
    public string microphoneName = null;

    // 音量值
    public float volume = 0f;

    // 更新频率
    public float updateRate = 0.1f;

    void Start()
    {
        // 检查是否有可用的麦克风
        if (Microphone.devices.Length > 0)
        {
            // 如果未指定麦克风名称，则使用默认麦克风
            if (string.IsNullOrEmpty(microphoneName))
                microphoneName = Microphone.devices[0];

            // 启动协程更新音量
            StartCoroutine(UpdateVolume());
        }
        else
        {
            Debug.LogWarning("No microphone found!");
        }
    }

    // 更新音量
    IEnumerator UpdateVolume()
    {
        while (true)
        {
            // 获取麦克风音量
            Microphone.GetDeviceCaps(microphoneName, out var minFreq, out var maxFreq);

            // 将麦克风音量映射到0到1之间的值
            volume = Mathf.Clamp01(minFreq / 100f);
            // Debug.LogError(minFreq+"     "+maxFreq);
            // 等待下一次更新
            yield return new WaitForSeconds(updateRate);
        }
    }
}