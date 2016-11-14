using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PacificSystem.Utility
{
    /// <summary>
    /// キャスト関連ヘルパークラス
    /// </summary>
	public static partial class Converts
    {
        


        /// <summary>
        /// 和暦変換フォーマットタイプ
        /// </summary>
        public enum DateFormatTypeEnum
        {
            /// <summary>ggyy年M月d日</summary>
            DateOnly = 0,
            /// <summary>ggyy年M月d日 HH:mm</summary>
            DateAndTime,
            /// <summary>HH:mm</summary>
            TimeOnly,
            /// <summary>yyyy年M月</summary>
            YearMonthOnly,
        }


        /// <summary>
        /// 小文字カナを大文字カナに変換する
        /// </summary>
        /// <param name="Source">対象の文字列</param>
        /// <returns>変換した文字列</returns>
        public static string ToUpperKana(string Source)
        {
            StringBuilder sb = new StringBuilder(Source);
            sb = sb.Replace('ｧ', 'ｱ');
            sb = sb.Replace('ｨ', 'ｲ');
            sb = sb.Replace('ｩ', 'ｳ');
            sb = sb.Replace('ｪ', 'ｴ');
            sb = sb.Replace('ｫ', 'ｵ');
            sb = sb.Replace('ｬ', 'ﾔ');
            sb = sb.Replace('ｭ', 'ﾕ');
            sb = sb.Replace('ｮ', 'ﾖ');
            sb = sb.Replace('ｯ', 'ﾂ');

            return Convert.ToString(sb);
        }

        /// <summary>
        /// 四捨五入
        /// </summary>
        /// <param name="Source">対象の数値</param>
        /// <param name="Digits">桁の指定</param>
        /// <returns>数値</returns>
        public static decimal ToRound(decimal Source, int Digits)
        {
            decimal result = 0;

            double dCoef = System.Math.Pow(10, Digits);
            double dSource = Convert.ToDouble(Source);

            if (dSource > 0)
            {
                result = Convert.ToDecimal(
                    System.Math.Floor((dSource * dCoef) + 0.5) / dCoef);
            }
            else
            {
                result = Convert.ToDecimal(
                    System.Math.Ceiling((dSource * dCoef) - 0.5) / dCoef);
            }

            return result;
        }

        /// <summary>
        /// 切り上げ
        /// </summary>
        /// <param name="Source">対象の数値</param>
        /// <param name="Digits">桁の指定</param>
        /// <returns>数値</returns>
        public static decimal ToRoundUp(decimal Source, int Digits)
        {
            decimal result = 0;
            double dCoef = System.Math.Pow(10, Digits);
            double dSource = Convert.ToDouble(Source);

            if (dSource > 0)
            {
                result = Convert.ToDecimal(System.Math.Ceiling(dSource * dCoef) / dCoef);
            }
            else
            {
                result = Convert.ToDecimal(System.Math.Floor(dSource * dCoef) / dCoef);
            }

            return result;
        }

        /// <summary>
        /// 切り捨て
        /// </summary>
        /// <param name="Source">対象の数値</param>
        /// <param name="Digits">桁の指定</param>
        /// <returns>数値</returns>
        public static decimal ToRoundDown(decimal Source, int Digits)
        {
            decimal result = 0;
            double dCoef = System.Math.Pow(10, Digits);
            double dSource = Convert.ToDouble(Source);

            if (dSource > 0)
            {
                result = Convert.ToDecimal(System.Math.Floor(dSource * dCoef) / dCoef);
            }
            else
            {
                result = Convert.ToDecimal(System.Math.Ceiling(dSource * dCoef) / dCoef);
            }

            return result;
        }


        /// <summary>
        /// Decimal型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <returns></returns>
        public static decimal ToTryDecimal(object Source)
        {
            return _ToTryDecimal(Source, 0m);
        }

        /// <summary>
        /// Decimal型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns>数値</returns>
        public static decimal ToTryDecimal(object Source, decimal Failure)
        {
            return _ToTryDecimal(Source, Failure);
        }

        /// <summary>
        /// Decimal型に強制キャストする
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        private static decimal _ToTryDecimal(object Source, decimal Failure)
        {
            decimal result = Failure;
            decimal CheckResult = 0;

            if (decimal.TryParse(Convert.ToString(Source), out CheckResult) == true)
            {
                result = CheckResult;
            }

            return result;
        }


        /// <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        public static int ToTryInt(object Source)
        {
            return _ToTryInt(Source, 0);
        }

        // <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns>数値</returns>
        public static int ToTryInt(object Source, int Failure)
        {
            return _ToTryInt(Source, Failure);
        }

        /// <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        private static int _ToTryInt(object Source, int Failure)
        {
            int result = Failure;
            int CheckResult = 0;

            if (int.TryParse(Convert.ToString(Source), out CheckResult) == true)
            {
                result = CheckResult;
            }

            return result;
        }

        /// <summary>
        /// Int型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <returns>数値</returns>
        public static int ToTryInt(bool Source)
        {
            int result = 0;

            if (Source == true)
            {
                result = 1;
            }

            return result;
        }

        /// <summary>
        /// string型に強制キャストする(VB6のNVLと同等の機能)
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns>文字</returns>
        public static string ToTryString(object Source)
        {
            return _ToTryString(Source, "");
        }

        /// <summary>
        /// string型に強制キャストする(VB6のNVLと同等の機能)
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns></returns>
        public static string ToTryString(object Source, string Failure)
        {
            return _ToTryString(Source, Failure);
        }

        /// <summary>
        /// string型に強制キャストする(VB6のNVLと同等の機能)
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        private static string _ToTryString(object Source, string Failure)
        {
            string result = Failure;

            if (String.IsNullOrEmpty(Convert.ToString(Source)) == true) //NULL, Emptyの場合
            {
                return result;
            }

            if (String.IsNullOrWhiteSpace(Convert.ToString(Source)) == true) //NULL, 空白の場合
            {
                return result;
            }

            return Convert.ToString(Source);
        }

        /// <summary>
        /// Bool型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <returns>bool(1or-1:true, 0:false)</returns>
        public static bool ToTryBool(object Source)
        {
            return _ToTryBool(Source, false);
        }

        /// <summary>
        /// Bool型に強制キャストする
        /// </summary>
        /// <param name="Source">対象のデータ</param>
        /// <param name="Failure">失敗した場合のデフォルト値</param>
        /// <returns>bool</returns>
        public static bool ToTryBool(object Source, bool Failure)
        {
            return _ToTryBool(Source, Failure);
        }

        /// <summary>
        /// Bool型に強制キャストする
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Failure"></param>
        /// <returns></returns>
        private static bool _ToTryBool(object Source, bool Failure)
        {
            bool result = Failure;
            int CheckResult = 0;

            bool BoolResult = false;

            if (bool.TryParse(Convert.ToString(Source), out BoolResult) == true)
            {
                result = BoolResult;
            }
            else if (int.TryParse(Convert.ToString(Source), out CheckResult) == true)
            {
                if (CheckResult == -1 || CheckResult == 1)
                {
                    result = true;
                }
                else if (CheckResult == 0)
                {
                    result = false;
                }
                else
                {
                    // Failure
                }
            }

            return result;
        }

        /// <summary>
        /// 暫定で定義。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="failure"></param>
        /// <returns></returns>
        public static double ToTryDouble(object source, double failure)
        {
            double tmp;
            if (Double.TryParse(Convert.ToString(source), out tmp))
            {
                return tmp;
            }
            return failure;
        }

        /// <summary>
        /// 暫定で定義。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="failure"></param>
        /// <returns></returns>
        public static DateTime ToTryDateTime(object source, DateTime failure)
        {
            DateTime tmp;
            if (DateTime.TryParse(Convert.ToString(source), out tmp))
            {
                return tmp;
            }
            return failure;
        }

        /// <summary>
        /// 暫定で定義。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="failure"></param>
        /// <returns></returns>
        public static DateTime? ToTryDateTimeNullable(object source, DateTime? failure)
        {
            DateTime tmp;
            if (DateTime.TryParse(Convert.ToString(source), out tmp))
            {
                return tmp;
            }
            return failure;
        }
    }
}
