using System;

namespace CTP
{
    public class CommandSchemaNode
    {
        public readonly CommandSchemaSymbol Symbol;

        public string NodeName { get; set; }
        /// <summary>
        /// This is the element or array that this node belongs to.
        /// </summary>
        public CommandSchemaNode ParentNode { get; private set; }
        /// <summary>
        /// This is the node that is next in sequence as defined by the CommandSchema.
        /// Null at the end.
        /// </summary>
        public CommandSchemaNode NextNode { get; private set; }
        /// <summary>
        /// This is the other node for Array's or Elements. For Define, it's End, for End, it's Define. For values, it's null
        /// </summary>
        public CommandSchemaNode PairedNode { get; private set; }

        private bool m_isReadOnly;

        public CommandSchemaNode(string nodeName, CommandSchemaSymbol symbol)
        {
            NodeName = nodeName;
            Symbol = symbol;
        }

        public void SetNodeName(string nodeName)
        {
            if (m_isReadOnly)
                throw new InvalidOperationException("Node already marked as read only");
            NodeName = nodeName;
        }

        internal void SetNextNode(CommandSchemaNode node)
        {
            if (m_isReadOnly)
                throw new InvalidOperationException("Node already marked as read only");
            NextNode = node;
        }

        internal void SetPairedNode(CommandSchemaNode node)
        {
            if (m_isReadOnly)
                throw new InvalidOperationException("Node already marked as read only");
            PairedNode = node;
        }

        internal void SetParentNode(CommandSchemaNode node)
        {
            if (m_isReadOnly)
                throw new InvalidOperationException("Node already marked as read only");
            ParentNode = node;
        }

        internal void SetReadOnly()
        {
            m_isReadOnly = true;
        }

        public override string ToString()
        {
            return $"{Symbol} {NodeName}";
        }
    }
}