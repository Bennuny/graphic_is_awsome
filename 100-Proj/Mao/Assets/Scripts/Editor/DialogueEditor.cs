using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using System;
using UnityEngine.Windows;

[CustomEditor(typeof(DialogueData_SO))]
public class DialogueCustomEditor: Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open In Editor"))
        {
            DialogueEditor.InitWindow((DialogueData_SO)target);
        }

        base.OnInspectorGUI();


    }
}

public class DialogueEditor : EditorWindow
{
    DialogueData_SO currentData;

    ReorderableList pieceList;

    Vector2 scrollPos = Vector2.zero;

    Dictionary<string, ReorderableList> optionListDict = new Dictionary<string, ReorderableList>();

    [MenuItem("BEN/Dialogue Editor")]

    public static void Init()
    {
        DialogueEditor win = GetWindow<DialogueEditor>("Dialogue Editor");

        // 
        win.autoRepaintOnSceneChange = true;
    }

    public static void InitWindow(DialogueData_SO data)
    {
        DialogueEditor win = GetWindow<DialogueEditor>("Dialogue Editor");

        win.currentData = data;
    }

    [OnOpenAsset]
    public static bool OpenAsset(int instanceID, int line)
    {
        DialogueData_SO data = EditorUtility.InstanceIDToObject(instanceID) as DialogueData_SO;

        if (data != null)
        {
            DialogueEditor.InitWindow(data);
            return true;
        }

        return false;
    }

    private void OnSelectionChange()
    {
        //
        var newData = Selection.activeObject as DialogueData_SO;
        if (newData != null)
        {
            currentData = newData;
            SetupRecorderableList();
        }
        else
        {
            currentData = null;
            pieceList = null;
        }

        Repaint();
    }

    private void OnGUI()
    {
        if (currentData != null)
        {
            EditorGUILayout.LabelField(currentData.name, EditorStyles.boldLabel);

            GUILayout.Space(10);

            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (pieceList == null)
            {
                SetupRecorderableList();
            }
            pieceList.DoLayoutList();

        }
        else
        {
            if (GUILayout.Button("Create new Dialogue"))
            {
                string dataPath = "Assets/Game Data/Dialogue Data/";
                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }

                DialogueData_SO newData = ScriptableObject.CreateInstance<DialogueData_SO>();
                AssetDatabase.CreateAsset(newData, dataPath + "/New Dialogue.asset");
                currentData = newData;
            }

            GUILayout.Label("NO DATA SELECTED!", EditorStyles.boldLabel);
        }
    }

    private void SetupRecorderableList()
    {
        pieceList = new ReorderableList(currentData.dialoguePieces, typeof(DialoguePiece), true, true, true, true);

        pieceList.drawHeaderCallback += OnDrawPieceHeader;

        pieceList.drawElementCallback += OnDrawPieceListElement;

        pieceList.elementHeightCallback += OnHeightChanged;
    }

    private float OnHeightChanged(int index)
    {
        return GetPieceHeight(currentData.dialoguePieces[index]);
    }

    float GetPieceHeight(DialoguePiece piece)
    {
        var height = EditorGUIUtility.singleLineHeight;

        height += EditorGUIUtility.singleLineHeight * 5;

        return height;
    }

    private void OnDrawPieceListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorUtility.SetDirty(currentData);

        GUIStyle textStyle = new GUIStyle("TextFiled");

        if (index < currentData.dialoguePieces.Count)
        {
            var currentPiece = currentData.dialoguePieces[index];

            var tempRect = rect;

            tempRect.height = EditorGUIUtility.singleLineHeight;

            tempRect.width = 30;

            EditorGUI.LabelField(tempRect, "ID");

            tempRect.x += tempRect.width;
            tempRect.width = 100;

            currentPiece.ID = EditorGUI.TextField(tempRect, currentPiece.ID);

            tempRect.x += tempRect.width + 10;
            tempRect.width = 100;

            EditorGUI.LabelField(tempRect, "Quest");

            tempRect.x += tempRect.width;
            currentPiece.quest = (QuestData_SO)EditorGUI.ObjectField(tempRect, currentPiece.quest, typeof(QuestData_SO), false);

            tempRect.y += EditorGUIUtility.singleLineHeight + 5;
            tempRect.x = rect.x;
            tempRect.height = 60;
            tempRect.width = tempRect.height;
            currentPiece.Image = (Sprite)EditorGUI.ObjectField(tempRect, currentPiece.Image, typeof(Sprite), false);

            tempRect.x += tempRect.width + 5;
            tempRect.width = rect.width - tempRect.x;
            textStyle.wordWrap = true;
            currentPiece.Text = EditorGUI.TextField(tempRect, currentPiece.Text, textStyle);

            // 画选项
            tempRect.y += tempRect.height + 5;
            tempRect.x = rect.x;
            tempRect.width = rect.width;

            string optionListKey = currentPiece.ID + currentPiece.Text;
            if (optionListKey != string.Empty)
            {
                //if (optionListKey)


                var optionList = new ReorderableList(currentPiece.options, typeof(DialogueUI), true, true, true, true);

                optionList.drawElementCallback = (optionRect, optionIndex, optionAcive, optionFocused) =>
                {
                    //OnDrawOptionElement(currentPiece, optionRect, optionIndex, optionAcive, optionFocused);
                };

            }
        }
    }

    private void OnDrawPieceHeader(Rect rect)
    {
        GUI.Label(rect, "Dialogue Piece");
    }
}
