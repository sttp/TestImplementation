namespace CTP
{
    internal enum CtpDocumentHeader
    {
        StartElement = 0,
        EndElement = 1,
        ValueNull = 2,
        ValueInt64 = 3,
        ValueInvertedInt64 = 4,
        ValueSingle = 5,
        ValueDouble = 6,
        ValueCtpTime = 7,
        ValueBooleanTrue = 8,
        ValueBooleanFalse = 9,
        ValueGuid = 10,
        ValueString = 11,
        ValueCtpBuffer = 12,
        ValueCtpDocument = 13,
    }
}