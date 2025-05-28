using System;
using System.Collections.Generic;
using System.Text;

namespace XmlFormat.SAX;

public static class XmlTokenParserExtension
{
    public static string ToString(this IEnumerable<XmlTokenParser.Attribute> attributes)
    {
        StringBuilder sb = new();
        sb.Append("[");
        foreach (var attr in attributes)
        {
            sb.Append(attr.ToString());
        }
        sb.Append("]");
        return sb.ToString();
    }

    public static string ToString(this XmlTokenParser.Attribute[] attributes)
    {
        return ((IEnumerable<XmlTokenParser.Attribute>)attributes).ToString();
    }
}
