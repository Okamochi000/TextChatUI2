using System;
using UnityEngine;

/// <summary>
/// �Z�[�t�G���A���C�A�E�g
/// </summary>
public class SafeAreaLayout : SafeAreaBehaviour
{
    private enum LayoutType
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public Action tempUpdatedCallback = null;

    [SerializeField] private VerticalOutsideLayout top = null;
    [SerializeField] private VerticalOutsideLayout bottom = null;
    [SerializeField] private HorizontalOutsideLayout left = null;
    [SerializeField] private HorizontalOutsideLayout right = null;
    [SerializeField] private bool isInvalidTop = false;
    [SerializeField] private bool isInvalidBottom = false;
    [SerializeField] private bool isInvalidLeft = false;
    [SerializeField] private bool isInvalidRight = false;

    private Vector2 prevTopSize_ = Vector2.zero;
    private Vector2 prevBottomSize_ = Vector2.zero;
    private Vector2 prevLeftSize_ = Vector2.zero;
    private Vector2 prevRightSize_ = Vector2.zero;

    protected override void Start()
    {
        UpdateLayoutLock();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        UpdateLayoutLock();
    }

    /// <summary>
    /// �m�b�W���X�V����
    /// </summary>
    protected override void UpdateLayout()
    {
        // �����ݒ�
        RectTransform selfRectTransform = GetRectTransform();
        selfRectTransform.pivot = new Vector2(0.5f, 0.5f);
        selfRectTransform.anchorMin = Vector2.zero;
        selfRectTransform.anchorMax = Vector2.one;
        selfRectTransform.offsetMin = Vector2.zero;
        selfRectTransform.offsetMax = Vector2.zero;

        // �X�P�[�����O
        Vector2 safeAreaOffsetMin = SafeAreaUtility.GetOutsideOffsetMin(this.transform);
        Vector2 safeAreaOffsetMax = SafeAreaUtility.GetOutsideOffsetMax(this.transform);
        Vector2 offsetMin = Vector2.zero;
        Vector2 offsetMax = Vector2.zero;

        // �g�b�v�ݒ�
        if (!isInvalidTop)
        {
            if (top == null)
            {
                offsetMax.y = safeAreaOffsetMax.y;
                prevTopSize_ = Vector2.zero;
            }
            else
            {
                SafeAreaBehaviour outsideLayout = top.GetComponent<SafeAreaBehaviour>();
                if (outsideLayout != null) { outsideLayout.UpdateLayoutLock(); }
                offsetMax.y = -top.GetRectTransform().sizeDelta.y;
                prevTopSize_ = top.GetRectTransform().sizeDelta;
            }
        }

        // �{�g���ݒ�
        if (!isInvalidBottom)
        {
            if (bottom == null)
            {
                offsetMin.y = safeAreaOffsetMin.y;
                prevBottomSize_ = Vector2.zero;
            }
            else
            {
                SafeAreaBehaviour outsideLayout = bottom.GetComponent<SafeAreaBehaviour>();
                if (outsideLayout != null) { outsideLayout.UpdateLayoutLock(); }
                offsetMin.y = bottom.GetRectTransform().sizeDelta.y;
                prevBottomSize_ = bottom.GetRectTransform().sizeDelta;
            }
        }

        // ���t�g�ݒ�
        if (!isInvalidLeft)
        {
            if (left == null)
            {
                offsetMin.x = safeAreaOffsetMin.x;
                prevLeftSize_ = Vector2.zero;
            }
            else
            {
                SafeAreaBehaviour outsideLayout = left.GetComponent<SafeAreaBehaviour>();
                if (outsideLayout != null) { outsideLayout.UpdateLayoutLock(); }
                offsetMin.x = left.GetRectTransform().sizeDelta.x;
                prevLeftSize_ = left.GetRectTransform().sizeDelta;
            }
        }

        // ���C�g�ݒ�
        if (!isInvalidRight)
        {
            if (right == null)
            {
                offsetMax.x = safeAreaOffsetMax.x;
                prevRightSize_ = Vector2.zero;
            }
            else
            {
                SafeAreaBehaviour outsideLayout = right.GetComponent<SafeAreaBehaviour>();
                if (outsideLayout != null) { outsideLayout.UpdateLayoutLock(); }
                offsetMax.x = -right.GetRectTransform().sizeDelta.x;
                prevRightSize_ = right.GetRectTransform().sizeDelta;
            }
        }

        // �X�V
        selfRectTransform.offsetMin = offsetMin;
        selfRectTransform.offsetMax = offsetMax;

        // �X�V�R�[���o�b�N�Ăяo��
        if (tempUpdatedCallback != null)
        {
            tempUpdatedCallback();
            tempUpdatedCallback = null;
        }
    }

    /// <summary>
    /// �X�V�����݂��邩
    /// </summary>
    /// <returns></returns>
    protected override bool IsExistUpdate()
    {
        if (base.IsExistUpdate()) { return true; }

        foreach (LayoutType layoutType in Enum.GetValues(typeof(LayoutType)))
        {
            if (IsExistUpdate(layoutType)) { return true; }
        }

        return false;
    }

    /// <summary>
    /// �X�V�����݂��邩
    /// </summary>
    /// <param name="layoutType"></param>
    /// <returns></returns>
    private bool IsExistUpdate(LayoutType layoutType)
    {
        switch (layoutType)
        {
            case LayoutType.Top:
                if (isInvalidTop || IsExistUpdateParts(top, prevTopSize_)) { return true; }
                break;
            case LayoutType.Bottom:
                if (isInvalidBottom || IsExistUpdateParts(bottom, prevBottomSize_)) { return true; }
                break;
            case LayoutType.Left:
                if (isInvalidLeft || IsExistUpdateParts(left, prevLeftSize_)) { return true; }
                break;
            case LayoutType.Right:
                if (isInvalidRight || IsExistUpdateParts(right, prevRightSize_)) { return true; }
                break;
            default: break;
        }

        return false;
    }

    /// <summary>
    /// �e���ʂ̍X�V�����݂��邩
    /// </summary>
    /// <param name="layoutBase"></param>
    /// <param name="prevSize"></param>
    /// <returns></returns>
    private bool IsExistUpdateParts(SafeAreaBehaviour layoutBase, Vector2 prevSize)
    {
        if (layoutBase == null && prevSize != Vector2.zero) { return true; }
        if (layoutBase != null && prevSize != layoutBase.GetRectTransform().sizeDelta) { return true; }

        return false;
    }
}
