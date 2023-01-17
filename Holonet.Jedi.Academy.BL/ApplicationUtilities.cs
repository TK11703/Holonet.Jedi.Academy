using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http; 

namespace Holonet.Jedi.Academy.BL
{
    public class ApplicationUtilities
    {
        public const string SHORTDATEFORMAT = "dd MMM yyyy";
        public const string FULLDATEFORMAT = "dd MMM yyyy HH:mm";
        public const string ISO8601FORMAT = "yyyy-MM-ddTHH:mm:ss.sssZ";
        public const string FULLTIMEFORMAT = "HH:mm:ss";
        public const string MILTIMEFORMAT = "HH:mm";
        public const string STDTIMEFORMAT = "hh:mm tt";
        public const string LINEBREAK = "\r\n";

        public ApplicationUtilities()
        {

        }

        /// <summary>
        /// Based on the current HTTP context, the absolute URL path is generated.
        /// </summary>
        /// <param name="context">An HTTP context</param>
        /// <returns>The URL path generated</returns>
        public static string GetAbsoluteURLPath(HttpContext context)
        {
            var request = context.Request;
            UriBuilder uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            return uriBuilder.Uri.AbsolutePath;
        }

        /// <summary>
        /// Recusively steps through the exception details and generated one error message, where each inner exception is seperated by the indicated string.
        /// </summary>
        /// <param name="err">The base exception to start building the error.</param>
        /// <param name="seperator">A value to add between the exception and inner exception.</param>
        /// <returns>A string of all combined exceptions and inner exceptions provided as input</returns>
        public static string GetExceptionMessages(Exception err, string seperator)
        {
            string ret = string.Empty;
            if (err.InnerException == null)
            {
                return err.Message;
            }
            else
            {
                ret += err.Message + seperator + ApplicationUtilities.GetExceptionMessages(err.InnerException, seperator);
            }
            return ret;
        }

        /// <summary>
        /// Recusively steps through the exception details (with stack trace) and generated one error message, where each inner exception is seperated by the indicated string.
        /// </summary>
        /// <param name="err">The base exception to start building the error.</param>
        /// <param name="seperator">A value to add between the exception and inner exception.</param>
        /// <returns>A string of all combined exceptions and inner exceptions provided as input</returns>
        public static string GetDetailedExceptionMessages(Exception err, string seperator)
        {
            string ret = string.Empty;
            if (err.InnerException == null)
            {
                return err.Message + Environment.NewLine + err.StackTrace;
            }
            else
            {
                ret += err.Message + seperator + ApplicationUtilities.GetDetailedExceptionMessages(err.InnerException, seperator);
            }
            return ret;
        }

        /// <summary>
        /// Determines the file extension of the filename
        /// </summary>
        /// <param name="fileName">A name or path of a file.</param>
        /// <returns>The extension of a path or file name, without the dot.</returns>
        public static string GetFileExtension(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string extension = Path.GetExtension(fileName);
                return extension.Replace(".", "");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the filename from a path
        /// </summary>
        /// <param name="filePath">A path to use for a filename search</param>
        /// <returns>A filename if found</returns>
        public static string GetFileName(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string fileName = Path.GetFileName(filePath);
                return fileName;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the integer value of a query string parameter, if found, in a HTTP context item.
        /// </summary>
        /// <param name="context">The HTTP context item to search for the query string item.</param>
        /// <param name="queryStringItem">The name of the query string parameter to return.</param>
        /// <returns>The integer value of the parameter, or a -1 if not found.</returns>
        public static int RetrieveItemID(HttpContext context, string queryStringItem)
        {
            int itemId = -1;
            if (context != null && context.Request != null)
            {
                if (context.Request.QueryString != null && !String.IsNullOrEmpty(context.Request.Query[queryStringItem]))
                {
                    try
                    {
                        itemId = Convert.ToInt32(context.Request.Query[queryStringItem]);
                    }
                    catch
                    {
                        itemId = -1;
                    }
                }
            }
            return itemId;
        }

        public static string RetrieveQueryStringItem(HttpContext context, string queryStringItem)
        {
            if (context != null && context.Request != null)
            {
                if (context.Request.QueryString != null && !String.IsNullOrEmpty(context.Request.Query[queryStringItem]))
                {
                    return context.Request.Query[queryStringItem];
                }
                else if (context.Request.Headers != null && context.Request.Headers.Count > 0 && !String.IsNullOrEmpty(context.Request.Headers["Referer"]))
                {
                    string httpReferrer = context.Request.Headers["Referer"];
                    return ApplicationUtilities.RetrieveQueryStringVariable(httpReferrer, queryStringItem);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string RetrieveQueryStringVariable(string uniformResourceLocation, string variableName)
        {
            string variableValue = string.Empty;
            string[] urlparts = uniformResourceLocation.Split('?');
            if (urlparts.Length > 1)
            {
                string[] queryVariables = urlparts[1].Split('&');
                if (queryVariables.Length > 0)
                {
                    bool found = false;
                    int index = 0;
                    while (!found && index < queryVariables.Length)
                    {
                        string[] queryVariablePair = queryVariables[index].Split('=');
                        if (queryVariablePair.Length > 0 && queryVariablePair[0].ToLower() == variableName.ToLower())
                        {
                            variableValue = queryVariablePair[1];
                            found = true;
                        }
                        index++;
                    }
                }
            }
            return variableValue;
        }

        public static string GetContentType(string fileExtension)
        {
            string detectedContentType = string.Empty;
            switch (fileExtension.ToLower())
            {
                case "pdf":
                case ".pdf":
                    detectedContentType = "application/pdf"; break;
                case "doc":
                case ".doc":
                    detectedContentType = "application/msword"; break;
                case "xls":
                case ".xls":
                    detectedContentType = "application/ms-excel"; break;
                case "txt":
                case ".txt":
                    detectedContentType = "text/plain"; break;
                case "rtf":
                case ".rtf":
                    detectedContentType = "application/rtf"; break;
                case "jpg":
                case ".jpg":
                case "jpeg":
                case ".jpeg":
                    detectedContentType = "image/jpeg"; break;
                case "png":
                case ".png":
                    detectedContentType = "image/png"; break;
                case "gif":
                case ".gif":
                    detectedContentType = "image/gif"; break;
                default:
                    detectedContentType = "application/octet-stream"; break;
            }
            return detectedContentType;
        }

        public static DateTime CalculateFutureDate(DateTime fromDate, int numberofWorkDays, ICollection<DateTime> holidays)
        {
            var futureDate = fromDate;
            IEnumerable<int> daterange = null;
            if (numberofWorkDays > 0)
            {
                daterange = Enumerable.Range(1, ((numberofWorkDays * 2) + holidays.Count));
            }
            else
            {
                daterange = Enumerable.Range(1, holidays.Count);
            }
            var dateSet = daterange.Select(d => futureDate.AddDays(d));
            var saturdays = dateSet.Where(s => s.DayOfWeek == DayOfWeek.Saturday);
            var sundays = dateSet.Where(s => s.DayOfWeek == DayOfWeek.Sunday);
            var dateSetElim = dateSet.Except(holidays).Except(saturdays).Except(sundays);

            //zero-based array
            int index = 0;
            if (numberofWorkDays > 0)
            {
                index = numberofWorkDays - 1;
            }

            futureDate = dateSetElim.ElementAt(index);
            return futureDate;
        }

        public static string ShortenDayOfWeek(string input)
        {
            string output = input;
            if (output.Contains("Sunday"))
            {
                output = output.Replace("Sunday", "Sun");
            }
            if (output.Contains("Monday"))
            {
                output = output.Replace("Monday", "Mon");
            }
            if (output.Contains("Tuesday"))
            {
                output = output.Replace("Tuesday", "Tue");
            }
            if (output.Contains("Wednesday"))
            {
                output = output.Replace("Wednesday", "Wed");
            }
            if (output.Contains("Thursday"))
            {
                output = output.Replace("Thursday", "Thu");
            }
            if (output.Contains("Friday"))
            {
                output = output.Replace("Friday", "Fri");
            }
            if (output.Contains("Saturday"))
            {
                output = output.Replace("Saturday", "Sat");
            }
            return output;
        }

        public static string ShortenMonth(string input)
        {
            string output = input;
            if (output.Contains("January"))
            {
                output = output.Replace("January", "Jan");
            }
            if (output.Contains("February"))
            {
                output = output.Replace("February", "Feb");
            }
            if (output.Contains("March"))
            {
                output = output.Replace("March", "Mar");
            }
            if (output.Contains("April"))
            {
                output = output.Replace("April", "Apr");
            }
            if (output.Contains("May"))
            {
                output = output.Replace("May", "May");
            }
            if (output.Contains("June"))
            {
                output = output.Replace("June", "Jun");
            }
            if (output.Contains("July"))
            {
                output = output.Replace("July", "Jul");
            }
            if (output.Contains("August"))
            {
                output = output.Replace("August", "Aug");
            }
            if (output.Contains("September"))
            {
                output = output.Replace("September", "Sep");
            }
            if (output.Contains("October"))
            {
                output = output.Replace("October", "Oct");
            }
            if (output.Contains("November"))
            {
                output = output.Replace("November", "Nov");
            }
            if (output.Contains("December"))
            {
                output = output.Replace("December", "Dec");
            }
            return output;
        }

        public static string FormatFileSize(double bytesToConvert)
        {
            const int kbValue = 1024;
            const long mbValue = kbValue * 1024;
            const long gbValue = mbValue * 1024;
            if (bytesToConvert < kbValue)
            {
                return string.Format("{0} bytes", bytesToConvert);
            }
            else if (bytesToConvert >= kbValue && bytesToConvert < mbValue)
            {
                return string.Format("{0:#.##} KB", bytesToConvert / kbValue);
            }
            else if (bytesToConvert >= mbValue && bytesToConvert < gbValue)
            {
                return string.Format("{0:#.##} MB", bytesToConvert / mbValue);
            }
            else if (bytesToConvert >= gbValue)
            {
                return string.Format("{0:#.##} GB", bytesToConvert / gbValue);
            }
            else
            {
                return string.Format("{0} bytes", bytesToConvert);
            }
        }

        public static int GetBusinessDays(DateTime begin, DateTime end)
        {
            TimeSpan diff = begin.Date.Subtract(end.Date);
            DateTime nd;
            int businessDayCounter = 0;
            for (int i = 0; i < diff.Days; i++)
            {
                nd = begin.AddDays(i);
                if (nd.DayOfWeek != DayOfWeek.Saturday && nd.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDayCounter++;
                }
            }
            return businessDayCounter;
        }
    }
}