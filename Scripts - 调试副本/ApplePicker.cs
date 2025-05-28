using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;
    public int maxBaskets = 5; // 新增：篮筐数量上限

    void Start()
    {
        Basket.currentScore = 0; // 重置静态分数
        PlayerPrefs.SetInt("NewRecordFlag", 0); // 重置破纪录标记

        basketList = new List<GameObject>();
        /*for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }*/
        for (int i = 0; i < numBaskets; i++)
        {
            // 检查数量是否已达上限（虽然Start时通常不会，但以防万一）
            if (basketList.Count < maxBaskets)
            {
                CreateBasket(i);
            }
        }
    }

    // 封装创建篮筐逻辑，方便复用
    void CreateBasket(int index)
        {
            GameObject tBasketGO = Instantiate(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * index);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }

        // 当一个苹果掉落到屏幕外时调用 (由 Apple.cs 调用)
        public void HandleAppleFell(GameObject appleThatFell)
        {
            if (appleThatFell == null) return; // 安全检查

            string appleTag = appleThatFell.tag; // 获取掉落苹果的标签

            if (appleTag == "Apple" || appleTag == "AppleGolden")
            {
                // 普通苹果或金苹果掉落：销毁所有苹果，失去一个篮筐
                DestroyAllApples();
                LoseBasket(); // 使用 LoseBasket 方法处理失去篮筐和游戏结束检查
            }
            else if (appleTag == "ApplePoison")
            {
                // 毒苹果掉落：无事发生
                // 不需要做任何操作
            }
        }

        // 新方法：处理失去一个篮筐
        public void LoseBasket()
        {
            // 确保还有篮筐可以移除
            if (basketList.Count > 0)
            {
                int basketIndex = basketList.Count - 1;
                GameObject tBasketGO = basketList[basketIndex];
                basketList.RemoveAt(basketIndex);
                Destroy(tBasketGO);

                // 检查游戏是否结束
                if (basketList.Count == 0)
                {
                    GameOver();
                }
            }
            // 如果 basketList.Count 已经是 0，理论上不应该再调用 LoseBasket，
            // 但如果发生了，这里不会出错。
        }

        // 新方法：处理增加一个篮筐
        public void GainBasket()
        {
            // 检查是否已达到篮筐上限
            if (basketList.Count < maxBaskets)
            {
                // 创建新篮筐，位置在当前最顶层篮筐之上
                CreateBasket(basketList.Count);
            }
            // 如果已达上限，则无事发生
        }

        // 辅助方法：销毁当前屏幕上所有标签为 "Apple", "ApplePoison", "AppleGolden" 的苹果
        void DestroyAllApples()
        {
            // 可以查找多个标签，或者如果所有苹果都继承自一个基类或接口会更优雅
            GameObject[] tAppleArrayNormal = GameObject.FindGameObjectsWithTag("Apple");
            GameObject[] tAppleArrayPoison = GameObject.FindGameObjectsWithTag("ApplePoison");
            GameObject[] tAppleArrayGolden = GameObject.FindGameObjectsWithTag("AppleGolden");

            foreach (GameObject tGO in tAppleArrayNormal) { if (tGO != null) Destroy(tGO); }
            foreach (GameObject tGO in tAppleArrayPoison) { if (tGO != null) Destroy(tGO); }
            foreach (GameObject tGO in tAppleArrayGolden) { if (tGO != null) Destroy(tGO); }
        }

        // 辅助方法：处理游戏结束
        void GameOver()
        {
            // 读取本局是否破纪录的标记
            int newRecordAchieved = PlayerPrefs.GetInt("NewRecordFlag", 0);

            // 保存最终分数
            PlayerPrefs.SetInt("LastScore", Basket.currentScore);
            // 保存是否显示"New Record"消息的标记
            PlayerPrefs.SetInt("ShowNewRecordMessage", newRecordAchieved);

            PlayerPrefs.Save(); // 确保数据被写入

            // 重新开始游戏
            SceneManager.LoadScene("End_Scene_0");
        }

        // !! 保留旧的 AppleDestroyed 以防万一，但新的逻辑应该调用 HandleAppleFell !!
        // 你可以考虑删除这个旧方法，或者将其内部逻辑替换为调用 HandleAppleFell("Apple")
        public void AppleDestroyed()
        {
            Debug.LogWarning("旧的 AppleDestroyed() 方法被调用了，请确保逻辑已迁移到 HandleAppleFell()");
            // 可以在这里调用新的逻辑作为兼容
            // HandleAppleFell("Apple"); // 假设旧调用代表普通苹果掉落
        }

        /*public void AppleDestroyed()
        {
            //销毁所有下落中的苹果
            GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
            foreach (GameObject tGO in tAppleArray)
            {
                Destroy(tGO);
            }

            // 销毁一个篮筐
            // 获取basketList中最后一个篮筐的序号
            int basketIndex = basketList.Count - 1;
            // 取得对该篮筐的引用
            GameObject tBasketGO = basketList[basketIndex];
            //从列表中销毁该篮筐并销毁该游戏对象
            basketList.RemoveAt(basketIndex);
            Destroy(tBasketGO);

            // 如果没有篮筐剩余，重新开始游戏
            if (basketList.Count == 0)
            {
                // 读取本局是否破纪录的标记
                int newRecordAchieved = PlayerPrefs.GetInt("NewRecordFlag", 0);
                // 在重新加载场景前保存最终分数
                PlayerPrefs.SetInt("LastScore", Basket.currentScore);
                // 保存是否显示"New Record"消息的标记
                PlayerPrefs.SetInt("ShowNewRecordMessage", newRecordAchieved);
                // 使用 Basket 的静态分数
                PlayerPrefs.Save(); // 确保数据被写入

                // 重新开始游戏
                SceneManager.LoadScene("End_Scene_0"); 
            }
        }*/
  
}