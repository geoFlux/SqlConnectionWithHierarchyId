using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.SqlServer.Types;

namespace GeoFlux.SqlConnectionFix
{
    public class SqlConnectionWithHierarchyId: DbConnection{
        private readonly SqlConnection inner;

        public SqlConnectionWithHierarchyId(SqlConnection inner){
            this.inner = inner;
        }

        public override string ConnectionString { get => inner.ConnectionString; set => inner.ConnectionString = value; }

        public override string Database => inner.Database;

        public override string DataSource => inner.DataSource;

        public override string ServerVersion => inner.ServerVersion;

        public override ConnectionState State => inner.State;

        public override void ChangeDatabase(string databaseName)
        {
            inner.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            inner.Close();
        }

        public override void Open()
        {
            inner.Open();
        }

        protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return inner.BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new SqlCommandWithHierarchyId(inner.CreateCommand());
        }
        public override void EnlistTransaction(Transaction transaction){
            inner.EnlistTransaction(transaction);
        }
        public override DataTable GetSchema(){
            return inner.GetSchema();
        }
        public override DataTable GetSchema(string collectionName){
            return inner.GetSchema(collectionName);
        }
        public override DataTable GetSchema(string collectionName, string[] restrictionValues){
            return inner.GetSchema(collectionName, restrictionValues);
        }
        public override Task OpenAsync(System.Threading.CancellationToken cancellationToken){
            return inner.OpenAsync(cancellationToken);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    inner.Dispose();
                }
                disposedValue = true;
            }
        }
        #endregion

    }
}