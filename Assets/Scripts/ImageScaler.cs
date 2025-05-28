using UnityEngine;

public class ImageScaler : MonoBehaviour
{
    [Header("缩放设置")]
    [SerializeField] // 使用 SerializeField 可以在 Inspector 中设置私有变量
    [Tooltip("最小缩放比例 (例如 0.9 表示 90%)")]
    private float minScale = 0.95f;

    [SerializeField]
    [Tooltip("最大缩放比例 (例如 1.1 表示 110%)")]
    private float maxScale = 1.05f;

    [SerializeField]
    [Tooltip("缩放循环的速度 (值越大越快)")]
    private float speed = 1.5f;

    private Vector3 originalScale; // 存储对象的初始缩放值

    // Start is called before the first frame update
    void Start()
    {
        // 在开始时记录下对象的原始缩放值
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // 1. 计算一个在 0 和 1 之间平滑变化的因子 (使用 Sin 函数)
        // Mathf.Sin(Time.time * speed) 会在 -1 和 1 之间振荡
        // (Mathf.Sin(...) + 1f) 将范围变为 0 到 2
        // / 2f 将范围变为 0 到 1
        float scaleFactor = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

        // 2. 使用这个因子在 minScale 和 maxScale 之间进行插值
        // Lerp(a, b, t): 当 t=0 时返回 a，t=1 时返回 b，t=0.5 时返回 a 和 b 的中间值
        float targetScaleMultiplier = Mathf.Lerp(minScale, maxScale, scaleFactor);

        // 3. 将计算出的缩放乘数应用到对象的原始缩放上
        // 乘以 originalScale 可以确保如果原始缩放不是 (1,1,1)，也能按比例缩放
        transform.localScale = originalScale * targetScaleMultiplier;
    }

    // (可选) 如果你希望在禁用脚本时恢复原始大小
    void OnDisable()
    {
        // 检查 originalScale 是否已被初始化 (避免在编辑器模式下或 Start 未执行时出错)
        if (originalScale != Vector3.zero) // Vector3.zero 是 (0,0,0)，一个简单的初始化检查
        {
            transform.localScale = originalScale;
        }
    }

    // (可选) 如果你希望在对象被销毁时也可能需要恢复 (虽然通常没必要)
    // void OnDestroy() { ... }
}