namespace CTP
{
internal enum CtpDocumentHeader
{
    StartElement = 0,
    EndElement = 1,
    StartArrayElement = 2,
    ValueNull = 3,
    ValueInt64 = 4,
    ValueInvertedInt64 = 5,
    ValueSingle = 6,
    ValueDouble = 7,
    ValueCtpTime = 8,
    ValueBooleanTrue = 9,
    ValueBooleanFalse = 10,
    ValueGuid = 11,
    ValueString = 12,
    ValueCtpBuffer = 13,
    ValueCtpDocument = 14,
}
}