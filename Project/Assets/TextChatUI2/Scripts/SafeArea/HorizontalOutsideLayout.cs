using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 横セーフエリア外レイアウト
/// </summary>
[RequireComponent(typeof(HorizontalLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
public class HorizontalOutsideLayout : SafeAreaBehaviour
{
    /// <summary>
    /// レイアウトタイプ
    /// </summary>
    [System.Serializable]
    private enum LayoutType
    {
        Left,
        Right
    }

    [SerializeField] private RectTransform outside = null;
    [SerializeField] private bool isVerticalSafeArea = false;
    [SerializeField] private LayoutType layoutType = LayoutType.Left;

    /// <summary>
    /// レイアウト更新
    /// </summary>
    protected override void UpdateLayout()
    {
        // 初期設定
        RectTransform selfRectTransform = GetRectTransform();
        Vector2 offsetMin = Vector2.zero;
        Vector2 offsetMax = Vector2.zero;
        if (layoutType == LayoutType.Left)
        {
            selfRectTransform.pivot = new Vector2(0.0f, 0.5f);
            selfRectTransform.anchorMin = new Vector2(0.0f, 0.0f);
            selfRectTransform.anchorMax = new Vector2(0.0f, 1.0f);
        }
        else
        {
            selfRectTransform.pivot = new Vector2(1.0f, 0.5f);
            selfRectTransform.anchorMin = new Vector2(1.0f, 0.0f);
            selfRectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        }

        // 横幅をセーフエリア内にする
        Vector2 outsideOffsetMin = SafeAreaUtility.GetOutsideOffsetMin(this.transform);
        Vector2 outsideOffsetMax = SafeAreaUtility.GetOutsideOffsetMax(this.transform);
        if (isVerticalSafeArea)
        {
            offsetMin.y = outsideOffsetMin.y;
            offsetMax.y = outsideOffsetMax.y;
        }

        // Offset更新
        selfRectTransform.offsetMin = offsetMin;
        selfRectTransform.offsetMax = offsetMax;

        // セーフエリア外更新
        if (outside != null)
        {
            Vector2 sizeDelta = selfRectTransform.sizeDelta;
            if (layoutType == LayoutType.Left) { sizeDelta.x = outsideOffsetMin.x ; }
            else { sizeDelta.x = -outsideOffsetMax.x; }
            outside.sizeDelta = sizeDelta;
        }

        // レイアウト更新
        HorizontalLayoutGroup layoutGroup = this.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childControlHeight = true;
        layoutGroup.childScaleHeight = false;
        layoutGroup.childForceExpandHeight = true;
        layoutGroup.SetLayoutHorizontal();
        layoutGroup.SetLayoutVertical();
        layoutGroup.CalculateLayoutInputHorizontal();
        layoutGroup.CalculateLayoutInputVertical();
        ContentSizeFitter sizeFitter = this.GetComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        sizeFitter.SetLayoutHorizontal();
        sizeFitter.SetLayoutVertical();
    }
}
