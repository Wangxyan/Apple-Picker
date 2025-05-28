using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Automatic Swing Settings")]
    [Tooltip("向左或向右自动摇摆的最大距离")]
    public float swingDistance = 0.5f; // 自动摇摆幅度

    [Tooltip("完成一次完整自动来回摇摆所需的时间（秒）")]
    public float swingPeriod = 10.0f; // 自动摇摆速度

    [Header("Mouse Parallax Settings")]
    [Tooltip("鼠标在屏幕边缘时，背景在X轴上反向移动的最大距离")]
    public float parallaxStrengthX = 1.0f; // 鼠标影响X轴幅度

    [Tooltip("鼠标在屏幕边缘时，背景在Y轴上反向移动的最大距离")]
    public float parallaxStrengthY = 0.75f; // 鼠标影响Y轴幅度

    [Tooltip("鼠标影响的非线性曲线。X轴代表鼠标距中心的归一化距离(0=中心, 1=边缘)，Y轴代表实际偏移比例(0=无偏移, 1=最大偏移)")]
    public AnimationCurve parallaxCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 默认缓入缓出

    private Vector3 startPosition; // 记录背景的初始位置
    private float startTime; // 记录自动摇摆的开始时间

    void Start()
    {
        // 记录初始位置，所有运动都基于此
        startPosition = transform.position;
        // 记录自动摇摆的开始时间
        startTime = Time.time;
    }

    void Update()
    {
        // --- 1. 计算自动摇摆偏移 (仅 X 轴) ---
        float swingOffsetX = 0f;
        if (swingPeriod > 0) // 避免除零
        {
            float cyclePosition = ((Time.time - startTime) / swingPeriod) * 2 * Mathf.PI;
            swingOffsetX = Mathf.Sin(cyclePosition) * swingDistance;
        }

        // --- 2. 计算鼠标视差偏移 (X 和 Y 轴) ---
        // 获取鼠标在屏幕上的位置 (0,0 在左下角)
        Vector2 mouseScreenPos = Input.mousePosition;

        // 获取屏幕中心点
        Vector2 screenCenter = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        // 计算鼠标位置相对于屏幕中心的偏移量 (范围在 -屏幕尺寸/2 到 +屏幕尺寸/2)
        Vector2 mouseOffsetFromCenter = mouseScreenPos - screenCenter;

        // 将偏移量归一化到 -1 到 1 的范围
        // 注意：如果鼠标移出屏幕，这个值可能超过-1或1，效果会延伸
        float normalizedOffsetX = mouseOffsetFromCenter.x / screenCenter.x; // screenCenter.x 就是 Screen.width / 2
        float normalizedOffsetY = mouseOffsetFromCenter.y / screenCenter.y; // screenCenter.y 就是 Screen.height / 2

        // 使用 AnimationCurve 计算非线性因子 (输入需要是 0 到 1，所以用绝对值)
        // Evaluate 的输入是时间（这里我们用归一化距离的绝对值模拟）
        // Evaluate 的输出是 0 到 1 的比例因子
        float curveFactorX = parallaxCurve.Evaluate(Mathf.Abs(normalizedOffsetX));
        float curveFactorY = parallaxCurve.Evaluate(Mathf.Abs(normalizedOffsetY));

        // 计算最终的视差偏移量
        // 乘以强度，乘以曲线因子，乘以归一化偏移的方向 (Mathf.Sign) 实现反向
        // 使用 -Mathf.Sign 来获得反向效果
        float parallaxOffsetX = -Mathf.Sign(normalizedOffsetX) * curveFactorX * parallaxStrengthX;
        float parallaxOffsetY = -Mathf.Sign(normalizedOffsetY) * curveFactorY * parallaxStrengthY;

        // --- 3. 合并偏移量并计算最终位置 ---
        float finalX = startPosition.x + swingOffsetX + parallaxOffsetX;
        float finalY = startPosition.y + parallaxOffsetY; // Y轴只有鼠标视差影响

        // --- 4. 更新背景位置 ---
        transform.position = new Vector3(finalX, finalY, startPosition.z); // 保持原始Z坐标
    }
}