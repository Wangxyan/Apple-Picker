using UnityEngine;
using UnityEngine.UI; // ��Ҫ���� UI
using UnityEngine.EventSystems; // ��Ҫ���� EventSystems

// ȷ����ť���ڵ� GameObject �� AudioSource
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class ButtonClickSound : MonoBehaviour, IPointerDownHandler // ʵ�ֽӿ��Ա��ڰ���ʱ����
{
    public AudioClip clickSound; // �� Inspector �����ð�ť�����Ч
    private AudioSource audioSource;
    private Button button;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();

        // (��ѡ) ��ֹ��ť����������������ȫ��ס
        audioSource.spatialBlend = 0f; // ȷ���� 2D ����

        // ���û���� Inspector ���������������Ը���Ĭ����ʾ
        if (clickSound == null)
        {
            Debug.LogWarning($"��ť '{gameObject.name}' û�����õ����Ч (Click Sound)��");
        }

        // (��ѡ����������ڰ���ʱ���ţ������� OnClick �¼������)
        // button.onClick.AddListener(PlaySound);
    }

    // �����ָ���ڰ�ť�ϰ���ʱ����
    public void OnPointerDown(PointerEventData eventData)
    {
        PlaySound();
    }

    // ���������ķ���
    public void PlaySound()
    {
        if (clickSound != null && audioSource != null && button.interactable) // ȷ�����������в������Ұ�ť�ɽ���
        {
            // PlayOneShot �����ڲ���ϵ�ǰ���ŵ�����²���һ������Ч
            // ���Դ���һ�������������ӣ�0��1֮�䣩
            audioSource.PlayOneShot(clickSound, 1.0f);
        }
    }

    // (��ѡ) ���ʹ�� AddListener ��ʽ
    // void OnDestroy()
    // {
    //    if (button != null) button.onClick.RemoveListener(PlaySound);
    // }
}