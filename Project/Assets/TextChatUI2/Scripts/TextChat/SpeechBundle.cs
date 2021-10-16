using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 吹き出し
/// </summary>
public class SpeechBundle : MonoBehaviour
{
    [SerializeField] private Text text = null;

    /// <summary>
    /// テキストを設定する
    /// </summary>
    /// <param name="message"></param>
    public void SetText(string message)
    {
        text.text = message;
    }

    /// <summary>
    /// レイアウト更新
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
