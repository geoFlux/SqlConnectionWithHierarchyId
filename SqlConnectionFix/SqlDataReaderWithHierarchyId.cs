using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
namespace GeoFlux.SqlConnectionFix
{
    public class SqlDataReaderWithHierarchyId: DbDataReader{
        readonly SqlDataReader sqlReader;
        IDataReader iReader => sqlReader;


        public SqlDataReaderWithHierarchyId(SqlDataReader inner){
            this.sqlReader = inner;
        }

       public override object this[int idx]
        {
            get
            {
                if (IsHierarchyIdColumn(idx))
                {
                    return GetHierarchyId(idx);
                }
                return iReader[idx];
            }
        }

        public override object this[string name]
        {
            get
            {
                var idx = iReader.GetOrdinal(name);
                if (IsHierarchyIdColumn(idx))
                {
                    return GetHierarchyId(idx);
                }
                return iReader[name];
            }
        }

        public override object GetValue(int i)
        {

            if (IsHierarchyIdColumn(i))
            {
                return GetHierarchyId(i);
            }
            return iReader.GetValue(i);
        }

        private bool IsHierarchyIdColumn(int i)
        {
            var columnType = iReader.GetDataTypeName(i);
            return (columnType.Contains("hierarchyid"));
        }

        public SqlHierarchyId GetHierarchyId(int i)
        {
            var udt = new SqlHierarchyId();

            using (var strm = sqlReader.GetStream(i))
            {
                var bytes = GetAllBytes(strm, 892);
                using (var ms = new MemoryStream(bytes))
                using (var bReader = new BinaryReader(ms))
                {
                    var len = ms.Length;
                    udt.Read(bReader);
                }
            }
            return udt;
        }
        public async Task<SqlHierarchyId> GetHierarchyIdAsync(int i){
            var udt = new SqlHierarchyId();

            using (var strm = sqlReader.GetStream(i))
            {
                var bytes = await GetAllBytesAsync(strm, 892);
                using (var ms = new MemoryStream(bytes))
                using (var bReader = new BinaryReader(ms))
                {
                    var len = ms.Length;
                    udt.Read(bReader);
                }
            }
            return udt;
        }

        byte[] GetAllBytes(Stream strm, int estimatedMaxSize)
        {

            var bufferSize = 892;
            var buffer = new byte[estimatedMaxSize];
            var result = new List<byte>();
            int numBytesRead;
            while ((numBytesRead = strm.Read(buffer, 0, bufferSize)) > 0)
            {
                result.AddRange(buffer.Take(numBytesRead));
            }
            return result.ToArray();
        }
        async Task<byte[]> GetAllBytesAsync(Stream strm, int estimatedMaxSize)
        {

            var bufferSize = 892;
            var buffer = new byte[estimatedMaxSize];
            var result = new List<byte>();
            int numBytesRead;
            while ((numBytesRead = await strm.ReadAsync(buffer, 0, bufferSize)) > 0)
            {
                result.AddRange(buffer.Take(numBytesRead));
            }
            return result.ToArray();
        }

        public override int Depth => iReader.Depth;

        public override bool IsClosed => iReader.IsClosed;

        public override int RecordsAffected => iReader.RecordsAffected;

        public override int FieldCount => iReader.FieldCount;

        public override bool HasRows => sqlReader.HasRows;

        public override void Close()
        {
            iReader.Close();
        }

        public override bool GetBoolean(int i)
        {
            return iReader.GetBoolean(i);
        }

        public override byte GetByte(int i)
        {
            return iReader.GetByte(i);
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return iReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public override char GetChar(int i)
        {
            return iReader.GetChar(i);
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return iReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }



        public override string GetDataTypeName(int i)
        {
            return iReader.GetDataTypeName(i);
        }

        public override DateTime GetDateTime(int i)
        {
            return iReader.GetDateTime(i);
        }

        public override decimal GetDecimal(int i)
        {
            return iReader.GetDecimal(i);
        }

        public override double GetDouble(int i)
        {
            return iReader.GetDouble(i);
        }

        public override Type GetFieldType(int i)
        {
            return iReader.GetFieldType(i);
        }

        public override float GetFloat(int i)
        {
            return iReader.GetFloat(i);
        }

        public override Guid GetGuid(int i)
        {
            return iReader.GetGuid(i);
        }

        public override short GetInt16(int i)
        {
            return iReader.GetInt16(i);
        }

        public override int GetInt32(int i)
        {
            return iReader.GetInt32(i);
        }

        public override long GetInt64(int i)
        {
            return iReader.GetInt64(i);
        }

        public override string GetName(int i)
        {
            return iReader.GetName(i);
        }

        public override int GetOrdinal(string name)
        {
            return iReader.GetOrdinal(name);
        }

        public override DataTable GetSchemaTable()
        {
            return iReader.GetSchemaTable();
        }

        public override string GetString(int i)
        {
            return iReader.GetString(i);
        }


        public override int GetValues(object[] values)
        {
            Console.WriteLine("GetValues");
            return iReader.GetValues(values);
        }

        public override bool IsDBNull(int i)
        {
            return iReader.IsDBNull(i);
        }

        public override bool NextResult()
        {
            return iReader.NextResult();
        }

        public override bool Read()
        {
            return iReader.Read();
        }

        public override IEnumerator GetEnumerator()
        {
            return sqlReader.GetEnumerator();
        }

        public  override T GetFieldValue<T>(int ordinal){

            if(typeof(T) == typeof(SqlHierarchyId)){
                return (T)(object)this.GetHierarchyId(ordinal);
            }
            return sqlReader.GetFieldValue<T>(ordinal);
        }
        public override async Task<T> GetFieldValueAsync<T>(int ordinal, System.Threading.CancellationToken cancellationToken){
            if(typeof(T) == typeof(SqlHierarchyId)){
                return (T)(object)(await this.GetHierarchyIdAsync(ordinal));
            }
            return await sqlReader.GetFieldValueAsync<T>(ordinal, cancellationToken);
        }
        public override Type GetProviderSpecificFieldType(int ordinal){
            return sqlReader.GetProviderSpecificFieldType(ordinal);
        }
        public override object GetProviderSpecificValue(int ordinal){
            return sqlReader.GetProviderSpecificValue(ordinal);
        }
        public override int GetProviderSpecificValues(object[] values){
            return sqlReader.GetProviderSpecificValues(values);
        }
        public override Task<bool> ReadAsync(System.Threading.CancellationToken cancellationToken){
            return sqlReader.ReadAsync();
        }
        public override Stream GetStream(int ordinal){
            return sqlReader.GetStream(ordinal);
        }
        public override TextReader GetTextReader(int ordinal){
            return sqlReader.GetTextReader(ordinal);
        }
        public override Task<bool> IsDBNullAsync(int ordinal, System.Threading.CancellationToken cancellationToken){
            return sqlReader.IsDBNullAsync(ordinal, cancellationToken);
        }
        public override Task<bool> NextResultAsync(System.Threading.CancellationToken cancellationToken){
            return sqlReader.NextResultAsync(cancellationToken);
        }
    }
}