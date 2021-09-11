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

        // 更新チェック開始
        EditorGUI.BeginChangeCheck();

        // シリアライズオブジェクト更新
        serializedObject.Update();

        // 変数設定
        EditorGUILayout.PropertyField(closeTapAreaProperty_);

        // シリアライズオブジェクト更新
        serializedObject.ApplyModifiedProperties();

        // 更新チェック終了
        EditorGUI.EndChangeCheck();
    }
}
