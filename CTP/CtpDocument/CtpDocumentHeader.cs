namespace CTP
{
    internal enum CtpDocumentHeader
    {
        ValueNull = 0,
        ValueInt64 = 1,
        ValueSingle = 2,
        ValueDouble = 3,
        ValueCtpTime = 4,
        ValueBoolean = 5,
        ValueGuid = 6,
        ValueString = 7,
        ValueCtpBuffer = 8,
        ValueCtpDocument = 9,
        StartElement = 10,
        EndElement = 11,
        StartArrayElement = 12,
    }
}