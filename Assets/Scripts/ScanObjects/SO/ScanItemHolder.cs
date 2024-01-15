using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GrassGame.ScanObjects.SO
{
    [CreateAssetMenu(fileName = "Holder", menuName = "Scriptable Object/ScanObjects/Scan Item Holder")]
    public class ScanItemHolder : ScriptableObject
    {
        public List<ScanItem> Items { get; set; }
    }
}