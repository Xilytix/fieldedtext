// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Text;

namespace Xilytix.FieldedText.Serialization.Formatting
{
    internal struct DeclaredParametersFormatter
    {
        private enum InQuotes { Out, In, InCheckingStuffed };

        private const char NameValueSeparator = '=';
        private const char QuoteChar = '"';
        private const string DoubleQuoteString = "\"\"";
        private const char SpaceCharacter = ' ';

        private static string ToText(DeclaredParameters.Rec[] recArray, int count)
        {
            StringBuilder resultBldr = new StringBuilder(50);
            for (int i = 0; i < count; i++)
            {
                string name = recArray[i].Name;
                if (name.IndexOf(NameValueSeparator) >= 0)
                    throw new FtSerializationException(FtSerializationError.DeclaredParameterNameContainsSeparator, string.Format(Properties.Resources.DeclaredParametersFormatter_ToText_NameContainsSeparatorCharacter, NameValueSeparator));
                else
                {
                    string value = recArray[i].Value;
                    value = value.Replace(QuoteChar.ToString(), DoubleQuoteString);

                    if (i > 0)
                    {
                        resultBldr.Append(SpaceCharacter);
                    }
                    resultBldr.Append(name);
                    resultBldr.Append(NameValueSeparator);
                    resultBldr.Append(QuoteChar);
                    resultBldr.Append(value);
                    resultBldr.Append(QuoteChar);
                }
            }

            return resultBldr.ToString();
        }

        private static bool ExtractValue(string asStringValue,
                                         string name,
                                         int position,
                                         out string value,
                                         out int newPosition,
                                         out string errorDescription)
        {
            errorDescription = "";
            bool result = false;

            int asStringValueLength = asStringValue.Length;
            InQuotes inQuotes = InQuotes.Out;

            StringBuilder valueBldr = new StringBuilder(40);

            int i = position;

            while (!result && errorDescription == "")
            {
                switch (inQuotes)
                {
                    case InQuotes.Out:
                        if (i >= asStringValueLength)
                            errorDescription = string.Format(Properties.Resources.DeclaredParametersFormatter_ExtractValue_ParameterIsMissingValue, name, i);
                        else
                        {
                            if (!char.IsWhiteSpace(asStringValue[i]))
                            {
                                if (asStringValue[i] != QuoteChar)
                                    errorDescription = string.Format(Properties.Resources.DeclaredParametersFormatter_ExtractValue_ValueForParameterIsNotQuoted, name, i);
                                else
                                    inQuotes = InQuotes.In;
                            }
                        }
                        break;

                    case InQuotes.In:
                        if (i >= asStringValueLength)
                            errorDescription = string.Format(Properties.Resources.DeclaredParametersFormatter_ExtractValue_ValueForParameterIsMissingClosingQuote, name, i);
                        else
                        {
                            if (asStringValue[i] != QuoteChar)
                                valueBldr.Append(asStringValue[i]);
                            else
                                inQuotes = InQuotes.InCheckingStuffed;
                        }
                        break;

                    case InQuotes.InCheckingStuffed:
                        if (i >= asStringValueLength)
                            result = true;
                        else
                        {
                            if (asStringValue[i] != QuoteChar)
                                result = true;
                            else
                            {
                                valueBldr.Append(QuoteChar);
                                inQuotes = InQuotes.In;
                            }
                        }
                        break;

                    default:
                        throw FtInternalException.Create(InternalError.DeclaredParametersFormatter_ExtractValue_UnknownInQuotes, inQuotes.ToString());
                }
            }

            value = valueBldr.ToString();
            newPosition = i;

            return result;
        }

        internal static string ToSignatureLineText(DeclaredParameters parameters)
        {
            DeclaredParameters.Rec versionRec;
            if (!parameters.TryGetVersionRec(out versionRec))
                throw FtInternalException.Create(InternalError.DeclaredParametersFormatter_ToSignatureLineText_VersionNotSpecified);
            else
            {
                DeclaredParameters.Rec[] recArray = new DeclaredParameters.Rec[1];
                recArray[0] = versionRec;
                return ToText(recArray, 1);
            }
        }

        internal static string ToLine2Text(DeclaredParameters parameters)
        {
            DeclaredParameters.Rec[] recArray;
            int recCount;
            parameters.GetAllRecsExceptVersion(out recArray, out recCount);
            return ToText(recArray, recCount);
        }

        internal static bool TryParse(string text, out DeclaredParameters parameters, out string errorDescription)
        {
            parameters = new DeclaredParameters();
            return TryAppendParse(text, ref parameters, out errorDescription);
        }
        internal static bool TryAppendParse(string text, ref DeclaredParameters parameters, out string errorDescription)
        {
            errorDescription = "";
            text = text.Trim();
            int asStringValueLength = text.Length;
            int currentPos = 0;

            while (currentPos < asStringValueLength && errorDescription == "")
            {
                int separatorPos = text.IndexOf(NameValueSeparator, currentPos);
                if (separatorPos < 0)
                    errorDescription = string.Format(Properties.Resources.DeclaredParametersFormatter_TryParse_ParameterMissingSeparator, currentPos);
                else
                {
                    string name = text.Substring(currentPos, separatorPos - currentPos).Trim();
                    string value;
                    if (ExtractValue(text, name, separatorPos + 1, out value, out currentPos, out errorDescription))
                    {
                        parameters.Add(name, value);
                    };
                };
            };

            return errorDescription == "";
        }
    }
}
