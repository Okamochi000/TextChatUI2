using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �c�Z�[�t�G���A�O���C�A�E�g
/// </summary>
[RequireComponent(typeof(VerticalLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
public class VerticalOutsideLayout : SafeAreaBehaviour
{
    /// <summary>
    /// ���C�A�E�g�^�C�v
    /// </summary>
    [System.Serializable]
    private enum LayoutType
    {
        Top,
        Bottom
    }

    [SerializeField] private RectTransform outside = null;
    [SerializeField] private bool isHorizontalSafeArea = false;
    [SerializeField] private LayoutType layoutType = LayoutType.Top;

    /// <summary>
    /// ���C�A�E�g�X�V
    /// </summary>
    protected override void UpdateLayout()
    {
        // �����ݒ�
        RectTransform selfRectTransform = GetRectTransform();
        Vector2 offsetMin = Vector2.zero;
        Vector2 offsetMax = Vector2.zero;
        if (layoutType == LayoutType.Top)
        {
            selfRectTransform.pivot = new Vector2(0.5f, 1.0f);
            selfRectTransform.anchorMin = new Vector2(0.0f, 1.0f);
            selfRectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        }
        else
        {
            selfRectTransform.pivot = new Vector2(0.5f, 0.0f);
            selfRectTransform.anchorMin = new Vector2(0.0f, 0.0f);
            selfRectTransform.anchorMax = new Vector2(1.0f, 0.0f);
        }

        // �������Z�[�t�G���A���ɂ���
        Vector2 outsideOffsetMin = SafeAreaUtility.GetOutsideOffsetMin(this.transform);
        Vector2 outsideOffsetMax = SafeAreaUtility.GetOutsideOffsetMax(this.transform);
        if (isHorizontalSafeArea)
        {
            offsetMin.x = outsideOffsetMin.x;
            offsetMax.x = outsideOffsetMax.x;
        }

        // Offset�X�V
        selfRectTransform.offsetMin = offsetMin;
        selfRectTransform.offsetMax = offsetMax;

        // �Z�[�t�G���A�O�X�V
        if (outside != null)
        {
            Vector2 sizeDelta = selfRectTransform.sizeDelta;
            if (layoutType == LayoutType.Top) { sizeDelta.y = -outsideOffsetMax.y; }
            else { sizeDelta.y = outsideOffsetMin.y; }
            outside.sizeDelta = sizeDelta;
        }

        // ���C�A�E�g�X�V
        VerticalLayoutGroup layoutGroup = this.GetComponent<VerticalLayoutGroup>();
        layoutGroup.childControlWidth = true;
        layoutGroup.childScaleWidth = false;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.SetLayoutHorizontal();
        layoutGroup.SetLayoutVertical();
        layoutGroup.CalculateLayoutInputHorizontal();
        layoutGroup.CalculateLayoutInputVertical();
        ContentSizeFitter sizeFitter = this.GetComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.SetLayoutHorizontal();
        sizeFitter.SetLayoutVertical();
    }
}
