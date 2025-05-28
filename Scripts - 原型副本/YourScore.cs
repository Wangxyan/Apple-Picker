using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ���� UI �����ռ���ʹ�� Text ���

public class YourScore : MonoBehaviour
{
    [Header("�� Inspector ������")]
    [Tooltip("ָ��������ʾ 'Your Score: ...' �� Text UI ���")]
    public Text scoreDisplayText; // ��� Text �������ʾ���յĸ�ʽ���ı�

    [Tooltip("ָ��������ǰԭʼ�������ֵ� Text UI ��� (���� Basket �ű����µ��Ǹ�)")]
    public Text sourceScoreText;  // ��� Text ��������� Basket �ű����µĴ���������

    // Update is called once per frame
    void Update()
    {
        if (scoreDisplayText != null && sourceScoreText != null)
        {
            string currentScore = sourceScoreText.text;

            // ��ʽ������Ҫ��ʾ���ı�
            scoreDisplayText.text = "Your Score: " + currentScore;
        }
        else
        {
            // �������δ���ã��ڿ���̨���������ʾ����������ʱ����
            if (scoreDisplayText == null)
            {
                // ���´�����ʾ�еĽű���
                Debug.LogError("����YourScore �ű��ϵ� 'Score Display Text' δ�� Inspector �����ã�", this.gameObject);
            }
            if (sourceScoreText == null)
            {
                // ���´�����ʾ�еĽű���
                Debug.LogError("����YourScore �ű��ϵ� 'Source Score Text' δ�� Inspector �����ã�", this.gameObject);
            }
            // ����������ѡ����ô˽ű�����ֹUpdate��������
            // this.enabled = false;
        }
    }
}
