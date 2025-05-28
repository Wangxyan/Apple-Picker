using UnityEngine;
using UnityEngine.UI; // �����Ҫֱ�Ӹ���UI�ı�������Ҫ����

public class ResetHighScoreButton : MonoBehaviour
{
    public void ResetHighScoreTo1000()
    {
        int resetValue = 1000; // ����Ҫ���õ���ֵ

        // 1. ���� PlayerPrefs �е� "HighScore"
        PlayerPrefs.SetInt("HighScore", resetValue);

        // 2. (��ѡ���Ƽ�) ������ HighScore �ű���һ����̬������Ҳͬ��������
        if (FindObjectOfType<HighScore>() != null) // ���HighScore�ű��Ƿ�����ڳ�����
        {
            HighScore.score = resetValue;
        }

        // 4. ȷ�����ݱ����棨��ĳЩƽ̨������¿�����Ҫ��
        PlayerPrefs.Save();

        // 5. �ڿ���̨��ӡ��־��ȷ�ϲ�����ִ��
        Debug.Log($"PlayerPrefs 'HighScore' has been reset to {resetValue}");
    }
}