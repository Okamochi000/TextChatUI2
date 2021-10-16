using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteAlways]
public class TextChatInputMenu : MonoBehaviour
{
    [SerializeField] private TextChatBody textChatBody = null;
    [SerializeField] private SafeAreaBehaviour parentSafeArea = null;
    [SerializeField] private ExpansionInputField inputField = null;
    [SerializeField] private RectTransform menuRectTransform = null;
    [SerializeField] private RectTransform barRectTransform = null;
    [SerializeField] private RectTransform keyboardRectTransform = null;
    [SerializeField] private Button sendButton = null;
    [SerializeField] private int maxLineCount = 5;
    [SerializeField] private float minHeight = 120.0f;
    [SerializeField] private float topPadding = 40.0f;
    [SerializeField] private float bottomPadding = 40.0f;

    private float canvasScale_ = 1.0f;

    void Start()
    {
        canvasScale_ = SafeAreaUtility.GetCanvasScale(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (menuRectTransform == null) { return; }
        if (barRectTransform == null) { return; }
        if (keyboardRectTransform == null) { return; }

        // エディター編集中
        Vector2 menuSizeDelta = menuRectTransform.sizeDelta;
        if (!Application.isPlaying)
        {
            Vector2 barSizeDelta = barRectTransform.sizeDelta;
            if (menuSizeDelta.y != barSizeDelta.y) { UpdateLayout(barSizeDelta.y); }
            return;
        }

        // テキスト縦幅に合わせる
        float currentHeight = (inputField.textComponent.preferredHeight + topPadding + bottomPadding);
        if (inputField.textComponent.cachedTextGenerator.lineCount <= 1) { currentHeight = minHeight; }
        float height = Mathf.Max(minHeight, currentHeight);
        float maxHeight = (inputField.textComponent.fontSize + inputField.textComponent.lineSpacing) * (float)maxLineCount + topPadding + bottomPadding;
        height = Mathf.Min(height, maxHeight);
        if (height != menuSizeDelta.y || keyboardRectTransform.sizeDelta.y != GetKeyboardHeight()) { UpdateLayout(height); }

        // モバイルキーボードを表示したときに表示されるホームメニューのサイズ分横幅を調整する
#if !UNITY_EDITOR && UNITY_ANDROID
        float width = menuRectTransform.rect.width;
        float keyboardWidth = UniSoftwareKeyboardArea.SoftwareKeyboardArea.GetLandscapeHomeWidth() * canvasScale_;
        ScreenOrientation screenOrientaion = Screen.orientation;
        if (screenOrientaion == ScreenOrientation.LandscapeLeft || screenOrientaion == ScreenOrientation.LandscapeRight) { width -= keyboardWidth; }
        Vector3 anchoredPosition = barRectTransform.anchoredPosition;
        if (screenOrientaion == ScreenOrientation.LandscapeLeft) { anchoredPosition.x = -(keyboardWidth / 2.0f); }
        else if (screenOrientaion == ScreenOrientation.LandscapeRight) { anchoredPosition.x = (keyboardWidth / 2.0f); }
        else { anchoredPosition.x = 0.0f; }
        barRectTransform.anchoredPosition = anchoredPosition;
        barRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
#endif

        // ボタン状態切替
        if (sendButton != null)
        {
            if (inputField != null && inputField.text.Length > 0) { sendButton.interactable = true; }
            else { sendButton.interactable = false; }
        }

        // キーボード表示中は常に一番下にスクロールする
        if (inputField.touchScreenKeyboard != null)
        {
            textChatBody.SetBottomPosition();
        }
    }

    /// <summary>
    /// 送信
    /// </summary>
    public void Send()
    {
        if (inputField == null) { return; }
        if (inputField.text == "") { return; }

        // コメント追加
        if (textChatBody != null) { textChatBody.AddComment(TextChatBody.CommentType.Mine, inputField.text); }

        // スクロール位置を調整
        textChatBody.UpdateLayout();
        textChatBody.SetBottomPosition();

        // テキスト初期化
        inputField.text = "";
        if (sendButton != null) { sendButton.interactable = false; }
    }

    /// <summary>
    /// レイアウト更新
    /// </summary>
    /// <param name="height"></param>
    private void UpdateLayout(float height)
    {
        if (menuRectTransform != null)
        {
            Vector2 sizeDelta = menuRectTransform.sizeDelta;
            sizeDelta.y = height;
            menuRectTransform.sizeDelta = sizeDelta;
        }
        if (barRectTransform != null)
        {
            Vector2 sizeDelta = barRectTransform.sizeDelta;
            sizeDelta.y = height;
            barRectTransform.sizeDelta = sizeDelta;
        }
        if (Application.isPlaying)
        {
            Vector2 keyboardSizeDelta = keyboardRectTransform.sizeDelta;
            keyboardSizeDelta.y = GetKeyboardHeight();
            keyboardRectTransform.sizeDelta = keyboardSizeDelta;
            Vector2 menuSizeDelta = menuRectTransform.sizeDelta;
            menuSizeDelta.y += keyboardSizeDelta.y;
            menuRectTransform.sizeDelta = menuSizeDelta;
        }

        if (parentSafeArea != null) { parentSafeArea.UpdateLayoutLock(); }
    }

    /// <summary>
    /// キーボードの高さを取得する
    /// </summary>
    /// <returns></returns>
    private float GetKeyboardHeight()
    {
        float height = (float)UniSoftwareKeyboardArea.SoftwareKeyboardArea.GetHeight();
        if (height > 0) { height -= Screen.safeArea.yMin; }
        height *= canvasScale_;

        return height;
    }
}
