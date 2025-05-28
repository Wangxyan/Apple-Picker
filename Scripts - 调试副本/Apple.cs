using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Apple : MonoBehaviour
{
    public static float bottomY = -20f;

    void Update()
    {
        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject);

            //获取对主摄像机的ApplePicker组件的引用
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();  
            // 调用apScript的AppleDestroyed方法
            apScript.AppleDestroyed();
        }
    }
}*/
public class Apple : MonoBehaviour
{
    public static float bottomY = -20f;
    private static ApplePicker apScript = null; // 静态缓存引用

    void Start()
    {
        // 在第一个苹果的 Start 中查找一次 ApplePicker
        if (apScript == null)
        {
            if (Camera.main != null)
            {
                apScript = Camera.main.GetComponent<ApplePicker>();
                if (apScript == null)
                {
                    Debug.LogError("Apple 无法从 Main Camera 上找到 ApplePicker 脚本!", Camera.main.gameObject);
                }
            }
            else
            {
                Debug.LogError("Apple 无法找到 Main Camera!");
            }
        }
    }

    void Update()
    {
        if (transform.position.y < bottomY)
        {
            if (apScript != null)
            {
                apScript.HandleAppleFell(this.gameObject);
            }
            else
            {
                Debug.LogError("ApplePicker 脚本引用丢失，无法处理苹果掉落！");
            }
            Destroy(this.gameObject);
        }
    }
}