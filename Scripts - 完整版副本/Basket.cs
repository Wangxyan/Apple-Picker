using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Basket : MonoBehaviour
{
    [Header("UI & 游戏逻辑引用")]
    public Text scoreGT;
    private ApplePicker applePickerScript;

    // 内部变量
    public static int currentScore = 0; // 静态变量跟踪分数
    private AudioSource sfxAudioSource; // 用于播放碰撞音效的 AudioSource

    // 基础分数常量
    private const int BASE_SCORE_NORMAL = 200;
    private const int BASE_SCORE_POISON = 400;
    private const int BASE_SCORE_GOLDEN = 100;

    // 新规则常量
    private const int BASELINE_BASKET_COUNT = 3; // 基准篮筐数
    private const int SCORE_PENALTY_PER_EXTRA_BASKET = 100; // 每多一个篮筐减少的分数
    private const float SCORE_MULTIPLIER_PER_LESS_BASKET = 2.0f; // 每少一个篮筐的乘数


    void Start()
    {
        // 查找 UI
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        if (scoreGO != null)
        {
            scoreGT = scoreGO.GetComponent<Text>();
            if (scoreGT != null) { scoreGT.text = currentScore.ToString(); }
        }
        // 查找 ApplePicker
        if (Camera.main != null)
        {
            applePickerScript = Camera.main.GetComponent<ApplePicker>();
        }
    }


    void Update()
    {
        // 篮筐移动代码
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
        if (applePickerScript == null) { return; }

        int baseScore = 0;
        bool applyScoreLogic = false;
        bool loseBasketEffect = false;
        bool gainBasketEffect = false;
        string soundType = null; // 存储要播放的声音类型标识

        // 1. 判断类型，设置基础分、效果标记、声音类型
        if (collidedWith.CompareTag("Apple"))
        {
            baseScore = BASE_SCORE_NORMAL;
            applyScoreLogic = true;
            soundType = "Normal"; // 用字符串或枚举标识
        }
        else if (collidedWith.CompareTag("ApplePoison"))
        {
            baseScore = BASE_SCORE_POISON;
            applyScoreLogic = true;
            loseBasketEffect = true;
            soundType = "Poison";
        }
        else if (collidedWith.CompareTag("AppleGolden"))
        {
            baseScore = BASE_SCORE_GOLDEN;
            applyScoreLogic = true;
            gainBasketEffect = true;
            soundType = "Golden";
        }

        // --- 请求播放声音 (移交给 ApplePicker) ---
        if (soundType != null)
        {
            applePickerScript.PlayCatchSound(soundType); // 调用新方法
        }
        // --- 声音请求结束 ---

        // 2. 如果是有效的苹果碰撞，则计算并更新分数
        if (applyScoreLogic)
        {
            int currentBasketCount = applePickerScript.basketList.Count;
            int differenceFromBaseline = currentBasketCount - BASELINE_BASKET_COUNT;
            int scoreToAdd = 0;

            // --- 根据篮筐数计算得分 ---
            if (differenceFromBaseline == 0)
            {
                scoreToAdd = baseScore;
            }
            else if (differenceFromBaseline > 0)
            {
                scoreToAdd = baseScore - (differenceFromBaseline * SCORE_PENALTY_PER_EXTRA_BASKET);
            }
            else // differenceFromBaseline < 0
            {
                scoreToAdd = Mathf.RoundToInt(baseScore * Mathf.Pow(SCORE_MULTIPLIER_PER_LESS_BASKET, -differenceFromBaseline));
            }

            // --- 更新分数和UI (在效果触发前) ---
            currentScore += scoreToAdd;
            if (scoreGT != null)
            {
                scoreGT.text = currentScore.ToString();
            }

            // --- 检查并更新最高分 (在效果触发前) ---
            if (currentScore > HighScore.score)
            {
                HighScore.score = currentScore;
                PlayerPrefs.SetInt("NewRecordFlag", 1);
            }
        }

        // 3. 执行特殊效果 (在分数更新之后)
        if (loseBasketEffect)
        {
            applePickerScript.LoseBasket(); 
        }
        if (gainBasketEffect)
        {
            applePickerScript.GainBasket();
        }

        // 4. 销毁碰到的苹果 (如果它是有效苹果)
        if (applyScoreLogic)
        {
            Destroy(collidedWith);
        }
    }
}