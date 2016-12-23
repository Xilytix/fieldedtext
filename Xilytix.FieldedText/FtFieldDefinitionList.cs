// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System.Collections.Generic;

namespace Xilytix.FieldedText
{
    using Factory;
    using List = List<FtFieldDefinition>;
    public class FtFieldDefinitionList
    {
        private List list;

        internal FtFieldDefinitionList() { list = new List(); }

        public int Count { get { return list.Count; } }
        public FtFieldDefinition this[int idx] { get { return list[idx]; } }

        internal void Clear() { list.Clear(); }
        internal int Capacity { get { return list.Capacity; } set { list.Capacity = value; } }
        internal FtFieldDefinition New(int dataType)
        {
            FtFieldDefinition definition = FieldFactory.CreateFieldDefinition(Count, dataType);
            list.Add(definition);
            return definition;
        }
    }
}
