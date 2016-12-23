// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText.Serialization
{
    public class DeclaredParameters
    {
        private const string VersionParameterName = "Version";
        private const char VersionPartsSeparator = '.';
        private const string MetaUrlParameterName = "MetaUrl";
        private const string MetaFileParameterName = "MetaFile";
        private const string MetaEmbeddedParameterName = "MetaEmbedded";
        private const string MetaEmbeddedParameterValue = "True";

        internal struct Rec
        {
            public string Name;
            public string Value;
        }

        private List<Rec> list; // does not include version

        private string VersionToText(int major, int minor, string comment)
        {
            string majorMinor = FtStandardText.Get(major) + VersionPartsSeparator.ToString() + FtStandardText.Get(minor);

            if (comment.Length == 0)
                return majorMinor;
            else
                return majorMinor + VersionPartsSeparator.ToString() + comment;
        }

        private bool ParseVersionValue(string text, out int major, out int minor, out string comment)
        {
            int majorMinorSeparatorIdx = text.IndexOf(VersionPartsSeparator);
            if (majorMinorSeparatorIdx <= 0 || majorMinorSeparatorIdx >= (text.Length-1))
            {
                major = -1;
                minor = -1;
                comment = "";
                return false;
            }
            else
            {
                int minorCommentSeparatorIdx = text.IndexOf(VersionPartsSeparator, majorMinorSeparatorIdx + 1);
                if (minorCommentSeparatorIdx < 0)
                {
                    comment = "";
                    minorCommentSeparatorIdx = text.Length;
                }
                else
                {
                    if (minorCommentSeparatorIdx >= (text.Length - 1))
                        comment = "";
                    else
                        comment = text.Substring(minorCommentSeparatorIdx + 1);
                }

                string majorText = text.Substring(0, majorMinorSeparatorIdx);
                if (!FtStandardText.TryParse(majorText, out major))
                {
                    major = -1;
                    minor = -1;
                    return false;
                }
                else
                {
                    string minorText = text.Substring(majorMinorSeparatorIdx + 1, minorCommentSeparatorIdx - majorMinorSeparatorIdx-1);
                    return FtStandardText.TryParse(minorText, out minor);
                }
            }
        }

        internal DeclaredParameters() { list = new List<Rec>(); }

        internal void SetVersion(int major, int minor, string comment = "")
        {
            Add(VersionParameterName, VersionToText(major, minor, comment));
        }

        internal bool TryGetVersion(out int major, out int minor, out string comment)
        {
            int idx = IndexOfVersion();
            if (idx >= 0)
                return ParseVersionValue(list[idx].Value, out major, out minor, out comment);
            else
            {
                major = -1;
                minor = -1;
                comment = "";
                return false;
            }
        }

        internal void GetMetaReference(out FtMetaReferenceType type, out string reference)
        {
            int idx = IndexOfName(MetaUrlParameterName);
            if (idx >= 0)
            {
                reference = list[idx].Value;
                type = FtMetaReferenceType.Url;
            }
            else
            {
                idx = IndexOfName(MetaFileParameterName);
                if (idx >= 0)
                {
                    reference = list[idx].Value;
                    type = FtMetaReferenceType.File;
                }
                else
                {
                    idx = IndexOfName(MetaEmbeddedParameterName);
                    if (idx >= 0)
                    {
                        reference = list[idx].Value; // not relevant
                        type = FtMetaReferenceType.Embedded;
                    }
                    else
                    {
                        reference = ""; // not relevant
                        type = FtMetaReferenceType.None;
                    }
                }
            }
        }
        internal void SetMetaReference(FtMetaReferenceType type, string reference)
        {
            Remove(MetaFileParameterName);
            Remove(MetaUrlParameterName);
            Remove(MetaEmbeddedParameterName);

            switch (type)
            {
                case FtMetaReferenceType.Embedded:
                    Add(MetaEmbeddedParameterName, MetaEmbeddedParameterValue);
                    break;
                case FtMetaReferenceType.File:
                    Add(MetaFileParameterName, reference);
                    break;
                case FtMetaReferenceType.Url:
                    Add(MetaUrlParameterName, reference);
                    break;
                case FtMetaReferenceType.None:
                    break;
                default:
                    throw FtInternalException.Create(InternalError.DeclaredParameters_SetMetaReference_UnknownMetaReferenceType, type.ToString());
            }
        }

        internal bool TryGetVersionRec(out Rec rec)
        {
            int idx = IndexOfName(VersionParameterName);
            if (idx < 0)
            {
                rec = new Rec();
                rec.Name = VersionParameterName;
                return false;
            }
            else
            {
                rec = list[idx];
                return true;
            }
        }

        internal void GetAllRecsExceptVersion(out Rec[] recs, out int recCount)
        {
            recs = new Rec[list.Count];
            recCount = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (!string.Equals(list[i].Name, VersionParameterName, System.StringComparison.OrdinalIgnoreCase))
                {
                    recs[recCount] = list[i];
                    recCount++;
                }
            }
        }

        internal int Count { get { return list.Count; } }

        internal string GetName(int idx) { return list[idx].Name; }
        internal string GetValue(int idx) { return list[idx].Value; }
        internal void Add(string name, string value)
        {
            Rec rec;
            rec.Name = name;
            rec.Value = value;
            int idx = IndexOfName(name);
            if (idx < 0)
                list.Add(rec);
            else
                list[idx] = rec;
        }
        internal void Remove(string name)
        {
            int idx = IndexOfName(name);
            if (idx >= 0)
            {
                list.RemoveAt(idx);
            }
        }
        internal void Clear()
        {
            list.Clear();
        }
        internal int IndexOfName(string name)
        {
            int result = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (string.Equals(list[i].Name, name, System.StringComparison.OrdinalIgnoreCase))
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
        internal int IndexOfVersion()
        {
            return IndexOfName(VersionParameterName);
        }
    }
}
