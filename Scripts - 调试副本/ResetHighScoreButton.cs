using UnityEngine;
using UnityEngine.UI; // 如果需要直接更新UI文本，则需要此行

public class ResetHighScoreButton : MonoBehaviour
{
    public void ResetHighScoreTo1000()
    {
        int resetValue = 1000; // 定义要重置到的值

        // 1. 更新 PlayerPrefs 中的 "HighScore"
        PlayerPrefs.SetInt("HighScore", resetValue);

        // 2. (可选但推荐) 如果你的 HighScore 脚本有一个静态变量，也同步更新它
        if (FindObjectOfType<HighScore>() != null) // 检查HighScore脚本是否存在于场景中
        {
            HighScore.score = resetValue;
        }

        // 4. 确保数据被保存（在某些平台或情况下可能需要）
        PlayerPrefs.Save();

        // 5. 在控制台打印日志，确认操作已执行
        Debug.Log($"PlayerPrefs 'HighScore' has been reset to {resetValue}");
    }
}