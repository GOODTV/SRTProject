using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

/// <summary>
/// BasePage 的摘要描述
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public ILog logger = LogManager.GetLogger("RollingFileAppender");

    public BasePage()
    {
        this.Load += new EventHandler(BasePage_Load);
    }

    /// <summary>
    /// Handles the Load event of the BasePage control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <history>
    /// 1.Tanya Wu, 2012/8/29, Create
    /// </history>
    private void BasePage_Load(object sender, EventArgs e)
    {
        //驗證 Session
        //this.CheckSessionValidation(); 
	}

    /// <summary>
    /// 驗證 Session
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2012/8/29, Create
    /// </history>
    private void CheckSessionValidation()
    {
        try
        {
            if (Session["LoginOK"] == null || Session["LoginOK"].Equals(false))
            {
                string redirectJS = @"parent.location.replace('../Error_Page.aspx');";
                RegisterStartupJS("Redirect", redirectJS);
            }
                
        }
        catch (Exception ex)
        {
            string redirectJS = @"parent.location.replace('../Error_Page.aspx');";
            RegisterStartupJS("Redirect", redirectJS);
            
            this.logger.Error(ex.Message, ex);
        }
    }

    /// <summary>
    /// alert訊息
    /// </summary>
    /// <param name="message">要alert的訊息</param>
    public void AlertMessage(string message)
    {
        string js = "setTimeout(function() { alert('" + EscapeStringForJS(message) + "'); alertFlag = false;},0);";
        //string js = "alert('" + EscapeStringForJS(message) + "');";
        RegisterStartupJS(message, js);
    }

    /// <summary>
    /// Replace characters for Javscript string literals
    /// 放訊息內文，不要連語法一起進去編碼，例如alert('"+ abc +"')";，應該只編碼abc
    /// </summary>
    /// <param name="text">raw string</param>
    /// <returns>escaped string</returns>
    public static string EscapeStringForJS(string s)
    {
        //REF: http://www.javascriptkit.com/jsref/escapesequence.shtml

        return s.Replace(@"\", @"\\")
                .Replace("\b", @"\b")
                .Replace("\f", @"\f")
                .Replace("\n", @"\n")
                .Replace("\0", @"\0")
                .Replace("\r", @"\r")
                .Replace("\t", @"\t")
                .Replace("\v", @"\v")
                .Replace("'", @"\'")
                .Replace(@"""", @"\""");
    }

    /// <summary>
    /// Registers the startup JS.
    /// </summary>
    /// <param name="RegisterName">Name of the register.</param>
    /// <param name="myJavascript">My javascript.</param>
    public void RegisterStartupJS(string RegisterName, string myJavascript)
    {
        string wholeJS = "<SCRIPT language=\"JavaScript\"  type=\"text/javascript\" >" + myJavascript + "</SCRIPT>";
        //wholeJS=EscapeStringForJS(wholeJS);
        if (ExistSM())
        { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), RegisterName, wholeJS, false); }
        else
        { Page.ClientScript.RegisterStartupScript(Page.GetType(), RegisterName, wholeJS); }

    }

    /// <summary>
    /// 檢查頁面上是否存在ScriptManager
    /// </summary>
    /// <returns></returns>
    private bool ExistSM()
    {
        return (ScriptManager.GetCurrent(this.Page) != null);
    }

    /// <summary>
    /// 取得今天日期 格式：YYYY/MM/DD or YYYYMMDD
    /// </summary>
    public string getToday(string Format="YYYY/MM/DD")
    {
        string dt = string.Empty;

        if (Format == "YYYYMMDD")
            dt = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Day.ToString().PadLeft(2, '0');
        else
            dt = DateTime.Today.Year.ToString() + "/" + DateTime.Today.Month.ToString().PadLeft(2, '0') + '/' + DateTime.Today.Day.ToString().PadLeft(2, '0');

        return dt;
    }
    //---------------------------------------------------------------------------
    //Session["Msg"]有值時，會Alert，並清空
    public void ShowSysMsg()
    {
        if (Session["Msg"] != null && Session["Msg"].ToString() != "")
        {
            Alert(Session["Msg"].ToString());
            Session["Msg"] = "";
        }
    }
    //---------------------------------------------------------------------------
    //直接顯示訊息
    public void ShowSysMsg(string SysMsg)
    {
        SetSysMsg(SysMsg);
        ShowSysMsg();
    }
    //---------------------------------------------------------------------------
    //設定訊息
    public void SetSysMsg(string SysMsg)
    {
        Session["Msg"] = SysMsg;
    }
    //---------------------------------------------------------------------------
    //顯示警告
    public void Alert(string AlertMessage)
    {
        String cstext = "alert('" + AlertMessage + "');";

        CreateJavaScript(cstext);
    }
    //---------------------------------------------------------------------------
    //嵌入JavaScript
    public void CreateJavaScript(string JavaScript)
    {
        String csname = "PageScript";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, csname, JavaScript, true);
    }
}