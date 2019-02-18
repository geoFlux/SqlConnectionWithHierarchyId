using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Types;
namespace GeoFlux.SqlConnectionFix
{
    public class SqlHierarchyIdTypeHandler : Dapper.SqlMapper.TypeHandler<SqlHierarchyId>
    {
        public override SqlHierarchyId Parse(object value)
        {
            if (value is SqlHierarchyId idValue)
                return idValue;
            throw new Exception("not a SqlHierarchyId");
        }

        public override void SetValue(IDbDataParameter parameter, SqlHierarchyId value)
        {
            if (parameter is SqlParameter sqlParam)
            {
                sqlParam.SqlDbType = SqlDbType.Binary;

                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    value.Write(bw);
                    sqlParam.Value = ms.ToArray();
                }
                return;
            }
            throw new NotSupportedException("Only Sql Server is supported for SqlHierarchyId values");
        }
    }
}