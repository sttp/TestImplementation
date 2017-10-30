namespace Sttp.WireProtocol
{
    /// <summary>
    /// All of the permitted commands for metadata.
    /// </summary>
    public enum MetadataSubCommand : byte
    {
        /// <summary>
        /// An invalid command to indicate that nothing is assigned.
        /// This cannot be sent over the wire.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Clears the existing database.
        /// </summary>
        Clear,

        /// <summary>
        /// Adds a table.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// string tableName, 
        /// TableFlags flags
        /// </summary>
        AddTable,

        /// <summary>
        /// Adds a column.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// short columnIndex, 
        /// string columnName, 
        /// ValueType columnType
        /// 
        /// </summary>
        AddColumn,

        /// <summary>
        /// Adds a row to an existing table.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// int rowIndex,
        /// 
        /// </summary>
        AddRow,

        /// <summary>
        /// Adds or updates a value. Deleting a value would be to assign it with null.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// short columnIndex, 
        /// int rowIndex, 
        /// byte[] value
        /// 
        /// </summary>
        AddValue,

        /// <summary>
        /// Removes an entire row of data.
        /// 
        /// Payload: 
        /// short tableIndex,
        /// int rowIndex,
        /// 
        /// </summary>
        DeleteRow,

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
        /// Specifies join clauses for tables so a filter can be applied on a foreign table.
        /// The foreign key must be the primary index of the foreign table. 
        /// Joining data in both tables are not supported.
        /// 
        /// Payload:
        /// short tableIndex         - The table that has the foreign key
        /// short columnIndex        - The column that has the foreign key
        /// short foreignTableIndex  - The table that this foreign key references. 
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

        /// <summary>
        /// Gets the schema for the database, This is all tables and columns
        /// 
        /// Payload:
        /// None
        /// 
        /// Response is a series of these commands:
        /// <see cref="AddTable"/>
        /// <see cref="AddColumn"/>
        /// 
        /// </summary>
        GetDatabaseSchema,

        /// <summary>
        /// Gets the current version of the database.
        /// 
        /// Payload:
        /// None
        /// </summary>
        GetDatabaseVersion,



    }
}