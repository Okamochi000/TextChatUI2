using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// テキストチャットのメッセージ表示欄
/// </summary>
public class TextChatBody : MonoBehaviour
{
    /// <summary>
    /// コメントの種類
    /// </summary>
    public enum CommentType
    {
        Mine,       // 自身
        Opponent    // 相手
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
    /// スクロールを一番下に移動する
    /// </summary>
    public void SetBottomPosition()
    {
        if (scrollRect != null) { scrollRect.verticalNormalizedPosition = 0.0f; }
    }

    /// <summary>
    /// レイアウト更新
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
    /// コメントを追加
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
