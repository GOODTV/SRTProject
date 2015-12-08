
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web;
using System.Web.SessionState;


/// <summary>
/// Summary description for util
/// </summary>

public class HtmlUtil
{
    public static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    public static HttpRequest Request { get { return HttpContext.Current.Request; } }
    public static HttpResponse Response { get { return HttpContext.Current.Response; } }
    public static HttpServerUtility Server { get { return HttpContext.Current.Server; } }
    public static Page page { get { return HttpContext.Current.CurrentHandler as Page; } }
    //-------------------------------------------------------------------------------------------------------------
    //程式產生 Html 碼
    public static string RenderControl(List<ControlData> list)
    {
        string RenderStr = "";

        foreach (ControlData item in list)
        {
            string strChecked = "";
            string strSelected = "";
            string strDisabled = "";
            string strReadonly = "";
            string strTextWidth = "";
            string strStyle = "";
            if (item.ControlType != "DropdownList")
            {
                item.Text = item.Text.Replace(" ", "&nbsp;");
            }

            if (item.ControlType != "DropdownList") //DropdownList selected 另外處理
            {
                if (item.DefaultValue == null) //Checked 屬性由外部指定
                {
                    if (item.Checked == true)
                    {
                        strChecked = "checked='checked'";
                    }
                }
                else
                {
                    string[] DefaultValues = item.DefaultValue.Split(',');
                    foreach (string str in DefaultValues)
                    {
                        if (item.Value == str)
                        {
                            strChecked = "checked='checked'";
                        }
                    }
                }
            }

            if (item.Disabled == true)
            {
                strDisabled = "disabled='disabled'";
            }
            if (item.Readonly == true)
            {
                strReadonly = "readonly='readonly'";
            }

            if (item.Style != "")
            {
                strStyle = " style='" + item.Style + "' ";
            }
            if (item.TextSize != "")
            {
                strTextWidth = " size='" + item.TextSize + "' ";
            }

            switch (item.ControlType)
            {
                case "Checkbox":
                    RenderStr += "<input type='checkbox' " + strChecked + strDisabled + strStyle + " name='" + item.Name + "' id='" + item.Id + "' value='" + item.Value + "' />" + item.Text + "&nbsp;";
                    break;
                case "Radio":
                    RenderStr += "<input type='radio' " + strChecked + strDisabled + strStyle + " name='" + item.Name + "' id='" + item.Id + "' value='" + item.Value + "' />" + item.Text + "&nbsp;";
                    break;
                case "Text":
                    RenderStr += "<input type='text'  " + strReadonly + strDisabled + strTextWidth + strStyle + "name='" + item.Name + "' id='" + item.Id + "' value='" + item.Value + "' />";
                    break;
                case "Label":
                    RenderStr += "<label>" + item.Text + "</label>";
                    break;
                case "DropdownList":
                    RenderStr += "<select id='" + item.Id + "' name='" + item.Name + "'" + strDisabled + ">\n";
                    for (int i = 0; i < item.OptionValue.Length; i++)
                    {
                        strSelected = "";
                        if (item.OptionValue[i] == item.DefaultValue)
                        {
                            strSelected = "selected";
                        }
                        item.OptionText[i] = item.OptionText[i].Replace(" ", "&nbsp;");
                        RenderStr += "<option " + strSelected + " value='" + item.OptionValue[i] + "'>" + item.OptionText[i] + "</option>\n";
                    }
                    RenderStr += "</select>\n";
                    break;
                case "Image":
                    RenderStr += "<img id='" + item.Id + "' alt='' src='" + item.Value + "' />";
                    break;
            }
            if (item.ShowBR == true)
            {
                RenderStr += "<br/>\n";
            }
        }
        return RenderStr;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 將空白資料轉成對應的Html
    /// </summary>
    public static string Space(int i)
    {
        string Str = "";
        for (int j = 1; j <= i; j++)
        {
            Str += "&nbsp";
        }
        return Str;
    }
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 將一些文字資料轉成對應的Html
    /// </summary>
    public static string ToHtml(string data)
    {
        string result = data.Replace(" ", "&nbsp;");
        result = result.Replace("\r\n", "<br />");
        result = result.Replace("\n", "<br />");
        return result;
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 將一些文字資料轉成對應的Html, 多加 ident 參數
    /// </summary>
    public static string ToHtml(string data, int ident)
    {
        string result = data.Replace(" ", "&nbsp;");
        string ReplaceStr = "<br />";
        for (int i = 0; i < ident; i++)
        {
            ReplaceStr += "&nbsp;";
        }
        result = result.Replace("\r\n", ReplaceStr);
        return result;
    }
    //-------------------------------------------------------------------------
    public static string GetCheckBoxStr(string DisplayStr, string CompareStr)
    {
        return GetCheckBoxStr(DisplayStr, CompareStr, "Checkbox");
    }
    //-------------------------------------------------------------------------
    public static string GetCheckBoxStr(string DisplayStr, string CompareStr, string type)
    {
        string str = "";
        if (Contains(DisplayStr, CompareStr))
        {
            if (type == "Checkbox")
            {
                str += "■" + DisplayStr;
            }
            else
            {
                str += "●" + DisplayStr;
            }
        }
        else
        {
            if (type == "Checkbox")
            {
                str += "□" + DisplayStr;
            }
            else
            {
                str += "○" + DisplayStr;
            }
        }
        return str;
    }
    //-------------------------------------------------------------------------
    public static bool Contains(string DisplayStr, string CompareStr)
    {
        string[] strArray = CompareStr.Split(',');
        foreach (string str in strArray)
        {
            if (str == DisplayStr)
            {
                return true;
            }
        }
        return false;
    }
    //-------------------------------------------------------------------------
    public static string TablePrint(string TableWidth, int BorderWidth, string FontSize, string RowHeight, string[] CellData, string[] Align, string[] CellWidth)
    {
        string strTemp = "";
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        table.Border = BorderWidth;
        table.CellPadding = 3;
        table.CellSpacing = 3;
        css = table.Style;
        css.Add("font-size", FontSize);
        css.Add("font-family", "標楷體");
        css.Add("width", TableWidth);

        row = new HtmlTableRow();
        row.Height = RowHeight;
        for (int i = 0; i < CellData.Length; i++)
        {
            strTemp = CellData[i];
            cell = new HtmlTableCell();
            cell.InnerHtml = (strTemp == "") ? "&nbsp" : strTemp;
            css = cell.Style;
            css.Add("text-align", Align[i]);
            if (i == 0)
            {
                css.Add("border-left", "0px");
            }
            css.Add("border-right", "0px");
            css.Add("border-top", "0px");
            css.Add("border-bottom", "0px");
            css.Add("width", CellWidth[i]);
            row.Cells.Add(cell);
        }
        table.Rows.Add(row);
        ////--------------------------------------------
        //轉成 html 碼
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 以 start 與 end 為界,產生連續數值的 DropDownList, Client 端版
    /// </summary>
    public static string GetDropDownList(string DDLName, int iStart, int iEnd, bool addEmpty, string DefaultValue)
    {
        List<string> NumberList = new List<string>();
        if (addEmpty == true)
        {
            NumberList.Add("");
        }
        for (int i = iStart; i <= iEnd; i++)
        {
            NumberList.Add(i.ToString());
        }
        string[] options = NumberList.ToArray();
        List<ControlData> list = new List<ControlData>();
        list.Add(new ControlData("DropdownList", DDLName, DDLName, options, options, false, DefaultValue));
        return RenderControl(list);
    }
    //-------------------------------------------------------------------------------------------------------------
} //end of class HtmlUtil
//-------------------------------------------------------------------------------------------------------------
public class ControlData
{
    private string _type;
    private string _name;
    private string _id;
    private string _value;
    private string _text;
    private string _textsize;
    private string _style;
    private string _defaultValue;
    private string _src;
    private bool _disabled;
    private bool _readonly;
    private bool _checked;
    string[] _optionValue;
    string[] _optionText;

    public string[] OptionText
    {
        get { return _optionText; }
        set { _optionText = value; }
    }

    public string[] OptionValue
    {
        get { return _optionValue; }
        set { _optionValue = value; }
    }

    public bool Disabled
    {
        get { return _disabled; }
        set { _disabled = value; }
    }
    public bool Readonly
    {
        get { return _readonly; }
        set { _readonly = value; }
    }
    public bool Checked
    {
        get { return _checked; }
        set { _checked = value; }
    }

    public string DefaultValue
    {
        get { return _defaultValue; }
        set { _defaultValue = value; }
    }

    public string Src
    {
        get { return _src; }
        set { _src = value; }
    }

    private bool _showBR;

    public string ControlType
    {
        get { return _type; }
        set { _type = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public string Text
    {
        get { return _text; }
        set { _text = value; }
    }

    public string TextSize
    {
        get { return _textsize; }
        set { _textsize = value; }
    }

    public string Style
    {
        get { return _style; }
        set { _style = value; }
    }

    public bool ShowBR
    {
        get { return _showBR; }
        set { _showBR = value; }
    }

    public ControlData()
    {
    }

    public ControlData(string type, string name, string id, string value, string text, bool showBR, string defaultValue)
    {
        _type = type;
        _name = name;
        _id = id;
        _value = value;
        _text = text;
        _showBR = showBR;
        _defaultValue = defaultValue;
        _textsize = "";
        _style = "";
    }

    public ControlData(string type, string name, string id, string[] optionValue, string[] optionText, bool showBR, string defaultValue)
    {
        _type = type;
        _name = name;
        _id = id;
        _optionValue = optionValue;
        _optionText = optionText;
        _showBR = showBR;
        _defaultValue = defaultValue;
        _textsize = "";
        _style = "";
    }

    public ControlData(string type, string name, string id, string value, string text)
        : this(type, name, id, value, text, false, "")
    {
    }

    public ControlData(string type, string text)
        : this(type, "", "", "", text, false, "")
    {
    }

    public ControlData(string type, string text, bool showBR)
        : this(type, "", "", "", text, showBR, "")
    {
    }
} //end of class ControlData

//-------------------------------------------------------------------------------------------------------------
public class ColumnData
{
    private string _columnName;
    private object _columnValue;
    private string _condition;
    private bool _active; //是否加入到 SQL Command 裡
    bool _addFlag;
    bool _updateFlag;
    bool _conditionFlag;

    public bool ConditionFlag
    {
        get { return _conditionFlag; }
        set { _conditionFlag = value; }
    }

    public bool AddFlag
    {
        get { return _addFlag; }
        set { _addFlag = value; }
    }
    public bool UpdateFlag
    {
        get { return _updateFlag; }
        set { _updateFlag = value; }
    }
    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }
    public string ColumnName
    {
        get { return _columnName; }
        set { _columnName = value; }
    }
    public object ColumnValue
    {
        get { return _columnValue; }
        set { _columnValue = value; }
    }
    public string Condition
    {
        get { return _condition; }
        set { _condition = value; }
    }
    //------------------------------------------------------------------------
    public ColumnData()
    {
    }
    //------------------------------------------------------------------------
    public ColumnData(string columnName, object columnValue,
                      bool addFlag, bool updateFlag, bool conditionFlag)
    {
        _columnName = columnName;
        _columnValue = columnValue;
        _addFlag = addFlag;
        _updateFlag = updateFlag;
        _conditionFlag = conditionFlag;
        _active = true;
    }
    //------------------------------------------------------------------------
}
