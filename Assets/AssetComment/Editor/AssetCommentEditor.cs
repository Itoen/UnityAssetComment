using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetComment
{
    [CustomEditor(typeof(Transform), true)]
    public sealed class AssetCommentEditor : Editor
    {
        #region Constants

        private static readonly Vector2 InputFieldSize = new Vector2(100, 100);

        private const string CreateWindowMenuPath = "Comment/Create Window";
        
        private const string DispSceneViewMenuPath = "Comment/Show SceneView";

        private const string DispFlagSaveKey = "CommentDispFlag";

        #endregion

        #region Variables

        private static AssetComment _assetComment;

        private static AssetCommentWindow _commentWindow;

        private static bool _isDispSceneView;

        #endregion

        #region Methods

        [MenuItem(CreateWindowMenuPath)]
        private static void CreateCommentWindow()
        {
            _commentWindow = AssetCommentWindow.GetWindow();
            _commentWindow.SetAssetComment(_assetComment);
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
            if (_assetComment == null)
            {
                _assetComment = new AssetComment();
                _assetComment.onChageComment += SceneView.RepaintAll;
            }
            
            if (AssetCommentWindow.HasOpenInstances<AssetCommentWindow>())
            {
                CreateCommentWindow();
            }

            _isDispSceneView = EditorPrefs.GetBool(DispFlagSaveKey, false);
            _assetComment.Load(target);
        }

        private void OnDestroy()
        {
            if (_assetComment != null)
            {
                _assetComment.Save();
                _assetComment.DisposeComment();
            }
        }

        private void OnSceneGUI()
        {
            if (!_isDispSceneView)
            {
                return;
            }

            if (_assetComment == null || !_assetComment.IsInitialized)
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

            _assetComment.Comment = GUI.TextArea(inputFieldRect, _assetComment.Comment);
            
            Handles.EndGUI();
        }

        #endregion
    }
}