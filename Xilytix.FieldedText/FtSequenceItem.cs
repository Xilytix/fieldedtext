// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    public class FtSequenceItem
    {
        private int index;

        private FtFieldDefinition fieldDefinition;
        private FtSequenceRedirectList redirectList;

        internal protected FtSequenceItem(int myIndex) { index = myIndex; redirectList = new FtSequenceRedirectList(); }

        public int Index { get { return index; } }

        public FtFieldDefinition FieldDefinition { get { return fieldDefinition; } }
        public FtSequenceRedirectList RedirectList { get { return redirectList; } }

        internal void LoadMeta(FtMetaSequenceItem metaSequenceItem, FtMetaFieldList metaFieldList, FtFieldDefinitionList fieldDefinitionList)
        {
            int fieldIdx = metaFieldList.IndexOf(metaSequenceItem.Field);
            if (fieldIdx < 0)
                throw FtInternalException.Create(InternalError.FtSequenceItem_LoadMeta_MetaSequenceItemFieldNotFoundInMetaFieldList, metaSequenceItem.Field.Name); // should never happen
            else
                fieldDefinition = fieldDefinitionList[fieldIdx]; // fieldDefinitions are in same order as Meta Fields
        }

        internal protected void LoadMetaSequenceRedirects(FtMetaSequenceItem metaSequenceItem, FtMetaSequenceList metaSequenceList, FtSequenceList sequenceList)
        {
            for (int i = 0; i < metaSequenceItem.RedirectList.Count; i++)
            {
                int redirectType = metaSequenceItem.RedirectList[i].Type;
                FtSequenceRedirect redirect = redirectList.New(redirectType);
                redirect.LoadMeta(metaSequenceItem.RedirectList[i], metaSequenceList, sequenceList);
            }
        }

        internal void SetFieldDefinition(FtFieldDefinition definition)
        {
            fieldDefinition = definition;
        }
    }
}
