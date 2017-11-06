namespace Sttp.WireProtocol.GetMetadata
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
        /// Indicates the current version of the database. This is required for update queries. Optional otherwise.
        /// If specified, a response of FailedRequest will result if the schemaVersion is mismatched.
        /// 
        /// Payload:
        /// Guid schemaVersion, 
        /// long revision, 
        /// bool isUpdateQuery       - Specifies that this query should only be run on rows that has been modified since the specified revision.
        /// 
        /// </summary>
        DatabaseVersion,

        /// <summary>
        /// Specifies a set of columns that are part of a data request.
        /// 
        /// Payload:
        /// string tableName        - Can be a renamed joined table.
        /// string columnName       - The original name of the column
        /// string aliasName        - The aliased name of the column.
        /// 
        /// </summary>
        Select,

        /// <summary>
        /// Traverses a predefined table relationship as part of this query.
        /// 
        /// Payload:
        /// string TableName         - The table that has the column with the foreign key.
        /// string ColumnName        - The name of the column with the foreign key.
        /// string ForeignTableName  - The foreign table that has the key. It could be itself of course.
        /// string TableAlias        - The joined table will be renamed with this name. Aliases will take precedence over other named tables.
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
        /// string tableName        - Can be a renamed joined table.
        /// string columnName       - The original name of the column
        /// bool areItemsRegularExpressions,
        /// string[] Items,
        /// </summary>
        WhereInString,

        /// <summary>
        /// Specifies a filter clause to apply to the data. 
        /// 
        /// Payload:
        /// string tableName        - Can be a renamed joined table.
        /// string columnName       - The original name of the column
        /// SttpValueSet Items,
        /// </summary>
        WhereInValue,

        /// <summary>
        /// Specifies a filter clause to apply to the data. 
        /// The item's type must match the type for the specified column and must naturally support the specified operator.
        /// 
        /// Payload:
        /// string tableName        - Can be a renamed joined table.
        /// string columnName       - The original name of the column
        /// byte CompareMethod (gt, lt, eq, ne, gte, lte)
        /// SttpValue Item,
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