using UnityEditor;

/*
 * @author: Djayp
 * https://forum.unity.com/threads/what-is-a-serializable-asset-type-for-folder.608875/#post-5225315
 */
namespace OT.Extensions.Types
{
    [System.Serializable]
    public class FolderReference
    {
        public string GUID;

        public string Path => $"{AssetDatabase.GUIDToAssetPath(GUID)}/";
    }
}