using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Automatic Swing Settings")]
    [Tooltip("����������Զ�ҡ�ڵ�������")]
    public float swingDistance = 0.5f; // �Զ�ҡ�ڷ���

    [Tooltip("���һ�������Զ�����ҡ�������ʱ�䣨�룩")]
    public float swingPeriod = 10.0f; // �Զ�ҡ���ٶ�

    [Header("Mouse Parallax Settings")]
    [Tooltip("�������Ļ��Եʱ��������X���Ϸ����ƶ���������")]
    public float parallaxStrengthX = 1.0f; // ���Ӱ��X�����

    [Tooltip("�������Ļ��Եʱ��������Y���Ϸ����ƶ���������")]
    public float parallaxStrengthY = 0.75f; // ���Ӱ��Y�����

    [Tooltip("���Ӱ��ķ��������ߡ�X������������ĵĹ�һ������(0=����, 1=��Ե)��Y�����ʵ��ƫ�Ʊ���(0=��ƫ��, 1=���ƫ��)")]
    public AnimationCurve parallaxCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Ĭ�ϻ��뻺��

    private Vector3 startPosition; // ��¼�����ĳ�ʼλ��
    private float startTime; // ��¼�Զ�ҡ�ڵĿ�ʼʱ��

    void Start()
    {
        // ��¼��ʼλ�ã������˶������ڴ�
        startPosition = transform.position;
        // ��¼�Զ�ҡ�ڵĿ�ʼʱ��
        startTime = Time.time;
    }

    void Update()
    {
        // --- 1. �����Զ�ҡ��ƫ�� (�� X ��) ---
        float swingOffsetX = 0f;
        if (swingPeriod > 0) // �������
        {
            float cyclePosition = ((Time.time - startTime) / swingPeriod) * 2 * Mathf.PI;
            swingOffsetX = Mathf.Sin(cyclePosition) * swingDistance;
        }

        // --- 2. ��������Ӳ�ƫ�� (X �� Y ��) ---
        // ��ȡ�������Ļ�ϵ�λ�� (0,0 �����½�)
        Vector2 mouseScreenPos = Input.mousePosition;

        // ��ȡ��Ļ���ĵ�
        Vector2 screenCenter = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        // �������λ���������Ļ���ĵ�ƫ���� (��Χ�� -��Ļ�ߴ�/2 �� +��Ļ�ߴ�/2)
        Vector2 mouseOffsetFromCenter = mouseScreenPos - screenCenter;

        // ��ƫ������һ���� -1 �� 1 �ķ�Χ
        // ע�⣺�������Ƴ���Ļ�����ֵ���ܳ���-1��1��Ч��������
        float normalizedOffsetX = mouseOffsetFromCenter.x / screenCenter.x; // screenCenter.x ���� Screen.width / 2
        float normalizedOffsetY = mouseOffsetFromCenter.y / screenCenter.y; // screenCenter.y ���� Screen.height / 2

        // ʹ�� AnimationCurve ������������� (������Ҫ�� 0 �� 1�������þ���ֵ)
        // Evaluate ��������ʱ�䣨���������ù�һ������ľ���ֵģ�⣩
        // Evaluate ������� 0 �� 1 �ı�������
        float curveFactorX = parallaxCurve.Evaluate(Mathf.Abs(normalizedOffsetX));
        float curveFactorY = parallaxCurve.Evaluate(Mathf.Abs(normalizedOffsetY));

        // �������յ��Ӳ�ƫ����
        // ����ǿ�ȣ������������ӣ����Թ�һ��ƫ�Ƶķ��� (Mathf.Sign) ʵ�ַ���
        // ʹ�� -Mathf.Sign ����÷���Ч��
        float parallaxOffsetX = -Mathf.Sign(normalizedOffsetX) * curveFactorX * parallaxStrengthX;
        float parallaxOffsetY = -Mathf.Sign(normalizedOffsetY) * curveFactorY * parallaxStrengthY;

        // --- 3. �ϲ�ƫ��������������λ�� ---
        float finalX = startPosition.x + swingOffsetX + parallaxOffsetX;
        float finalY = startPosition.y + parallaxOffsetY; // Y��ֻ������Ӳ�Ӱ��

        // --- 4. ���±���λ�� ---
        transform.position = new Vector3(finalX, finalY, startPosition.z); // ����ԭʼZ����
    }
}