using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Reflection;
using System.Xml;
using System.Globalization;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Data;
using System.Linq;

namespace HabitatManagement.Business
{
    public class Functions
    {
        private static Dictionary<string, bool> _userSessionTaskToken;

        #region Converters

        public static decimal ToDec(object o)
        {
            decimal i = 0;

            if (o != null)
                decimal.TryParse(o.ToString(), out i);

            return i;
        }

        public static int ToInt(object o)
        {
            int i = 0;

            if (o is double)
                i = Convert.ToInt32(Math.Truncate((double)o));
            else if (o is decimal)
                i = Convert.ToInt32(Math.Truncate((decimal)o));
            else if (o != null)
                int.TryParse(o.ToString(), out i);

            return i;
        }

        public static int? ToNullableInt(object o)
        {
            int i = 0;

            if (o == null || o == DBNull.Value)
                return null;

            if (o is double)
                i = Convert.ToInt32(Math.Truncate((double)o));
            else if (o is decimal)
                i = Convert.ToInt32(Math.Truncate((decimal)o));
            else if (o != null)
                int.TryParse(o.ToString(), out i);

            return i;
        }

        public static long ToLong(object o)
        {
            long i = 0;
            if (o is double)
                i = (long)(Math.Truncate((double)o));
            else if (o is decimal)
                i = (long)(Math.Truncate((decimal)o));
            else if (o != null)
                long.TryParse(o.ToString(), out i);

            return i;
        }

        public static long? ToNullableLong(object o)
        {
            long i = 0;

            if (o == null || o == DBNull.Value)
                return null;

            long.TryParse(o.ToString(), out i);

            return i;
        }

        public static bool IsValidInt(object o)
        {
            int i;
            return o != null && int.TryParse(o.ToString(), out i);
        }

        public static bool IsValidLong(object o)
        {
            long i;
            return o != null && long.TryParse(o.ToString(), out i);
        }

        public static short ToShort(object o)
        {
            short i = 0;

            if (o != null)
                short.TryParse(o.ToString(), out i);

            return i;
        }

        public static double ToDouble(object o)
        {
            double d = 0.0;

            if (o != null)
                double.TryParse(o.ToString(), out d);

            return d;
        }

        public static double? ToNullableDouble(object o)
        {
            if (o == null || o == DBNull.Value)
                return null;

            return ToDouble(o);
        }

        public static bool? ToNullableBool(object o)
        {
            if (o == null || o == DBNull.Value)
                return null;

            return ToBool(o);
        }

        public static double ToDouble(object o, CultureInfo currentCulture)
        {
            double d = 0.0;

            if (o != null)
                double.TryParse(o.ToString(), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.CurrentCulture, out d);

            return d;
        }

        public static decimal ToDecimal(object o)
        {
            decimal d = 0.0M;

            if (o != null)
                decimal.TryParse(o.ToString(), out d);

            return d;
        }

        public static DateTime ToDateTime(object o)
        {
            DateTime d = DateTime.MinValue;

            if (o != null && o is DateTime)
                d = (DateTime)o;

            return d;
        }

        public static DateTime? ToNullableDateTime(object o)
        {
            if (o == null || o == DBNull.Value)
                return null;

            return ToDateTime(o);
        }

        public static bool ToBool(object o)
        {
            bool b = false;

            if (o != null)
            {
                // Exception - for some reason checked boxes on forms submit two values in a string "true,false"
                if (o is string && (string)o == "true,false")
                    return true;

                bool.TryParse(o.ToString(), out b);
            }

            return b;
        }

        public static Guid ToGuid(object o)
        {
            Guid g = Guid.Empty;
            if (Guid.TryParse(o.ToString(), out g))
            {
                return g;
            }
            else
            {
                Trace.WriteLine("Error: " + g == null ? string.Empty : g.ToString());
            }

            return g;
        }

        /// <summary>
        /// Idhammar database values can be 1/0 or Y/N to represent booleans
        /// This function will convert them to bool
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IdhammarCharToBool(string s)
        {
            bool b = false;
            if (s != null)
            {
                switch (s.Trim())
                {
                    case "1":
                        b = true;
                        break;
                    case "Y":
                        b = true;
                        break;
                    case "0":
                        b = false;
                        break;
                    case "N":
                        b = false;
                        break;
                    default:
                        break;
                }
            }

            return b;
        }

        public static bool IdhammarCharToBool(object o)
        {
            return IdhammarCharToBool(o == null ? null : o.ToString());
        }

        #endregion

        public static bool IsValidPositiveDoubleQuantity(string s)
        {
            //
            // Validate that the quantity is positive integer or double
            //
            // ^            Start regex
            // \d+          Any positive integer
            // ([.]\d+)?    Optional : ( '.' and any positive integer)
            // $            End regex
            //
            if (System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator == ".")
            {
                Regex rx = new Regex(@"^\d+([.]\d+)?$");

                return rx.IsMatch(s);
            }
            else
            {
                Regex rx = new Regex(@"^\d+([,]\d+)?$");

                return rx.IsMatch(s);
            }
        }

        public static bool IsValidPositiveInteger(string s)
        {
            //
            // Validate that the quantity is positive integer or double
            //
            // ^            Start regex
            // \d+          Any positive integer
            // $            End regex
            //
            Regex rx = new Regex(@"^\d+$");

            return rx.IsMatch(s);
        }

        /// <summary>
        /// Trims off white space at the end of a field returned from the database
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimRight(string s)
        {
            if (s == null)
                return s;

            return s.TrimEnd(new char[] { ' ' });
        }

        /// <summary>
        /// Trims off white space at the end of a field returned from the database
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimRight(object o)
        {
            if (o is string)
                return TrimRight((string)o);
            else
                return "";
        }

        public static char ToChar(object o)
        {
            if (o != null && o is char)
                return ((char)o);
            else
                return default(char);
        }

        /// <summary>
        /// Converts the internal value of an enum to a string
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEnumNumericKeyAsString(Enum e)
        {
            return Convert.ToInt32(e).ToString();
        }

        /// <summary>
        /// When doing string format operations on script it is sometimes handy to escape braces and double quotes
        /// </summary>
        /// <returns></returns>
        public static string EscapeDoubleQuotesAndBraces(string s)
        {
            string escaped = s;

            if (escaped != null)
            {
                escaped = escaped.Replace("{", "{{");
                escaped = escaped.Replace("}", "}}");
                escaped = escaped.Replace("\"", "\"\"");
            }

            return escaped;
        }

        public static DateTime GetDateTimeFromISOFormat(string Date, string Time)
        {
            try
            {
                if (string.IsNullOrEmpty(Date))
                    Date = "0";

                if (string.IsNullOrEmpty(Time))
                    Time = "0";

                if (Functions.ToInt(Date) == 0)
                    Date = "00010101";

                if (Functions.ToInt(Time) == 0)
                    Time = "000000";

                // Make sure time is six digit
                Time = String.Format("{0:D6}", Functions.ToInt(Time));

                return new DateTime(ToInt(Date.Substring(0, 4)), ToInt(Date.Substring(4, 2)), ToInt(Date.Substring(6, 2)), ToInt(Time.Substring(0, 2)), ToInt(Time.Substring(2, 2)), 0);
            }
            catch
            {
            }
            return DateTime.MinValue;
        }

        // Are these Is(Not)Null methods needed, overkill?
        public static bool IsNull(object o)
        {
            if (o == null)
            {
                return true;
            }

            return false;
        }

        public static bool IsNotNull(object o)
        {
            if (o == null)
            {
                return false;
            }

            return true;
        }


        public static double CalculateHourMinuteToDouble(int hours, int minutes)
        {
            double ret = 0.0;

            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(hours).AddMinutes(minutes);
            TimeSpan ts = end.Subtract(start);
            ret = ts.TotalHours;
            return ret;
        }

        public static void GetHourAndMinutes(double hrsAndMins, out int hours, out int minutes)
        {
            hours = 0;
            minutes = 0;
            int labourHours1Sign = 1;
            if (hrsAndMins < 0)
            {
                labourHours1Sign = -1;
                hrsAndMins *= labourHours1Sign;
            }
            var timeSpan = TimeSpan.FromHours(hrsAndMins);
            hours = (int)Math.Floor(timeSpan.TotalMinutes / 60);
            minutes = (int)(Math.Round(timeSpan.TotalMinutes) % 60);
            hours *= labourHours1Sign;
            minutes *= labourHours1Sign;
        }

        public static string GetHourAndMinutes(double hrsAndMins)
        {
            string hoursAndMintuesString = string.Empty;
            int hours = 0;
            int minutes = 0;
            GetHourAndMinutes(hrsAndMins, out hours, out minutes);
            return hoursAndMintuesString = (hours > 0 ? hours.ToString().PadLeft(2, '0') : "00") + ":" + (minutes > 0 ? minutes.ToString().PadLeft(2, '0') : "00");
        }
        public static DateTime GetDateWithHoursMinutes(string dateString, string timeString)
        {
            DateTime newDate = DateTime.MinValue;
            string hour = "00";
            string minute = "00";
            if (!string.IsNullOrEmpty(timeString) && timeString.Contains(":"))
            {
                string[] tempTimeString = timeString.Split(':');
                hour = string.IsNullOrEmpty(tempTimeString[0]) ? "00" : tempTimeString[0];
                minute = string.IsNullOrEmpty(tempTimeString[1]) ? "00" : tempTimeString[1];
            }
            else if (!string.IsNullOrEmpty(timeString))
            {
                hour = timeString;
            }
            string tempDateTime = dateString + " " + hour + ":" + minute;
            try
            {
                DateTime.TryParse(tempDateTime, CultureInfo.CurrentCulture, DateTimeStyles.None, out newDate);
            }
            catch { }
            return newDate;
        }
        /// <summary>
        /// Return hour and min from timestring of timepicker
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
        public static Tuple<int, int> GetHoursAndMinutes(string dateTimeString)
        {
            int h = 0, min = 0;
            string hour = "00";
            string minute = "00";
            if (!string.IsNullOrEmpty(dateTimeString) && dateTimeString.Contains(":"))
            {
                string[] tempTimeString = dateTimeString.Split(':');
                hour = string.IsNullOrEmpty(tempTimeString[0]) ? "00" : tempTimeString[0];
                minute = string.IsNullOrEmpty(tempTimeString[1]) ? "00" : tempTimeString[1];
            }
            try
            {
                h = Convert.ToInt32(hour);
                min = Convert.ToInt32(minute);
            }
            catch { }
            return new Tuple<int, int>(h, min);
        }

        public static byte[] GetImageByteArray(string imagePath, ImageFormat imgFormat)
        {
            MemoryStream ms = new MemoryStream();
            Image itemImg = Image.FromFile(imagePath);
            itemImg.Save(ms, imgFormat);
            return ms.ToArray();
        }

        public static ImageFormat GetImageFormat(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            ImageFormat imgFormat = ImageFormat.Png;

            if (extension != null)
            {
                switch (extension.ToLower())
                {
                    case ".png":
                        imgFormat = ImageFormat.Png;
                        break;
                    case ".jpg":
                    case ".jpeg":
                        imgFormat = ImageFormat.Jpeg;
                        break;
                    case ".gif":
                        imgFormat = ImageFormat.Gif;
                        break;
                    case ".bmp":
                        imgFormat = ImageFormat.Bmp;
                        break;
                    case ".tif":
                    case ".tiff":
                        imgFormat = ImageFormat.Tiff;
                        break;
                    case ".icon":
                        imgFormat = ImageFormat.Icon;
                        break;
                    default:
                        imgFormat = null;
                        break;
                }
            }
            return imgFormat;
        }

        public static string GetErrorsFormatData(List<string> errors)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<ul style=\"color:red;margin-left:5px;\">");
            foreach (var error in errors)
            {
                sb.AppendLine("<li>" + error + "</li>");
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        public static string GetErrorsData(List<string> errors)
        {
            var sb = new StringBuilder();
            if (errors != null && errors.Count > 0)
            {
                sb.AppendLine("<ul style=\"margin-left:5px;list-style:none;\">");
                foreach (var error in errors)
                {
                    sb.AppendLine("<li>" + error + "</li>");
                }
                sb.AppendLine("</ul>");
            }
            return sb.ToString();
        }

        public static DateTime GetQuickDate(string quickType)
        {
            DateTime date = DateTime.Now;
            switch (quickType)
            {
                case "Today":
                    date = DateTime.Now;
                    break;
                case "Tomorrow":
                    date = DateTime.Now.AddDays(+1);
                    break;
                case "ThisWeek":
                    date = DateTime.Now.AddDays(-6);
                    break;
                case "Yesterday":
                    date = DateTime.Now.AddDays(-1);
                    break;
                case "LastWeek":
                    date = DateTime.Now.AddDays(-13);
                    break;
                case "NextWeek":
                    date = DateTime.Now.AddDays(7);
                    break;
                default:
                    date = DateTime.MinValue;
                    break;

            }
            return date;
        }

        public static bool GetUserSessionTaskToken(string key)
        {
            bool canceled = false;
            if (_userSessionTaskToken != null && _userSessionTaskToken.ContainsKey(key))
            {
                canceled = _userSessionTaskToken[key];
            }
            return canceled;
        }

        public static void SetUserSessionTaskToken(string key, bool canceled)
        {
            if (_userSessionTaskToken == null)
            {
                _userSessionTaskToken = new Dictionary<string, bool>();
            }
            if (_userSessionTaskToken.ContainsKey(key))
            {
                _userSessionTaskToken[key] = canceled;
            }
            else
            {
                _userSessionTaskToken.Add(key, canceled);
            }
        }

        public static void RemoveUserSessionTaskToken(string key)
        {
            if (_userSessionTaskToken != null && _userSessionTaskToken.ContainsKey(key))
            {
                _userSessionTaskToken.Remove(key);
            }
        }

        public static DataTable ToTVPDataTable(string columnName, string values)
        {
            DataTable tvp = new DataTable();
            tvp.Columns.Add(columnName, typeof(string));
            if (!string.IsNullOrWhiteSpace(values))
            {
                string[] columnValues = values.Trim().Split(',');
                if (columnValues != null)
                {
                    foreach (var value in columnValues)
                    {
                        tvp.Rows.Add(value);
                    }
                }
            }
            return tvp;
        }

        public static DataTable ToTVPDataTable(string columnName, List<string> columnValues)
        {
            DataTable tvp = new DataTable();
            tvp.Columns.Add(columnName, typeof(string));

            if (columnValues != null)
            {
                foreach (var value in columnValues)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                        tvp.Rows.Add(value);
                }
            }
            return tvp;
        }

        public static DataTable ToTVPDataTable<T>(string columnName, IEnumerable<T> columnValues)
        {
            DataTable tvp = new DataTable();
            tvp.Columns.Add(columnName, typeof(T));
            if (columnValues != null)
            {
                foreach (var value in columnValues)
                {
                    tvp.Rows.Add(value);
                }
            }
            return tvp;
        }
        public static string FormatSerialNumber(long serialNumber, string templateformat, DateTime yearStartDate)
        {
            int indexofSerial = templateformat.IndexOfAny("0123456789".ToCharArray());
            string stringFirstPart = templateformat.Substring(0, indexofSerial);
            string stringLastPart = templateformat.Substring(indexofSerial);
            stringLastPart = stringLastPart.Replace("/[0-9]/g", "0");
            stringLastPart = NumberFormat(serialNumber, stringLastPart.Length);
            templateformat = stringFirstPart + stringLastPart;
            // Note:  The Foramt must be either xx/yyww/9999 or xx-yyww-9990 yyww could also be yy or ww
            DateTime currentDate = DateTime.Now.Date;
            DateTime currentYearStartDate = yearStartDate;

            string weeks = ((currentDate - currentYearStartDate).TotalDays / 7).ToString("00");
            templateformat = Regex.Replace(templateformat, "yyyy", currentDate.ToString("yyyy"), RegexOptions.IgnoreCase);
            templateformat = Regex.Replace(templateformat, "yy", currentDate.ToString("yy"), RegexOptions.IgnoreCase);
            templateformat = Regex.Replace(templateformat, "MMM", currentDate.ToString("MMM"));
            templateformat = Regex.Replace(templateformat, "MM", currentDate.ToString("MM"));
            templateformat = Regex.Replace(templateformat, "DD", currentDate.ToString("dd"), RegexOptions.IgnoreCase);
            templateformat = Regex.Replace(templateformat, "WW", weeks, RegexOptions.IgnoreCase);
            try
            {


                return templateformat;
            }
            catch (Exception e)
            {
            }

            return string.Empty;
        }
        private static string NumberFormat(long number, int length)
        {
            try
            {
                string _str = "00000000000000" + number;

                return _str.Substring(_str.Length - length, length);
            }
            catch (Exception e)
            {
            }
            return number.ToString();
        }

        public static string GetFileType(string attachmentPath)
        {
            string extType = Path.GetExtension(attachmentPath);
            if (extType != null)
            {
                switch (extType.ToLower())
                {
                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                    case ".gif":
                    case ".bmp":
                    case ".tif":
                    case ".tiff":
                    case ".xbm":
                    case ".wbmp":
                    case ".xpm":
                    case ".wmf":
                    case ".emf":
                    case ".wmz":
                    case ".emz":
                        return "image";
                    case ".wmv":
                    case ".avi":
                    case ".flv":
                    case ".mov":
                    case ".mng":
                    case ".f4v":
                    case ".f4p":
                    case ".f4a":
                    case ".f4b":
                    case ".3gp":
                    case ".3g2":
                    case ".svi":
                    case ".amv":
                    case ".mpg":
                    case ".mp4":
                    case ".m4v":
                    case ".ogv":
                    case ".mpeg":
                        return "video";
                    case ".mp3":
                    case ".ogg":
                    case ".wav":
                    case ".aac":
                        return "audio";
                    case ".sql":
                    case ".txt":
                        return "text";
                }
            }
            return null;
        }


        public static void DeleteFileFromLocation(string attachmentPath)
        {
            try
            {
                FileInfo file = new FileInfo(attachmentPath);
                if (file.Exists)//check file exsit or not
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(attachmentPath);
                }
            }
            catch (Exception ex)
            {
            }
        }

        // 84203 Delete attachments web - delete files (setting)
        public static bool DeleteAttachmentsFromLocation(string attachmentPath, List<string> attachments)
        {
            var success = true;
            try
            {
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (var attachment in attachments)
                    {
                        string attachmentCombinePath = string.Empty;
                        if (!string.IsNullOrWhiteSpace(attachmentPath))
                        {
                            string filename = Path.GetFileName(attachment);
                            attachmentCombinePath = Path.Combine(attachmentPath, filename);
                        }
                        else
                        {
                            attachmentCombinePath = attachment;
                        }

                        Functions.DeleteFileFromLocation(attachmentCombinePath);
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        public static bool IsValidPurchaseTemplateFormat(string format)
        {
            bool isValid = true;
            if (!string.IsNullOrWhiteSpace(format))
            {
                var charIndex = 0;
                var digitIndex = 0;

                if (!format.Any(char.IsDigit) || !format.Contains("9"))
                {
                    return false;
                }

                for (int i = 0; i < format.Length; i++)
                {
                    if (!(format[0] >= 'a' && format[0] <= 'z' || format[0] >= 'A' && format[0] <= 'Z')
                        || Char.IsLetterOrDigit(format[i]) && (!(format[i] >= 'a' && format[i] <= 'z' || format[i] >= 'A' && format[i] <= 'Z') && format[i] != '9')
                        || (format[i] >= '0' && format[i] <= '8')
                        || (!Char.IsLetterOrDigit(format[i]) && format[i] != '/' && format[i] != '-')
                        || Char.IsWhiteSpace(format[i]))
                    {
                        isValid = false;
                        break;
                    }
                    else if (!Char.IsLetterOrDigit(format[i]) && (format[i] == '/' || format[i] == '-'))
                    {
                        if (charIndex != 0)
                        {
                            int oldCharIndex = charIndex;
                            charIndex = i;
                            if ((oldCharIndex + 1) == charIndex)
                            {
                                isValid = false;
                                break;
                            }
                        }
                        charIndex = i;
                    }

                    if (format[i] == '9')
                    {
                        if (digitIndex != 0)
                        {
                            int oldDigitIndex = digitIndex;
                            digitIndex = i;
                            if ((oldDigitIndex + 1) != digitIndex)
                            {
                                isValid = false;
                                break;
                            }
                        }
                        digitIndex = i;
                    }
                }
            }

            return isValid;
        }


        public static bool IsValidEmail(string email)
        {
            try
            {
                Regex regex = new Regex(@"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
                Match match = regex.Match(email);
                return match.Success;
            }
            catch (Exception ex)
            {
               return false;
            }
        }
    }
}
