// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Serialization;

    public class FtSubstitution
    {
        private int index;
        private FtSubstitutionType type;
        private char token;
        private string value;

        internal FtSubstitution(int myIndex) { index = myIndex; }

        public FtSubstitutionType Type { get { return type; } }
        public char Token { get { return token; } }
        public string Value { get { return value; } }

        public void LoadMeta(FtMetaSubstitution metaSubstitution, FtEndOfLineAutoWriteType autoWriteType)
        {
            type = metaSubstitution.Type;
            token = metaSubstitution.Token;
            switch (type)
            {
                case FtSubstitutionType.String:
                    value = metaSubstitution.Value;
                    break;
                case FtSubstitutionType.AutoEndOfLine:
                    switch (autoWriteType)
                    {
                        case FtEndOfLineAutoWriteType.CrLf:
                            value = SerializationCore.CarriageReturnLineFeedString;
                            break;
                        case FtEndOfLineAutoWriteType.Cr:
                            value = SerializationCore.CarriageReturnChar.ToString();
                            break;
                        case FtEndOfLineAutoWriteType.Lf:
                            value = SerializationCore.LineFeedChar.ToString();
                            break;
                        case FtEndOfLineAutoWriteType.Local:
                            value = System.Environment.NewLine;
                            break;
                        default:
                            throw FtInternalException.Create(InternalError.FtSubstitution_LoadMeta_UnsupportedAutoWriteType, autoWriteType.ToString());
                    }
                    value = metaSubstitution.Value;
                    break;
            }
        }
    }
}
