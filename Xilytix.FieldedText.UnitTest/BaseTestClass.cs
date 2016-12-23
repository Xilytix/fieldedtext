// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.IO;
using System.Xml;

namespace Xilytix.FieldedText.UnitTest
{
    public class BaseTestClass
    {
        private const string BaseTestFolderName = "FieldedTextTest";

        private string testFolder;
        protected string TestFolder { get { return testFolder; } }
        protected XmlWriterSettings xmlWriterSettings;

        public BaseTestClass(string myTestFolderName)
        {
            if (myTestFolderName != "")
            {
                string tempPath = Path.GetTempPath();
                string baseTestFolder = Path.Combine(tempPath, BaseTestFolderName);
                testFolder = Path.Combine(baseTestFolder, myTestFolderName);

                DirectoryInfo dirInfo = new DirectoryInfo(testFolder);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                foreach (FileInfo fileInfo in dirInfo.GetFiles())
                {
                    fileInfo.Delete();
                }
            }

            xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "  ";
            xmlWriterSettings.NewLineChars = "\x0D\x0A";
        }

        protected bool TextFilesAreEqual(string leftFilePath, string rightFilePath)
        {
            StreamReader leftReader = new StreamReader(leftFilePath);
            StreamReader rightReader = new StreamReader(rightFilePath);

            bool result = true;
            int leftInt;
            int rightInt;
            do
            {
                leftInt = leftReader.Read();
                rightInt = rightReader.Read();

                if (leftInt != rightInt)
                {
                    result = false;
                    break;
                }
            }
            while (leftInt != -1 && rightInt != -1);

            return result;
        }
    }
}
