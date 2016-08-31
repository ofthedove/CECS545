using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogEntry = System.Collections.Generic.KeyValuePair<int, System.Tuple<bool, string>>;
/* KeyValuePair - LogEntry
 * {
 *      int id
 *      tuple - LogMessage
 *      {
 *          bool isData
 *          string message
 *      }
 * }
 */

namespace CECS545AI_Project1Combs
{
    class OutputLog
    {
        public delegate void UpdateHandler(object sender, EventArgs e);
        public event UpdateHandler OnLogUpdate;

        private List<LogEntry> log;
        private int index;

        public bool supressUpdates = false;

        public OutputLog()
        {
            log = new List<LogEntry>();
            index = 0;
        }

        public void writeLogMessage(string message)
        {
            log.Add(new LogEntry(index, new Tuple<bool, string>(false, message)));
            index++;

            if (!supressUpdates)
            {
                OnLogUpdate?.Invoke(this, new EventArgs());
            }
        }

        public void writeResultData(string data)
        {
            log.Add(new LogEntry(index, new Tuple<bool, string>(true, data)));
            index++;

            if (!supressUpdates)
            {
                OnLogUpdate?.Invoke(this, new EventArgs());
            }
        }

        public string readLogMessage()
        {
            string result = "";
            foreach (LogEntry entry in log.Where(x => x.Value.Item1 == false).OrderBy(x => x.Key)) // select only non-data items, order by id
            {
                result += entry.Value.Item2 + Environment.NewLine;
            }
            return result;
        }

        public string readResultData()
        {
            string result = "";
            foreach (LogEntry entry in log.Where(x => x.Value.Item1 == true).OrderBy(x => x.Key)) // select only data items, order by id
            {
                result += entry.Value.Item2 + Environment.NewLine;
            }
            return result;
        }

        public string readCompleteLog()
        {
            string result = "";
            foreach(LogEntry entry in log) //.OrderBy(x=>x.Key)) // order by id
            {
                result += entry.Key.ToString().PadLeft(4, ' ') + "   " + (entry.Value.Item1 ? "D" : "L") + "   " + entry.Value.Item2 + Environment.NewLine;
            }
            return result;
        }
    }
}
