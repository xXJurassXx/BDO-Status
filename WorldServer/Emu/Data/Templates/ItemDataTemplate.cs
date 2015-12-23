using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WorldServer.Emu.Data.Templates
{
    [Serializable][XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "Item_Table", Namespace = "", IsNullable = false)]
    public class ItemDataTemplate
    {
        [XmlElement("Item", Form = XmlSchemaForm.Unqualified)]
        public List<ItemTemplate> ItemTemplateList;

        [XmlIgnore]
        private Dictionary<int, ItemTemplate> _byId;

        private readonly object _iLock = new object();

        [XmlIgnore]
        public Dictionary<int, ItemTemplate> TemplatesById
        {
            get
            {
                lock(_iLock)
                 return _byId ?? (_byId = ItemTemplateList.ToDictionary(el => el.ItemId));                
            }
        }

        [XmlIgnore]
        public ItemTemplate this[int index] => TemplatesById[index];
    }
    [Serializable][XmlType(AnonymousType = true)]
    public class ItemTemplate
    {
        [XmlElement("Index")] public int ItemId;
        [XmlElement("ItemName")] public string ItemName;
        [XmlElement("ItemType")] public int Type;
        [XmlElement("ItemClassify")] public string Classify;
        [XmlElement("GradeType")] public string GradeType;
        [XmlElement("EquipType")] public string EquipType;
        [XmlElement("IconImageFile")] public string IconImageFile;
        [XmlElement("OriginalPrice")] public string OriginalPrice;
        [XmlElement("SellPriceToNpc")] public string SellPriceToNpc;
        [XmlElement("RepairPrice")] public string RepairPrice;
        [XmlElement("RepairTime")] public string RepairTime;
        [XmlElement("Description")] public string Description;
        [XmlElement("MinLevel")] public string MinLevel;
        [XmlElement("MaxLevel")] public string MaxLevel;
        [XmlElement("ItemAccessLevel")] public string ItemAccessLevel;
        [XmlIgnore] public int MaxStack = 1;
    }
}
