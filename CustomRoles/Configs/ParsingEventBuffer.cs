namespace CustomRoles.Configs
{
    using System.Collections.Generic;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;

    public class ParsingEventBuffer : IParser
    {
        private readonly LinkedList<ParsingEvent> _buffer;

        private LinkedListNode<ParsingEvent> _current;

        public ParsingEventBuffer(LinkedList<ParsingEvent> events)
        {
            _buffer = events;
            _current = events.First;
        }

        public ParsingEvent Current => _current?.Value;

        public bool MoveNext()
        {
            _current = _current.Next;
            return _current is not null;
        }

        public void Reset()
        {
            _current = _buffer.First;
        }
    }
}