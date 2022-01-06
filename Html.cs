using System;
using System.Collections.Generic;
using System.Text;

namespace TableCreator
{
    public static class Html
    {
        public class Table : HtmlBase, IDisposable
        {
            public Table(StringBuilder sb, List<string> classAttributes = null, string id = "") : base(sb)
            {
                Append("<table");
                AddOptionalAttributes(classAttributes, id);
            }

            public void StartHead(List<string> classAttributes = null, string id = "")
            {
                Append("<thead");
                AddOptionalAttributes(classAttributes, id);
            }

            public void EndHead()
            {
                Append("</thead>");
            }

            public void StartFoot(List<string> classAttributes = null, string id = "")
            {
                Append("<tfoot");
                AddOptionalAttributes(classAttributes, id);
            }

            public void EndFoot()
            {
                Append("</tfoot>");
            }

            public void StartBody(List<string> classAttributes = null, string id = "")
            {
                Append("<tbody");
                AddOptionalAttributes(classAttributes, id);
            }

            public void EndBody()
            {
                Append("</tbody>");
            }

            public void Dispose()
            {
                Append("</table>");
            }

            public Row AddRow(List<string> classAttributes, string id = "")
            {
                return new Row(GetBuilder(), classAttributes, id);
            }
        }

        public class Row : HtmlBase, IDisposable
        {
            public Row(StringBuilder sb, List<string> classAttributes, string id = "") : base(sb)
            {
                Append("<tr");
                AddOptionalAttributes(classAttributes, id);
            }
            public void Dispose()
            {
                Append("</tr>");
            }
            public void AddCell(string innerText, List<string> classAttributes = null, string id = "", string colSpan = "")
            {
                Append("<td");
                AddOptionalAttributes(classAttributes, id, colSpan);
                Append(innerText);
                Append("</td>");
            }
            public void AddActionCell(string entityId, List<string> classAttributes = null, string id = "", string colSpan = "")
            {
                Append("<td");
                AddOptionalAttributes(classAttributes, id, colSpan);
                Append($"<a class='btn btn-warning mr-1 text-white update-btn' data-toggle='modal' data-id='{entityId}'> <i class='fas fa-pen'></i></a>");
                Append($"<a class='btn btn-danger text-white delete-btn' data-toggle='modal' data-id='{entityId}'> <i class='fas fa-trash'></i></a>");
                Append("</td>");
            }
        }

        public abstract class HtmlBase
        {
            private StringBuilder _sb;

            protected HtmlBase(StringBuilder sb)
            {
                _sb = sb;
            }

            public StringBuilder GetBuilder()
            {
                return _sb;
            }

            protected void Append(string toAppend)
            {
                _sb.Append(toAppend);
            }

            protected void AddOptionalAttributes(List<string> classes = null, string id = "", string colSpan = "")
            {

                if (!string.IsNullOrEmpty(id))
                {
                    _sb.Append($" id='{id}'");
                }


                if (classes != null)
                {
                    _sb.Append($" class='{string.Join(" ", classes)}'");
                }


                if (!string.IsNullOrEmpty(colSpan))
                {
                    _sb.Append($" colspan='{colSpan}'");
                }
                _sb.Append(">");
            }
        }
    }
}
