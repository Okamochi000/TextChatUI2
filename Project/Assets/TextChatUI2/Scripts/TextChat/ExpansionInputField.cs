using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 領域外タップで閉じないInputField
/// </summary>
public class ExpansionInputField : InputField
{
    [SerializeField] private GameObject closeTapArea = null;

#if !UNITY_EDITOR && UNITY_IOS
    private string keyboardText_ = "";
#endif

#if !UNITY_EDITOR && UNITY_ANDROID
    protected override void Start()
    {
        base.Start();
        // Androidは領域外をタップできないので入力欄を隠さない
        shouldHideMobileInput = false;
    }
#endif

    protected override void OnDisable()
    {
        base.OnDisable();
        if (closeTapArea != null) { closeTapArea.SetActive(false); }
    }

#if !UNITY_EDITOR && UNITY_IOS
    public void Update()
    {
        if (closeTapArea == null) { return; }

        if (m_Keyboard == null)
        {
            if (!closeTapArea.activeSelf) { DeactivateInputField(); }
            closeTapArea.SetActive(false);
        }
        else
        {
            if (touchScreenKeyboard.status == TouchScreenKeyboard.Status.Canceled)
            {
                // キャンセルされた場合は直前の値を保持
                DeactivateInputField();
                this.SetTextWithoutNotify(keyboardText_);
            }
            else
            {
                keyboardText_ = this.text;
            }
            closeTapArea.SetActive(true);
        }
    }

    /// <summary>
    /// フォーカス解除にモバイルキーボードを閉じない
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnDeselect(BaseEventData eventData) {}
#endif
}