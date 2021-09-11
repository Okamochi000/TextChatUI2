using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ExpansionInputField), true)]
public class ExpansionInputFieldEditor : InputFieldEditor
{
    private SerializedProperty scriptProperty_ = null;
    private SerializedProperty closeTapAreaProperty_ = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        scriptProperty_ = serializedObject.FindProperty("m_Script");
        closeTapAreaProperty_ = serializedObject.FindProperty("closeTapArea");
    }

    public override void OnInspectorGUI()
    {
        using (new EditorGUI.DisabledScope(true)) { EditorGUILayout.PropertyField(scriptProperty_); }
        base.OnInspectorGUI();

        // �X�V�`�F�b�N�J�n
        EditorGUI.BeginChangeCheck();

        // �V���A���C�Y�I�u�W�F�N�g�X�V
        serializedObject.Update();

        // �ϐ��ݒ�
        EditorGUILayout.PropertyField(closeTapAreaProperty_);

        // �V���A���C�Y�I�u�W�F�N�g�X�V
        serializedObject.ApplyModifiedProperties();

        // �X�V�`�F�b�N�I��
        EditorGUI.EndChangeCheck();
    }
}
