using UnityEngine;
using UnityEngine.UI; // 需要引入 UI
using UnityEngine.EventSystems; // 需要引入 EventSystems

// 确保按钮所在的 GameObject 有 AudioSource
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class ButtonClickSound : MonoBehaviour, IPointerDownHandler // 实现接口以便在按下时播放
{
    public AudioClip clickSound; // 在 Inspector 中设置按钮点击音效
    private AudioSource audioSource;
    private Button button;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();

        // (可选) 防止按钮声音被背景音乐完全盖住
        audioSource.spatialBlend = 0f; // 确保是 2D 声音

        // 如果没有在 Inspector 中设置声音，尝试给个默认提示
        if (clickSound == null)
        {
            Debug.LogWarning($"按钮 '{gameObject.name}' 没有设置点击音效 (Click Sound)。");
        }

        // (可选，如果不想在按下时播放，可以在 OnClick 事件里调用)
        // button.onClick.AddListener(PlaySound);
    }

    // 当鼠标指针在按钮上按下时调用
    public void OnPointerDown(PointerEventData eventData)
    {
        PlaySound();
    }

    // 播放声音的方法
    public void PlaySound()
    {
        if (clickSound != null && audioSource != null && button.interactable) // 确保有声音、有播放器且按钮可交互
        {
            // PlayOneShot 允许在不打断当前播放的情况下播放一个短音效
            // 可以传入一个音量缩放因子（0到1之间）
            audioSource.PlayOneShot(clickSound, 1.0f);
        }
    }

    // (可选) 如果使用 AddListener 方式
    // void OnDestroy()
    // {
    //    if (button != null) button.onClick.RemoveListener(PlaySound);
    // }
}