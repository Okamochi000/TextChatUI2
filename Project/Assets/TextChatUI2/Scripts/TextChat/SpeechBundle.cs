using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����o��
/// </summary>
public class SpeechBundle : MonoBehaviour
{
    [SerializeField] private Text text = null;

    /// <summary>
    /// �e�L�X�g��ݒ肷��
    /// </summary>
    /// <param name="message"></param>
    public void SetText(string message)
    {
        text.text = message;
    }

    /// <summary>
    /// ���C�A�E�g�X�V
    /// </summary>
    public void UpdateLayout()
    {
        HorizontalOrVerticalLayoutGroup layoutGroup = this.GetComponent<HorizontalOrVerticalLayoutGroup>();
        if (layoutGroup != null)
        {
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();
        }
        ContentSizeFitter sizeFitter = this.GetComponent<ContentSizeFitter>();
        if (sizeFitter != null)
        {
            sizeFitter.SetLayoutHorizontal();
            sizeFitter.SetLayoutVertical();
        }
    }
}
