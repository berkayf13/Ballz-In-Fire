#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using F13StandardUtils.CollectTicket.Core.Script;
using F13StandardUtils.Scripts.Core;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace F13StandardUtils.ExternalToolbar
{
    public static class ToolbarStyles
    {
        public static GUIStyle SmallCommandButtonStyle() =>new GUIStyle("Command")
        {
            fixedWidth = 20,
            fontSize = 9,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
        public static GUIStyle MediumCommandButtonStyle() => new GUIStyle("Command")
        {
            fixedWidth = 35,
            fontSize = 9,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
        public static GUIStyle LargeCommandButtonStyle() => new GUIStyle("Command")
        {
            fixedWidth = 55,
            fontSize = 9,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
        public static GUIStyle XLargeCommandButtonStyle() => new GUIStyle("Command")
        {
            fixedWidth = 80,
            fontSize = 8,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
    }

    [InitializeOnLoad]
    public class ToolbarSwitchButton : IPreprocessBuildWithReport
    {

        static ToolbarSwitchButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUILeft);
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUIRight);
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

        }

        static List<SceneAsset> scenes = new List<SceneAsset>();

        static void OnToolbarGUILeft()
        {

            GUILayout.BeginHorizontal();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (GUILayout.Button(new GUIContent(i.ToString()), ToolbarStyles.SmallCommandButtonStyle()))
                {
                    SceneHelper.OpenScene(EditorBuildSettings.scenes[i].path);
                }
            }
            GUILayout.EndHorizontal();
        }

        static void OnToolbarGUIRight()
        {

            GUILayout.BeginHorizontal();
            
            AddEditModeButtonOnToolbar("SaveAssets", Color.magenta, ToolbarStyles.LargeCommandButtonStyle(), 
                (buttonName,buttonColor,buttonGuiStyle) =>
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });

            if (MoveZ.Instance)
            {
                AddPlayModeButtonOnToolbar("MoveZ",MoveZ.Instance.isMove? Color.red+Color.yellow : Color.black ,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        MoveZ.Instance.isMove = !MoveZ.Instance.isMove;
                        Debug.Log("Şu anda play mode açıkken bu button görünür");
                    });
                AddPlayModeButtonOnToolbar("VeloZ",MoveZ.Instance.isMove? Color.red+Color.yellow : Color.black ,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        MoveZ.Instance.SetMultiplier( (MoveZ.Instance.Multiplier + 1) % 2);

                    });
            }
            if (GameController.Instance)
            {
                AddPlayModeButtonOnToolbar("Success",Color.green, ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        GameController.Instance.SuccessLevel();
                        Debug.Log("Şu anda play mode açıkken bu button görünür");
                    });
                AddPlayModeButtonOnToolbar("Fail",Color.red,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        GameController.Instance.FailLevel();
                        Debug.Log("Şu anda play mode açıkken bu button görünür");
                    });
                AddPlayModeButtonOnToolbar("NxtLvl",Color.cyan,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        GameController.Instance.SetLevel(GameController.Instance.Level+1);
                        Debug.Log("Şu anda play mode açıkken bu button görünür");
                    });
                AddPlayModeButtonOnToolbar("PreLvl",Color.yellow,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        GameController.Instance.SetLevel(GameController.Instance.Level-1);
                        Debug.Log("Şu anda play mode açıkken bu button görünür");
                    });
            }

            if (CustomerQueueManager.Instance)
            {
                AddPlayModeButtonOnToolbar("Q",Color.green,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        SendCustomerService.Instance.QueueCustomer();
                        Debug.Log("Şu anda play mode açıkken bu button görünür");
                    });
                AddPlayModeButtonOnToolbar("DeQ",Color.red,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        SendCustomerService.Instance.DequeueAndGoAway();
                        Debug.Log("Şu anda play mode açıkken bu button görünür");
                    });
            }
            
            if (LevelMoneyController.Instance)
            {
                AddPlayModeButtonOnToolbar("Money",Color.green,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        LevelMoneyController.Instance.AddMoney(200);
                        Debug.Log("Şu anda play mode açıkken bu button görünür");
                    });
            }
            
            GUILayout.EndHorizontal();
        }

        private static void AddPlayModeButtonOnToolbar(string buttonName, Color color, GUIStyle guiStyle, Action<string,Color,GUIStyle> onClick)
        {
            if (Application.isPlaying)
            {
                AddButtonOnToolbar(buttonName,color,guiStyle,onClick);
            }
        }
        
        private static void AddEditModeButtonOnToolbar(string buttonName, Color color, GUIStyle guiStyle, Action<string,Color,GUIStyle> onClick)
        {
            if (!Application.isPlaying)
            {
                AddButtonOnToolbar(buttonName,color,guiStyle,onClick);
            }
        }

        private static void AddButtonOnToolbar(string buttonName, Color color, GUIStyle guiStyle, Action<string,Color,GUIStyle> onClick)
        {
            guiStyle.normal.textColor = color;
            var guiContent = new GUIContent(buttonName);
            if (GUILayout.Button(guiContent, guiStyle))
            {
                onClick?.Invoke(buttonName,color,guiStyle);
            }
        }

        public int callbackOrder { get; }
        
        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    //DO Some stuff when enter editmode !
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    //DO Some stuff when exit editmode before press play !
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    //DO Some stuff when enter play mode after press play !
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    //DO Some stuff when exit play mode after press stop!
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }
        public void OnPreprocessBuild(BuildReport report)
        {
            //DO Some stuff before BUILDING!!!!!
        }
    }
    
    static class SceneHelper
    {
        static string sceneToOpen;

        public static void OpenScene(string scene)
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scene);
        }
    }
}
#endif
