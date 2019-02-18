using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Types;
namespace GeoFlux.SqlConnectionFix
{
    public class SqlDataReaderWithHierarchyId : IDataReader
    {
        readonly SqlDataReader sqlReader;
        IDataReader iReader => sqlReader;
        public SqlDataReaderWithHierarchyId(SqlDataReader innerReader)
        {
            Console.WriteLine("SqlDataReaderWithHierarchyId Constructor");
            sqlReader = innerReader;
        }
        public object this[int idx]
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

        public object this[string name]
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

        public object GetValue(int i)
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

        byte[] GetAllBytes(Stream strm, int estimatedMaxSize)
        {

            var bufferSize = 1;
            var buffer = new byte[estimatedMaxSize];
            var result = new List<byte>();
            int numBytesRead;
            while ((numBytesRead = strm.Read(buffer, 0, bufferSize)) > 0)
            {
                result.AddRange(buffer.Take(numBytesRead));
            }
            return result.ToArray();
        }

        public int Depth => iReader.Depth;

        public bool IsClosed => iReader.IsClosed;

        public int RecordsAffected => iReader.RecordsAffected;

        public int FieldCount => iReader.FieldCount;

        public void Close()
        {
            iReader.Close();
        }

        public bool GetBoolean(int i)
        {
            return iReader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            return iReader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return iReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return iReader.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return iReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return iReader.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            return iReader.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return iReader.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return iReader.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return iReader.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return iReader.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return iReader.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return iReader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return iReader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            return iReader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return iReader.GetInt64(i);
        }

        public string GetName(int i)
        {
            return iReader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return iReader.GetOrdinal(name);
        }

        public DataTable GetSchemaTable()
        {
            return iReader.GetSchemaTable();
        }

        public string GetString(int i)
        {
            return iReader.GetString(i);
        }


        public int GetValues(object[] values)
        {
            Console.WriteLine("GetValues");
            return iReader.GetValues(values);
        }

        public bool IsDBNull(int i)
        {
            return iReader.IsDBNull(i);
        }

        public bool NextResult()
        {
            return iReader.NextResult();
        }

        public bool Read()
        {
            return iReader.Read();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    iReader.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

    }
}