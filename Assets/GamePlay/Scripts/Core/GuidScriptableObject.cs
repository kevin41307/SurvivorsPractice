using Sirenix.OdinInspector;
using UnityEditor;

namespace GamePlay.Scripts.Core
{
    public abstract class GuidScriptableObject : SerializedScriptableObject
    {
        [ReadOnly]
        [ShowInInspector]
        private string guid;

        public string Guid => guid;

#if UNITY_EDITOR
        protected virtual void OnEnable()
        {
            TryAssignGuidFromAsset();
        }

        private void OnValidate()
        {
            TryAssignGuidFromAsset();
        }

        private void TryAssignGuidFromAsset()
        {
            var path = AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var assetGuid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(assetGuid))
            {
                return;
            }

            if (guid == assetGuid)
            {
                return;
            }

            guid = assetGuid;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}

