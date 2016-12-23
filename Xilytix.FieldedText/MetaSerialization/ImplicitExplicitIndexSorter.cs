// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;

namespace Xilytix.FieldedText.MetaSerialization
{
    internal static class ImplicitExplicitIndexSorter<T>
    {
        internal struct Rec: IComparable<Rec>
        {
            internal T Target;
            internal int Implicit;
            internal int Explicit;

            public int CompareTo(Rec other)
            {
                bool gotExplicit = Explicit >= 0;
                int idx = gotExplicit ? Explicit : Implicit;
                bool otherGotExplicit = other.Explicit >= 0;
                int otherIdx = otherGotExplicit ? other.Explicit : other.Implicit;

                int result = idx.CompareTo(otherIdx);

                if (result == 0)
                {
                    if (gotExplicit && !otherGotExplicit)
                        result = -1;
                    else
                    {
                        if (!gotExplicit && otherGotExplicit)
                        {
                            result = 1;
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Sorts type T according to its implicit and explicit index
        /// </summary>
        /// <param name="recArray">Array of Rec which specify instance of T and the instance's implicit and explicit index</param>
        /// <param name="sortedT">Sorted array of T instances</param>
        /// <param name="duplicateExplicitIndex">Duplicate Explicit Index detected in array of Recs or -1 if no duplicates detected</param>
        /// <returns>true if success.  false if duplicate explicit index was detected</returns>

        internal static bool TrySort(Rec[] recArray, out T[] sortedT, out int duplicateExplicitIndex)
        {
            duplicateExplicitIndex = -1;
            if (recArray.Length == 0)
            {
                sortedT = new T[0];
                return true;
            }
            else
            {
                sortedT = new T[recArray.Length];

                Array.Sort<Rec>(recArray);

                sortedT[0] = recArray[0].Target;

                bool result = true;

                for (int i = 1; i < recArray.Length; i++)
                {
                    bool IsDuplicate = recArray[i].Explicit >= 0
                                       &&
                                       recArray[i - 1].Explicit >= 0
                                       &&
                                       recArray[i].Explicit == recArray[i-1].Explicit;
                    if (!IsDuplicate)
                        sortedT[i] = recArray[i].Target;
                    else
                    {
                        duplicateExplicitIndex = recArray[i].Explicit;
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }
    }
}
