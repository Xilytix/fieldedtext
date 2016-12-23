// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText.Factory
{
    public abstract class FieldConstructor
    {
        protected abstract int GetDataType();

        protected internal int DataType { get { return GetDataType(); } }
        protected internal string DataTypeName { get { return FtStandardDataType.ToName(DataType); } }
        protected internal abstract FtMetaField CreateMetaField(int headingCount);
        protected internal abstract FtFieldDefinition CreateFieldDefinition(int index);
        protected internal abstract FtField CreateField(FtSequenceInvokation sequenceInvokation, FtSequenceItem sequenceItem);
    }
}
