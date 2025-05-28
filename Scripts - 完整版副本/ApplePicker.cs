using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 添加 RequireComponent 确保总有 AudioSource
[RequireComponent(typeof(AudioSource))]

public class ApplePicker : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 0.5f; 
    public List<GameObject> basketList;
    public int maxBaskets = 5; // 篮筐数量上限

    [Header("接住音效片段 (Catch SFX)")] // 分组更清晰
    public AudioClip normalAppleSound;
    public AudioClip poisonAppleSound;
    public AudioClip goldenAppleSound;

    [Header("掉落音效片段 (Fall SFX)")] // 新增音效组
    public AudioClip normalAppleFallSound; // 新增
    public AudioClip goldenAppleFallSound; // 新增
    // 注意：毒苹果掉落无效果也无声音

    private AudioSource sfxAudioSource; // 新增：用于播放音效

    // --- 新增状态标记 ---
    private bool isHandlingFall = false; // 标记是否正在处理因苹果掉落引发的游戏状态改变

    void Awake() // 使用 Awake 确保 AudioSource 在 Start 前获取
    {
        // 获取附加在自身 (Main Camera) 上的 AudioSource
        sfxAudioSource = GetComponent<AudioSource>();
        if (sfxAudioSource == null)
        {
            Debug.LogError("错误：ApplePicker (Main Camera) 对象上没有找到 AudioSource 组件！");
        }
        // (可选) 设置默认值
        sfxAudioSource.playOnAwake = false;
        sfxAudioSource.loop = false;
    }

    void Start()
    {
        Basket.currentScore = 0; // 重置静态分数
        PlayerPrefs.SetInt("NewRecordFlag", 0); // 重置破纪录标记

        basketList = new List<GameObject>();

        for (int i = 0; i < numBaskets; i++)
        {
            // 检查数量是否已达上限
            if (basketList.Count < maxBaskets)
            {
                CreateBasket(i);
            }
        }
        // 游戏开始时，确保标记为 false
        isHandlingFall = false;
    }

    // --- 新增：播放接住音效的方法 ---
    public void PlayCatchSound(string appleType)
    {
        AudioClip clipToPlay = null;
        switch (appleType)
        {
            case "Normal":
                clipToPlay = normalAppleSound;
                break;
            case "Poison":
                clipToPlay = poisonAppleSound;
                break;
            case "Golden":
                clipToPlay = goldenAppleSound;
                break;
        }

        if (clipToPlay != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(clipToPlay);
        }
        else if (sfxAudioSource == null)
        {
            Debug.LogError("无法播放音效，因为 ApplePicker 上的 AudioSource 未找到！");
        }
        // else { Debug.LogWarning($"未找到类型为 {appleType} 的音效片段或未设置。"); }
    }
    // --- 新增方法结束 ---

    // --- 新增：播放掉落音效的方法 ---
    public void PlayFallSound(string appleType)
    {
        AudioClip clipToPlay = null;
        switch (appleType)
        {
            case "Apple": // 使用 Tag 名称
                clipToPlay = normalAppleFallSound;
                break;
            case "AppleGolden": // 使用 Tag 名称
                clipToPlay = goldenAppleFallSound;
                break;
                // 不需要处理 "ApplePoison"，因为它掉落无效果
        }

        if (clipToPlay != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(clipToPlay);
        }
        else if (sfxAudioSource == null)
        {
            Debug.LogError("无法播放掉落音效，因为 ApplePicker 上的 AudioSource 未找到！");
        }
        // else { Debug.LogWarning($"未找到类型为 {appleType} 的掉落音效片段或未设置。"); }
    }
    // --- 新增方法结束 ---

    // 封装创建篮筐逻辑，方便复用
    void CreateBasket(int index)
    {
        GameObject tBasketGO = Instantiate(basketPrefab);
        Vector3 pos = Vector3.zero;
        // 根据索引计算Y坐标，确保篮筐垂直排列
        pos.y = basketBottomY + (basketSpacingY * index);
        tBasketGO.transform.position = pos;
        basketList.Add(tBasketGO);
    }

    // 当一个苹果掉落到屏幕外时调用 (由 Apple.cs 调用)
    public void HandleAppleFell(GameObject appleThatFell)
    {
        // 如果这个掉落的苹果对象已经被销毁了（可能因为 DestroyAllApples），或者
        // 如果我们已经在处理一个掉落事件了，就直接返回，避免重复处理。
        if (appleThatFell == null || isHandlingFall)
        {
            return;
        }

        string appleTag = appleThatFell.tag;

        // 普通苹果或金苹果掉落，并且 *尚未* 开始处理掉落流程
        if (appleTag == "Apple" || appleTag == "AppleGolden")
        {
            // --- 标记开始处理掉落流程 ---
            isHandlingFall = true;

            // 播放对应的掉落音效 (只播放这一次)
            PlayFallSound(appleTag);

            // 执行清屏和扣篮筐操作 (只执行这一次)
            DestroyAllApples();
            LoseBasket(); // LoseBasket 内部会检查是否 GameOver

            // --- 处理完成后，重置标记？ ---
            // 这里是否重置标记取决于你的游戏逻辑。
            // 如果游戏在 LoseBasket 后可能继续（比如还有篮筐），
            // 那么应该在下一帧或稍后重置标记，以便能处理后续的掉落。
            // 如果 LoseBasket 导致 GameOver 并重新加载场景，标记会在 Start 中重置。
            // 为了安全起见，可以在帧末重置或使用协程延迟重置。
            StartCoroutine(ResetHandlingFallFlag()); // 使用协程方式
        }
        else if (appleTag == "ApplePoison")
        {
            // 毒苹果掉落，无事发生，也不需要设置 isHandlingFall 标记
        }
        // 苹果本身的销毁由 Apple.cs 处理
    }

    // (可选) 用于延迟重置标记的协程
    private IEnumerator ResetHandlingFallFlag()
    {
        // 等待到当前帧的末尾
        yield return new WaitForEndOfFrame();
        // 重置标记，允许处理下一个掉落事件（如果游戏没有结束）
        isHandlingFall = false;
    }

    // 处理失去一个篮筐的方法
    public void LoseBasket()
    {
        // 确保还有篮筐可以移除
        if (basketList.Count > 0)
        {
            int basketIndex = basketList.Count - 1; // 获取最后一个篮筐的索引
            GameObject tBasketGO = basketList[basketIndex]; // 获取对象引用
            basketList.RemoveAt(basketIndex); // 从列表中移除
            Destroy(tBasketGO); // 从场景中销毁

            // 检查游戏是否结束
            if (basketList.Count == 0)
            {
                GameOver();
            }
        }
    }

    // 处理增加一个篮筐的方法
    public void GainBasket()
    {
        // 检查是否已达到篮筐上限
        if (basketList.Count < maxBaskets)
        {
            // 创建新篮筐，位置在当前最顶层篮筐之上 (索引即为当前数量)
            CreateBasket(basketList.Count);
        }
        // 如果已达上限，则无事发生
    }

    // 辅助方法：销毁当前屏幕上所有类型的苹果
    void DestroyAllApples()
    {
        // 查找所有带有苹果相关标签的对象
        GameObject[] tAppleArrayNormal = GameObject.FindGameObjectsWithTag("Apple");
        GameObject[] tAppleArrayPoison = GameObject.FindGameObjectsWithTag("ApplePoison");
        GameObject[] tAppleArrayGolden = GameObject.FindGameObjectsWithTag("AppleGolden");

        foreach (GameObject tGO in tAppleArrayNormal) { if (tGO != null) Destroy(tGO); }
        foreach (GameObject tGO in tAppleArrayPoison) { if (tGO != null) Destroy(tGO); }
        foreach (GameObject tGO in tAppleArrayGolden) { if (tGO != null) Destroy(tGO); }
    }

    // 辅助方法：处理游戏结束逻辑
    void GameOver()
    {
        // 读取本局是否破纪录的标记
        int newRecordAchieved = PlayerPrefs.GetInt("NewRecordFlag", 0);

        // 保存最终分数到 PlayerPrefs
        PlayerPrefs.SetInt("LastScore", Basket.currentScore);
        // 保存是否显示"New Record"消息的标记到 PlayerPrefs
        PlayerPrefs.SetInt("ShowNewRecordMessage", newRecordAchieved);

        PlayerPrefs.Save(); // 确保数据被写入磁盘

        // 重新加载指定的结束场景或主场景
        SceneManager.LoadScene("End_Scene_0"); 
    }
}