using Oracle.DataAccess.Client;
using System;
using System.Data;

namespace CommonOracleDriver
{
    public static class DbUtility
    {
        /// <summary>
        /// Creates Oracle SP Parameter
        /// </summary>
        /// <param name="parametername">Parameter Name</param>
        /// <param name="type">OracleDb Type</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="direction">Default direction - Input</param>
        /// <returns>OracleParameter type</returns>
        public static OracleParameter CreateParameter(string parametername, OracleDbType type, object value,
            ParameterDirection direction = ParameterDirection.Input)
        {
            return new OracleParameter
            {
                ParameterName = parametername,
                OracleDbType = type,
                Direction = direction,
                Value = value
            };
        }

        public static OracleParameter CreateParameter(string parametername, OracleDbType type, object value, int size,
            ParameterDirection direction = ParameterDirection.Input)
        {
            return new OracleParameter
            {
                ParameterName = parametername,
                OracleDbType = type,
                Direction = direction,
                Value = value,
                Size = size
            };
        }

        public static short ToDbBool(this bool value)
        {
            return value ? (short)1 : (short)0;
        }

        public static short? ToDbBool(this bool? value)
        {
            return value.HasValue ? (value.Value ? (short?)1 : (short?)0) : null;
        }

        public static bool? ToNullableBool(this object value)
        {
            return (value == DBNull.Value)
                ? new bool?()
                : ((short)value == 1);
        }

        public static bool ToBool(this object value)
        {
            return ((short)value == 1);
        }
        public static string ToString(this object value)
        {
            return (value == DBNull.Value)
                ? null
                : (string)value;
        }

        //Parser Delegate
        public delegate bool TryParseDelegate<T>(string input, out T output);


        public static T TryParse<T>(object reader, TryParseDelegate<T> handler) where T : struct
        {
            T result;
            return handler(reader.ToString(), out result) ? result : default(T);
        }

        public static T? TryParseNullable<T>(object reader, TryParseDelegate<T> handler) where T : struct
        {
            T result;
            return handler(reader.ToString(), out result) ? result : (T?)null;
        }

        /// <summary>
        /// Checks whether input can be converted to decimal and returns decimal if yes, otherwise 0
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static decimal GetDecimalValue(object input)
        {
            var result = TryParseNullable<decimal>(input, decimal.TryParse);

            return result ?? 0;

        }

        public static string NullToEmptyString(this string input)
        {
            if (input == null)
                return string.Empty;
            return input;
        }

        public static TDest? ConvertTo<TSource, TDest>(this TSource? source) where TDest : struct where TSource : struct
        {
            if (source == null)
            {
                return null;
            }
            return (TDest)Convert.ChangeType(source.Value, typeof(TDest));
        }


        public static TDest ConvertTo<TSource, TDest>(this TSource source)
            where TDest : struct
            where TSource : struct
        {
            return (TDest)Convert.ChangeType(source, typeof(TDest));
        }
    }
}