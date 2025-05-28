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

            //��ȡ�����������ApplePicker���������
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();  
            // ����apScript��AppleDestroyed����
            apScript.AppleDestroyed();
        }
    }
}*/
public class Apple : MonoBehaviour
{
    public static float bottomY = -20f;
    private static ApplePicker apScript = null; // ��̬��������

    void Start()
    {
        // �ڵ�һ��ƻ���� Start �в���һ�� ApplePicker
        if (apScript == null)
        {
            if (Camera.main != null)
            {
                apScript = Camera.main.GetComponent<ApplePicker>();
                if (apScript == null)
                {
                    Debug.LogError("Apple �޷��� Main Camera ���ҵ� ApplePicker �ű�!", Camera.main.gameObject);
                }
            }
            else
            {
                Debug.LogError("Apple �޷��ҵ� Main Camera!");
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
                Debug.LogError("ApplePicker �ű����ö�ʧ���޷�����ƻ�����䣡");
            }
            Destroy(this.gameObject);
        }
    }
}