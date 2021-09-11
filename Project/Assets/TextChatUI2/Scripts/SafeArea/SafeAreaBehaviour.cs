using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// �Z�[�t�G���A�x�[�X
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
        // �X�V�����邩
        bool isUpdate = IsExistUpdate();

        // �C���X�y�N�^�[�X�V�t���O��߂�
        isChangedValidate_ = false;

        // �Z�[�t�G���A�X�V
        prevSafeArea_ = Screen.safeArea;

        // ���C�A�E�g�X�V
        if (isUpdate) { UpdateLayoutLock(); }
    }

    protected override void Awake()
    {
        base.Awake();
        UpdateLayoutLock();
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^�[�ύX���m
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
        isChangedValidate_ = true;
    }
#endif

    /// <summary>
    /// ���b�N�������ă��C�A�E�g���X�V����
    /// </summary>
    public void UpdateLayoutLock()
    {
        // ���b�N���͖���
        if (isLock_) { return; }

        // ���b�N
        isLock_ = true;

        // �C���X�y�N�^�[�X�V�t���O��߂�
        isChangedValidate_ = false;

        // �Z�[�t�G���A�X�V
        prevSafeArea_ = Screen.safeArea;

        // ���C�A�E�g�X�V
        UpdateLayout();

        // ���b�N����
        isLock_ = false;
    }

    /// <summary>
    /// RectTransform�擾
    /// </summary>
    /// <returns></returns>
    public RectTransform GetRectTransform()
    {
        if (selfRectTransform_ == null) { selfRectTransform_ = this.GetComponent<RectTransform>(); }
        return selfRectTransform_;
    }

    /// <summary>
    /// �m�b�W���X�V����
    /// </summary>
    protected virtual void UpdateLayout() { }

    /// <summary>
    /// �X�V�����݂��邩
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsExistUpdate()
    {
        bool isUpdate = isChangedValidate_;
        if (!isUpdate) { isUpdate = !SafeAreaUtility.IsMatchSafeArea(prevSafeArea_); }

        return isUpdate;
    }
}
