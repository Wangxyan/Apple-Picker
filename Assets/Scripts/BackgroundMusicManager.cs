using UnityEngine;
using UnityEngine.SceneManagement; // ��Ҫ���� SceneManagement

// ��� RequireComponent ����ȷ������������ AudioSource
[RequireComponent(typeof(AudioSource))]
public class PersistentBackgroundMusic : MonoBehaviour
{
    // ʹ�þ�̬����ʵ�ֵ���ģʽ��������ʣ�����ֹ�ظ�����
    public static PersistentBackgroundMusic Instance { get; private set; }

    private AudioSource audioSource;

    // �� Inspector ������Ҫѭ�����ŵı�������
    [Header("����")]
    [Tooltip("��קΨһ�ı��������ļ�������")]
    public AudioClip backgroundMusicClip;

    [Range(0f, 1f)] // ��������
    [Tooltip("�������ֵ�����")]
    public float musicVolume = 0.8f;

    void Awake()
    {
        // --- ����ģʽʵ�� ---
        if (Instance == null)
        {
            // �����û��ʵ��������ǰ�����Ϊʵ��
            Instance = this;
            // ����� GameObject �ڼ����³���ʱ��������
            DontDestroyOnLoad(gameObject);

            // ��ȡ AudioSource ��� (��Ϊ�� RequireComponent�����Կ϶�����)
            audioSource = GetComponent<AudioSource>();

            // --- ��ʼ������������ ---
            InitializeAndPlayMusic();
        }
        else if (Instance != this)
        {
            // ����Ѿ�����һ��ʵ���ˣ�ͨ�������ڷ�����������ʱ����
            // �����ٵ�ǰ����ظ��� GameObject��������һ�������ġ�
            Destroy(gameObject);
            return; // ��ֹ����ظ�ʵ��ִ�к�������
        }
        // --- �������� ---
    }

    // ��ʼ�� AudioSource ����ʼ��������
    private void InitializeAndPlayMusic()
    {
        if (backgroundMusicClip != null)
        {
            audioSource.clip = backgroundMusicClip; // ��������Ƭ��
            audioSource.volume = musicVolume;     // ��������
            audioSource.loop = true;              // ȷ��ѭ������
            audioSource.playOnAwake = false;      // ȷ�������� PlayOnAwake ������
            audioSource.spatialBlend = 0f;        // ȷ���� 2D ��Ч

            // ֻ���ڵ�ǰû�в�������ʱ�ſ�ʼ����
            // (����Է�ֹ�ڷ����Ѵ���ʵ���ĳ���ʱ���²���)
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("��̨���ֿ�ʼ����: " + backgroundMusicClip.name);
            }
        }
        else
        {
            Debug.LogError("���󣺱�������Ƭ�� (Background Music Clip) δ�� PersistentBackgroundMusic �� Inspector �����ã�");
        }
    }

    // (��ѡ) �ṩ�ⲿ���������ķ���
    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume); // ���������� 0 �� 1 ֮��
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
        }
    }

    // (��ѡ) �ṩ�ⲿֹͣ���ֵķ��� (ͨ������Ҫ����Ϊ�ǳ־ñ�������)
    // public void StopMusic()
    // {
    //     if (audioSource != null && audioSource.isPlaying)
    //     {
    //         audioSource.Stop();
    //     }
    // }
}