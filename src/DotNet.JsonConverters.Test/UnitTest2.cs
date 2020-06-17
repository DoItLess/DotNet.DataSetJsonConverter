using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DotNet.JsonConverters.Test
{
    public class UnitTest2
    {
        [Test]
        public void Test1()
        {
            var ds = new DataSet("用户管理");
            var dsMaster = new DataTable("dsMaster");
            dsMaster.Columns.Add(new DataColumn("FLoginId",typeof(string)){Caption = "登录Id"});
            dsMaster.Columns.Add(new DataColumn("FName",typeof(string)){Caption = "用户名称"});
            dsMaster.Columns.Add(new DataColumn("FEmployeeNumber",typeof(string)){Caption = "绑定职员代码"});
            dsMaster.Columns.Add(new DataColumn("FEmployeeName",typeof(string)){Caption = "绑定职员名称"});
            dsMaster.Columns.Add(new DataColumn("FErpUsername",typeof(string)){Caption = "Erp用户名"});
            dsMaster.Columns.Add(new DataColumn("FErpPassword",typeof(string)){Caption = "Erp密码"});
            dsMaster.Columns.Add(new DataColumn("FDate1",typeof(DateTime)){Caption = "DateTime1"});
            dsMaster.Columns.Add(new DataColumn("FDate2",typeof(DateTime)){Caption = "DateTime2"});

            var dsNewRow = dsMaster.NewRow();
            dsNewRow["FLoginId"] = Guid.NewGuid().ToString();
            dsNewRow["FName"] = "许JJ";
            dsNewRow["FEmployeeNumber"] = "NB0001";
            dsNewRow["FEmployeeName"] = "害羞的小蜜蜂";
            dsNewRow["FErpUsername"] = "shy bee";
            dsNewRow["FErpPassword"] = Guid.NewGuid().ToString();
            dsNewRow["FDate1"] = DBNull.Value;
            dsNewRow["FDate2"] = null;

            dsMaster.Rows.Add(dsNewRow);

            ds.Tables.Add(dsMaster);

            var json = JsonConvert.SerializeObject(ds, new DataSetConverter(ConvertLevel.Minimal, DateTimeFormatStyle.TimeStampMillisecond));
            Console.WriteLine(json);
        }
    }
}
