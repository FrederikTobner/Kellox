using Kellox.Classes;
using Kellox.Keywords;
using System.Text;

namespace Kellox.Utils
{
    internal static class KelloxInstanceSerializer
    {

        /// <summary>
        /// The depth in the KelloxInstanceTree
        /// </summary>
        private static int depth = 1;

        /// <summary>
        /// Serializes a loxInstance to JSON
        /// </summary>
        /// <param name="loxInstance">The LoxInstance that is serialized</param>
        internal static string Serialize(IReadOnlyDictionary<string, object?> fields)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("{");
            foreach (KeyValuePair<string, object?> field in fields)
            {
                for (int i = 0; i < depth; i++)
                {
                    stringBuilder.Append('\t');
                }
                stringBuilder.Append('"');
                stringBuilder.Append(field.Key);
                stringBuilder.Append("\": ");
                switch (field.Value)
                {
                    // A Kellox Instance✨
                    case KelloxInstance:
                        depth++;
                        stringBuilder.Append(' ');
                        stringBuilder.Append(field.Value is not null ? field.Value : KeywordConstants.NilKeyword);
                        depth--;
                        break;
                    //Strings
                    case string:
                        stringBuilder.Append('"');
                        stringBuilder.Append(field.Value);
                        stringBuilder.Append('"');
                        break;
                    //Numbers, Boolean's and nil
                    default:
                        stringBuilder.Append(field.Value is not null ? field.Value : KeywordConstants.NilKeyword);
                        break;
                }
                stringBuilder.AppendLine(",");
            }
            for (int i = 0; i < depth - 1; i++)
            {
                stringBuilder.Append('\t');
            }
            stringBuilder.Append('}');
            if (depth is 1)
            {
                stringBuilder.Append('\n');
            }
            return stringBuilder.ToString();
        }
    }
}
