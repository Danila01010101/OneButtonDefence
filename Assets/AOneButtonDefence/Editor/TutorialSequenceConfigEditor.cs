using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(TutorialSequenceConfig))]
public class TutorialSequenceConfigEditor : Editor
{
    private ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(
            serializedObject,
            serializedObject.FindProperty("tutorials"),
            true, true, true, true);

        list.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Tutorial Entries (Drag Prefabs Here)");
        };

        list.drawElementCallback = (rect, index, active, focused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            var tutorialObj = element.FindPropertyRelative("tutorialObject");
            var messagePreview = element.FindPropertyRelative("messagePreview");
            var idx = element.FindPropertyRelative("index");

            rect.y += 2;
            float width = rect.width;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, width / 2, EditorGUIUtility.singleLineHeight),
                tutorialObj, GUIContent.none);

            EditorGUI.LabelField(
                new Rect(rect.x + width / 2 + 5, rect.y, width / 2 - 5, EditorGUIUtility.singleLineHeight),
                $"Index: {idx.intValue} | {messagePreview.stringValue}");
        };

        list.onAddCallback = l =>
        {
            var cfg = (TutorialSequenceConfig)target;
            cfg.tutorials.Add(new TutorialSequenceConfig.TutorialEntry());
            SortEntries(cfg);
        };

        list.onChangedCallback = l =>
        {
            var cfg = (TutorialSequenceConfig)target;
            SortEntries(cfg);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var dropRect = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(dropRect, "Drag Tutorial Prefabs Here", EditorStyles.helpBox);

        var e = Event.current;
        if (e.type == EventType.DragUpdated || e.type == EventType.DragPerform)
        {
            if (dropRect.Contains(e.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (e.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (var obj in DragAndDrop.objectReferences)
                    {
                        if (obj is GameObject go)
                        {
                            var tutorials = go.GetComponentsInChildren<TutorialObject>(true);
                            var cfg = (TutorialSequenceConfig)target;

                            foreach (var t in tutorials)
                            {
                                if (cfg.tutorials.Exists(x => x.tutorialObject == t))
                                    continue;

                                cfg.tutorials.Add(new TutorialSequenceConfig.TutorialEntry
                                {
                                    tutorialObject = t,
                                    index = t.Index,
                                    messagePreview = t.Message
                                });
                            }

                            SortEntries(cfg);
                        }
                    }
                }
                e.Use();
            }
        }

        list.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void SortEntries(TutorialSequenceConfig cfg)
    {
        cfg.tutorials.Sort((a, b) =>
        {
            if (a.tutorialObject == null && b.tutorialObject == null) return 0;
            if (a.tutorialObject == null) return 1;
            if (b.tutorialObject == null) return -1;
            return a.index.CompareTo(b.index);
        });

        foreach (var entry in cfg.tutorials)
        {
            if (entry.tutorialObject != null)
            {
                entry.index = entry.tutorialObject.Index;
                entry.messagePreview = entry.tutorialObject.Message;
            }
        }

        EditorUtility.SetDirty(cfg);
    }
}