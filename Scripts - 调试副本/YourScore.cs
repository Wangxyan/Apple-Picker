using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ���� UI �����ռ���ʹ�� Text ���

public class YourScore : MonoBehaviour
{
    [Header("�� Inspector ������")]
    [Tooltip("ָ��������ʾ 'Your Score: ...' �����")]
    public Text scoreDisplayText;

    [Tooltip("ָ��������ʾ 'New Record!!' �����")] 
    public Text newRecordText; //���ڿ����¼�¼��ʾ

    void Start()
    {
        // ��� PlayerPrefs ���Ƿ񱣴�����һ�ֵķ���
        if (PlayerPrefs.HasKey("LastScore"))
        {
            // ��ȡ��һ�ֵķ���
            int lastScore = PlayerPrefs.GetInt("LastScore");
            // ��ʾ��һ�ֵķ���
            scoreDisplayText.text = "Your Score:" + lastScore;
            // ����Ѷ�ȡ�ķ���
            PlayerPrefs.DeleteKey("LastScore");
        }
        else
        {
            // ���û�б���ķ����������һ���棩������ʾ 0 ���ʼ״̬
            scoreDisplayText.text = "Your Score:99";
        }
        // ����Ƿ���Ҫ��ʾ�¼�¼��ʾ
        int showNewRecord = PlayerPrefs.GetInt("ShowNewRecordMessage", 0); // 0 ��Ĭ��ֵ

        if (newRecordText != null) // ȷ�� newRecordText ���� Inspector ������
        {
            if (showNewRecord == 1)
            {
                // ������Ϊ 1������ʾ "New Record!!" �ı�
                newRecordText.gameObject.SetActive(true); // �����ı�����ʹ��ɼ�
            }
            else
            {
                // ����ȷ���������ص�
                newRecordText.gameObject.SetActive(false); // �����ı�����ʹ�䲻�ɼ�
            }
        }
    }
}
