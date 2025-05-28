using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间以使用 Text 组件

public class YourScore : MonoBehaviour
{
    [Header("在 Inspector 中设置")]
    [Tooltip("指定用于显示 'Your Score: ...' 的 Text UI 组件")]
    public Text scoreDisplayText; // 这个 Text 组件将显示最终的格式化文本

    [Tooltip("指定包含当前原始分数数字的 Text UI 组件 (例如 Basket 脚本更新的那个)")]
    public Text sourceScoreText;  // 这个 Text 组件包含由 Basket 脚本更新的纯分数数字

    // Update is called once per frame
    void Update()
    {
        if (scoreDisplayText != null && sourceScoreText != null)
        {
            string currentScore = sourceScoreText.text;

            // 格式化最终要显示的文本
            scoreDisplayText.text = "Your Score: " + currentScore;
        }
        else
        {
            // 如果引用未设置，在控制台输出错误提示，避免运行时出错
            if (scoreDisplayText == null)
            {
                // 更新错误提示中的脚本名
                Debug.LogError("错误：YourScore 脚本上的 'Score Display Text' 未在 Inspector 中设置！", this.gameObject);
            }
            if (sourceScoreText == null)
            {
                // 更新错误提示中的脚本名
                Debug.LogError("错误：YourScore 脚本上的 'Source Score Text' 未在 Inspector 中设置！", this.gameObject);
            }
            // 可以在这里选择禁用此脚本，防止Update持续报错
            // this.enabled = false;
        }
    }
}
