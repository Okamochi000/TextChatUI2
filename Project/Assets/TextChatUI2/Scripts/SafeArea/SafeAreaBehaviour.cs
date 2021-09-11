using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// セーフエリアベース
/// </summary>
[DisallowMultipleComponent]
[ExecuteAlways]
public class SafeAreaBehaviour : UIBehaviour
{
    public bool IsUpdating { get { return isLock_; } }

    private RectTransform selfRectTransform_ = null;
    private Rect prevSafeArea_ = new Rect();
    private bool isChangedValidate_ = false;
    private bool isLock_ = false;

    // Update is called once per frame
    protected void Update()
    {
        // 更新があるか
        bool isUpdate = IsExistUpdate();

        // インスペクター更新フラグを戻す
        isChangedValidate_ = false;

        // セーフエリア更新
        prevSafeArea_ = Screen.safeArea;

        // レイアウト更新
        if (isUpdate) { UpdateLayoutLock(); }
    }

    protected override void Awake()
    {
        base.Awake();
        UpdateLayoutLock();
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクター変更検知
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
        isChangedValidate_ = true;
    }
#endif

    /// <summary>
    /// ロックをかけてレイアウトを更新する
    /// </summary>
    public void UpdateLayoutLock()
    {
        // ロック中は無視
        if (isLock_) { return; }

        // ロック
        isLock_ = true;

        // インスペクター更新フラグを戻す
        isChangedValidate_ = false;

        // セーフエリア更新
        prevSafeArea_ = Screen.safeArea;

        // レイアウト更新
        UpdateLayout();

        // ロック解除
        isLock_ = false;
    }

    /// <summary>
    /// RectTransform取得
    /// </summary>
    /// <returns></returns>
    public RectTransform GetRectTransform()
    {
        if (selfRectTransform_ == null) { selfRectTransform_ = this.GetComponent<RectTransform>(); }
        return selfRectTransform_;
    }

    /// <summary>
    /// ノッジを更新する
    /// </summary>
    protected virtual void UpdateLayout() { }

    /// <summary>
    /// 更新が存在するか
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsExistUpdate()
    {
        bool isUpdate = isChangedValidate_;
        if (!isUpdate) { isUpdate = !SafeAreaUtility.IsMatchSafeArea(prevSafeArea_); }

        return isUpdate;
    }
}
