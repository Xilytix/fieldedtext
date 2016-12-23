// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    internal sealed class StringFieldConstructor : FieldConstructor
    {
        protected override int GetDataType() { return FtStringFieldDefinition.DataType; }

        protected internal override FtMetaField CreateMetaField(int headingCount) { return new FtStringMetaField(headingCount); }
        protected internal override FtFieldDefinition CreateFieldDefinition(int index) { return new FtStringFieldDefinition(index); }
        protected internal override FtField CreateField(FtSequenceInvokation sequenceInvokation, FtSequenceItem sequenceItem)
        {
            return new FtStringField(sequenceInvokation, sequenceItem, sequenceItem.FieldDefinition as FtStringFieldDefinition);
        }
    }
}
