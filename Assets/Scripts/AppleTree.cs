using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    // 用来初始化苹果实例的预设
    public GameObject applePrefab;
    public GameObject poisonApplePrefab; // 新增：毒苹果预设
    public GameObject goldenApplePrefab; // 新增：金苹果预设

    // 苹果树移动的速度
    public float speed = 1f;
    // 苹果树的活动区域，到达边界时则改变方向
    public float leftAndRightEdge = 10f;
    // 苹果树改变方向的概率
    public float chanceToChangeDirections = 0.1f;
    // 苹果出现的时间间隔
    public float secondsBetweenAppleDrops = 1f;
    // 首个苹果掉落时间
    public float secondsFirstAppleDrops = 2f;

    // 新增：特殊苹果出现概率 (0.0 to 1.0)
    public float chanceToSpawnPoison = 0.1f; // 10%
    public float chanceToSpawnGolden = 0.05f; // 5%

    void Start()
    {
        //每秒掉落一个苹果
        Invoke("DropApple", secondsFirstAppleDrops);
    }
    void DropApple()
    {
        GameObject prefabToDrop; // 用来决定实例化预设
        float randomValue = Random.value; // 获取一个 0.0 到 1.0 的随机数
        if (randomValue < chanceToSpawnGolden) // 检查金苹果概率
        {
            prefabToDrop = goldenApplePrefab;
        }
        else if (randomValue < chanceToSpawnGolden + chanceToSpawnPoison) // 检查毒苹果概率
        {
            prefabToDrop = poisonApplePrefab;
        }
        else // 剩余概率生成普通苹果
        {
            prefabToDrop = applePrefab;
        }

        GameObject apple = Instantiate<GameObject>(prefabToDrop);
        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);
    }


    void Update()
    {
        // 基本运动
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        // 改变方向
        if (pos.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed); //向右运动
        }
        else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed); //向左运动
        }
    }

    void FixedUpdate()
    {
        //随机改变运动方向
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1; // 改变方向
        }
    }
}