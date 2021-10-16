using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// セーフエリアの汎用処理
/// </summary>
public static class SafeAreaUtility
{
    /// <summary>
    /// セーフエリアがあるか
    /// </summary>
    /// <returns></returns>
    public static bool IsSafeArea()
    {
        if (Screen.cutouts.Length > 0) { return true; }

        Rect area = Screen.safeArea;
        if (area.xMin > 0.0f || area.yMin > 0.0f) { return true; }
        if (area.xMax != area.width || area.yMax != area.height) { return true; }

        return false;
    }

    /// <summary>
    /// 現在のセーフエリアと一致するか
    /// </summary>
    /// <param name="safeArea"></param>
    /// <returns></returns>
    public static bool IsMatchSafeArea(Rect safeArea)
    {
        Rect currentSafeArea = Screen.safeArea;
        if (currentSafeArea.xMin != safeArea.xMin || currentSafeArea.yMin != safeArea.yMin) { return false; }
        if (currentSafeArea.width != safeArea.width || currentSafeArea.height != safeArea.height) { return false; }

        return true;
    }

    /// <summary>
    /// キャンバスのスケール値を取得する
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static float GetCanvasScale(Transform transform)
    {
        CanvasScaler canvasScaler = GetParentCanvasScaler(transform);
        if (canvasScaler != null && canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
        {
            Resolution resolution = Screen.currentResolution;
            float scale = canvasScaler.referenceResolution.y / resolution.height;
            return scale;
        }

        return 1.0f;
    }

    /// <summary>
    /// 親キャンバスを取得する
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static CanvasScaler GetParentCanvasScaler(Transform transform)
    {
        if (transform.parent == null) { return null; }

        CanvasScaler canvas = transform.parent.GetComponent<CanvasScaler>();
        if (canvas == null) { return GetParentCanvasScaler(transform.parent); }
        else { return canvas; }
    }

    /// <summary>
    /// セーフエリア外取得
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="offsetMin"></param>
    /// <param name="offsetMax"></param>
    public static void GetOutsideOffset(Transform transform, out Vector2 offsetMin, out Vector2 offsetMax)
    {
        offsetMin = GetOutsideOffsetMin(transform);
        offsetMax = GetOutsideOffsetMax(transform);
    }

    /// <summary>
    /// セーフエリア外取得(ボトム、レフト)
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static Vector2 GetOutsideOffsetMin(Transform transform)
    {
        if (!SafeAreaUtility.IsSafeArea()) { return Vector2.zero; }

        Resolution resolution = Screen.currentResolution;
        Rect area = Screen.safeArea;
        float scale = GetCanvasScale(transform);
        Vector2 offsetMin = Vector2.zero;
        offsetMin.y = area.yMin * scale;
        offsetMin.x = area.xMin * scale;

        return offsetMin;
    }

    /// <summary>
    /// セーフエリア外取得(トップ、ライト)
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static Vector2 GetOutsideOffsetMax(Transform transform)
    {
        if (!SafeAreaUtility.IsSafeArea()) { return Vector2.zero; }

        Resolution resolution = Screen.currentResolution;
        Rect area = Screen.safeArea;
        float scale = SafeAreaUtility.GetCanvasScale(transform);
        Vector2 offsetMax = Vector2.zero;
        offsetMax.y = (area.yMax - resolution.height) * scale;
        offsetMax.x = (area.xMax - resolution.width) * scale;

        return offsetMax;
    }
}
