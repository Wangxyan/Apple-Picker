using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ���� UI �����ռ���ʹ�� Text ���

public class YourScore : MonoBehaviour
{
    [Header("�� Inspector ������")]
    [Tooltip("ָ��������ʾ 'Your Score: ...' �����")]
    public Text scoreDisplayText;

    // --- �޸ģ�ͬʱ���� Text �� Image ---
    [Tooltip("ָ��������ʾ 'New Record!!' �� Text ��� (��ѡ)")]
    public Text newRecordText; // ���ڿ����¼�¼��ʾ����

    [Tooltip("ָ��������ʾ�¼�¼����/ͼ��ȵ� Image ��� (��ѡ)")] // �����ֶ�
    public Image newRecordImage; // �����ֶΣ����ڿ����¼�¼ͼ��
    // --- �޸Ľ��� ---

    void Start()
    {
        // --- ������һ�ֵķ�����ʾ ---
        if (PlayerPrefs.HasKey("LastScore"))
        {
            int lastScore = PlayerPrefs.GetInt("LastScore");
            // (����) ȷ�� scoreDisplayText �����ٷ���
            if (scoreDisplayText != null)
            {
                scoreDisplayText.text = "��ĵ÷� : " + lastScore;
            }
            PlayerPrefs.DeleteKey("LastScore");
        }
        else
        {
            // (����) ȷ�� scoreDisplayText �����ٷ���
            if (scoreDisplayText != null)
            {
                scoreDisplayText.text = "��ĵ÷� : 0"; // ��ʼ����ͨ����Ϊ 0
            }
        }

        // --- �����¼�¼��ʾ����ʾ (Text �� Image) ---
        int showNewRecord = PlayerPrefs.GetInt("ShowNewRecordMessage", 0); // 0 ��Ĭ��ֵ

        bool shouldShow = (showNewRecord == 1); // �����Ƿ�Ӧ����ʾ

        // ���� Text ����ʾ/����
        if (newRecordText != null) // ȷ�� newRecordText ���� Inspector ������
        {
            newRecordText.gameObject.SetActive(shouldShow); // ���ݱ�����ü���״̬
        }
        // ������Լ�һ�����棬������ Text �Ǳ����
        // else { Debug.LogWarning("YourScore: New Record Text δ�� Inspector �����ã�"); }

        // ���� Image ����ʾ/���� (�����߼�)
        if (newRecordImage != null) // ȷ�� newRecordImage ���� Inspector ������
        {
            newRecordImage.gameObject.SetActive(shouldShow); // ������ͬ�ı�����ü���״̬
        }
        // ������Լ�һ�����棬������ Image �Ǳ����
        // else { Debug.LogWarning("YourScore: New Record Image δ�� Inspector �����ã�"); }


        // (��Ҫ) ���������ʾ��Ϣ�ı�ǣ������´���Ϸ����ʱ�����û�Ƽ�¼������������ʾ
        // �����һ��û�Ƽ�¼(showNewRecord=0), ����ɾ��Ҳ�������
        PlayerPrefs.DeleteKey("ShowNewRecordMessage");
        // ���� PlayerPrefs.SetInt("ShowNewRecordMessage", 0);

    } // End of Start()

    // Update ������Ҫ
    // void Update() { }

} // End of class YourScore
