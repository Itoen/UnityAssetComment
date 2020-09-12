using System;
using UnityEditor;
using UnityEngine;

namespace AssetComment
{
    public sealed class AssetCommentWindow : EditorWindow
    {
        #region Variables

        private AssetComment _assetComment;

        #endregion

        #region Methods

        public static AssetCommentWindow GetWindow()
        {
            return GetWindow<AssetCommentWindow>("Comment");
        }

        public void SetAssetComment(AssetComment assetComment)
        {
            this._assetComment = assetComment;
            this._assetComment.onChageComment += this.Repaint;
            this.Repaint();
        }

        private void OnGUI()
        {
            if (this._assetComment == null)
            {
                return;
            }

            GUI.enabled = this._assetComment.IsInitialized;
            
            this._assetComment.Comment = EditorGUILayout.TextArea(this._assetComment.Comment, GUILayout.Height(this.position.height - 5));
            
            GUI.enabled = true;
        }

        #endregion
    }
}
