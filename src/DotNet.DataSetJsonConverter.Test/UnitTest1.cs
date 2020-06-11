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

            dataTable.PrimaryKey = new[] {dataTable.Columns["FRowId"],dataTable.Columns["FString"]};

            var newRow = dataTable.NewRow();
            newRow["FRowId"] = Guid.NewGuid().ToString();
            newRow["FString"] = Guid.NewGuid().ToString();
            newRow["FInt"] = int.MaxValue;
            newRow["FLong"] = long.MaxValue;
            newRow["FDecimal"] = 12.555d;
            newRow["FDateTime"] = DateTime.Now;
            newRow["FBool"] = true;

            dataTable.Rows.Add(newRow);
            dataTable.AcceptChanges();

            var json = JsonConvert.SerializeObject(dataTable, Formatting.Indented,
                                                   new[] {new DataTableJsonConverter(DataTableJsonConverter.ConvertLevel.Minimal),});

            Console.WriteLine(json);

            var rs = JsonConvert.DeserializeObject<DataTable>(json, new[] {new DataTableJsonConverter()});
        }
    }
}