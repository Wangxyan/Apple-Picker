using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public static float bottomY = -20f;
    private static ApplePicker apScript = null; // 静态缓存引用

    void Start()
    {
        // 在第一个苹果的 Start 中查找一次 ApplePicker
        if (apScript == null)
        {
            apScript = Camera.main.GetComponent<ApplePicker>();
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
            Destroy(this.gameObject);
        }
    }
}