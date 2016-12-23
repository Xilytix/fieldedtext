// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

namespace Xilytix.FieldedText
{
    using Factory;
    using MetaSerialization;

    public class FtMetaSubstitution
    {
        public enum PropertyId
        {
            Token,
            Type,
            Value,
        }

        public const FtSubstitutionType DefaultType = MetaSerializationDefaults.Substitution.Type;
        public const char DefaultToken = '\\';
        public const string DefaultValue = "";

        public FtMetaSubstitution()
        {
            LoadBaseDefaults();
        }

        public FtSubstitutionType Type { get; set; }
        public char Token { get; set; }
        public string Value { get; set; }

        public virtual void LoadDefaults()
        {
            LoadBaseDefaults();
        }

        private void LoadBaseDefaults()
        {
            Type = DefaultType;
            Token = DefaultToken;
            Value = DefaultValue;
        }

        public FtMetaSubstitution CreateCopy()
        {
            FtMetaSubstitution substitution = SubstitutionFactory.CreateMetaSubstitution();
            substitution.Assign(this);
            return substitution;
        }
        public virtual void Assign(FtMetaSubstitution source)
        {
            Type = source.Type;
            Token = source.Token;
            Value = source.Value;
        }
    }
}
