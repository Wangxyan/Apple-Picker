using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    public Text scoreGT;
    public static int currentScore = 0;

    private ApplePicker applePickerScript;

    // 基础分数常量
    private const int BASE_SCORE_NORMAL = 200;
    private const int BASE_SCORE_POISON = 400;
    private const int BASE_SCORE_GOLDEN = 100;

    // 新规则常量
    private const int BASELINE_BASKET_COUNT = 3; // 基准篮筐数
    private const int SCORE_PENALTY_PER_EXTRA_BASKET = 100; // 每多一个篮筐减少的分数
    private const float SCORE_MULTIPLIER_PER_LESS_BASKET = 2.0f; // 每少一个篮筐的乘数 (用 float)


    void Start()
    {
        // 保留查找 UI 和 ApplePicker 的代码
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        if (scoreGO != null)
        {
            scoreGT = scoreGO.GetComponent<Text>();
            if (scoreGT != null)
            {
                scoreGT.text = currentScore.ToString(); // 显示当前静态分数
            }
            else { Debug.LogError("错误：ScoreCounter 对象上没有找到 Text 组件！"); }
        }
        else { Debug.LogError("错误：场景中找不到名为 ScoreCounter 的对象！"); }

        if (Camera.main != null)
        {
            applePickerScript = Camera.main.GetComponent<ApplePicker>();
            if (applePickerScript == null)
            {
                Debug.LogError("错误：未能从 Main Camera 上找到 ApplePicker 脚本组件！", Camera.main.gameObject);
            }
        }
        else
        {
            Debug.LogError("错误：场景中找不到 Main Camera！");
        }
    }

    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        if (Camera.main != null)
        {
            mousePos2D.z = -Camera.main.transform.position.z;
            Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
            Vector3 pos = this.transform.position;
            pos.x = mousePos3D.x;
            this.transform.position = pos;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (applePickerScript == null)
        {
            Debug.LogError("无法处理碰撞，因为 ApplePicker 脚本引用丢失！");
            return;
        }

        int baseScore = 0;
        bool isValidAppleCollision = true;
        string appleType = "Unknown";

        // 1. 确定类型、基础分、效果
        if (collidedWith.CompareTag("Apple"))
        {
            appleType = "Normal";
            baseScore = BASE_SCORE_NORMAL;
        }
        else if (collidedWith.CompareTag("ApplePoison"))
        {
            appleType = "Poison";
            baseScore = BASE_SCORE_POISON;
            applePickerScript.LoseBasket();
        }
        else if (collidedWith.CompareTag("AppleGolden"))
        {
            appleType = "Golden";
            baseScore = BASE_SCORE_GOLDEN;
            Debug.Log($"[Golden Apple] Collision Enter. Current Score BEFORE effect: {currentScore}");
            applePickerScript.GainBasket();
            Debug.Log($"[Golden Apple] GainBasket() called.");
        }
        else
        {
            isValidAppleCollision = false;
        }

        // 2. 计算并添加分数 (如果有效)
        if (isValidAppleCollision)
        {
            // --- 修改得分计算逻辑 ---
            int currentBasketCount = applePickerScript.basketList.Count;
            int differenceFromBaseline = currentBasketCount - BASELINE_BASKET_COUNT;
            int scoreToAdd = 0;

            if (differenceFromBaseline == 0) // 正好 3 个篮筐
            {
                scoreToAdd = baseScore;
            }
            else if (differenceFromBaseline > 0) // 多于 3 个篮筐
            {
                // 每多一个，奖励减少 100
                scoreToAdd = baseScore - (differenceFromBaseline * SCORE_PENALTY_PER_EXTRA_BASKET);
                // 允许负数结果
            }
            else // 少于 3 个篮筐 (differenceFromBaseline < 0)
            {
                // 每少一个，奖励乘以 2
                // -differenceFromBaseline 是少的数量 (例如，差是-1，少1个；差是-2，少2个)
                // 奖励 = 基础分 * (2 ^ 少的数量)
                scoreToAdd = Mathf.RoundToInt(baseScore * Mathf.Pow(SCORE_MULTIPLIER_PER_LESS_BASKET, -(differenceFromBaseline + 1)));
            }
            // --- 得分计算逻辑修改结束 ---


            // --- 日志记录 ---
            Debug.Log($"[{appleType} Apple] Baskets: {currentBasketCount}, Diff: {differenceFromBaseline}, Base: {baseScore}, ToAdd: {scoreToAdd}");

            int scoreBeforeAdding = currentScore;
            currentScore += scoreToAdd;
            Debug.Log($"[{appleType} Apple] Score Calculation: {scoreBeforeAdding} + {scoreToAdd} = {currentScore}");


            // --- 更新UI和最高分 ---
            if (scoreGT != null)
            {
                scoreGT.text = currentScore.ToString();
                Debug.Log($"[{appleType} Apple] UI Updated to: {scoreGT.text}");
            }

            if (currentScore > HighScore.score)
            {
                HighScore.score = currentScore;
                PlayerPrefs.SetInt("NewRecordFlag", 1);
                Debug.Log($"[{appleType} Apple] New High Score set!");
            }

            // 销毁苹果
            Destroy(collidedWith);
        }
    }
}