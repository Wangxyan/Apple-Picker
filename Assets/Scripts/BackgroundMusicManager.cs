using UnityEngine;
using UnityEngine.SceneManagement; // 需要引入 SceneManagement

// 添加 RequireComponent 属性确保对象总是有 AudioSource
[RequireComponent(typeof(AudioSource))]
public class PersistentBackgroundMusic : MonoBehaviour
{
    // 使用静态变量实现单例模式，方便访问，并防止重复创建
    public static PersistentBackgroundMusic Instance { get; private set; }

    private AudioSource audioSource;

    // 在 Inspector 中设置要循环播放的背景音乐
    [Header("设置")]
    [Tooltip("拖拽唯一的背景音乐文件到这里")]
    public AudioClip backgroundMusicClip;

    [Range(0f, 1f)] // 音量滑块
    [Tooltip("背景音乐的音量")]
    public float musicVolume = 0.8f;

    void Awake()
    {
        // --- 单例模式实现 ---
        if (Instance == null)
        {
            // 如果还没有实例，将当前这个设为实例
            Instance = this;
            // 让这个 GameObject 在加载新场景时不被销毁
            DontDestroyOnLoad(gameObject);

            // 获取 AudioSource 组件 (因为有 RequireComponent，所以肯定存在)
            audioSource = GetComponent<AudioSource>();

            // --- 初始化并播放音乐 ---
            InitializeAndPlayMusic();
        }
        else if (Instance != this)
        {
            // 如果已经存在一个实例了（通常发生在返回启动场景时），
            // 就销毁当前这个重复的 GameObject，保留第一个创建的。
            Destroy(gameObject);
            return; // 阻止这个重复实例执行后续代码
        }
        // --- 单例结束 ---
    }

    // 初始化 AudioSource 并开始播放音乐
    private void InitializeAndPlayMusic()
    {
        if (backgroundMusicClip != null)
        {
            audioSource.clip = backgroundMusicClip; // 设置音乐片段
            audioSource.volume = musicVolume;     // 设置音量
            audioSource.loop = true;              // 确保循环播放
            audioSource.playOnAwake = false;      // 确保不是由 PlayOnAwake 触发的
            audioSource.spatialBlend = 0f;        // 确保是 2D 音效

            // 只有在当前没有播放音乐时才开始播放
            // (这可以防止在返回已存在实例的场景时重新播放)
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("后台音乐开始播放: " + backgroundMusicClip.name);
            }
        }
        else
        {
            Debug.LogError("错误：背景音乐片段 (Background Music Clip) 未在 PersistentBackgroundMusic 的 Inspector 中设置！");
        }
    }

    // (可选) 提供外部控制音量的方法
    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume); // 限制音量在 0 到 1 之间
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
        }
    }

    // (可选) 提供外部停止音乐的方法 (通常不需要，因为是持久背景音乐)
    // public void StopMusic()
    // {
    //     if (audioSource != null && audioSource.isPlaying)
    //     {
    //         audioSource.Stop();
    //     }
    // }
}