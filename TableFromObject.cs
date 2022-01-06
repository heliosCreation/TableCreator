using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace TableCreator
{
    public static class TableFromObject<T> where T : class
    {
        private static List<string> tableClasses = new List<string>() { "table", "table-bordered", "outer-shadow" };
        private static List<string> theadClasses = new List<string>() { "table-head" };
        private static List<string> dNone = new List<string>() { "d-none" };

        public static string CreateTable(List<T> model)
        {
            StringBuilder sb = new StringBuilder();
            var headers = GetDisplayNames(model[0]);
            using (Html.Table table = new Html.Table(sb, classAttributes: tableClasses, id: ""))
            {
                table.StartHead(theadClasses);
                using (var thead = table.AddRow(new List<string>()))
                {
                    foreach (var header in headers)
                    {
                        thead.AddCell(header, classAttributes: theadClasses);
                    }
                    thead.AddCell("Actions", classAttributes: theadClasses);
                }
                table.EndHead();
                table.StartBody();
                foreach (var item in model)
                {
                    var id = "";
                    var value = "";
                    using (var tr = table.AddRow(classAttributes: new List<string>()))
                    {
                        foreach (PropertyInfo prop in item.GetType().GetProperties())
                        {
                            if (prop.Name != "Id")
                            {
                                if (prop.PropertyType == typeof(DateTime))
                                {
                                    value = ((DateTime)prop.GetValue(item, null)).ToShortDateString();
                                }
                                else
                                {
                                    value = prop.GetValue(item, null) != null ? prop.GetValue(item, null).ToString() : string.Empty;
                                }
                                value.Replace("<", "");
                                value.Replace(">", "");
                                tr.AddCell(value.Length < 50 ? value : value.Substring(0, 50) + " ...");
                            }
                            else
                            {
                                tr.AddCell(prop.GetValue(item, null).ToString(), classAttributes: dNone);
                                id = prop.GetValue(item, null).ToString();
                            }

                        }
                        tr.AddActionCell(id);

                    }

                }
                table.EndBody();
            }
            sb.Replace("<script>", "");
            sb.Replace("</script>", "");
            return sb.ToString();
        }


        private static List<string> GetDisplayNames(T model)
        {
            var list = new List<string>();
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in Props)
            {
                if (prop.Name != "Id")
                {

                    DisplayAttribute dna = (DisplayAttribute)Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute));
                    if (dna != null)
                        list.Add(dna.Name);
                    else
                        list.Add(prop.Name);

                }
            }

            return list;
        }
    }
}
