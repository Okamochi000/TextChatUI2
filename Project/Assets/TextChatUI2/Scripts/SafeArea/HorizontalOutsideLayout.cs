using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���Z�[�t�G���A�O���C�A�E�g
/// </summary>
[RequireComponent(typeof(HorizontalLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
public class HorizontalOutsideLayout : SafeAreaBehaviour
{
    /// <summary>
    /// ���C�A�E�g�^�C�v
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
    /// ���C�A�E�g�X�V
    /// </summary>
    protected override void UpdateLayout()
    {
        // �����ݒ�
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

        // �������Z�[�t�G���A���ɂ���
        Vector2 outsideOffsetMin = SafeAreaUtility.GetOutsideOffsetMin(this.transform);
        Vector2 outsideOffsetMax = SafeAreaUtility.GetOutsideOffsetMax(this.transform);
        if (isVerticalSafeArea)
        {
            offsetMin.y = outsideOffsetMin.y;
            offsetMax.y = outsideOffsetMax.y;
        }

        // Offset�X�V
        selfRectTransform.offsetMin = offsetMin;
        selfRectTransform.offsetMax = offsetMax;

        // �Z�[�t�G���A�O�X�V
        if (outside != null)
        {
            Vector2 sizeDelta = selfRectTransform.sizeDelta;
            if (layoutType == LayoutType.Left) { sizeDelta.x = outsideOffsetMin.x ; }
            else { sizeDelta.x = -outsideOffsetMax.x; }
            outside.sizeDelta = sizeDelta;
        }

        // ���C�A�E�g�X�V
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
