using Lox.Classes;
using Lox.i18n;
using System.Text;

namespace Lox.Utils
{
    internal static class LoxInstanceSerializer
    {
        private static readonly StringBuilder stringBuilder = new();

        /// <summary>
        /// Serializes a loxInstance to JSON
        /// </summary>
        /// <param name="loxInstance">The LoxInstance that is serialized</param>
        internal static string Serialize(IReadOnlyDictionary<string, object?> fields)
        {
            stringBuilder.Clear();
            stringBuilder.AppendLine("{");
            foreach (KeyValuePair<string, object?> field in fields)
            {
                stringBuilder.Append("\t\"");
                stringBuilder.Append(field.Key);
                stringBuilder.Append("\": ");
                switch (field.Value)
                {
                    //strings
                    case string:
                        stringBuilder.Append('"');
                        stringBuilder.Append(field.Value);
                        stringBuilder.Append('"');
                        break;
                    //Numbers, boolean's and null
                    default:
                        stringBuilder.Append(field.Value is not null ? field.Value : Constants.NilKeyword);
                        break;
                }
                stringBuilder.AppendLine(",");
            }
            stringBuilder.AppendLine("}");
            return stringBuilder.ToString();
        }
    }
}
