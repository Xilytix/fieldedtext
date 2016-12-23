// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    public class FtSequence
    {
        private const string AutoRootName = "AutoRoot";

        private int index;

        private string name;
        private bool root;
        private FtSequenceItemList itemList;

        internal protected FtSequence(int myIndex) { index = myIndex; itemList = new FtSequenceItemList(); }

        public int Index { get { return index; } }

        public string Name { get { return name; } }
        public bool Root { get { return root; } }
        public FtSequenceItemList ItemList { get { return itemList; } }

        internal protected void SetRoot(bool value) { root = value; }

        internal void LoadMeta(FtMetaSequence metaSequence, FtMetaFieldList metaFieldList, FtFieldDefinitionList fieldDefinitionList)
        {
            name = metaSequence.Name;
            root = metaSequence.Root;

            for (int i = 0; i < metaSequence.ItemList.Count; i++)
            {
                FtSequenceItem Item = itemList.New();
                Item.LoadMeta(metaSequence.ItemList[i], metaFieldList, fieldDefinitionList);
            }
        }

        internal void LoadMetaSequenceRedirects(FtMetaSequence metaSequence, FtMetaSequenceList metaSequenceList, FtSequenceList sequenceList)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                // Note that Sequence Items are in same order as MetaSequence Items
                itemList[i].LoadMetaSequenceRedirects(metaSequence.ItemList[i], metaSequenceList, sequenceList);
            }
        }

        internal void LoadRootFieldDefinitionList(FtFieldDefinitionList fieldDefinitionList)
        {
            name = AutoRootName;
            root = true;
            for (int i = 0; i < fieldDefinitionList.Count; i++)
            {
                FtSequenceItem item = itemList.New();
                item.SetFieldDefinition(fieldDefinitionList[i]);
            }
        }
    }
}
