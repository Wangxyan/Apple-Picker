using UnityEngine;

public class ImageScaler : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] // ʹ�� SerializeField ������ Inspector ������˽�б���
    [Tooltip("��С���ű��� (���� 0.9 ��ʾ 90%)")]
    private float minScale = 0.95f;

    [SerializeField]
    [Tooltip("������ű��� (���� 1.1 ��ʾ 110%)")]
    private float maxScale = 1.05f;

    [SerializeField]
    [Tooltip("����ѭ�����ٶ� (ֵԽ��Խ��)")]
    private float speed = 1.5f;

    private Vector3 originalScale; // �洢����ĳ�ʼ����ֵ

    // Start is called before the first frame update
    void Start()
    {
        // �ڿ�ʼʱ��¼�¶����ԭʼ����ֵ
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // 1. ����һ���� 0 �� 1 ֮��ƽ���仯������ (ʹ�� Sin ����)
        // Mathf.Sin(Time.time * speed) ���� -1 �� 1 ֮����
        // (Mathf.Sin(...) + 1f) ����Χ��Ϊ 0 �� 2
        // / 2f ����Χ��Ϊ 0 �� 1
        float scaleFactor = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

        // 2. ʹ����������� minScale �� maxScale ֮����в�ֵ
        // Lerp(a, b, t): �� t=0 ʱ���� a��t=1 ʱ���� b��t=0.5 ʱ���� a �� b ���м�ֵ
        float targetScaleMultiplier = Mathf.Lerp(minScale, maxScale, scaleFactor);

        // 3. ������������ų���Ӧ�õ������ԭʼ������
        // ���� originalScale ����ȷ�����ԭʼ���Ų��� (1,1,1)��Ҳ�ܰ���������
        transform.localScale = originalScale * targetScaleMultiplier;
    }

    // (��ѡ) �����ϣ���ڽ��ýű�ʱ�ָ�ԭʼ��С
    void OnDisable()
    {
        // ��� originalScale �Ƿ��ѱ���ʼ�� (�����ڱ༭��ģʽ�»� Start δִ��ʱ����)
        if (originalScale != Vector3.zero) // Vector3.zero �� (0,0,0)��һ���򵥵ĳ�ʼ�����
        {
            transform.localScale = originalScale;
        }
    }

    // (��ѡ) �����ϣ���ڶ�������ʱҲ������Ҫ�ָ� (��Ȼͨ��û��Ҫ)
    // void OnDestroy() { ... }
}