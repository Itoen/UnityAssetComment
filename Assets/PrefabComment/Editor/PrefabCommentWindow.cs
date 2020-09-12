using System;
using UnityEditor;
using UnityEngine;

namespace PrefabComment
{
    public sealed class PrefabCommentWindow : EditorWindow
    {
        #region Variables

        private PrefabComment _prefabComment;

        #endregion

        #region Methods

        public static PrefabCommentWindow GetWindow()
        {
            return GetWindow<PrefabCommentWindow>("Comment");
        }

        public void SetAssetComment(PrefabComment prefabComment)
        {
            this._prefabComment = prefabComment;
            this._prefabComment.onChageComment += this.Repaint;
            this.Repaint();
        }

        private void OnGUI()
        {
            if (this._prefabComment == null)
            {
                return;
            }

            GUI.enabled = this._prefabComment.IsInitialized;
            
            this._prefabComment.Comment = EditorGUILayout.TextArea(this._prefabComment.Comment, GUILayout.Height(this.position.height - 5));
            
            GUI.enabled = true;
        }

        #endregion
    }
}
