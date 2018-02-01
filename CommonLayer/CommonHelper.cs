using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace CommonLayer
{
    public static class CommonHelper
    {
        public const string DateFormat = "dd/MM/yyyy";

        public const string MonthFormat = "MMM yyyy";

        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";

        public const int PageSize = 10;

        public const string PleaseSelect = "---Select---";

        public static string GetErrorMessage(Exception ex, bool getStactRetrace = true)
        {
            try
            {
                var errorMessage = String.Empty;
                if (ex == null) return errorMessage;
                errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += Environment.NewLine + GetErrorMessage(ex.InnerException, getStactRetrace);
                if (getStactRetrace)
                    errorMessage += Environment.NewLine + ex.StackTrace;

                errorMessage = errorMessage.Replace("An error occurred while updating the entries. See the inner exception for details.", "");
                return errorMessage;
            }
            catch
            {
                return ex != null ? ex.Message : String.Empty;
            }
        }

        public static void WriteToLogText(string message, string logSource, string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string filePath = Path.Combine(directory, logSource + " " + DateTime.Now.ToString("dd MMM yyyy") + ".txt");
            using (StreamWriter outfile = new StreamWriter(filePath, true))
                outfile.WriteLine(message);
        }


        public static string GetDeleteErrorMessage(Exception ex, bool getStactRetrace = true)
        {
            try
            {
                var errorMessage = String.Empty;
                if (ex == null) return errorMessage;
                errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += Environment.NewLine + GetDeleteErrorMessage(ex.InnerException, getStactRetrace);
                if (getStactRetrace)
                    errorMessage += Environment.NewLine + ex.StackTrace;

                errorMessage = errorMessage.Replace("\"", " ");
                errorMessage = errorMessage.Replace("'", " ");
                return errorMessage;
            }
            catch
            {
                return ex != null ? ex.Message : String.Empty;
            }
        }

        public static string GetDeleteException(Exception exception)
        {
            string errorMessage = GetDeleteErrorMessage(exception, false);
            return errorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint") ? ParseDeleteMessage(errorMessage) : errorMessage;
        }

        private static string ParseDeleteMessage(string message)
        {
            try
            {
                const string str = "This record link to another record(s), you can not delete this record";
                return str;
            }
            catch
            {
                return message;
            }
        }

        public static string GetNameWithspace(string name)
        {
            try
            {
                string name2 = "";
                int nameCount = name.Count();
                for (int i = 0; i < nameCount; i++)
                {
                    char car = name[i];
                    if (Char.IsUpper(car))
                    {
                        if ((i + 1) == nameCount)
                        {
                            if (!Char.IsUpper(name[i - 1]))
                                name2 = name2 + " " + car;
                            else
                                name2 = name2 + car;
                        }
                        else if (((i + 1) < nameCount && Char.IsUpper(name[i + 1])) && ((i - 1) >= 0 && !Char.IsUpper(name[i - 1])))
                        {
                            name2 = name2 + " " + car;
                        }
                        else if (((i + 1) < nameCount && Char.IsUpper(name[i + 1])) && ((i - 1) >= 0 && Char.IsUpper(name[i - 1])))
                        {
                            name2 = name2 + car;
                        }
                        else if (((i + 1) < nameCount && !Char.IsUpper(name[i + 1])) && ((i - 1) >= 0 && Char.IsUpper(name[i - 1])))
                        {
                            name2 = name2 + car;
                        }
                        else
                        {
                            name2 = name2 + " " + car;
                        }
                    }
                    else
                        name2 = name2 + car;
                }

                name2 = name2.Replace("_", " ");

                if (name == "RECOOrders") name = "RECO Orders";
                return name2.Trim();
            }
            catch (Exception)
            {
                return name;
            }

        }

        public static StringBuilder GetMailTemplate(MailTemplate template)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/MailTemplate/" + template.GetDescription());
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                StringBuilder mailContent = new StringBuilder(sr.ReadToEnd());
                sr.Close();
                return mailContent;
            }
            return null;
        }

        public static string SiteRootPathUrl
        {
            get
            {
                string msRootUrl;
                if (HttpContext.Current.Request.ApplicationPath != null && String.IsNullOrEmpty(HttpContext.Current.Request.ApplicationPath.Split('/')[1]))
                {
                    msRootUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +
                                ":" +
                                HttpContext.Current.Request.Url.Port;
                }
                else
                {
                    msRootUrl = HttpContext.Current.Request.ApplicationPath;
                }

                return msRootUrl;
            }
        }

        public static string GetServerData()
        {
            return DateTime.Now.ToShortDateString();
        }

        public static List<MonthClass> GetMonth(bool setDefaultMonth = false)
        {
            var lstMonthName = Enumerable.Range(1, 12).Select(x => DateTimeFormatInfo.CurrentInfo != null ?
                new MonthClass { MonthaName = DateTimeFormatInfo.CurrentInfo.GetMonthName(x), MonthaValue = x } : null).ToList();
            return lstMonthName;
        }

        public static int WeekOfYear(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, DayOfWeek.Monday);

        }

        public static DateTime GetFirstDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            var firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        public static DateTime GetLastDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            var lastDayInWeek = dayInWeek.Date;
            while (lastDayInWeek.DayOfWeek != firstDay)
                lastDayInWeek = lastDayInWeek.AddDays(1);

            return lastDayInWeek;
        }

        public static SelectList MonthSelectList(int? monthId)
        {
            return monthId != null ? new SelectList(GetMonth(), "MonthaValue", "MonthaName", monthId)
                : new SelectList(GetMonth(), "MonthaValue", "MonthaName");
        }

        public static SelectList ToSelectListWithString(Enum enumeration, string selectedvalue = null)
        {
            var list = (Enum.GetValues(enumeration.GetType()).Cast<Enum>().Select(
                d =>
                new
                {
                    ID = d.GetDescription(),
                    Name = d.GetDescription()
                })).ToList().OrderBy(d => d.Name);
            return new SelectList(list, "ID", "Name", selectedvalue);
        }

        public static bool CheckFileExtension(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            string extn = fi.Extension.ToLower();
            return (extn == ".xls" || extn == ".xlsx");
        }

        public static bool CheckFileExists(string fileName)
        {
            return File.Exists(HttpContext.Current.Server.MapPath("~/Upload/") + fileName);
        }

        public static string CreateURL(string ActionName, string ControllerName)
        {
            return SiteRootPathUrl + "/" + ControllerName + "/" + ActionName;
        }
        /// <summary>
        /// Function to add space before capital letter
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string AddSpacesBeforeCapitalLatter(string text)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(text))
                    return "";
                StringBuilder newText = new StringBuilder(text.Length * 2);
                newText.Append(text[0]);
                for (int i = 1; i < text.Length; i++)
                {
                    if (Char.IsUpper(text[i]) && text[i - 1] != ' ')
                        newText.Append(' ');
                    newText.Append(text[i]);
                }
                return newText.ToString();
            }
            catch
            {
                return text;
            }
        }

        /// <summary>
        /// IsUpper
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUpper(this string value)
        {
            // Consider string to be uppercase if it has no lowercase letters.
            return value.All(t => !Char.IsLower(t));
        }

        /// <summary>
        /// ConvertToCamelCase
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConvertToCamelCase(string text)
        {
            try
            {
                if (text.Length == 1) return text;
                TextInfo myTi = new CultureInfo("en-US", false).TextInfo;
                //text = text.ToLower();

                string[] stringArray = text.Split(' ');
                string foramttedText = "";
                foreach (string s in stringArray)
                {
                    if (IsUpper(s))
                        foramttedText += s + " ";
                    else if (s.Length > 2)
                        foramttedText += myTi.ToTitleCase(s) + " ";
                    else if (s.Length == 1)
                        foramttedText += myTi.ToTitleCase(s) + " ";
                    else
                        foramttedText += s + " ";
                }

                return String.IsNullOrWhiteSpace(text) ? text : foramttedText.Trim();
            }
            catch
            {
                return text;
            }
        }


        public static string ShowAlertMessage(string url, string message, string messageType)
        {
            message = message.Replace("'", " ");
            return @"<script type='text/javascript' language='javascript'>$(function() { showMessage('" + url + "',' " + message + "',' " + messageType + "') ; })</script>";
        }
        public static string ShowAlertMessage(string message)
        {
            message = message.Replace("'", " ");
            var strString = @"<script type='text/javascript' language='javascript'>$(function() { showMessageOnly(' " + message + "') ; })</script>";
            return strString;
        }

        public static string DetermineCompName(string ip)
        {
            var myIp = IPAddress.Parse(ip);
            var getIpHost = Dns.GetHostEntry(myIp);
            var compName = getIpHost.HostName.Split('.').ToList();
            return compName.First();
        }
    }


}
