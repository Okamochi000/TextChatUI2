using UnityEngine;

/// <summary>
/// セーフエリア分のオフセット拡張
/// </summary>
public class SafeAreaExpansion : SafeAreaBehaviour
{
    [SerializeField] private bool isTop = false;
    [SerializeField] private bool isBottom = false;
    [SerializeField] private bool isLeft = false;
    [SerializeField] private bool isRight = false;

    /// <summary>
    /// ノッジを更新する
    /// </summary>
    protected override void UpdateLayout()
    {
        if (!isTop && !isBottom && !isLeft & !isRight) { return; }

        Vector2 outsideOffsetMin = SafeAreaUtility.GetOutsideOffsetMin(this.transform);
        Vector2 outsideOffsetMax = SafeAreaUtility.GetOutsideOffsetMax(this.transform);
        RectTransform rectTransform = GetRectTransform();
        Vector2 offsetMin = rectTransform.offsetMin;
        Vector2 offsetMax = rectTransform.offsetMax;
        if (isTop) { offsetMax.y = -outsideOffsetMax.y; }
        if (isBottom) { offsetMin.y = -outsideOffsetMin.y; }
        if (isRight) { offsetMax.x = -outsideOffsetMax.x; }
        if (isLeft) { offsetMin.x = -outsideOffsetMin.x; }
        rectTransform.offsetMax = offsetMax;
        rectTransform.offsetMin = offsetMin;
    }
}
