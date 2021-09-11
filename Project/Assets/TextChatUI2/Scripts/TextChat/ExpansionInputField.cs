using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// �̈�O�^�b�v�ŕ��Ȃ�InputField
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
        // Android�͗̈�O���^�b�v�ł��Ȃ��̂œ��͗����B���Ȃ�
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
                // �L�����Z�����ꂽ�ꍇ�͒��O�̒l��ێ�
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
    /// �t�H�[�J�X�����Ƀ��o�C���L�[�{�[�h����Ȃ�
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnDeselect(BaseEventData eventData) {}
#endif
}