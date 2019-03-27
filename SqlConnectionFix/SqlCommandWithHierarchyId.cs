using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GeoFlux.SqlConnectionFix
{
    public class SqlCommandWithHierarchyId: DbCommand{
        private readonly SqlCommand i;

        DbCommand ii => i;

        public SqlCommandWithHierarchyId(SqlCommand inner){
            this.i = inner;
        }
        public override string CommandText { get => i.CommandText; set => i.CommandText = value; }
        public override int CommandTimeout { get => i.CommandTimeout; set => i.CommandTimeout = value; }
        public override CommandType CommandType { get => i.CommandType; set => i.CommandType = value; }
        public override bool DesignTimeVisible { get => i.DesignTimeVisible; set => i.DesignTimeVisible = value; }
        public override UpdateRowSource UpdatedRowSource { get => i.UpdatedRowSource; set => i.UpdatedRowSource = value; }
        protected override DbConnection DbConnection { get => i.Connection; set => i.Connection = (SqlConnection)value; }//should maybe throw not implemented

        protected override DbParameterCollection DbParameterCollection => i.Parameters;

        protected override DbTransaction DbTransaction { get => i.Transaction; set => i.Transaction = (SqlTransaction)value; }//should maybe throw not implemented

        public override void Cancel()
        {
            i.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            return i.ExecuteNonQuery();
        }

        public override object ExecuteScalar()
        {
            return i.ExecuteScalar();
        }

        public override void Prepare()
        {
            i.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return i.CreateParameter();
        }


        public override Task<int> ExecuteNonQueryAsync(System.Threading.CancellationToken cancellationToken){
            return i.ExecuteNonQueryAsync(cancellationToken);
        }
        public override Task<object> ExecuteScalarAsync(System.Threading.CancellationToken cancellationToken){
            return i.ExecuteScalarAsync(cancellationToken);
        }
        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, System.Threading.CancellationToken cancellationToken){
            var inner = await i.ExecuteReaderAsync(behavior, cancellationToken);
            return new SqlDataReaderWithHierarchyId(inner);
        }
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return new SqlDataReaderWithHierarchyId(i.ExecuteReader(behavior));
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    i.Dispose();
                }
                disposedValue = true;
            }
        }
        #endregion

    }
}