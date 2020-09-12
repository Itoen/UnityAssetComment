using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PrefabComment
{
    public sealed class CommentData
    {
        public string comment;
    }
    
    public sealed class PrefabComment
    {
        #region Variables
        
        private string guid;

        private long localId;
        
        private CommentData serverCommentData;

        private string comment;

        #endregion

        #region Properties

        public bool IsInitialized
        {
            get;
            private set;
        }

        public string Comment
        {
            get => this.comment;
            set
            {
                this.comment = value;
                this.onChageComment?.Invoke();
            }
        }
        
        #endregion

        #region Events

        public Action onChageComment;

        #endregion

        #region Methods

        public void Load(Object target)
        {
            if (this.IsInitialized)
            {
                return;
            }

            var targetTransform = (Transform) target;
            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(targetTransform.gameObject, out this.guid,
                out this.localId))
            {
                var sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(targetTransform.gameObject);

                if (sourcePrefab == null)
                {
                    return;
                }
                
                if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(sourcePrefab, out this.guid,
                    out this.localId))
                {
                    return;
                }
            }

            var webRequestAsyncOperation = FirebaseUtility.GetComment(this.guid, this.localId);

            webRequestAsyncOperation.completed += _ =>
            {
                if (webRequestAsyncOperation.webRequest.isNetworkError)
                {
                    Debug.LogError("[AssetComment]Comment Get failed.");
                    return;
                }

                var jsonData = webRequestAsyncOperation.webRequest.downloadHandler.text;
                if (!string.IsNullOrEmpty(jsonData) && !jsonData.Equals("null"))
                {
                    this.serverCommentData = JsonUtility.FromJson<CommentData>(jsonData);
                    this.Comment = this.serverCommentData.comment;
                }

                this.IsInitialized = true;
            };
        }
        
        public void Save()
        {
            if (this.serverCommentData == null)
            {
                this.serverCommentData = new CommentData();
            }
            else if(this.serverCommentData.comment == this.Comment)
            {
                return;
            }
            
            this.serverCommentData.comment = this.Comment;
            var webRequestAsyncOperation =
                FirebaseUtility.UploadComment(this.guid, this.localId, this.serverCommentData);
            webRequestAsyncOperation.completed += _ =>
            {
                if (webRequestAsyncOperation.webRequest.isNetworkError ||
                    webRequestAsyncOperation.webRequest.isHttpError)
                {
                    Debug.LogError(webRequestAsyncOperation.webRequest.error);
                }
            };
        }

        public void DisposeComment()
        {
            this.Comment = string.Empty;
            this.IsInitialized = false;
        }

        #endregion
    }
}
