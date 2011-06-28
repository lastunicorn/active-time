using System.Data.SQLite;
using DustInTheWind.ActiveTime.Goose;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DustInTheWind.ActiveTime
{
    public class Dal
    {
        private const string DB_FILE_PATH = "db.s3db";

        private const string CONNECTION_STRING = "Data Source=" + DB_FILE_PATH;

        public void AddRecord(Record record)
        {
            using (SQLiteConnection cnx = new SQLiteConnection(CONNECTION_STRING))
            {
                cnx.Open();

                string sql = string.Format("insert into records(date,start_time,end_time) values('{0}', '{1}', '{2}')", record.Date.ToString("yyyy-MM-dd"), record.StartTime.ToString(@"hh\:mm\:ss"), record.EndTime.ToString(@"hh\:mm\:ss"));
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateEndTime(Record record)
        {
            using (SQLiteConnection cnx = new SQLiteConnection(CONNECTION_STRING))
            {
                cnx.Open();

                string sql = string.Format("update records set end_time='{0}' where date='{1}' and start_time='{2}'", record.EndTime.ToString(@"hh\:mm\:ss"), record.Date.ToString("yyyy-MM-dd"), record.StartTime.ToString(@"hh\:mm\:ss"));
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Record[] GetRecords(DateTime date)
        {
            using (SQLiteConnection cnx = new SQLiteConnection(CONNECTION_STRING))
            {
                cnx.Open();
                string sql = string.Format("select start_time, end_time from records where date='{0}'", date.ToString("yyyy-MM-dd"));
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    List<Record> records = new List<Record>();

                    while (reader.Read())
                    {
                        if (date == new DateTime(2011, 1, 17))
                            Debugger.Break();

                        object o1 = reader["start_time"];
                        object o2 = reader["end_time"];

                        records.Add(new Record(date, DateTime.Parse(o1.ToString()).TimeOfDay, DateTime.Parse(o2.ToString()).TimeOfDay));
                    }

                    return records.ToArray();
                }
            }
        }

        public void DeleteRecord(Record record)
        {
            using (SQLiteConnection cnx = new SQLiteConnection(CONNECTION_STRING))
            {
                cnx.Open();

                string sql = string.Format("delete from records where date='{0}' and start_time='{1}' and end_time='{2}'", record.Date.ToString("yyyy-MM-dd"), record.StartTime.ToString(@"hh\:mm\:ss"), record.EndTime.ToString(@"hh\:mm\:ss"));
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string SqlTextEncode(string text)
        {
            return text.Replace("'", "''");
        }

        internal void UpdateOrInsertComment(DateTime date, string comment)
        {
            using (SQLiteConnection cnx = new SQLiteConnection(CONNECTION_STRING))
            {
                cnx.Open();
                string sql = string.Format("select date from comments where date='{0}'", date.ToString("yyyy-MM-dd"));
                using (SQLiteCommand cmdSelect = new SQLiteCommand(sql, cnx))
                {
                    SQLiteDataReader reader = cmdSelect.ExecuteReader();

                    if (reader.Read())
                    {
                        sql = string.Format("update comments set comment='{0}' where date='{1}'", SqlTextEncode(comment), date.ToString("yyyy-MM-dd"));
                        using (SQLiteCommand cmdUpdate = new SQLiteCommand(sql, cnx))
                        {
                            if (cmdUpdate.ExecuteNonQuery() == 0)
                            {
                                throw new Exception();
                            }
                        }
                    }
                    else
                    {
                        sql = string.Format("insert into comments(date,comment) values('{0}', '{1}')", date.ToString("yyyy-MM-dd"), SqlTextEncode(comment));
                        using (SQLiteCommand cmdInsert = new SQLiteCommand(sql, cnx))
                        {

                            if (cmdInsert.ExecuteNonQuery() == 0)
                            {
                                throw new Exception();
                            }
                        }
                    }
                }
            }
        }

        internal string GetComment(DateTime date)
        {
            using (SQLiteConnection cnx = new SQLiteConnection(CONNECTION_STRING))
            {
                cnx.Open();
                string sql = string.Format("select comment from comments where date='{0}'", date.ToString("yyyy-MM-dd"));
                using (SQLiteCommand cmd = new SQLiteCommand(sql, cnx))
                {
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return (string)reader["comment"];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public DayRecord GetDayRecord(DateTime date)
        {
            Record[] records = GetRecords(date);
            string comment = GetComment(date);

            DayRecord dayRecord = new DayRecord();

            if ((records != null && records.Length > 0) || (comment != null && comment.Length > 0))
            {
                dayRecord.Date = date;
                dayRecord.Records = records;
                dayRecord.Comment = comment;
            }

            return dayRecord;
        }
    }
}
