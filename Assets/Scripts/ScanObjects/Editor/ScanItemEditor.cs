using GrassGame.Utilities;
using UnityEditor;
using UnityEngine;

namespace GrassGame.ScanObjects.SO
{
    [CustomEditor(typeof(ScanItem))]
    public class ScanItemEditor : Editor
    {
        private ScanItem item;

        private string itemName;
        private string itemDescription;

        private void OnEnable()
        {
            item = (ScanItem)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Remove"))
            {
                item.Remove();
            }

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Properties", EditorStyles.centeredGreyMiniLabel);
            
            MaintainItemData();

            if (DrawName(out string name))
            {
                item.Data.ItemName = name;
                UpdateItemName(name);
            }

            EditorGUILayout.Space(0);

            if (DrawDescription(out string description))
            {
                item.Data.ItemDescription = description;
            }
        }

        #region Item Methods
        private void MaintainItemData()
        {
            if (item.Data == null)
            {
                ReinitializeItem();
            }

            itemName = item.Data.ItemName;
            itemDescription = item.Data.ItemDescription;
        }

        private void ReinitializeItem()
        {
            string path = AssetDatabase.GetAssetPath(item);
            ScanItemHolder holder = AssetDatabase.LoadAssetAtPath<ScanItemHolder>(path);
            int index = holder.Items.IndexOf(item);

            item.Data = new ScanItemData(index, itemName, itemDescription);
        }

        private void UpdateItemName(string name)
        {
            item.name = $"{item.Data.Index}: {name}";
            //item.Reimport();
        }
        #endregion

        #region Draw Methods
        private bool DrawName(out string name)
        {
            // returns true if changed

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(40));
            name = EditorGUILayout.TextField(itemName);

            if (GUILayout.Button("Reimport", GUILayout.MaxWidth(80)))
            {
                item.Reimport();
            }

            EditorGUILayout.EndHorizontal();
            if (name != itemName)
            {
                return true;
            }
            return false;
        }

        private bool DrawDescription(out string description)
        {
            // returns true if changed

            EditorGUILayout.LabelField("Description");
            description = EditorGUILayout.TextArea(itemDescription, GUILayout.MinHeight(100));
            EditorStyles.textField.wordWrap = true;

            if (description != itemDescription)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}