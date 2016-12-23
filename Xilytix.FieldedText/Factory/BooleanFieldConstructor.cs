// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    internal sealed class BooleanFieldConstructor: FieldConstructor
    {
        protected override int GetDataType() { return FtBooleanFieldDefinition.DataType;  }

        protected internal override FtMetaField CreateMetaField(int headingCount) { return new FtBooleanMetaField(headingCount); }
        protected internal override FtFieldDefinition CreateFieldDefinition(int index) { return new FtBooleanFieldDefinition(index); }
        protected internal override FtField CreateField(FtSequenceInvokation sequenceInvokation, FtSequenceItem sequenceItem)
        {
            return new FtBooleanField(sequenceInvokation, sequenceItem, sequenceItem.FieldDefinition as FtBooleanFieldDefinition);
        }
    }
}
