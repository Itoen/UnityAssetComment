using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PrefabComment
{
    [CustomEditor(typeof(Transform), true)]
    public sealed class PrefabCommentEditor : Editor
    {
        #region Constants

        private static readonly Vector2 InputFieldSize = new Vector2(100, 100);

        private const string CreateWindowMenuPath = "Comment/Create Window";
        
        private const string DispSceneViewMenuPath = "Comment/Show SceneView";

        private const string DispFlagSaveKey = "CommentDispFlag";

        #endregion

        #region Variables

        private static PrefabComment _prefabComment;

        private static PrefabCommentWindow _commentWindow;

        private static bool _isDispSceneView;

        #endregion

        #region Methods

        [MenuItem(CreateWindowMenuPath)]
        private static void CreateCommentWindow()
        {
            _commentWindow = PrefabCommentWindow.GetWindow();
            _commentWindow.SetAssetComment(_prefabComment);
        }
        
        [MenuItem(DispSceneViewMenuPath)]
        private static void SetDispSceneViewFlag()
        {
            _isDispSceneView = !_isDispSceneView;
            Menu.SetChecked(DispSceneViewMenuPath, _isDispSceneView);
            EditorPrefs.SetBool(DispFlagSaveKey, _isDispSceneView);
            SceneView.RepaintAll();
        }

        public void Awake()
        {
            if (_prefabComment == null)
            {
                _prefabComment = new PrefabComment();
                _prefabComment.onChageComment += SceneView.RepaintAll;
            }
            
            if (PrefabCommentWindow.HasOpenInstances<PrefabCommentWindow>())
            {
                CreateCommentWindow();
            }

            _isDispSceneView = EditorPrefs.GetBool(DispFlagSaveKey, false);
            _prefabComment.Load(target);
        }

        private void OnDestroy()
        {
            _prefabComment.Save();
            _prefabComment.DisposeComment();
        }

        private void OnSceneGUI()
        {
            if (!_isDispSceneView)
            {
                return;
            }

            if (_prefabComment == null || !_prefabComment.IsInitialized)
            {
                return;
            }
            
            var camera = SceneView.currentDrawingSceneView.camera;
            var component = (Component) target;
            var targetObject = component.gameObject;

            var screenPos = camera.WorldToScreenPoint(targetObject.transform.position);

            Handles.BeginGUI();

            var inputFieldRect = new Rect(
                screenPos.x - InputFieldSize.x * 2f,
                SceneView.currentDrawingSceneView.position.height - screenPos.y + InputFieldSize.y,
                InputFieldSize.x,
                InputFieldSize.y);

            _prefabComment.Comment = GUI.TextArea(inputFieldRect, _prefabComment.Comment);
            
            Handles.EndGUI();
        }

        #endregion
    }
}