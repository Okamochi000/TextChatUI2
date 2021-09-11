using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �e�L�X�g�`���b�g�̃��b�Z�[�W�\����
/// </summary>
public class TextChatBody : MonoBehaviour
{
    /// <summary>
    /// �R�����g�̎��
    /// </summary>
    public enum CommentType
    {
        Mine,       // ���g
        Opponent    // ����
    }

    [SerializeField] private ScrollRect scrollRect = null;
    [SerializeField] private GameObject myComment = null;
    [SerializeField] private GameObject opponentComment = null;

    public void Start()
    {
        if (myComment != null) { myComment.SetActive(false); }
        if (opponentComment != null) { opponentComment.SetActive(false); }
    }

    /// <summary>
    /// �X�N���[������ԉ��Ɉړ�����
    /// </summary>
    public void SetBottomPosition()
    {
        if (scrollRect != null) { scrollRect.verticalNormalizedPosition = 0.0f; }
    }

    /// <summary>
    /// ���C�A�E�g�X�V
    /// </summary>
    public void UpdateLayout()
    {
        if (scrollRect == null) { return; }
        HorizontalOrVerticalLayoutGroup layoutGroup = scrollRect.content.GetComponent<HorizontalOrVerticalLayoutGroup>();
        if (layoutGroup != null)
        {
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();
        }
        ContentSizeFitter sizeFitter = scrollRect.content.GetComponent<ContentSizeFitter>();
        if (sizeFitter != null)
        {
            sizeFitter.SetLayoutHorizontal();
            sizeFitter.SetLayoutVertical();
        }
    }

    /// <summary>
    /// �R�����g��ǉ�
    /// </summary>
    /// <param name="commentType"></param>
    /// <param name="message"></param>
    public void AddComment(CommentType commentType, string message)
    {
        GameObject baseObj = null;
        switch (commentType)
        {
            case CommentType.Mine: baseObj = myComment; break;
            case CommentType.Opponent: baseObj = opponentComment; break;
            default: break;
        }
        if (baseObj == null) { return; }

        GameObject copy = GameObject.Instantiate(baseObj);
        copy.transform.SetParent(baseObj.transform.parent, false);
        copy.SetActive(true);
        SpeechBundle speechBundle = copy.GetComponent<SpeechBundle>();
        speechBundle.SetText(message);
        speechBundle.UpdateLayout();
    }
}
