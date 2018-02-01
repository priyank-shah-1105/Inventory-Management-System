using System;
using System.ComponentModel;

namespace CommonLayer
{
    #region Enum Extension
    public static class EnumExtension
    {
        /// <summary>
        /// The get description.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDescription(this Enum element)
        {
            var type = element.GetType();
            var memberInfo = type.GetMember(Convert.ToString(element));
            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return Convert.ToString(element);
        }

        /// <summary>
        /// The get string value
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetStringValuefromEnum(Enum value)
        {
            string output = null;
            var type = value.GetType();
            {
                // Look for our 'StringValueAttribute' in the field's custom attributes
                var fi = type.GetField(Convert.ToString(value));
                var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    output = attrs[0].Value;
                }
            }

            return output;
        }
    }

    public sealed class StringValueAttribute : Attribute
    {
        /// <summary>
        /// The _value.
        /// </summary>
        private readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringValueAttribute"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public StringValueAttribute(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }
    }
    #endregion

    public enum MailTemplate
    {
        [Description("ForgotPassword.html")]
        ForgotPassword,

        [Description("Error.html")]
        Error,
    }

    public class ChartType
    {
        public static string Line = "line";
        public static string Column = "column";
        public static string Bar = "bar";
        public static string Area = "area";
        public static string Stacked = "stack";
    }

    public enum AuditTrailOperations
    {
        Insert,
        Update,
        Delete,
    }

    public class MonthClass
    {
        public String MonthaName { get; set; }
        public Int32? MonthaValue { get; set; }
    }

    public enum ColumnType
    {
        Column,
        Line,
        Area,
        Donut
    }

    public enum AxisLocation
    {
        Left,
        Right
    }

    public enum FollowStatus
    {
        CallBack = 1,
        PromiseToPay = 2,
        FollowUp = 3,
        DeadLead = 4,
        Confirm = 5,
        Activate = 6,
    }

    public enum RoleEnum
    {
        Admin = 1,
        Executive = 2,
        HR = 3,
        DeadLead = 4,
        Support = 5,
        Account = 6,
        Leader = 7,
    }

    public enum LeadType
    {
        AdminForward = 1,
        GoogleForward = 2,
        SelfGenerated = 3,
    }

    #region MultipleItemStatus
    public enum MultipleItemStatus
    {
        [Description("Add")]
        Add,
        [Description("Edit")]
        Edit,
        [Description("Delete")]
        Delete,
        [Description("NoChange")]
        NoChange
    }
    #endregion
}
