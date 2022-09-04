using DataCommon.DatabaseMetadata;

using System;

namespace DataADO
{
    public interface IDDLFactory
    {
        public string CreateForeignkey(string relativeTable, string table, string column, bool? onDeleteCascade = false, bool? onUpdateCascade = null);
        public string CreateTable(Type metadata);
        public TableMetaData CreateTableMetaData(Type metadata);
        public string CreateTable(TableMetaData metadata);
    }
}