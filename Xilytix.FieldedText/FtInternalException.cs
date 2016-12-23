// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText
{
    [Serializable]
    public class FtInternalException: FtException
    {
        private FtInternalException(string message) : base(message)
        {
        }

        internal static FtInternalException Create(InternalError error,
                                                   string message  = "",
                                                   [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
                                                   [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
                                                   [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string errorMessage = ((int)error).ToString();
            if (message.Length > 0)
            {
                errorMessage += " (" + message + ")";
            }
            return new FtInternalException(string.Format(Properties.Resources.InternalExceptionMessage, 
                                                         new object[] {errorMessage, memberName, sourceFilePath, sourceLineNumber }));
        }
    }
}
