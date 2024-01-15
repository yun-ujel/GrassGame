using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace GrassGame.ScanObjects.SO
{
    [CustomEditor(typeof(ScanItemHolder))]
    public class ScanItemHolderEditor : Editor
    {
        private ScanItemHolder holder;

        private void OnEnable()
        {
            holder = (ScanItemHolder)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);

            DisplayItemListInfo();

            if (GUILayout.Button("Create New Item"))
            {
                CreateNewItem();
                Reimport();
            }
        }

        private void CreateNewItem()
        {
            ScanItem item = CreateInstance<ScanItem>();

            int index = holder.Items.Count;

            item.name = $"{index}:";
            item.OnRemovedEvent += RemoveItem;

            item.Data = new ScanItemData(index);

            holder.Items.Add(item);
            AssetDatabase.AddObjectToAsset(item, holder);
        }

        private void RemoveItem(object sender, ScanItem.OnRemovedEventArgs args)
        {
            ScanItem item = (ScanItem)sender;
            item.OnRemovedEvent -= RemoveItem;

            for (int i = args.Index + 1; i < holder.Items.Count; i++)
            {
                holder.Items[i].Data.Index = i - 1;
                holder.Items[i].name = $"{i - 1}:";
            }

            holder.Items.RemoveAt(args.Index);

            AssetDatabase.RemoveObjectFromAsset(item);
            DestroyImmediate(item);

            Reimport();
        }
        private void DisplayItemListInfo()
        {
            if (holder.Items == null)
            {
                holder.Items = new List<ScanItem>();
            }

            EditorGUILayout.LabelField($"{holder.Items.Count} Items Stored", EditorStyles.centeredGreyMiniLabel);
        }

        private void Reimport()
        {
            string path = AssetDatabase.GetAssetPath(holder.GetInstanceID());
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ImportRecursive);
        }
    }
}