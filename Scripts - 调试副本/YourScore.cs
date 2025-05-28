using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间以使用 Text 组件

public class YourScore : MonoBehaviour
{
    [Header("在 Inspector 中设置")]
    [Tooltip("指定用于显示 'Your Score: ...' 的组件")]
    public Text scoreDisplayText;

    [Tooltip("指定用于显示 'New Record!!' 的组件")] 
    public Text newRecordText; //用于控制新记录提示

    void Start()
    {
        // 检查 PlayerPrefs 中是否保存了上一局的分数
        if (PlayerPrefs.HasKey("LastScore"))
        {
            // 读取上一局的分数
            int lastScore = PlayerPrefs.GetInt("LastScore");
            // 显示上一局的分数
            scoreDisplayText.text = "Your Score:" + lastScore;
            // 清除已读取的分数
            PlayerPrefs.DeleteKey("LastScore");
        }
        else
        {
            // 如果没有保存的分数（比如第一次玩），则显示 0 或初始状态
            scoreDisplayText.text = "Your Score:99";
        }
        // 检查是否需要显示新纪录提示
        int showNewRecord = PlayerPrefs.GetInt("ShowNewRecordMessage", 0); // 0 是默认值

        if (newRecordText != null) // 确保 newRecordText 已在 Inspector 中设置
        {
            if (showNewRecord == 1)
            {
                // 如果标记为 1，则显示 "New Record!!" 文本
                newRecordText.gameObject.SetActive(true); // 激活文本对象使其可见
            }
            else
            {
                // 否则，确保它是隐藏的
                newRecordText.gameObject.SetActive(false); // 禁用文本对象使其不可见
            }
        }
    }
}
