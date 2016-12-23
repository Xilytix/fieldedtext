// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    using Factory;
    using MetaSerialization;

    public class FtMetaSequence
    {
        public enum PropertyId
        {
            Name,
            Root,
            FieldIndices,
        }

        public const string DefaultName = "";
        public const bool DefaultRoot = MetaSerializationDefaults.Sequence.Root;

        internal delegate void RootedDelegate(FtMetaSequence sender);
        internal RootedDelegate RootedEvent;

        private FtMetaSequenceItemList itemList;
        private bool root;

        private void SetRoot(bool value)
        {
            root = value;
            if (root)
            {
                RootedEvent(this);
            }
        }

        public FtMetaSequence()
        {
            itemList = new FtMetaSequenceItemList();
            LoadBaseDefaults();
        }

        public string Name { get; set; }
        public bool Root { get { return root; } set { SetRoot(value); } }
        public FtMetaSequenceItemList ItemList { get { return itemList; } }

        public virtual void LoadDefaults()
        {
            LoadBaseDefaults();
        }

        private void LoadBaseDefaults()
        {
            Name = DefaultName;
            Root = DefaultRoot;
        }

        public static bool SameName(string left, string right) { return string.Equals(left, right, StringComparison.OrdinalIgnoreCase); }

        public void RemoveItemsWithField(FtMetaField field)
        {
            for (int i = itemList.Count-1; i>=0; i--)
            {
                if (itemList[i].Field == field)
                {
                    itemList.RemoveAt(i);
                }
            }
        }
        public void RemoveAllItems() { itemList.Clear(); }

        internal FtMetaSequence CreateCopyExcludingRedirects(FtMetaFieldList fieldList, FtMetaFieldList sourceFieldList)
        {
            FtMetaSequence sequence = SequenceFactory.CreateMetaSequence();
            sequence.AssignExcludingRedirects(this, fieldList, sourceFieldList);
            return sequence;
        }

        internal protected virtual void AssignExcludingRedirects(FtMetaSequence source, FtMetaFieldList fieldList, FtMetaFieldList sourceFieldList)
        {
            Name = source.Name;
            root = source.Root;

            itemList.AssignExcludingRedirects(source.itemList, fieldList, sourceFieldList);
        }

        internal void AssignRedirects(FtMetaSequenceList sequenceList, FtMetaSequenceList sourceSequenceList)
        {
            itemList.AssignRedirects(sequenceList, sourceSequenceList);
        }

        internal bool AnyItemWithNullField(out int itemIndex)
        {
            itemIndex = -1;
            bool result = false;

            for (int i = 0; i < ItemList.Count; i++)
            {
                FtMetaSequenceItem item = ItemList[i];

                if (item.Field == null)
                {
                    itemIndex = i;
                    result = true;
                    break;
                }
            }

            return result;
        }
        internal bool AnyItemWithConstantFieldHasRedirects(out int itemIndex)
        {
            itemIndex = -1;
            bool result = false;

            for (int i = 0; i < ItemList.Count; i++)
            {
                FtMetaSequenceItem item = ItemList[i];

                if (item.HasConstantFieldAndHasRedirects())
                {
                    itemIndex = i;
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
