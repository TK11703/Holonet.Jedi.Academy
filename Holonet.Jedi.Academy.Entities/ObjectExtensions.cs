using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Holonet.Jedi.Academy.Entities
{
    public static class ObjectExtensions
    {
        public static T DeepCopy<T>(this T objectToCopy)
        {
            T copy;
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, objectToCopy);
                ms.Position = 0;
                copy = (T)serializer.ReadObject(ms);
            }
            return copy;
        }

        public static DateTime ToTimeZone(DateTime input, string timezone)
        {
            TimeZoneInfo tzTo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            DateTime output = TimeZoneInfo.ConvertTimeFromUtc(input.ToUniversalTime(), tzTo);
            return output;
        }

        public static DateTime AdjustToLocalTimeZone(DateTime input)
        {
            return input.ToLocalTime();
        }

        public static DateTime EndOfDay(this DateTime d)
        {
            return DateTime.Parse(d.ToShortDateString().Trim() + " 23:59:59");
        }

        // -------------------------------- Method to Check for the existence of a column within a datarecord ---------------------- /

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        //Fisher-Yates in-place shuffle O(n) 
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string TrimIfNotNull(this string item)
        {
            if (string.IsNullOrEmpty(item))
                return item;
            else
                return item.Trim();
        }

        public static string StripHTMLTags(this string markup)
        {
            try
            {
                if (!string.IsNullOrEmpty(markup))
                {
                    markup = Regex.Replace(markup, "<[^>]*(>|$)", string.Empty);
                    //markup = Regex.Replace(markup, @"[\s\r\n]+", " ");
                }
                return markup;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool Between<T>(this T source, T low, T high) where T : IComparable
        {
            return source.CompareTo(low) >= 0 && source.CompareTo(high) <= 0;
        }

        //public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string memberName, bool asc = true)
        //{
        //    ParameterExpression[] typeParams = new ParameterExpression[] { Expression.Parameter(typeof(T), "") };
        //    System.Reflection.PropertyInfo pi = typeof(T).GetProperty(memberName);

        //    return (IOrderedQueryable<T>)query.Provider.CreateQuery(
        //        Expression.Call(
        //            typeof(Queryable),
        //            asc ? "OrderBy" : "OrderByDescending",
        //            new Type[] { typeof(T), pi.PropertyType },
        //            query.Expression,
        //            Expression.Lambda(Expression.Property(typeParams[0], pi), typeParams))
        //        );
        //}
    }
}
