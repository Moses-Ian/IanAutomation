using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IanAutomation.FileHelpers
{
    public static class CSVHelper
    {
        public static DataTable ReadCSV(string FilePath)
        {
            string[] lines = File.ReadAllLines(FilePath); // reads each line of the file into the array
            DataTable Data = new DataTable();
            string[] headers = lines[0].Split(',');

            foreach (string header in headers)
            {
                Data.Columns.Add(header, typeof(string));
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',');
                DataRow row = Data.NewRow();
                for (int j = 0; j < headers.Length; j++)
                {
                    row[j] = data[j];
                }
                Data.Rows.Add(row);
            }

            return Data;
        }

        public static void WriteCSV(DataTable Data, string Filepath)
        {
            using (StreamWriter writer = new StreamWriter(Filepath))
            {
                // Write header
                foreach (DataColumn column in Data.Columns)
                {
                    writer.Write(column.ColumnName);
                    if (column.Ordinal < Data.Columns.Count - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();

                // Write data
                foreach (DataRow row in Data.Rows)
                {
                    for (int i = 0; i < Data.Columns.Count; i++)
                    {
                        writer.Write(row[i]);
                        if (i < Data.Columns.Count - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }

        public static DataTable ListToDataTable<T>(List<T> Data)
        {
            // Gather info
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields();              // public int Id;
            PropertyInfo[] properties = type.GetProperties();   // public int Id { get; set; }

            // Initialize the table
            DataTable table = new DataTable();

            // Add the headers -> includes the name and the type (decimal, string, etc)
            foreach (FieldInfo field in fields)
            {
                table.Columns.Add(field.Name, field.FieldType);
            }
            foreach (PropertyInfo property in properties)
            {
                table.Columns.Add(property.Name, property.PropertyType);
            }

            // Add the rows
            foreach (T item in Data)
            {
                DataRow row = table.NewRow();

                // Use reflection to pull the value of the property from the name of the property
                foreach (FieldInfo field in fields)
                {
                    row[field.Name] = field.GetValue(item);
                }
                foreach (PropertyInfo property in properties)
                {
                    row[property.Name] = property.GetValue(item);
                }

                // And add it
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
