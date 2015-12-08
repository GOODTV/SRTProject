using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.SessionState;
using System.Security.Cryptography;
using System.Text;
using System.IO;


/// <summary>
/// Summary description for util
/// </summary>

public enum DateType
{
    yyyyMMdd,
    yyyyMMddHHmmss,
    HHmmss
}
public enum EmptyType
{
    ReturnEmpty,
    ReturnNull
}
public enum WeekType
{
    Long, //星期三
    Short //三
}

public class Util
{
    public static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    public static HttpRequest Request { get { return HttpContext.Current.Request; } }
    public static HttpResponse Response { get { return HttpContext.Current.Response; } }
    public static HttpServerUtility Server { get { return HttpContext.Current.Server; } }
    public static Page page { get { return HttpContext.Current.CurrentHandler as Page; } }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 字串轉數字(int), 如果無法轉數字, 則轉為 0
    /// </summary>
    public static int String2Number(string strNumber)
    {
        int retValue = 0;
        try
        {
            retValue = Convert.ToInt32(strNumber);
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 字串轉數字(double), 如果無法轉數字, 則轉為 0
    /// </summary>
    public static double String2Double(string strNumber)
    {
        double retValue = 0;
        try
        {
            retValue = Convert.ToDouble(strNumber);
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 數字字串加 1, 如果無法轉數字, 則轉為 0
    /// </summary>
    public static string AddStringNumber(string strNumber)
    {
        int retValue = 0;
        try
        {
            retValue = Convert.ToInt32(strNumber);
            retValue++;
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 數字字串減 1, 如果無法轉數字, 則轉為 0
    /// </summary>
    public static string MinusStringNumber(string strNumber)
    {
        int retValue = 0;
        try
        {
            retValue = Convert.ToInt32(strNumber);
            retValue--;
        }
        catch (Exception)
        {
            retValue = 0;
        }
        return retValue.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 得到字串長度,英文字算 1,中文字算 2
    /// </summary>
    public static int GetStrLength(string strData)
    {
        int nLength = 0;
        for (int i = 0; i < strData.Length; i++)
        {
            if (strData[i] >= 0x3000 && strData[i] <= 0x9FFF)
            {
                nLength += 2;
            }
            else
            {
                nLength++;
            }
        }
        return nLength;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 得到固定長度字串, 右邊未滿部分以空白代替
    /// </summary>
    public static string GetRightPaddedString(string strData, int iPadLen)
    {
        int iDiffLen; //為了計算要 pad 的空格,中英文長度不一樣,所以要先計算
        iDiffLen = GetStrLength(strData) - strData.Length;
        if (iPadLen - iDiffLen < 0)
        {
            return strData;
        }
        return strData.PadRight(iPadLen - iDiffLen, ' ');
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 得到固定長度字串, 左邊未滿部分以空白代替
    /// </summary>
    public static string GetLeftPaddedString(string strData, int iPadLen)
    {
        int iDiffLen; //為了計算要 pad 的空格,中英文長度不一樣,所以要先計算
        iDiffLen = GetStrLength(strData) - strData.Length;
        if (iPadLen - iDiffLen < 0)
        {
            return strData;
        }
        return strData.PadLeft(iPadLen - iDiffLen, ' ');
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以 start 與 end 為界,產生連續數值的 DropDownList
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, int iStart, int iEnd, bool addEmpty, int iDefault)
    {
        ddl.Items.Clear();
        if (addEmpty == true)
        {
            ListItem newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            ddl.Items.Add(newitem);
        }
        for (int i = iStart; i <= iEnd; i++)
        {
            ListItem newitem = new ListItem();
            newitem.Text = i.ToString();
            newitem.Value = i.ToString();
            ddl.Items.Add(newitem);
        }
        if (iDefault != -1)
        {
            ddl.SelectedValue = iDefault.ToString();
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 array 為內容產生 DropDownList
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, string[] arText, string[] arValue)
    {
        ddl.Items.Clear();
        for (int i = 0; i < arText.Length; i++)
        {
            ListItem newitem = new ListItem();
            newitem.Text = arText[i];
            newitem.Value = arValue[i];
            ddl.Items.Add(newitem);
        }
        ddl.SelectedIndex = 0;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 SQL command 為內容產生 DropDownList
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, string strSql, string TextField, string ValueField, bool AddEmpty)
    {
        DataTable dt = NpoDB.GetDataTableS(strSql, null);

        ddl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            ddl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            ddl.Items.Add(newitem);
        }
        ddl.SelectedIndex = 0;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 DataTable 為內容產生 DropDownList
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, DataTable dt, string TextField, string ValueField, bool AddEmpty)
    {
        ddl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            ddl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            ddl.Items.Add(newitem);
        }
        if (ddl.Items.Count > 0)
        {
            ddl.SelectedIndex = 0;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的值(Text 或 Value)定位DropDownList
    /// </summary>
    public static int SetDdlIndex(DropDownList ddl, string value)
    {
        int iRet = GetDdlIndex(ddl, value);
        ddl.SelectedIndex = iRet;
        return iRet;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的值找 DropDownList 的 index
    /// </summary>
    public static int GetDdlIndex(DropDownList ddl, string sItem)
    {
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            if (sItem.Trim() == ddl.Items[i].Text.Trim() || sItem.Trim() == ddl.Items[i].Value.Trim())
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// 以傳入的 DataTable 為內容產生 CheckBoxList
    /// </summary>
    public static void FillCheckBoxList(CheckBoxList cbl, DataTable dt, string TextField, string ValueField, bool AddEmpty)
    {
        cbl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            cbl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            cbl.Items.Add(newitem);
        }
        if (cbl.Items.Count > 0)
        {
            cbl.SelectedIndex = 0;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 DataTable 為內容產生 RadioButtonList
    /// </summary>
    public static void FillRadioButton(RadioButtonList rbl, DataTable dt, string TextField, string ValueField, bool AddEmpty)
    {
        rbl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            rbl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            rbl.Items.Add(newitem);
        }
        if (rbl.Items.Count > 0)
        {
            rbl.SelectedIndex = 0;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 計算 HtmlTable 某個 Col 的加總
    /// </summary>
    public static string CalcuCols(HtmlTableRow row, int ColStart, int ColEnd)
    {
        int iTotal = 0;
        for (int i = ColStart; i <= ColEnd; i++)
        {
            iTotal += String2Number(row.Cells[i].InnerText);
        }
        return iTotal.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 計算 HtmlTable 某個 Col 的加總, 有 Col step 參數
    /// </summary>
    public static string CalcuCols(HtmlTableRow row, int ColStart, int ColEnd, int Step)
    {
        int iTotal = 0;
        for (int i = ColStart; i <= ColEnd; i += Step)
        {
            iTotal += String2Number(row.Cells[i].InnerText);
        }
        return iTotal.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 計算 HtmlTable 某個 Row 的加總
    /// </summary>
    public static string CalcuRows(HtmlTable table, int Col, int RowStart, int RowEnd)
    {
        int iTotal = 0;
        for (int i = RowStart; i <= RowEnd; i++)
        {
            iTotal += String2Number(table.Rows[i].Cells[Col].InnerText);
        }
        return iTotal.ToString();
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 取得 Form 上 Control 的值, 沒有則傳回空字串
    /// </summary>
    public static string GetControlValue(string ItemName)
    {
        string retString = "";
        try
        {
            retString = page.Request.Form[ItemName].ToString();
        }
        catch
        {
            retString = "";
        }
        return retString;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 取得 QueryString 的值, 沒有則傳回空字串
    /// </summary>
    public static string GetQueryString(string ItemName)
    {
        string retString = "";
        try
        {
            retString = page.Request.QueryString[ItemName].ToString();
        }
        catch
        {
            retString = "";

        }
        return retString;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 產生 Insert 的 SQL 指令
    /// </summary>
    public static string CreateInsertCommand(string tableName, List<ColumnData> list, Dictionary<string, object> dict)
    {
        string ReturnStr = "";
        //欄位寫入值
        WriteDict(list, dict);

        string str1 = "";
        string str2 = "";
        ReturnStr += "insert into " + tableName + "(";
        int iCount = list.Count;
        for (int i = 0; i < iCount; i++)
        {
            ColumnData item = list[i];
            if (item.AddFlag == false)
            {
                continue;
            }
            str1 += item.ColumnName;
            str2 += "@" + item.ColumnName;
            //if (i != iCount - 1)
            //{
            str1 += ",";
            str2 += ",";
            //}
        }
        str1 = TrimLastChar(str1);
        str2 = TrimLastChar(str2);

        return ReturnStr + str1 + ") values (" + str2 + ")";
    } //end of CreateInsertCommand()
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 產生 Update 的 SQL 指令
    /// </summary>
    public static string CreateUpdateCommand(string tableName, List<ColumnData> list, Dictionary<string, object> dict)
    {
        string ReturnStr = "";
        //欄位寫入值
        WriteDict(list, dict);

        ReturnStr += "update " + tableName + " set \n";
        int iCount = list.Count;
        for (int i = 0; i < iCount; i++)
        {
            ColumnData item = list[i];
            if (item.UpdateFlag == false)
            {
                continue;
            }
            ReturnStr += item.ColumnName + "=@" + item.ColumnName;
            if (i != iCount - 1)
            {
                ReturnStr += ",";
            }
        }
        ReturnStr += " where 1=1 ";
        for (int i = 0; i < iCount; i++)
        {
            ColumnData item = list[i];
            if (item.ConditionFlag == true)
            {
                ReturnStr += " and " + item.ColumnName + "=@" + item.ColumnName;
            }
        }
        return ReturnStr;
    } //end of CreateUpdateCommand()
    //-------------------------------------------------------------------------------------------------------------
    //欄位寫入值
    private static void WriteDict(List<ColumnData> list, Dictionary<string, object> dict)
    {
        int iCount = list.Count;
        dict.Clear();
        for (int i = 0; i < iCount; i++)
        {
            ColumnData item = list[i];
            dict.Add(item.ColumnName, item.ColumnValue);
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 刪除字串最後一個字
    /// </summary>
    public static string TrimLastChar(string str)
    {
        if (str != null && str.Length > 0)
        {
            str = str.Substring(0, str.Length - 1);
        }
        return str;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 刪除字串最後一個字, 有指定字元
    /// </summary>
    public static string TrimLastChar(string str, char ch)
    {
        if (str != null && str.Length > 0)
        {
            if (str.EndsWith(ch.ToString()))
            {
                str = str.Substring(0, str.Length - 1);
            }
        }
        return str;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 檢查字串內容是否為數字
    /// </summary>
    public static bool IsNumeric(string number)
    {
        try
        {
            Single.Parse(number);
            return true;
        }
        catch
        {
            return false;
        }
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 移除 array 中重複的項目
    /// </summary>
    public static string[] RemoveDuplicates(string[] myStringArray)
    {
        List<string> myStringList = new List<string>();
        foreach (string s in myStringArray)
        {
            if (s == "")
            {
                continue;
            }
            if (!myStringList.Contains(s))
            {
                myStringList.Add(s);
            }
        }
        return myStringList.ToArray();
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 移除以逗點為分格的 string 中重複的項目
    /// </summary>
    public static string[] RemoveDuplicates(string myString)
    {
        string[] myStringArray = myString.Split(',');
        string retStr = "";

        List<string> myStringList = new List<string>();
        foreach (string s in myStringArray)
        {
            if (s == "")
            {
                continue;
            }
            if (!myStringList.Contains(s))
            {
                myStringList.Add(s);
            }
        }
        return myStringList.ToArray();
    }
    //-------------------------------------------------------------------------
    /// <summary>
    ///取得某個日期當月的第一天
    /// </summary>
    public static DateTime GetStartDateOfMonth(DateTime d)
    {
        int Year = d.Year;
        int numberOfDays = DateTime.DaysInMonth(d.Year, d.Month);
        return new DateTime(d.Year, d.Month, 1);
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///取得某個日期當月的最後一天
    /// </summary>
    public static DateTime GetEndDateOfMonth(DateTime d)
    {
        int numberOfDays = DateTime.DaysInMonth(d.Year, d.Month);
        return new DateTime(d.Year, d.Month, numberOfDays, 23, 59, 59);
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///取得某個日期當天的最後時間
    /// </summary>
    public static string GetEndDateOfDay(string strDate)
    {
        try
        {
            DateTime d = Convert.ToDateTime(strDate);
            DateTime d2 = new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);
            return DateTime2String(d2, DateType.yyyyMMddHHmmss, EmptyType.ReturnEmpty);
        }
        catch (Exception ex)
        {
            return strDate;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///取得某個月有幾天
    /// </summary>
    public static int GetNumberOfDays(DateTime d)
    {
        return DateTime.DaysInMonth(d.Year, d.Month);
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以 SQL command 為來源, 取前兩個欄位組成 List of list
    /// </summary>
    public static List<List<string>> GetListFromDB(string strSQL)
    {
        List<List<string>> list = new List<List<string>>();
        List<string> list1 = new List<string>();
        List<string> list2 = new List<string>();
        DataTable dt = NpoDB.GetDataTableS(strSQL, null);
        foreach (DataRow dr in dt.Rows)
        {
            list1.Add(dr[0].ToString());
            list2.Add(dr[1].ToString());
        }
        list.Add(list1);
        list.Add(list2);
        return list;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 輸出 Excel, Word 及 PDF
    /// </summary>
    public static void OutputTxt(string strHtml, string Type, string filName)
    {
        Response.Clear();//從緩衝區資料流清除所有內容輸出
        Response.Buffer = true;//使PWS/IIS要輸出的資料先寫到緩衝區
        Response.ContentType = "text/html";//將檔案顯示在網頁的類型 
        Response.Charset = "utf8";//設定文件編碼改成大五碼字集
        string filename = "attachment;filename=";
        if (Request.Browser.Browser == "IE")
        {
            filName = Server.UrlEncode(filName);
        }

        switch (Type)
        {
            case "1":   //Excel
                Response.ContentType = "application/ms-excel";
                filename += filName + ".xls";

                //開頭0變成文字
                //Response.Write("<meta http-equiv=Content-Type content=text/html; charset=utf-8>"); //避免亂碼
                //Response.Write("<style type=text/css>");
                //Response.Write("td{mso-number-format:\"\\@\";}"); //將所有欄位格式改為"文字"
                //Response.Write("</style>");
                //
                //
                //20140410修改 by Ian_Kao
                //處理excel匯出有空一行的情況
                strHtml = "<style type=text/css>td{mso-number-format:\"\\@\";}</style>" + strHtml;
                break;
            case "2":  //Word
                Response.ContentType = "application/ms-word";
                filename += filName + ".doc";

                //開頭0變成文字
                Response.Write("<meta http-equiv=Content-Type content=text/html; charset=utf-8>"); //避免亂碼
                Response.Write("<style type=text/css>");
                Response.Write("td{mso-number-format:\"\\@\";}"); //將所有欄位格式改為"文字"
                Response.Write("</style>");
                //
                break;
            case "3":  //PDF
                Response.ContentType = "application/OCTET-STREAM";
                filename += filName + ".pdf";

                //開頭0變成文字
                Response.Write("<meta http-equiv=Content-Type content=text/html; charset=utf-8>"); //避免亂碼
                Response.Write("<style type=text/css>");
                Response.Write("td{mso-number-format:\"\\@\";}"); //將所有欄位格式改為"文字"
                Response.Write("</style>");
                //
                break;
            case "4":  //TXT
                //Response.ContentType = "application/ms-txt";
                Response.ContentType = "application/vnd.ms-txt";
                Response.Charset = "big5";
                filename += filName + ".txt";
                break;
            case "5":
                //Response.ContentType = "application/ms-txt";
                Response.ContentType = "application/vnd.ms-txt";
                Response.Charset = "big5";
                filename = filName;
                break;
        }

        //將指定的文件直接寫入HTTP 內容輸出流
        if (Type == "5")
        {
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");//理論上輸出應該是utf-8才正常
        }
        else if (Type == "4")
        {
            Response.AppendHeader("Content-Disposition", filename);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
        }
        else
        {
            Response.AppendHeader("Content-Disposition", filename);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");//理論上輸出應該是utf-8才正常
        }
        System.IO.StringWriter objStringWriter = new System.IO.StringWriter();//定義將資料寫入字串
        System.Web.UI.HtmlTextWriter objHtmlTextWriter = new System.Web.UI.HtmlTextWriter(objStringWriter);
        //2013/07/23, utf8 要加加 BOM, Excel 開檔才不會亂碼 
        if (Type == "4" || Type == "5")
            Response.Write(strHtml);//發送objStringWriter到瀏覽器
        else
            Response.Write("\uFEFF" + strHtml);//發送objStringWriter到瀏覽器
        //Response.Write("\uFEFF" + strHtml.Trim());//發送objStringWriter到瀏覽器
        Response.Flush();
        Response.End();// 結束資料輸出
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 輸入生日,得到年紀
    /// </summary>
    public static int CalculateAge(string strBirthDate)
    {
        int age = 0;
        try
        {
            DateTime birthDate = Convert.ToDateTime(strBirthDate);
            DateTime now = DateTime.Now;
            age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
            {
                age--;
            }
        }
        catch (Exception ex)
        {
        }
        return age;

    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 輸入生日,得到年紀
    /// </summary>
    public static int CalculateAge(DateTime birthDate)
    {
        DateTime now = DateTime.Now;
        int age = now.Year - birthDate.Year;
        if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
        {
            age--;
        }
        return age;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 顯示訊息,連同動態產生的 javascript 碼
    /// </summary>
    public static void ShowSysMsgWithScript(string AnotherScript)
    {
        String Script = "";
        if (Session["Msg"] != null && Session["Msg"].ToString() != "")
        {
            Script = AnotherScript + ";\nalert('" + Session["Msg"].ToString() + "');\n";
            CreateJavaScript(Script);
            Session["Msg"] = "";
        }
        else
        {
            CreateJavaScript(AnotherScript);
        }
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 嵌入 JavaScript 碼
    /// </summary>
    public static void CreateJavaScript(string JavaScript)
    {
        String csname = "PageScript";
        Type cstype = page.GetType();
        ClientScriptManager cs = page.ClientScript;
        cs.RegisterStartupScript(cstype, csname, JavaScript, true);
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 日期資料轉字串, 輸入型態可為 DateTime, string 或資料庫欄位
    /// </summary>
    public static string DateTime2String(object objDate, DateType dateType, EmptyType emptyType)
    {
        if (objDate is DateTime)
        {
            DateTime d = (DateTime)objDate;
            if (dateType == DateType.yyyyMMdd)
            {
                return d.ToString("yyyy/MM/dd");
            }
            else if (dateType == DateType.yyyyMMddHHmmss)
            {
                return d.ToString("yyyy/MM/dd HH:mm:ss");
            }
            else if (dateType == DateType.HHmmss)
            {
                return d.ToString("HH:mm:ss");
            }
        }
        else if (objDate is string)
        {
            string strDate = objDate as string;
            if (strDate == "")
            {
                if (emptyType == EmptyType.ReturnEmpty)
                {
                    return "";
                }
                else if (emptyType == EmptyType.ReturnNull)
                {
                    return null;
                }
            }
            else
            {
                strDate = FixDateTime(strDate);
                //如果字串不能轉成日期,則傳回
                try
                {
                    DateTime d = Convert.ToDateTime(strDate);
                    if (dateType == DateType.yyyyMMdd)
                    {
                        return d.ToString("yyyy/MM/dd");
                    }
                    else if (dateType == DateType.yyyyMMddHHmmss)
                    {
                        return d.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    else if (dateType == DateType.HHmmss)
                    {
                        return d.ToString("HH:mm:ss");
                    }
                }
                catch (Exception ex)
                {
                    if (emptyType == EmptyType.ReturnEmpty)
                    {
                        return "";
                    }
                    else if (emptyType == EmptyType.ReturnNull)
                    {
                        return null;
                    }
                }
            }
        }
        else if (objDate == DBNull.Value)
        {
            if (emptyType == EmptyType.ReturnEmpty)
            {
                return "";
            }
            else if (emptyType == EmptyType.ReturnNull)
            {
                return null;
            }
        }
        return null;
    }
    //--------------------------------------------------------------------------
    /// <summary>
    /// 處理時間日期出現上下午以至於無法寫入資料庫問題
    /// </summary>
    public static string FixDateTime(string retdatetime)
    {
        if (retdatetime.IndexOf(" 上午") >= 0)
        {
            retdatetime = retdatetime.Replace(" 上午", "") + " AM";
        }

        if (retdatetime.IndexOf(" 下午") >= 0)
        {
            retdatetime = retdatetime.Replace(" 下午", "") + " PM";
        }
        if (retdatetime == "")
        {
            return null;
        }
        return retdatetime;
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 為了不Cache而在連結後面加入亂數
    /// </summary>
    /// <param name="Link">連結</param>
    /// <param name="QueryString">自訂Querystring</param>
    /// <param name="forEdit">是否使用於Edit相關連結使用</param>
    /// <returns></returns>
    public static string RedirectByTime(string Link, string QueryString, bool forEdit)
    {
        if (forEdit == true)
            return Link + "?rand=" + DateTime.Now.Ticks.ToString() + "&" + QueryString + "=";
        else
            return Link + "?rand=" + DateTime.Now.Ticks.ToString() + "&" + QueryString;
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 為了不Cache而在連結後面加入亂數
    /// </summary>
    /// <param name="Link">連結</param>
    /// <param name="QueryString">自訂Querystring</param>
    /// <returns></returns>
    public static string RedirectByTime(string Link, string QueryString)
    {
        return Link + "?rand=" + DateTime.Now.Ticks.ToString() + "&" + QueryString;
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// Link後面加，亂數時間
    /// </summary>
    /// <param name="Link"></param>
    /// <returns></returns>
    public static string RedirectByTime(string Link)
    {
        return Link + "?rand=" + DateTime.Now.Ticks.ToString();
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 填入縣市名稱
    /// </summary>
    public static void FillCityData(DropDownList ddl)
    {
        string strSql = "select ZipCode as CityID, Name as CityName\n";
        strSql += "from CODECITYNew where ParentCityID='0' order by Sort\n";
        FillDropDownList(ddl, strSql, "CityName", "CityID", true);
    }
    //----------------------------------------------------------------------
    ///<summary>
    ///填入鄉鎮試資料
    ///</summary>
    public static void FillAreaData(DropDownList ddl, string CityID)
    {
        string strSql = "select ZipCode as AreaID, Name as AreaName\n";
        strSql += "from CODECITYNew where ParentCityID=@CityID order by Sort\n";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CityID", CityID);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        Util.FillDropDownList(ddl, dt, "AreaName", "AreaID", true);
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///取得 GetSessionValue 值,如果失效,導至 default 頁面
    ///</summary>
    public static string GetSessionValue(string SessionName)
    {
        if (Session[SessionName] == null)
        {
            Response.Write(@"<script>window.parent.location.href='../Default.aspx';</script>");
            return "";
        }
        else
        {
            return page.Session[SessionName].ToString();
        }
    }
    //----------------------------------------------------------------------
    /// <summary>
    /// 顯示訊息
    /// </summary>
    public static void ShowMsg(string Msg)
    {
        String cstext = "alert('" + Msg + "');";
        CreateJavaScript(cstext);
    }
    //---------------------------------------------------------------------------
    /// <summary>
    /// 取得 appSettings 的 value
    /// </summary>
    public static string GetAppSetting(string key)
    {
        return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得縣市代碼之函式
    ///</summary>
    public static string GetCityCode(string strCityName, string strParentCity)
    {
        string strRet = "";

        string strSQL = "select ZipCode from CODECITYNew where Name=@CityName and ParentCityID=@ParentCity";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CityName", strCityName);
        dict.Add("ParentCity", strParentCity);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["ZipCode"].ToString();
        }
        return strRet;
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得縣市名稱之函式
    ///</summary>
    public static string GetCityName(string strCityCode)
    {
        string strRet = "";

        string strSQL = "select Name from CODECITYNew where ZipCode=@CityCode";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("CityCode", strCityCode);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["Name"].ToString();
        }
        return strRet;
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得鄉鎮市區代碼之函式
    ///</summary>
    public static string GetAreaCode(string strAreaName)
    {
        string strRet = "";

        string strSQL = "select ZipCode from CODECITYNew where Name=@AreaName";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("AreaName", strAreaName);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["ZipCode"].ToString();
        }
        return strRet;
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得鄉鎮市區名稱之函式
    ///</summary>
    public static string GetAreaName(string strAreaCode)
    {
        string strRet = "";

        string strSQL = "select Name from CODECITYNew where ZipCode=@AreaCode";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("AreaCode", strAreaCode);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["Name"].ToString();
        }
        return strRet;
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得鄉鎮市區父代碼之函式
    ///</summary>
    public static string GetAreaParentID(string strAreaCode)
    {
        string strRet = "";

        string strSQL = "select ParentCityID from CODECITYNew where ZipCode=@AreaCode";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("AreaCode", strAreaCode);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["ParentCityID"].ToString();
        }
        return strRet;
    }
    //--------------------------------------------------------------------------
    ///<summary>
    ///取得郵遞區號之函式
    ///</summary>
    public static string GetZipCode(string strAreaName)
    {
        string strRet = "";

        string strSQL = "select ZipCode from CODECITYNew where Name=@AreaName";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("AreaName", strAreaName);
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);

        if (dt.Rows.Count > 0)
        {
            strRet = dt.Rows[0]["ZipCode"].ToString();
        }
        return strRet;
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///一般的 GridView, 有 Key list 參數
    ///</summary>
    //---------------------------------------------------------------------------
    public static string ShowGrid(DataTable DataSource, string CssClass, List<string> Keys)
    {
        NPOGridView npoGridView = new NPOGridView();
        npoGridView.Source = NPOGridViewDataSource.fromDataTable;
        npoGridView.dataTable = DataSource;
        npoGridView.Keys = Keys;
        npoGridView.DisableColumn = Keys;
        npoGridView.ShowPage = false;
        if (CssClass != null)
        {
            npoGridView.CssClass = CssClass;
        }
        return npoGridView.Render();
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///一般的GridView
    ///</summary>
    public static string ShowGrid(DataTable DataSource, string CssClass)
    {
        List<string> list = new List<string>();
        list.Add("uid");
        return ShowGrid(DataSource, CssClass, list);
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///檢查欄位值是否存在
    ///</summary>
    public static bool Exist(string TableName, string FieldName, string FieldValue)
    {
        string strSql = "select " + FieldName + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName + "=@FieldValue\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue", FieldValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt.Rows.Count > 0;
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///檢查欄位值是否存在
    ///</summary>
    public static bool Exist(string TableName, string FieldName1, string FieldValue1, string FieldName2, string FieldValue2)
    {
        string strSql = "select " + FieldName1 + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName1 + "=@FieldValue1\n";
        strSql += "and " + FieldName2 + "=@FieldValue2\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue1);
        dict.Add("FieldValue2", FieldValue2);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt.Rows.Count > 0;
    }
    //---------------------------------------------------------------------------
    ///<summary>
    ///返回今天的字串
    ///</summary>
    public static string GetToday(DateType dateType)
    {
        return DateTime2String(DateTime.Now, dateType, EmptyType.ReturnNull);
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的值(一個欄位)
    ///</summary>
    public static string GetDBValue(string TableName, string ReturnField, string FieldName, string FieldValue)
    {
        string strSql = "select " + ReturnField + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName + "=@FieldValue\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue", FieldValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        return "";
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的值(兩個欄位)
    ///</summary>
    public static string GetDBValue(string TableName, string ReturnField, string FieldName1, string FieldValue1, string FieldName2, string FieldValue2)
    {
        string strSql = "select " + ReturnField + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName1 + "=@FieldValue1\n";
        strSql += "and " + FieldName2 + "=@FieldValue2\n";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue1);
        dict.Add("FieldValue2", FieldValue2);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        return "";
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的 DataTable(一個欄位)
    ///</summary>
    public static DataTable GetDataTable(string TableName, string FieldName, string FieldValue, string OrderBy, string AscDesc)
    {
        string strSql = "select *\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName + "=@FieldValue1\n";
        if (OrderBy != "" && OrderBy != null)
        {
            strSql += "order by " + OrderBy + "\n";
            strSql += AscDesc;
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的 DataTable(兩個欄位)
    ///</summary>
    public static DataTable GetDataTable(string TableName, string FieldName1, string FieldValue1, string FieldName2, string FieldValue2, string OrderBy, string AscDesc)
    {
        string strSql = "select *\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName1 + "=@FieldValue1\n";
        strSql += "and " + FieldName2 + "=@FieldValue2\n";
        if (OrderBy != "" && OrderBy != null)
        {
            strSql += "order by " + OrderBy + "\n";
            strSql += AscDesc;
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue1);
        dict.Add("FieldValue2", FieldValue2);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的 DataTable(一個欄位)(有需求特別欄位)
    ///</summary>
    public static DataTable GetDataTable(string TableName, string ReturnField, string FieldName, string FieldValue, string OrderBy, string AscDesc)
    {
        string strSql = "select " + ReturnField + "\n";
        strSql += "from " + TableName + "\n";
        strSql += "where " + FieldName + "=@FieldValue1\n";
        if (OrderBy != "" && OrderBy != null)
        {
            strSql += "order by " + OrderBy + "\n";
            strSql += AscDesc;
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///輸入欄位及值,返回合乎條件的 DataTable(一個欄位)(有需求特別欄位)
    ///</summary>
    public static DataTable GetDataTable2(string TableName, string ReturnField, string JoinValue, string FieldName, string FieldValue, string OrderBy, string AscDesc)
    {
        string strSql = "select " + ReturnField + "\n";
        strSql += "from " + TableName + "\n";
        strSql += " " + JoinValue + "\n";
        strSql += "where " + FieldName + "=@FieldValue1\n";
        if (OrderBy != "" && OrderBy != null)
        {
            strSql += "order by " + OrderBy + "\n";
            strSql += AscDesc;
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("FieldValue1", FieldValue);
        DataTable dt = NpoDB.GetDataTableS(strSql, dict);
        return dt;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static string GetWeekString(DateTime d, WeekType weekType)
    {
        string strTemp = "";
        switch (d.DayOfWeek)
        {
            case DayOfWeek.Monday:
                strTemp = "星期一";
                break;
            case DayOfWeek.Tuesday:
                strTemp = "星期二";
                break;
            case DayOfWeek.Wednesday:
                strTemp = "星期三";
                break;
            case DayOfWeek.Thursday:
                strTemp = "星期四";
                break;
            case DayOfWeek.Friday:
                strTemp = "星期五";
                break;
            case DayOfWeek.Saturday:
                strTemp = "星期六";
                break;
            case DayOfWeek.Sunday:
                strTemp = "星期日";
                break;
        }
        if (weekType == WeekType.Short)
        {
            return strTemp.Substring(2);
        }
        else if (weekType == WeekType.Long)
        {
            return strTemp;
        }
        return "參數錯誤";
    }
    //-------------------------------------------------------------------------------------------------------------
    public static void AddTDLine(CssStyleCollection css, int type)
    {
        switch (type)
        {
            case 1234:
                css.Add("border-left", ".5pt solid windowtext");
                css.Add("border-top", ".5pt solid windowtext");
                css.Add("border-right", ".5pt solid windowtext");
                css.Add("border-bottom", ".5pt solid windowtext");
                break;
            case 234:
                css.Add("border-top", ".5pt solid windowtext");
                css.Add("border-right", ".5pt solid windowtext");
                css.Add("border-bottom", ".5pt solid windowtext");
                break;
            case 134:
                css.Add("border-left", ".5pt solid windowtext");
                css.Add("border-right", ".5pt solid windowtext");
                css.Add("border-bottom", ".5pt solid windowtext");
                break;
            case 13:
                css.Add("border-left", ".5pt solid windowtext");
                css.Add("border-right", ".5pt solid windowtext");
                break;
            case 34:
                css.Add("border-right", ".5pt solid windowtext");
                css.Add("border-bottom", ".5pt solid windowtext");
                break;
            case 3:
                css.Add("border-right", ".5pt solid windowtext");
                break;
            case 4:
                css.Add("border-bottom", ".5pt solid windowtext");
                break;
            default:
                break;
        }
    }
    ///<summary>
    ///DataTable 行列交換：ColumnHeader轉為第一欄之Data，第一欄之Data轉為ColumnHeader
    ///</summary>
    public static DataTable ConvertRowColumn(DataTable dtTarget, string strNewColName0, string strNewColNameFromRow)
    {
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add(strNewColName0, typeof(string));
        for (int i = 0; i < dtTarget.Rows.Count; i++)
        {
            dtNew.Columns.Add(dtTarget.Rows[i][strNewColNameFromRow].ToString(), typeof(string));
        }
        foreach (DataColumn dc in dtTarget.Columns)
        {
            DataRow drNew = dtNew.NewRow();
            drNew[strNewColName0] = dc.ColumnName;
            for (int i = 0; i < dtTarget.Rows.Count; i++)
            {
                drNew[i + 1] = dtTarget.Rows[i][dc].ToString();
            }
            dtNew.Rows.Add(drNew);
        }
        //移除第一筆資料，因已加入到Header
        dtNew.Rows.RemoveAt(0);
        return dtNew;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static DateTime GetDBDateTime()
    {
        string strSql = "select GetDate()";
        DataTable dt = NpoDB.GetDataTableS(strSql, null);

        DateTime dtRet = Convert.ToDateTime(dt.Rows[0][0].ToString());
        return dtRet;
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///取得單位為KB的檔案Size
    ///</summary>
    public static double GetFileSizeKB(int intLength)
    {
        int intKbSize = 1024;
        return intLength / intKbSize;
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///取得單位為MB的檔案Size
    ///</summary>
    public static double GetFileSizeMB(int intLength)
    {
        int intMbSize = 1048576;
        return intLength / intMbSize;
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///excel檔中的日期會轉換成數字,從數字轉回日期
    ///</summary>
    public static string getDateStr(string strValue)
    {
        int i = Convert.ToInt32(strValue);
        DateTime d1 = Convert.ToDateTime("1900-1-1");
        DateTime d2 = d1.AddDays(i - 2);
        string strTemp = d2.ToString("d");

        return strTemp;
    }
    ///<summary >
    ///取得 Close_Day, 並確認是否超過關帳日期
    ///</summary>
    public static bool Get_Close(string Close_Kind, string Dept_Id, string Donate_Date, string UserID)
    {
        string strSQL = "select * from Dept where DeptId='" + Dept_Id + "' ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataTable dt = NpoDB.GetDataTableS(strSQL, dict);
        string Donate_Close = "";
        string Donate_Open_User = "";
        string Donate_Open_LastDate = "";
        if (dt.Rows.Count > 0)
        {
            if (Close_Kind == "1")
            {
                Donate_Close = dt.Rows[0]["Donate_Close"].ToString();
                Donate_Open_User = dt.Rows[0]["Donate_Open_User"].ToString();
                Donate_Open_LastDate = dt.Rows[0]["Donate_Open_LastDate"].ToString();
            }
            else if (Close_Kind == "2")
            {
                Donate_Close = dt.Rows[0]["Contribute_Close"].ToString();
                Donate_Open_User = dt.Rows[0]["Contribute_Open_User"].ToString();
                Donate_Open_LastDate = dt.Rows[0]["Contribute_Open_LastDate"].ToString();
            }
        }

        //true為已關帳，false為尚未關帳
        bool result = true;
        if (Donate_Close != "")
        {
            if (Convert.ToDateTime(Donate_Date) <= Convert.ToDateTime(Donate_Close))
            {
                if (Donate_Open_LastDate != "" && Donate_Open_User != "")
                {
                    if (Convert.ToDateTime(Donate_Open_LastDate) == DateTime.Now && Donate_Open_User == UserID)
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }
        return result;
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary>
    ///取得由左數來第N位字串
    ///</summary>
    public static string Left(string str, int len)
    {
        if (str.Trim().Length > 0)
        {
            return str.Substring(0, len > str.Length ? str.Length : len);
        }
        else
        {
            return "";
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary >
    /// 取得由右數來第N位字串
    ///</summary>
    public static string Right(string str, int len)
    {
        if (str.Trim().Length > 0)
        {
            return str.Substring(len > str.Length ? 0 : str.Length - len);
        }
        else
        {
            return "";
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary >
    /// 取得起始位置至結束之字串
    ///</summary>
    public static string Mid(string str, int start, int len)
    {
        if (str.Trim().Length > 0)
        {
            int iStartPoint = start > str.Length ? str.Length : start;
            return str.Substring(iStartPoint, iStartPoint + len > str.Length ? str.Length - iStartPoint : len);
        }
        else
        {
            return "";
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary >
    /// 取得字元對應的ASCII碼
    ///</summary>
    public static int Asc(string character)
    {
        if (character.Trim().Length == 1)
        {
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
            return (intAsciiCode);
        }
        else
        {
            throw new ApplicationException("Character is not valid.");
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    ///<summary >
    ///填入空字串
    ///</summary>
    public static string Space(int i)
    {
        string str = "";
        for (int j = 0; j < i; j++)
        {
            str += " ";
        }
        return str;
    }
    /// <summary>
    /// 三位一撇 + 小數??位
    /// </summary> 
    /// <param name="pInt">傳入字串數字。
    /// <param name="小數點要幾位">傳入字串。
    public static string ToThree(string pInt, string p0)
    {
        string money = string.Empty;

        if (pInt != "")
        {
            if (IsNumeric(pInt))
            {
                money = string.Format("{0:N" + p0 + "}", Convert.ToDecimal(pInt));
            }
        }
        else
        {
            money = string.Format("{0:N" + p0 + "}", "0");

        }
        return money;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 DataTable 為內容產生 ListBox
    /// </summary>
    public static void FillListBox(ListBox lst, DataTable dt, string TextField, string ValueField, bool AddEmpty)
    {
        lst.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = "";
            newitem.Value = "";
            lst.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            lst.Items.Add(newitem);
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    public static void FillDropDownList(DropDownList ddl, int iStart, int iEnd, int iDefault, bool PadLeft, int Width, char PadChar)
    {
        ddl.Items.Clear();
        for (int i = iStart; i <= iEnd; i++)
        {
            System.Web.UI.WebControls.ListItem newitem = new System.Web.UI.WebControls.ListItem();
            if (PadLeft == true)
            {
                newitem.Text = i.ToString().PadLeft(Width, PadChar);
                newitem.Value = i.ToString();
            }
            else
            {
                newitem.Text = i.ToString();
                newitem.Value = i.ToString();
            }
            ddl.Items.Add(newitem);
        }
        if (iDefault != -1)
        {
            ddl.SelectedValue = iDefault.ToString();
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 SQL command 為內容產生 DropDownList,  有 EmptyValue, EmptyText 參數
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, string strSql, string TextField, string ValueField, bool AddEmpty, string EmptyValue, string EmptyText)
    {
        DataTable dt = NpoDB.GetDataTableS(strSql, null);

        ddl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = EmptyText;
            newitem.Value = EmptyValue;
            ddl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            ddl.Items.Add(newitem);
        }
        ddl.SelectedIndex = 0;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 以傳入的 DataTable 為內容產生 DropDownList, 有 EmptyValue, EmptyText 參數
    /// </summary>
    public static void FillDropDownList(DropDownList ddl, DataTable dt, string TextField, string ValueField, bool AddEmpty, string EmptyValue, string EmptyText)
    {
        ddl.Items.Clear();
        ListItem newitem;
        if (AddEmpty == true)
        {
            newitem = new ListItem();
            newitem.Text = EmptyText;
            newitem.Value = EmptyValue;
            ddl.Items.Add(newitem);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newitem = new ListItem();
            newitem.Text = dt.Rows[i][TextField].ToString();
            newitem.Value = dt.Rows[i][ValueField].ToString();
            ddl.Items.Add(newitem);
        }
        if (ddl.Items.Count > 0)
        {
            ddl.SelectedIndex = 0;
        }
    }
    //-------------------------------------------------------------------------------------------
    public static string ChineseMoney(long money)
    {
        if (money == 0.0)
        {
            return "零元整";
        }

        string strDallor = money.ToString();
        string[] chDallor = new string[] { "", "拾", "佰", "仟", "萬", "拾", "佰", "仟", "億", "拾", "佰", "仟", "兆" };
        string[] chB = new string[] { "", "萬", "億", "兆" };
        int i;
        string result = "";
        int strDallorLen = strDallor.Length;
        for (i = 0; i < strDallorLen; i++)
        {
            if (strDallor[i] != '0')
            {
                result = result + strDallor[i] + chDallor[strDallorLen - i - 1];
            }
            else
            {
                if (result[result.Length - 1] != '零')
                {
                    if ((strDallorLen - i - 1) % 4 == 0)
                        result = result + chB[(strDallorLen - i - 1) / 4];
                    else
                        result = result + "零";
                }
                else
                {
                    if ((strDallorLen - i - 1) % 4 == 0)
                        result = result.Substring(0, result.Length - 1) + chB[(strDallorLen - i - 1) / 4];

                }
            }
        }

        if (result[result.Length - 1] == '零')
        {
            result.Substring(0, result.Length - 1);
        }

        result = result.Replace("1", "壹");
        result = result.Replace("2", "貳");
        result = result.Replace("3", "參");
        result = result.Replace("4", "肆");
        result = result.Replace("5", "伍");
        result = result.Replace("6", "陸");
        result = result.Replace("7", "柒");
        result = result.Replace("8", "捌");
        result = result.Replace("9", "玖");

        return result + "元整";
    }
    //-------------------------------------------------------------------------------------------------------------

    public static string desEncryptBase64(string source)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] key = Encoding.ASCII.GetBytes("81582054");
        byte[] iv = Encoding.ASCII.GetBytes("45028518");
        byte[] dataByteArray = Encoding.UTF8.GetBytes(source);

        des.Key = key;
        des.IV = iv;
        string encrypt = "";
        using (MemoryStream ms = new MemoryStream())
        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(dataByteArray, 0, dataByteArray.Length);
            cs.FlushFinalBlock();
            encrypt = Convert.ToBase64String(ms.ToArray());
        }
        return encrypt;
    }


    public static string desDecryptBase64(string encrypt)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] key = Encoding.ASCII.GetBytes("81582054");
        byte[] iv = Encoding.ASCII.GetBytes("45028518");
        des.Key = key;
        des.IV = iv;

        byte[] dataByteArray = Convert.FromBase64String(encrypt);
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }

} //end of class Util
