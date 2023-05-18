using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;
using BalDUtilities.EditorUtils;
using BalDUtilities.Misc;
using System;
using System.IO;
using System.Collections.Generic;

public class WINDOW_Utils : EditorWindow
{
    private Vector2 windowScroll = Vector2.zero;
    private Vector2 scenesScroll = Vector2.zero;

    private int scenesScrollView = 0;

    private bool showScenes;

    private bool showUIUtils;

    private List<string> scenes = new List<string>();

    private const string SCENES_FOLDER_PATH = "Assets/4_Scenes/";

    [MenuItem("Window/Utils")]
    public static void ShowWindow()
    {
        GetWindow<WINDOW_Utils>("Utils Window");
    }

    private void OnGUI()
    {
        ReadOnlyDraws.EditorScriptDraw(typeof(WINDOW_Utils), this);

        windowScroll = EditorGUILayout.BeginScrollView(windowScroll);

        if (GUILayout.Button("Force scripts recompile")) AssetDatabase.Refresh();

        ScenesManagement();

        SimpleDraws.HorizontalLine();

        UIUtils();

        EditorGUILayout.EndScrollView();
    }

    private void ScenesManagement()
    {
        EditorGUILayout.BeginVertical("GroupBox");

        // Set the scollView size, keep at 0 for auto size
        scenesScrollView = EditorGUILayout.IntField("View Size", scenesScrollView);
        scenesScrollView = Mathf.Clamp(scenesScrollView, 0, int.MaxValue);

        scenesScroll = EditorGUILayout.BeginScrollView(scenesScroll, GUILayout.Height(scenesScrollView));

        showScenes = EditorGUILayout.Foldout(showScenes, "Scenes management");

        if (showScenes)
        {
            EditorGUI.indentLevel++;

            // Repopulates the scene list in SCENES_FOLDER_PATH
            if (GUILayout.Button("Update Scenes List") || scenes == null || scenes.Count == 0)
            {
                scenes = new List<string>();

                // get the files
                string[] files = Directory.GetFiles(SCENES_FOLDER_PATH);

                string current;
                for (int i = 0; i < files.Length; ++i)
                {
                    current = files[i];

                    // if is scene
                    if (current.EndsWith(".unity"))
                    {
                        // strip string to only get the scene name
                        current = current.Replace(".unity", "");
                        current = current.Replace(SCENES_FOLDER_PATH, "");
                        scenes.Add(current);
                    }
                }
            }

            EditorGUILayout.BeginVertical("GroupBox");

            // iterate through every scene names
            foreach (var item in scenes)
            {
                // ignore if "item" is the current scene
                if (item == SceneManager.GetActiveScene().name) continue;

                // button that loads the target scene
                if (GUILayout.Button("Go to " + item))
                {
                    if (Application.isPlaying)
                        SceneManager.LoadScene(item);
                    else
                        EditorSceneManager.OpenScene(SCENES_FOLDER_PATH + item + ".unity");
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndScrollView();


        EditorGUILayout.EndVertical();
    }

    private void UIUtils()
    {
        showUIUtils = EditorGUILayout.Foldout(showUIUtils, "UI Utils");
        if (!showUIUtils) return;

        EditorGUILayout.BeginVertical("GroupBox");

        // creates a button that sets the ancors of every active UI elements to surround them
        if (GUILayout.Button("Setup every UI elements anchors"))
        {
            RectTransform[] elements = GameObject.FindObjectsOfType<RectTransform>();

            for (int i = 0; i < elements.Length; i++)
            {
                // we want to handle these differently
                if (elements[i].parent == null) continue;
                if (elements[i].rotation.eulerAngles != Vector3.zero) return;

                SetupAnchors(ref elements[i]);
            }

            void SetupAnchors(ref RectTransform itemTransform)
            {
                RectTransform parentTransform = itemTransform.parent as RectTransform;

                if (parentTransform == null) return;

                // sets the anchors depending of the elements size & current offset
                Vector2 newAnchorsMin = new Vector2(itemTransform.anchorMin.x + itemTransform.offsetMin.x / parentTransform.rect.width,
                                                    itemTransform.anchorMin.y + itemTransform.offsetMin.y / parentTransform.rect.height);
                Vector2 newAnchorsMax = new Vector2(itemTransform.anchorMax.x + itemTransform.offsetMax.x / parentTransform.rect.width,
                                                    itemTransform.anchorMax.y + itemTransform.offsetMax.y / parentTransform.rect.height);

                itemTransform.anchorMin = newAnchorsMin;
                itemTransform.anchorMax = newAnchorsMax;

                // reset the offsets
                itemTransform.offsetMin = itemTransform.offsetMax = new Vector2(0, 0);
            }
        }

        EditorGUILayout.EndVertical();
    }
}
