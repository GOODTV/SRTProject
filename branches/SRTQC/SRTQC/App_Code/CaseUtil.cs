using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CaseUtil 的摘要描述
/// </summary>
public class CaseUtil
{
    public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        int dateDiff = 0;
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2);
        dateDiff = (ts.Days / 30); //單純回傳月數
        return dateDiff;
    }
}