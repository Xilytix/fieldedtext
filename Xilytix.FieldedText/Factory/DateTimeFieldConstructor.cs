// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    internal sealed class DateTimeFieldConstructor : FieldConstructor
    {
        protected override int GetDataType() { return FtDateTimeFieldDefinition.DataType; }

        protected internal override FtMetaField CreateMetaField(int headingCount) { return new FtDateTimeMetaField(headingCount); }
        protected internal override FtFieldDefinition CreateFieldDefinition(int index) { return new FtDateTimeFieldDefinition(index); }
        protected internal override FtField CreateField(FtSequenceInvokation sequenceInvokation, FtSequenceItem sequenceItem)
        {
            return new FtDateTimeField(sequenceInvokation, sequenceItem, sequenceItem.FieldDefinition as FtDateTimeFieldDefinition);
        }
    }
}
