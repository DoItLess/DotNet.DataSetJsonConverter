using System;
using System.Data;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DotNet.DataSetJsonConverter.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var source = CreateTable("table1");

            var json = JsonConvert.SerializeObject(source, Formatting.Indented, new DataTableJsonConverter(ConvertLevel.Minimal));

            Console.WriteLine(json);

            var result = JsonConvert.DeserializeObject<DataTable>(json, new DataTableJsonConverter(ConvertLevel.Minimal));


            Assert.AreEqual(result?.TableName, source.TableName);
            Assert.AreEqual(result?.Namespace, source.Namespace);
            Assert.AreEqual(result?.Prefix, source.Prefix);

        }


        [Test]
        public void Test2()
        {
            var dataSet = new DataSet();

            var dataTable = new DataTable();
            dataTable.TableName = "Test.TableName";
            dataTable.Namespace = "Test.Namespace";
            dataTable.Columns.Add(new DataColumn("FRowId", typeof(string)));
            dataTable.Columns.Add(new DataColumn("FString", typeof(string)));
            dataTable.Columns.Add(new DataColumn("FInt", typeof(int)));
            dataTable.Columns.Add(new DataColumn("FLong", typeof(long)));
            dataTable.Columns.Add(new DataColumn("FDecimal", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("FDateTime", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("FBool", typeof(bool)));

            dataTable.PrimaryKey = new[] {dataTable.Columns["FRowId"], dataTable.Columns["FString"]};

            var newRow = dataTable.NewRow();
            newRow["FRowId"]    = Guid.NewGuid().ToString();
            newRow["FString"]   = Guid.NewGuid().ToString();
            newRow["FInt"]      = int.MaxValue;
            newRow["FLong"]     = long.MaxValue;
            newRow["FDecimal"]  = 12.555d;
            newRow["FDateTime"] = DateTime.Now;
            newRow["FBool"]     = true;

            dataTable.Rows.Add(newRow);
            dataTable.AcceptChanges();

            var dataTable2 = new DataTable();
            dataTable2.TableName = "Test.TableName2";
            dataTable2.Namespace = "Test.Namespace2";
            dataTable2.Columns.Add(new DataColumn("FRowId", typeof(string)));
            dataTable2.Columns.Add(new DataColumn("FString", typeof(string)));
            dataTable2.Columns.Add(new DataColumn("FInt", typeof(int)));
            dataTable2.Columns.Add(new DataColumn("FLong", typeof(long)));
            dataTable2.Columns.Add(new DataColumn("FDecimal", typeof(decimal)));
            dataTable2.Columns.Add(new DataColumn("FDateTime", typeof(DateTime)));
            dataTable2.Columns.Add(new DataColumn("FBool", typeof(bool)));
            dataTable2.Columns.Add(new DataColumn("FParentId", typeof(string)));

            dataTable2.PrimaryKey = new[] {dataTable2.Columns["FRowId"], dataTable2.Columns["FString"]};

            var newRow2 = dataTable2.NewRow();
            newRow2["FRowId"]    = Guid.NewGuid().ToString();
            newRow2["FString"]   = Guid.NewGuid().ToString();
            newRow2["FInt"]      = int.MaxValue;
            newRow2["FLong"]     = long.MaxValue;
            newRow2["FDecimal"]  = 12.555d;
            newRow2["FDateTime"] = DateTime.Now;
            newRow2["FBool"]     = true;
            newRow2["FParentId"] = dataTable.Rows[0]["FRowId"];


            dataTable2.Rows.Add(newRow2);
            dataTable2.AcceptChanges();

            dataSet.Tables.Add(dataTable);
            dataSet.Tables.Add(dataTable2);

            var relation = new DataRelation("Test.Relation1", dataTable.Columns["FRowId"], dataTable2.Columns["FParentId"]);
            dataSet.Relations.Add(relation);

            var json = JsonConvert.SerializeObject(dataSet, Formatting.Indented,
                                                   new[] {new DataSetJsonConverter(ConvertLevel.Minimal),});

            Console.WriteLine(json);


            var ds = JsonConvert.DeserializeObject<DataSet>(json, new DataSetJsonConverter(ConvertLevel.Minimal));
        }


        private DataTable CreateTable(string tableName)
        {
            var dataTable = new DataTable();
            dataTable.TableName = "Test.TableName";
            dataTable.Namespace = "Test.Namespace";
            dataTable.Columns.Add(new DataColumn("FRowId", typeof(string)));
            dataTable.Columns.Add(new DataColumn("FString", typeof(string)));
            dataTable.Columns.Add(new DataColumn("FInt", typeof(int)));
            dataTable.Columns.Add(new DataColumn("FLong", typeof(long)));
            dataTable.Columns.Add(new DataColumn("FDecimal", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("FDateTime", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("FBool", typeof(bool)));
            dataTable.PrimaryKey = new[] {dataTable.Columns["FRowId"]};
            var newRow = dataTable.NewRow();
            newRow["FRowId"]    = Guid.NewGuid().ToString();
            newRow["FString"]   = Guid.NewGuid().ToString();
            newRow["FInt"]      = int.MaxValue;
            newRow["FLong"]     = long.MaxValue;
            newRow["FDecimal"]  = 12.555d;
            newRow["FDateTime"] = DateTime.Now;
            newRow["FBool"]     = true;
            dataTable.Rows.Add(newRow);
            dataTable.AcceptChanges();

            return dataTable;
        }
    }
}