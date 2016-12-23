// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using System;
using System.Runtime.Serialization;

namespace Xilytix.FieldedText
{
    [Serializable]
    public class FtSerializationException: FtException
    {
        [NonSerialized]
        private State state;

        [Serializable]
        private struct State : ISafeSerializationData
        {
            private FtSerializationError error;
            private string fieldName;
            private int fieldIndex;
            private string sequenceName;
            private int sequenceItemIndex;

            public FtSerializationError Error { get { return error; } set { error = value; } }
            public string FieldName { get { return fieldName; } set { fieldName = value; } }
            public int FieldIndex { get { return fieldIndex; } set { fieldIndex = value; } }
            public string SequenceName { get { return sequenceName; } set { sequenceName = value; } }
            public int SequenceItemIndex { get { return sequenceItemIndex; } set { sequenceItemIndex = value; } }

            void ISafeSerializationData.CompleteDeserialization(object obj)
            {
                FtSerializationException exception = obj as FtSerializationException;
                exception.state = this;
            }
        }

        public FtSerializationException(FtSerializationError error, FtField field, string message, Exception innerException): base(Enum.GetName(typeof(FtSerializationError), error) + ((message == "")? "": (": " + message)), innerException)
        {
            state.Error = error;
            if (field == null)
            {
                state.FieldName = null;
                state.FieldIndex = -1;
                state.SequenceName = null;
                state.SequenceItemIndex = -1;
            }
            else
            {
                state.FieldName = field.Name;
                state.FieldIndex = field.Index;
                FtSequence sequence = field.Sequence;
                if (sequence == null)
                    state.SequenceName = null;
                else
                    state.SequenceName = sequence.Name;
                FtSequenceItem sequenceItem = field.SequenceItem;
                if (sequenceItem == null)
                    state.SequenceItemIndex = -1;
                else
                    state.SequenceItemIndex = sequenceItem.Index;
            }

            SerializeObjectState += delegate (object exception, SafeSerializationEventArgs eventArgs)
            {
                eventArgs.AddSerializedState(state);
            };
        }

        public FtSerializationException(FtSerializationError code, string message) : this(code, null, message, null)
        {
            
        }

        public FtSerializationException(FtSerializationError code, string message, Exception innerException) : this(code, null, message, innerException)
        {

        }

        public FtSerializationException(FtSerializationError code, FtField field, string message) : this(code, field, message, null)
        {
            
        }

        public FtSerializationException(FtSerializationError code, FtField field, Exception innerException) : this(code, field, innerException.Message, innerException)
        {
            
        }

        public FtSerializationError Error { get { return state.Error; } }
        public string FieldName { get { return state.FieldName; } }
        public int FieldIndex { get { return state.FieldIndex; } }
        public string SequenceName { get { return state.SequenceName; } }
        public int SequenceItemIndex { get { return state.SequenceItemIndex; } }
    }
}
