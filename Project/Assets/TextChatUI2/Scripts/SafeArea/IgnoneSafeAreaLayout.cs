using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// セーフエリア無視レイアウト
/// </summary>
public class IgnoneSafeAreaLayout : SafeAreaBehaviour
{
    [SerializeField] private bool isTopSafeArea = false;
    [SerializeField] private bool isBottomSafeArea = false;
    [SerializeField] private bool isLeftSafeArea = false;
    [SerializeField] private bool isRightSafeArea = false;

    private RectTransform canvasRectTransform_ = null;
    private bool isSafeAreaLayout_ = false;

    protected override void OnEnable()
    {
        CanvasScaler canvasScaler = SafeAreaUtility.GetParentCanvasScaler(this.transform);
        if (canvasScaler != null) { canvasRectTransform_ = canvasScaler.GetComponent<RectTransform>(); }
        UpdateLayoutLock();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        if (IsUpdating) { return; }
        if (isSafeAreaLayout_) { return; }

        SafeAreaLayout safeAreaLayout = GetSafeAreaLayout(this.transform);
        if (safeAreaLayout != null && safeAreaLayout.IsUpdating)
        {
            isSafeAreaLayout_ = true;
            safeAreaLayout.tempUpdatedCallback += UpdateLayoutLock;
        }
        else
        {
            CanvasScaler canvasScaler = SafeAreaUtility.GetParentCanvasScaler(this.transform);
            if (canvasScaler != null) { canvasRectTransform_ = canvasScaler.GetComponent<RectTransform>(); }
            UpdateLayoutLock();
        }
    }

    /// <summary>
    /// ノッジを更新する
    /// </summary>
    protected override void UpdateLayout()
    {
        if (canvasRectTransform_ == null) { return; }

        isSafeAreaLayout_ = false;

        // アンカー設定
        RectTransform selfRectTransform = GetRectTransform();
        selfRectTransform.pivot = new Vector2(0.5f, 0.5f);
        selfRectTransform.anchorMin = Vector2.zero;
        selfRectTransform.anchorMax = Vector2.one;

        // サイズ設定
        Vector2 sizeDelta = canvasRectTransform_.sizeDelta;
        Vector2 outsideOffsetMin = SafeAreaUtility.GetOutsideOffsetMin(this.transform);
        Vector2 outsideOffsetMax = SafeAreaUtility.GetOutsideOffsetMax(this.transform);
        if (isTopSafeArea) { sizeDelta.y += outsideOffsetMax.y; }
        if (isBottomSafeArea) { sizeDelta.y -= outsideOffsetMin.y; }
        if (isRightSafeArea) { sizeDelta.x += outsideOffsetMax.x; }
        if (isLeftSafeArea) { sizeDelta.x -= outsideOffsetMin.x; }
        selfRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeDelta.x);
        selfRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeDelta.y);

        // 座標設定
        Vector3 centerPosition = canvasRectTransform_.position;
        if (isTopSafeArea) { centerPosition.y += (outsideOffsetMax.y / 2.0f); }
        if (isBottomSafeArea) { centerPosition.y += (outsideOffsetMin.y / 2.0f); }
        if (isRightSafeArea) { centerPosition.x += (outsideOffsetMax.x / 2.0f); }
        if (isLeftSafeArea) { centerPosition.x += (outsideOffsetMin.x / 2.0f); }
        selfRectTransform.position = centerPosition;
    }

    /// <summary>
    /// 親SafeAreaLayoutを取得する
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    private SafeAreaLayout GetSafeAreaLayout(Transform transform)
    {
        if (transform.parent == null) { return null; }

        SafeAreaLayout safeAreaLayout = transform.parent.GetComponent<SafeAreaLayout>();
        if (safeAreaLayout == null) { return GetSafeAreaLayout(transform.parent); }
        else { return safeAreaLayout; }
    }
}
