using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间以使用 Text 组件

public class YourScore : MonoBehaviour
{
    [Header("在 Inspector 中设置")]
    [Tooltip("指定用于显示 'Your Score: ...' 的组件")]
    public Text scoreDisplayText;

    // --- 修改：同时控制 Text 和 Image ---
    [Tooltip("指定用于显示 'New Record!!' 的 Text 组件 (可选)")]
    public Text newRecordText; // 用于控制新记录提示文字

    [Tooltip("指定用于显示新纪录背景/图标等的 Image 组件 (可选)")] // 新增字段
    public Image newRecordImage; // 新增字段，用于控制新记录图像
    // --- 修改结束 ---

    void Start()
    {
        // --- 处理上一局的分数显示 ---
        if (PlayerPrefs.HasKey("LastScore"))
        {
            int lastScore = PlayerPrefs.GetInt("LastScore");
            // (修正) 确保 scoreDisplayText 存在再访问
            if (scoreDisplayText != null)
            {
                scoreDisplayText.text = "你的得分 : " + lastScore;
            }
            PlayerPrefs.DeleteKey("LastScore");
        }
        else
        {
            // (修正) 确保 scoreDisplayText 存在再访问
            if (scoreDisplayText != null)
            {
                scoreDisplayText.text = "你的得分 : 0"; // 初始分数通常设为 0
            }
        }

        // --- 处理新纪录提示的显示 (Text 和 Image) ---
        int showNewRecord = PlayerPrefs.GetInt("ShowNewRecordMessage", 0); // 0 是默认值

        bool shouldShow = (showNewRecord == 1); // 计算是否应该显示

        // 控制 Text 的显示/隐藏
        if (newRecordText != null) // 确保 newRecordText 已在 Inspector 中设置
        {
            newRecordText.gameObject.SetActive(shouldShow); // 根据标记设置激活状态
        }
        // 否则可以加一个警告，如果这个 Text 是必须的
        // else { Debug.LogWarning("YourScore: New Record Text 未在 Inspector 中设置！"); }

        // 控制 Image 的显示/隐藏 (新增逻辑)
        if (newRecordImage != null) // 确保 newRecordImage 已在 Inspector 中设置
        {
            newRecordImage.gameObject.SetActive(shouldShow); // 根据相同的标记设置激活状态
        }
        // 否则可以加一个警告，如果这个 Image 是必须的
        // else { Debug.LogWarning("YourScore: New Record Image 未在 Inspector 中设置！"); }


        // (重要) 清除用于显示消息的标记，这样下次游戏结束时（如果没破纪录）不会错误地显示
        // 如果上一局没破纪录(showNewRecord=0), 这里删除也不会出错
        PlayerPrefs.DeleteKey("ShowNewRecordMessage");
        // 或者 PlayerPrefs.SetInt("ShowNewRecordMessage", 0);

    } // End of Start()

    // Update 不再需要
    // void Update() { }

} // End of class YourScore
