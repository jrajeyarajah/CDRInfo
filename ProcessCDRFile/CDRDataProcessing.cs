using System;
using System.Text.RegularExpressions;
using DataLayer;

namespace ProcessCDRFile
{
    public class CDRDataProcessing
    {
        //public void readFile(string csvFile)
        //{
        //    IEnumerable<string[]> lines = File.ReadAllLines(csvFile).Select(x => x.Split(','));
        //    int lineLength = lines.First().Count(); //Find number of cols
        //    var CSV = lines.Skip(1)
        //   .Select(x => x) //Row
        //   .Select((v, i) => new { Value = v, Index = i % lineLength }) // Col
        //   .Select(x => x.Value); // value
        //    foreach (var value in CSV)
        //    {
        //        WriteRow(
        //            value[0],     // caller_id
        //            value[1],     // recipient
        //            value[2],     // call_date
        //            value[3],     // end_time
        //            value[4],     // duration
        //            value[5],     // cost
        //            value[6],     // reference
        //            value[7],     // currency
        //            value[8]);     // type
        //    }
        //}
        private static string WriteRow(
        string _caller_id,
        string _recipient,
        DateTime _call_date,
        DateTime _end_time,
        int _duration,
        float _cost,
        string _reference,
        string _currency,
        char _type)
        {
            CDRDataLayer cdrdataLayer = new CDRDataLayer();
            return cdrdataLayer.CDRDataSave(new CDRData
            {
                caller_id = _caller_id,
                recipient = _recipient,
                call_date = _call_date,
                end_time = _end_time,
                duration = _duration,
                cost = _cost,
                reference = _reference,
                currency = _currency,
                type = _type
            });




        }

        public string ProcessLine(string line)
        {
            string[] col = line.Split(',');
            if (col.Length > 9)
            { return "Error: Too many columns"; }
            else
            {
                if (col.Length < 9)
                { return "Error: Not enough columns"; }
                else
                {
                    if ((Regex.IsMatch(col[0], @"\D")) &&
                        ((Regex.IsMatch(col[1], @"\D"))) &&
                        ((Regex.IsMatch(col[2], @"\D"))) &&
                        ((Regex.IsMatch(col[3], @"\D"))) &&
                        ((Regex.IsMatch(col[4], @"\D"))) &&
                        ((Regex.IsMatch(col[5], @"\D"))) &&
                        ((Regex.IsMatch(col[6], @"\D"))) &&
                        ((Regex.IsMatch(col[7], @"\D"))) &&
                        ((Regex.IsMatch(col[8], @"\D")))
                       )
                    {
                        return "";//"Header row";

                    }
                    else
                    {
                        DateTime callDate;
                        DateTime endTime;
                        int duration;
                        float cost;
                        if (DateTime.TryParse(col[2], out callDate))
                        {
                            if (DateTime.TryParse(col[3], out endTime))
                            {
                                if (int.TryParse(col[4], out duration))
                                {
                                    if (float.TryParse(col[5], out cost))
                                    {

                                        endTime = callDate.Date.Add(endTime.TimeOfDay);
                                        callDate = endTime.AddSeconds(-duration);

                                        //= col[2];     // call_date

                                        return WriteRow(
                                        col[0],     // caller_id
                                        col[1],     // recipient
                                        callDate,   // call_date
                                        endTime,    // end_time
                                        duration,   // duration
                                        cost,       // cost
                                        col[6],     // reference
                                        col[7],     // currency
                                        char.Parse(col[8]));     // type


                                    }
                                    else { return "Error in cost"; }
                                }
                                else
                                { return "Error in duration"; }
                            }
                            else
                            { return "Error in endTime"; }
                        }
                        else
                        { return "Error in callDate"; }


                    }
                }
            }
        }
    }
}