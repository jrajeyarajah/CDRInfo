using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using DataLayer;

namespace ProcessCDRFile
{
    public  class CDRDataProcessing
    {
        public void readFile(string csvFile)
        {
            IEnumerable<string[]> lines = File.ReadAllLines(csvFile).Select(x => x.Split(','));
            int lineLength = lines.First().Count(); //Find number of cols
            var CSV = lines.Skip(1)
           .Select(x => x) //Row
           .Select((v, i) => new { Value = v, Index = i % lineLength }) // Col
           .Select(x => x.Value); // value
            foreach (var value in CSV)
            {
                WriteRow(
                    value[0],     // caller_id
                    value[1],     // recipient
                    value[2],     // call_date
                    value[3],     // end_time
                    value[4],     // duration
                    value[5],     // cost
                    value[6],     // reference
                    value[7],     // currency
                    value[8]);     // type
            }
        }
        private static void WriteRow(
        string _caller_id,
        string _recipient,
        string _call_date,
        string _end_time,
        string _duration,
        string _cost,
        string _reference,
        string _currency,
        string _type)
        {
            CDRDataLayer cdrdataLayer = new CDRDataLayer();
            cdrdataLayer.CDRDataSave(new InCDRData 
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
    }
}
