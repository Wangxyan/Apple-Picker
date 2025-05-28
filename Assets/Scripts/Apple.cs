using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public static float bottomY = -20f;
    private static ApplePicker apScript = null; // ��̬��������

    void Start()
    {
        // �ڵ�һ��ƻ���� Start �в���һ�� ApplePicker
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