using System.Data.Common;

namespace Sttp.Core.Data
{
    public interface IMetadataProcedureHandler
    {
        DbDataReader ProcessRequest(SttpMarkup options);
    }
}