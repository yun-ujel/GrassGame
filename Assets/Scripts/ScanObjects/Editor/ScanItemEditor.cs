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

            GUILayout.Space(10);

            if (item.Data == null)
            {
                item.Data = new ScanItemData(int.Parse(item.name), itemName, itemDescription);
            }

            itemName = item.Data.ItemName;
            itemDescription = item.Data.ItemDescription;

            if (DrawName(out string name))
            {
                item.Data.ItemName = name;
            }
        }

        private bool DrawName(out string name)
        {
            name = EditorGUILayout.TextField("Item Name", itemName);

            if (name != itemName)
            {
                return true;
            }
            return false;
        }
    }
}