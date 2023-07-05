// ----------------------------------------------------------------------------
// Author: Ryan Hipple
// Date:   05/07/2018
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace OT.Extensions.Types.Editor
{
    /// <summary>
    /// Editor for a scene reference that can display error prompts and offer
    /// solutions when the scene is not valid.
    /// </summary>
    [CustomPropertyDrawer(typeof(Inspector.SceneReference))]
    public class SceneReferenceEditor : PropertyDrawer
    {
        #region -- Private Variables ------------------------------------------
        private SerializedProperty _scene;
        private SerializedProperty _sceneName;
        private SerializedProperty _sceneIndex;
        private SerializedProperty _sceneEnabled;
        private SceneAsset _sceneAsset;
        private string _sceneAssetPath;
        private string _sceneAssetGuid;
        private GUIStyle _errorStyle;
        #endregion -- Private Variables ---------------------------------------
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            CacheProperties(property);
            UpdateSceneState();

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, _scene, GUIContent.none, false);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                CacheProperties(property);
                UpdateSceneState();
                Validate();
            }
            
            EditorGUI.EndProperty();
        }
        
        /// <summary>
        /// Cache all used properties as local variables so that they can be
        /// used by other methods. This needs to be called every frame since a
        /// PropertyDrawer can be reused on different properties.
        /// </summary>
        /// <param name="property">Property to search through.</param>
        private void CacheProperties(SerializedProperty property)
        {
            _scene = property.FindPropertyRelative("Scene");
            _sceneName = property.FindPropertyRelative("SceneName");
            _sceneIndex = property.FindPropertyRelative("sceneIndex");
            _sceneEnabled = property.FindPropertyRelative("sceneEnabled");
            _sceneAsset = _scene.objectReferenceValue as SceneAsset;

            if (_sceneAsset != null)
            {
                _sceneAssetPath = AssetDatabase.GetAssetPath(_scene.objectReferenceValue);
                _sceneAssetGuid = AssetDatabase.AssetPathToGUID(_sceneAssetPath);
            }
            else
            {
                _sceneAssetPath = null;
                _sceneAssetGuid = null;
            }
        }

        /// <summary>
        /// Updates the scene index and enabled flags of a scene property by
        /// scanning through the scenes in EditorBuildSettings.
        /// </summary>
        private void UpdateSceneState()
        {
            if (_sceneAsset != null)
            {
                EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

                _sceneIndex.intValue = -1;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == _sceneAssetGuid)
                    {
                        if(_sceneIndex.intValue != i)
                            _sceneIndex.intValue = i;
                        _sceneEnabled.boolValue = scenes[i].enabled;
                        if (scenes[i].enabled)
                        {
                            if (_sceneName.stringValue != _sceneAsset.name)
                                _sceneName.stringValue = _sceneAsset.name;
                        }
                        break;
                    }
                }
            }
            else
            {
                _sceneName.stringValue = "";
            }
        }

        /// <summary>
        /// Validate any new values in the scene property. This will display
        /// popup errors if there are issues with the current value.
        /// </summary>
        private void Validate()
        {
            if (_sceneAsset != null)
            {
                EditorBuildSettingsScene[] scenes = 
                    EditorBuildSettings.scenes;

                _sceneIndex.intValue = -1;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == _sceneAssetGuid)
                    {
                        if(_sceneIndex.intValue != i)
                            _sceneIndex.intValue = i;
                        if (scenes[i].enabled)
                        {
                            if (_sceneName.stringValue != _sceneAsset.name)
                                _sceneName.stringValue = _sceneAsset.name;
                        }
                        break;
                    }
                }
            }
            else
            {
                _sceneName.stringValue = "";
            }
        }
    }
}