using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.TestClientUI
{
    public class GlobalIdentifierViewModel
    {
        public long GID { get; set; }

        public string Type { get; set; }
    }

    public class ClassTypeViewModel
    {
        public ModelCode ClassType { get; set; }
    }

    public class PropertyViewModel
    {
        public ModelCode Property { get; set; }
    }

    public static class StringAppender
    {
        public static void AppendReferenceVector(StringBuilder sb, Property property)
        {
            sb.Append($"\t{property.Id}: {Environment.NewLine}");
            foreach (long gid in property.AsReferences())
            {
                sb.Append($"\t\tGid:{gid}{ Environment.NewLine}");
            }
        }

        public static void AppendReference(StringBuilder sb, Property property)
        {
            sb.Append($"\t{property.Id}: {property.AsReference()}{Environment.NewLine}");
        }

        public static void AppendString(StringBuilder sb, Property property)
        {
            sb.Append($"\t{property.Id}: {property.AsString()}{Environment.NewLine}");
        }

        public static void AppendFloat(StringBuilder sb, Property property)
        {
            sb.Append($"\t{property.Id}: {property.AsFloat()}{Environment.NewLine}");
        }

        public static void AppendLong(StringBuilder sb, Property property)
        {
            sb.Append($"\t{property.Id}: {property.AsLong()}{Environment.NewLine}");
        }
    }
}
