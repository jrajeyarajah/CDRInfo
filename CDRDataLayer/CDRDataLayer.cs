using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DataLayer
{
    public class CDRDataLayer
    {
        public string sql = "select * from CDRData";
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString;

        public IEnumerable<CDRData> CdrDatas
        {
            get
            {
                List<CDRData> cdrDatas = new List<CDRData>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        float.TryParse(rdr["cost"].ToString(), out float ca);
                        CDRData cdrData = new CDRData
                        {
                            caller_id = rdr["caller_id"].ToString(),
                            recipient = rdr["recipient"].ToString(),
                            call_date = DateTime.ParseExact(rdr["call_date"].ToString(), "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")),
                            end_time = DateTime.ParseExact(rdr["end_time"].ToString(), "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")),
                            duration = Convert.ToInt32(rdr["duration"]),
                            cost = ca,
                            reference = rdr["reference"].ToString(),
                            currency = rdr["currency"].ToString(),
                            type = Convert.ToChar(rdr["type"].ToString())

                        };

                        cdrDatas.Add(cdrData);
                    }
                    con.Close();
                }
                return cdrDatas;
            }
        }

        public string CDRDataSave(CDRData cdrdata)
        {
            string RequestStatus = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spInsertCDRData", con))
                //con.CreateCommand())
                //
                {
                    //cmd.CommandText = parameterStatment.
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@caller_id", SqlDbType.VarChar, 50)).Value = cdrdata.caller_id;
                    cmd.Parameters.Add(new SqlParameter("@recipient", SqlDbType.VarChar, 50)).Value = cdrdata.recipient;
                    cmd.Parameters.Add(new SqlParameter("@call_date", SqlDbType.DateTime)).Value = cdrdata.call_date;
                    cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.DateTime)).Value = cdrdata.end_time;
                    cmd.Parameters.Add(new SqlParameter("@duration", SqlDbType.Int, 50)).Value = cdrdata.duration;

                    cmd.Parameters.Add(new SqlParameter("@cost", SqlDbType.Decimal)
                    {
                        Precision = 18,
                        Scale = 8
                    }).Value = cdrdata.cost;
                    cmd.Parameters.Add(new SqlParameter("@reference", SqlDbType.VarChar, 50)).Value = cdrdata.reference;
                    cmd.Parameters.Add(new SqlParameter("@currency", SqlDbType.VarChar, 3)).Value = cdrdata.currency;
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar, 1)).Value = cdrdata.type;

                    var returnParameter = cmd.Parameters.Add("@errorMessage", SqlDbType.VarChar, 200);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    //cmd.Parameters.Add(RuturnValue);
                    try
                    {
                        con.Open();
                        cmd.ExecuteReader();
                        RequestStatus = cmd.Parameters["@errorMessage"].Value.ToString();
                        if (RequestStatus != "0") { RequestStatus = "Error " + RequestStatus; }
                        //cmd.ExecuteNonQuery();
                        //RequestStatus = cmd.returnParameter.Value;

                    }

                    catch (Exception e)
                    {
                        RequestStatus = "Error " + e.Message;
                    }
                    finally
                    {
                        con.Close();
                    }
                    return RequestStatus;

                }
            }

        }
    }
}
