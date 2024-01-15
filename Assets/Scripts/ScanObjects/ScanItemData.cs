using UnityEngine;

namespace GrassGame.ScanObjects
{
    public class ScanItemData
    {
        public int Index { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }

        public ScanItemData(int index, string name, string description)
        {
            Index = index;
            ItemName = name;
            ItemDescription = description;
        }

        public ScanItemData(int index)
        {
            Index = index;
            ItemName = "";
            ItemDescription = "";
        }
    }
}