using UnityEngine;
using UnityEngine.Networking;

namespace PrefabComment
{
    public static class FirebaseUtility
    {
        public static UnityWebRequestAsyncOperation GetComment(string guid, long localId)
        {
            var url =
                $"{FirebaseConfig.FirebaseRealtimeDatabaseURL}/{guid}/{localId}.json?auth={FirebaseConfig.DatabaseSecret}";
            var request = UnityWebRequest.Get(url);
            var requestAsyncOperation = request.SendWebRequest();
            return requestAsyncOperation;
        }

        public static UnityWebRequestAsyncOperation UploadComment(string guid, long localId, CommentData commentData)
        {
            var url =
                $"{FirebaseConfig.FirebaseRealtimeDatabaseURL}/{guid}/{localId}.json?auth={FirebaseConfig.DatabaseSecret}";
            var commentJson = JsonUtility.ToJson(commentData);
            var request = UnityWebRequest.Put(url, commentJson);
            var requestAsyncOperation = request.SendWebRequest();
            return requestAsyncOperation;
        }


    }
}
