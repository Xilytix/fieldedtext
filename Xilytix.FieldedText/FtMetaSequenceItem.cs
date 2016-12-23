// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    using Factory;

    public class FtMetaSequenceItem
    {
        public enum PropertyId
        {
            Index,
            FieldIndex
        }

        private FtMetaSequenceRedirectList redirectList;

        public FtMetaSequenceItem() { redirectList = new FtMetaSequenceRedirectList(); }

        public FtMetaField Field { get; set; }
        public FtMetaSequenceRedirectList RedirectList {  get { return redirectList; } }

        public virtual void LoadDefaults()
        {
            // nothing to do
        }

        protected internal FtMetaSequenceItem CreateCopyExcludingRedirects(FtMetaFieldList fieldList, FtMetaFieldList sourceFieldList)
        {
            FtMetaSequenceItem item = SequenceItemFactory.CreateMetaSequenceItem();
            item.AssignExcludingRedirects(this, fieldList, sourceFieldList);
            return item;
        }
        protected internal void AssignExcludingRedirects(FtMetaSequenceItem source, FtMetaFieldList fieldList, FtMetaFieldList sourceFieldList)
        {
            int fieldIndex = sourceFieldList.IndexOf(source.Field);
            if (fieldIndex < 0)
                throw FtInternalException.Create(InternalError.FtMetaSequenceItem_AssignExcludingRedirects_SourceFieldNotFound); // should never happen
            else
            {
                if (fieldIndex >= fieldList.Count)
                    throw FtInternalException.Create(InternalError.FtMetaSequenceItem_AssignExcludingRedirects_FieldIndexOutOfRange, fieldIndex.ToString()); // should never happen
                else
                    Field = fieldList[fieldIndex];
            }
        }
        protected internal void AssignRedirects(FtMetaSequenceList sequenceList, FtMetaSequenceList sourceSequenceList)
        {
            redirectList.Assign(redirectList, sequenceList, sourceSequenceList);
        }

        internal bool HasConstantFieldAndHasRedirects()
        {
            return Field != null && Field.Constant && RedirectList.Count > 0;
        }
    }
}
