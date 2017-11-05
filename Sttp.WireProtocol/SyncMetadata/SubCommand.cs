namespace Sttp.WireProtocol.SyncMetadata
{
    /// <summary>
    /// All of the permitted commands for metadata.
    /// </summary>
    public enum SubCommand : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Indicates the current version of the database.
        /// 
        /// Payload:
        /// Guid majorVersion, 
        /// long minorVersion, 
        /// </summary>
        DatabaseVersion,

        /// <summary>
        /// Specifies a set of columns that are part of a data request.
        /// 
        /// Payload:
        /// short tableIndex
        /// short columnIndex
        /// 
        /// </summary>
        Select,

        /// <summary>
        /// Traverses a predefined table relationship as part of this query.
        /// 
        /// Payload:
        /// short tableIndex         - The table that has the foreign key
        /// short columnIndex        - The column that has the foreign key
        /// short foreignTableIndex  - The table that this foreign key references. 
        /// bool isLeftJoin          - Indicates if this relationship is a left join or an inner join. 
        ///                            Outer Joins are invalid given the one to many relationship requirement 
        ///                            for a table relationship.
        /// 
        /// </summary>
        Join,

        /// <summary>
        /// Specifies a filter clause to apply to the data. To do a single Like statement, include 1 item in the list.
        /// 
        /// Payload:
        /// short tableIndex, 
        /// short columnIndex,
        /// bool areItemsRegularExpressions,
        /// string[] Items,
        /// </summary>
        WhereInString,

        /// <summary>
        /// Specifies a filter clause to apply to the data. 
        /// 
        /// Payload:
        /// short tableIndex, 
        /// short columnIndex,
        /// byte[][] Items,
        /// </summary>
        WhereInValue,

        /// <summary>
        /// Specifies a filter clause to apply to the data. 
        /// The item's type must match the type for the specified column and must naturally support the specified operator.
        /// 
        /// Payload:
        /// short tableIndex, 
        /// short columnIndex,
        /// byte CompareOperator (gt, lt, eq, ne, gte, lte)
        /// byte[] Item,
        /// </summary>
        WhereCompare,

        /// <summary>
        /// Specifies how a boolean expressions will be combined to yield a true/false result. 
        /// Must be specified in a reverse polish notation Left to Right.
        /// 
        /// Payload:
        /// byte OperatorCode (AND, OR, NOT, XOR, NOR, NAND, XNOR)
        /// </summary>
        WhereOperator,
    }
}