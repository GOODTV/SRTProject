using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;

public enum NPOGridViewDataSource
{
    fromDataTable,
    fromSQLCommand
}
public enum NPOColumnType
{
    Checkbox,
    Radiobox,
    Dropdownlist,
    Textbox,
    NormalText,
    CurrecyText,
    Button,
    Link
}
public class NPOGridViewColumn
{
    public string Caption;
    public string Length;
    public NPOColumnType ColumnType;
    public string AddLink;
    public string Link;
    public string LinkText;
    public string IconPath;
    public string IconTooltip;
    public string RowSpan;
    public string ColSpan;
    public bool ShowTitle;
    //for Control
    public bool DataFromDataTable;
    public List<string> ControlName;
    public List<string> ControlId;
    public List<string> ControlValue;
    public List<string> ControlText;
    public List<string> ControlDefaultValue;
    public List<string> ControlTextSize;
    public List<string> ColumnName;
    public List<string> ControlOptionValue;
    public List<string> ControlOptionText;
    public List<string> ControlKeyColumn;
    public bool CaptionWithControl;
    public string DisableValue; //當 value 為此值時, control 為 disable 狀態
    public string SortColumnName; //排序時的欄位名稱
    public bool Readonly;
    public bool ShowConfirmDialog;
    public string ConfirmDialogMsg;

    public NPOGridViewColumn()
    {
        ColumnType = NPOColumnType.NormalText;
        DataFromDataTable = true;
        ControlName = new List<string>();
        ControlId = new List<string>();
        ControlValue = new List<string>();
        ControlText = new List<string>();
        ControlDefaultValue = new List<string>();
        ControlTextSize = new List<string>();
        ColumnName = new List<string>();
        ControlKeyColumn = new List<string>();
        ControlOptionValue = new List<string>();
        ControlOptionText = new List<string>();
        AddLink = "";
        Link = "";
        LinkText = "";
        IconPath = "";
        IconTooltip = "";
        CaptionWithControl = false;
        DisableValue = "";
        ShowConfirmDialog = false;
        ConfirmDialogMsg = "";
        SortColumnName = "";
        RowSpan = "";
        ColSpan = "";
        ShowTitle = true;
    }
    public NPOGridViewColumn(string caption)
        : this()
    {
        Caption = caption;
    }
} //end of NPOGridViewColumn

public class NPOGridView
{
    public bool ShowTitlwWhenEmpty;
    public bool UseUserDefineColumn;
    public int PageSize;
    public int CurrentPage;
    public string CurrentSortFieldLinkID;
    public string CurrentSortDirLinkID;
    public string AddLink;
    public string EditLink;
    public string AddLinkTarget;
    public string EditLinkTarget;
    public string sqlCommand;
    public NPOGridViewDataSource Source;
    public DataTable dataTable;
    public Dictionary<string, object> dict;
    public string PreviousPageLinkID;
    public string NextPageLinkID;
    public string GoPageLinkID;
    public string CurrentPageLinkID;
    public string GoPageControlName;
    public string AddLinkFontSize;
    public string AddLinkFontFamily;
    public string AddLinkText;
    public bool ShowPage;
    public string AddLinkImage;
    public string CssClass;
    public string TableID;
    public List<NPOGridViewColumn> ExtraColumns;
    public List<NPOGridViewColumn> Columns;
    public List<string> DisableColumn;
    public List<string> Keys;
    public List<string> QuerySyting;
    public List<string> QuerySytingData;
    public string EditClick;
    //20140509 新增 by Ian_Kao
    //變更資料顏色用
    public string ColumnNameToChangeColor;
    public string ColumnDataToChangeColor;
    //2015/6/15 新增每行設定幾個欄位
    public int ColumnByRow;
    public bool EditLinkDoubleClick;
    //----------------------------------------------------------------------
    public NPOGridView()
    {
        ShowTitlwWhenEmpty = false;
        PageSize = 30;
        CurrentPage = 1;
        CurrentSortFieldLinkID = "HFD_CurrentSortField";
        CurrentSortDirLinkID = "HFD_CurrentSortDir";
        AddLinkTarget = "_self";
        EditLinkTarget = "_self";
        AddLink = "";
        EditLink = "";
        Source = NPOGridViewDataSource.fromSQLCommand;
        PreviousPageLinkID = "btnPreviousPage";
        NextPageLinkID = "btnNextPage";
        GoPageLinkID = "btnGoPage";
        CurrentPageLinkID = "HFD_CurrentPage";
        GoPageControlName = "GoPage";
        AddLinkFontSize = "9pt";
        AddLinkFontFamily = "新細明體";
        AddLinkText = "新增資料";
        AddLinkImage = "../images/DIR_tri.gif";
        CssClass = "table_h";
        TableID = "";
        ShowPage = true;
        Columns = new List<NPOGridViewColumn>();
        ExtraColumns = new List<NPOGridViewColumn>();
        DisableColumn = new List<string>();
        QuerySyting = new List<string>();
        QuerySytingData = new List<string>();
        Keys = new List<string>();
        UseUserDefineColumn = false;
        EditClick = "";
        //20140509 新增 by Ian_Kao
        //變更資料顏色用
        ColumnNameToChangeColor = "";
        ColumnDataToChangeColor = "";
        //2015/6/15 新增每行設定幾個欄位
        ColumnByRow = 0;
        EditLinkDoubleClick = false;
    }
    //----------------------------------------------------------------------
    public string Render()
    {
        string strTemp = "";
        HtmlTable tableHeader = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        CssStyleCollection css;

        if (Source == NPOGridViewDataSource.fromSQLCommand)
        {
            GetData();
        }

        int RecordCount = dataTable.Rows.Count;
        //分頁與頁數計算
        int TotalPage = 0;
        if (RecordCount % PageSize == 0)
        {
            TotalPage = RecordCount / PageSize;
        }
        else
        {
            TotalPage = RecordCount / PageSize + 1;
        }

        if (CurrentPage > TotalPage)
        {
            CurrentPage = TotalPage;
        }
        if (CurrentPage <= 0)
        {
            CurrentPage = 1;
        }
        int endRecNo = CurrentPage * PageSize - 1;
        int startRecNo = (CurrentPage - 1) * PageSize;
        if (endRecNo > RecordCount - 1)
        {
            endRecNo = RecordCount - 1;
        }
        //將本頁要顯示的資料複製一份
        DataTable dtTemp = dataTable.Clone();
        if (ShowPage == true)
        {
            for (int i = startRecNo; i <= endRecNo; i++)
            {
                dtTemp.ImportRow(dataTable.Rows[i]);
            }
        }
        else
        {
            dtTemp = dataTable;
        }
        //end of 分頁與頁數計算
        //-------------------------------------------------------------------
        //畫面上方的分頁和上一頁下一頁功能        
        string strPage = "<table class='pager' border='0' cellspacing='0' cellpadding='1' style='border-collapse: collapse' width='100%'>\n";
        strPage += "<tr><td width='20%'></td>";
        strPage += "<td width='60%' align='center'>";
        //顯示頁簽與否
        if (ShowPage == true && RecordCount > 0)
        {
            strPage += "   <span>第 " + CurrentPage + "/" + TotalPage + " 頁&nbsp;&nbsp;</span>";
            if (CurrentPage != 1)
            {
                string Link = " | <a onclick=\"document.getElementById('" + CurrentPageLinkID + "').value='" + CurrentPage + "';window.event.cancelBubble=true;document.getElementById('" + PreviousPageLinkID + "').click();\" >上一頁</a>";
                strPage += Link;
            }
            if (CurrentPage != TotalPage && CurrentPage < TotalPage)
            {
                string Link = " | <a onclick=\"document.getElementById('" + CurrentPageLinkID + "').value='" + CurrentPage + "';window.event.cancelBubble=true;document.getElementById('" + NextPageLinkID + "').click();\"  >下一頁</a>";
                strPage += Link;
            }
            strPage += " |&nbsp;<span> 跳至第 <select name=" + GoPageControlName + " size='1' onchange=\"document.getElementById('" + CurrentPageLinkID + "').value=" + GoPageControlName + ".options[" + GoPageControlName + ".selectedIndex].value;window.event.cancelBubble=true;document.getElementById('" + GoPageLinkID + "').click();\" style='text-decoration:none'>";
            int iPage = 0;
            string strSelected = "";
            for (iPage = 1; iPage <= TotalPage; iPage++)
            {
                if (iPage == CurrentPage)
                {
                    strSelected = "selected";
                }
                else
                {
                    strSelected = "";
                }
                strPage += "<option value=\'" + iPage + "\' " + strSelected + ">" + iPage + "</option></br>";
            }
            strPage += "</select> 頁</span></br>";
        }
        else
        {
            strPage += "";
        }
        strPage += "<td width='20%'></td>\n";
        strPage += "</table>\n";
        //end of 畫面上方的分頁和上一頁下一頁功能        
        //-------------------------------------------------------------------
        //資料顯示與 Link
        string showhand = "";
        string onClickString = "";
        string Cellshowhand = "";
        string CellonClickString = "";
        string strBody = "";
        string strTableID = "";

        //顯示抬頭列
        if (TableID != "")
        {
            strTableID = " id='" + TableID + "' ";
        }
        strBody += "<table " + strTableID + " class='" + CssClass + "'" + " width='100%'>";

        //讓 Title 可以多一行,加強視覺效果
        if (ExtraColumns.Count != 0)
        {
            strBody += "<tr>";
            foreach (NPOGridViewColumn col in ExtraColumns)
            {
                string RowColSpan = " ";
                if (col.RowSpan != "")
                {
                    RowColSpan += " rowspan='" + col.RowSpan + "' ";
                }
                if (col.ColSpan != "")
                {
                    RowColSpan += " colspan='" + col.ColSpan + "' ";
                }
                strBody += "<th nowrap " + RowColSpan + "><span>" + col.Caption + "</span></th>\n";
            }
            strBody += "</tr>";
        }

        strBody += "<tr>";


        //全部轉成小寫,避免資料庫欄位大小寫問題
        for (int i = 0; i < DisableColumn.Count; i++)
        {
            DisableColumn[i] = DisableColumn[i].ToLower();
        }

        List<ControlData> list = new List<ControlData>();

        int cut = 0;

        //顯示抬頭
        if (Columns.Count == 0) //沒有定義欄位,用  DataTable 的 Columns
        {
            foreach (DataColumn dc in dtTemp.Columns)
            {   
                //DisableColumn 所列的欄位不顯示,只當成 key 值用
                if (!DisableColumn.Contains(dc.ColumnName.ToLower()))
                {
                    strBody += "<th nowrap><span>" + dc.ColumnName + "</span></th>\n";
                    cut++;
                }

                //2015/6/15 新增每行設定幾個欄位
                if (ColumnByRow != 0 && ColumnByRow == cut && dtTemp.Columns.Count != cut)
                {
                    strBody += "</tr>\n<tr>\n";
                    cut = 0;
                }
            }
        }
        else
        {
            //用自行定義的欄位資料, 顯示抬頭
            foreach (NPOGridViewColumn col in Columns)
            {
                if (col.ShowTitle == false)
                {
                    continue;
                }
                string Content = "";
                if (col.CaptionWithControl == true)
                {
                    list.Clear();
                    switch (col.ColumnType)
                    {
                        case NPOColumnType.Checkbox:
                            for (int j = 0; j < col.ControlName.Count; j++)
                            {
                                list.Add(new ControlData("Checkbox", col.ControlName[j] + "All", col.ControlId[j] + "All", col.ControlValue[j], "", false, ""));
                            }
                            break;
                        case NPOColumnType.Radiobox:
                            for (int j = 0; j < col.ControlName.Count; j++)
                            {
                                list.Add(new ControlData("Radio", col.ControlName[j] + "All", col.ControlId[j] + "All", col.ControlValue[j], "", false, ""));
                            }
                            break;
                        case NPOColumnType.Dropdownlist:
                            for (int j = 0; j < col.ControlName.Count; j++)
                            {
                                list.Add(new ControlData("DropdownList", col.ControlName[j] + "All", col.ControlId[0] + "All", col.ControlOptionValue.ToArray(), col.ControlOptionText.ToArray(), false, ""));
                            }
                            break;
                    }
                    Content = HtmlUtil.RenderControl(list) + "&nbsp";
                }
                //點表頭可以排序
                string Link = "";
                if (col.SortColumnName != "")
                {
                    Link = "onclick='$(\"#" + CurrentSortFieldLinkID + "\").val(\"" + col.SortColumnName + "\"); $(\"#" + CurrentPageLinkID + "\").val(\"1\");;if ($(\"#" + CurrentSortDirLinkID + "\").val() == \"asc\"){ $(\"#" + CurrentSortDirLinkID + "\").val(\"desc\");} else $(\"#" + CurrentSortDirLinkID + "\").val(\"asc\");; $(\"#" + GoPageLinkID + "\").click();' style='text-decoration:none;cursor:pointer;' ";
                }

                string RowColSpan = " ";
                if (col.RowSpan != "")
                {
                    RowColSpan += " rowspan='" + col.RowSpan + "' ";
                }
                if (col.ColSpan != "")
                {
                    RowColSpan += " colspan='" + col.ColSpan + "' ";
                }
                strBody += "<th nowrap " + RowColSpan + Link + "><span>" + Content + col.Caption + "</span></th>\n";
                //Todo:加 asc 或 desc 圖示
                //strBody += "<th nowrap " + Link + "><span>" + Content + col.Caption + "</span><img src='../images/asc.gif' ></th>\n";
                
                //2015/6/15 新增每行設定幾個欄位
                cut++;
                if (ColumnByRow != 0 && ColumnByRow == cut && Columns.Count != cut)
                {
                    strBody += "</tr>\n<tr>\n";
                    cut = 0;
                }
            }
        }
        if (ColumnByRow > cut)
        {
            for (int i = 0; i < ColumnByRow - cut; i++)
            {
                strBody += "<th><span>&nbsp;</span></th>";
            }
        }
        strBody += "</tr>";

        Page page = HttpContext.Current.CurrentHandler as Page;
        //資料列
        int IdCount = 1;
        foreach (DataRow dr in dtTemp.Rows)
        {
            if (EditLink != "")
            {
                string EditURL = "";
                string KeyValue = "";
                for (int i = 0; i < Keys.Count; i++)
                {
                    KeyValue += dr[Keys[i]];
                }
                EditURL = EditLink + page.Server.UrlEncode(KeyValue);
                if (QuerySyting.Count != 0)
                {
                    foreach (string str in QuerySyting)
                    {
                        EditURL = EditURL + "&" + str + "=" + dr[str].ToString();
                    }
                }
                if (EditLinkDoubleClick)
                {
                    onClickString = " ondblclick =\"window.open(\'" + EditURL + "\',\'" + EditLinkTarget + "\',\'\')\" ";
                }
                else
                {
                    onClickString = " onclick =\"window.open(\'" + EditURL + "\',\'" + EditLinkTarget + "\',\'\')\" ";
                }
                showhand = @"style='cursor:pointer;'";
            }
            if (EditClick != "")
            {
                string Edit_JavaScript = "";
                string KeyValue = "";
                for (int i = 0; i < Keys.Count; i++)
                {
                    KeyValue += dr[Keys[i]] + "|";
                }
                Edit_JavaScript = EditClick + "('" + KeyValue + "')";
                onClickString = " OnClick =" + "\"" + Edit_JavaScript + ";" + "\"";
                showhand = @"style='cursor:pointer;'";
            }

            cut = 0;

            if (Columns.Count == 0)
            {
                strBody += "<tr " + showhand + "  " + onClickString + ">";
                foreach (DataColumn dc in dtTemp.Columns)
                {
                    if (!DisableColumn.Contains(dc.ColumnName.ToLower()))
                    {
                        string Content = dr[dc.ColumnName].ToString();
                        if (Content == "")
                        {
                            Content = "&nbsp;";
                        }
                        strBody += "<td><span>" + Content + "</span></td>\n";
                        cut++;
                    }
                    //2015/6/15 新增每行設定幾個欄位
                    if (ColumnByRow != 0 && ColumnByRow == cut && dtTemp.Columns.Count != cut)
                    {
                        strBody += "</tr>\n<tr " + showhand + "  " + onClickString + ">\n";
                        cut = 0;
                    }
                }
            }
            else
            { //有定義資料欄位
                strBody += "<tr " + showhand + "  " + onClickString + ">\n";
                foreach (NPOGridViewColumn col in Columns)
                {
                    string Content = "";
                    string ColumnValue = "";
                    string DefValue = "";
                    string KeyValue = "";
                    string ConfirmStr = "";
                    for (int j = 0; j < col.ControlKeyColumn.Count; j++)
                    {
                        if (col.ControlKeyColumn[j] != "")
                        {
                            KeyValue += dr[col.ControlKeyColumn[j]].ToString();
                        }
                    }
                    switch (col.ColumnType)
                    {
                        case NPOColumnType.NormalText:
                            if (col.DataFromDataTable == true)
                            {
                                //Content = dr[col.ColumnName[0]].ToString();
                                Content = HtmlUtil.ToHtml(dr[col.ColumnName[0]].ToString());
                            }
                            else
                            {
                                Content = col.ControlValue[0];
                            }
                            Content = Content == "" ? "&nbsp;" : Content;
                            break;
                        case NPOColumnType.CurrecyText:
                            if (col.DataFromDataTable == true)
                            {
                                //Content = dr[col.ColumnName[0]].ToString();
                                Content = "<font color='red'>NT$ " + HtmlUtil.ToHtml(string.Format("{0:N}", Convert.ToUInt32(dr[col.ColumnName[0]].ToString()))) + "</font>";
                            }
                            else
                            {
                                Content = col.ControlValue[0];
                            }
                            Content = Content == "" ? "&nbsp;" : Content;
                            break;
                        case NPOColumnType.Checkbox:
                            list.Clear();
                            for (int j = 0; j < col.ControlName.Count; j++)
                            {
                                DefValue = "";
                                if (col.ColumnName[j] != "")
                                {
                                    DefValue = dr[col.ColumnName[j]].ToString();
                                }
                                list.Add(new ControlData("Checkbox", col.ControlName[j] + "_" + KeyValue, col.ControlId[j], col.ControlValue[j] + KeyValue, col.ControlText[j], false, DefValue));
                                if (DefValue != "" && DefValue == col.DisableValue)
                                {
                                    list[list.Count - 1].Disabled = true;
                                }
                                //20140509 新增 by Ian_Kao 信用卡有效月年過期後不勾選
                                if (ColumnNameToChangeColor != "")
                                {
                                    string strTmp = ColumnDataToChangeColor;
                                    DateTime start = DateTime.Now;
                                    DateTime end = DateTime.Now;
                                    start = DateTime.Parse(dr[ColumnNameToChangeColor].ToString());
                                    end = DateTime.Parse(strTmp);
                                    int month = CaseUtil.DateDiff(start, end);
                                    if (month < 0)
                                    {
                                        list[list.Count - 1].Disabled = true;
                                    }
                                }
                            }
                            Content = HtmlUtil.RenderControl(list);
                            break;
                        case NPOColumnType.Radiobox:
                            list.Clear();
                            for (int j = 0; j < col.ControlName.Count; j++)
                            {
                                DefValue = "";
                                if (col.ColumnName[j] != "")
                                {
                                    DefValue = dr[col.ColumnName[j]].ToString();
                                }
                                list.Add(new ControlData("Radio", col.ControlName[j] + "_" + KeyValue, col.ControlId[j] + "_" + KeyValue, col.ControlValue[j], col.ControlText[j], false, DefValue));
                                if (DefValue != "" && DefValue == col.DisableValue)
                                {
                                    list[list.Count - 1].Disabled = true;
                                }
                            }
                            Content = HtmlUtil.RenderControl(list);
                            break;
                        case NPOColumnType.Dropdownlist:
                            list.Clear();
                            for (int j = 0; j < col.ControlName.Count; j++)
                            {
                                DefValue = "";
                                if (col.ColumnName[j] != "")
                                {
                                    DefValue = dr[col.ColumnName[j]].ToString();
                                }
                                list.Add(new ControlData("DropdownList", col.ControlName[j] + "_" + KeyValue, col.ControlId[j] + "_" + KeyValue, col.ControlOptionValue.ToArray(), col.ControlOptionText.ToArray(), false, DefValue));
                            }
                            Content = HtmlUtil.RenderControl(list);
                            break;
                        case NPOColumnType.Textbox:
                            list.Clear();
                            for (int j = 0; j < col.ControlName.Count; j++)
                            {
                                if (col.DataFromDataTable == true)
                                {
                                    //欄位資料來源,沒設定的話,就以 DataTable 的欄位為來源
                                    if (col.ColumnName.Count == 0)
                                    {
                                        ColumnValue = "";
                                    }
                                    else
                                    {
                                        //預設資料欄位名稱為空白時,
                                        if (col.ColumnName.Count == 0)
                                        {
                                            ColumnValue = "";
                                        }
                                        else
                                        {
                                            if (col.ColumnName[j] == "")
                                            {
                                                ColumnValue = "";
                                            }
                                            else
                                            {
                                                //以欄位名稱抓資料
                                                ColumnValue = dr[col.ColumnName[j]].ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ColumnValue = col.ControlValue[j];
                                }
                                list.Add(new ControlData("Text", col.ControlName[j] + "_" + KeyValue, col.ControlId[j] + "_" + KeyValue, ColumnValue, "", false, ""));
                                //當 Textbox 欄位的 DisableValue 有設定值的話,不管值為何都認定為 disabled
                                if (col.Readonly == true)
                                {
                                    list[list.Count - 1].Readonly = true;
                                }
                            }
                            Content = HtmlUtil.RenderControl(list);
                            break;
                        case NPOColumnType.Link:
                            list.Clear();
                            if (col.ShowConfirmDialog == true)
                            {
                                ConfirmStr = "if (!window.confirm('" + col.ConfirmDialogMsg + "')) return;";
                            }
                            //CellonClickString = " onclick =\"" + ConfirmStr + "window.open(\'" + col.Link + page.Server.UrlEncode(KeyValue) + "\',\'" + EditLinkTarget + "\',\'\')\" ";
                            CellonClickString = " onclick =\"" + ConfirmStr + "window.open(\'" + col.Link + page.Server.UrlEncode(KeyValue) + "\',\'" + EditLinkTarget + "\',\'\')\" ";
                            Cellshowhand = @"style='cursor:pointer;color:#8b0000;text-decoration:underline;font-weight:100;'";
                            if (col.LinkText != "")
                            {
                                Content += "<span " + Cellshowhand + "  " + CellonClickString + ">" + col.LinkText + "</span>";
                            }
                            else
                            {
                                //空白則使用 icon 圖示
                                string icon = "<img src='" + col.IconPath + "' border=0 width='20' height='20' title='" + col.IconTooltip + "'>";
                                Content += "<span " + Cellshowhand + "  " + CellonClickString + ">" + icon + "</span>";
                            }
                            break;
                        case NPOColumnType.Button:
                            list.Clear();
                            if (col.ShowConfirmDialog == true)
                            {
                                ConfirmStr = "if (!window.confirm('" + col.ConfirmDialogMsg + "')) return;";
                            }
                            CellonClickString = " onclick =\"window.event.cancelBubble=true;" + ConfirmStr + "window.open(\'" + col.Link + page.Server.UrlEncode(KeyValue) + "\',\'" + EditLinkTarget + "\',\'\');\" ";
                            Cellshowhand = @"style='cursor:pointer;'";
                            string Disabled = "";
                            //Button 是否要 Disabled
                            string value = "";
                            if (col.ColumnName.Count != 0)
                            {
                                value = dr[col.ColumnName[0]].ToString();
                            }
                            if (value != "" && value == col.DisableValue)
                            {
                                Disabled = "disabled='disabled'";
                            }
                            Content += "<input type='button'" + Disabled + " value='" + col.ControlText[0] + "'" + Cellshowhand + "  " + CellonClickString + "></input>";
                            break;
                    }
                    //-------------------
                    string strChangeColor = "";
                    if (ColumnNameToChangeColor != "")
                    {
                        string strTmp = ColumnDataToChangeColor;
                        DateTime start = DateTime.Now;
                        DateTime end = DateTime.Now;
                        start = DateTime.Parse(dr[ColumnNameToChangeColor].ToString());
                        end = DateTime.Parse(strTmp);
                        int month = CaseUtil.DateDiff(start, end);
                        if (month < 0)
                        {
                            strChangeColor = @"style='background-color:#FF6666;'";
                        }
                        else if (month == 0)
                        {
                            strChangeColor = @"style='background-color:#FFCCFF;'";
                        }
                        else if (month == 1)
                        {
                            strChangeColor = @"style='background-color:#FFFF99;'";
                        }
                        else if (month == 2)
                        {
                            strChangeColor = @"style='background-color:#66FF99;'";
                        }
                    }
                    //-------------------
                    strBody += "<td " + strChangeColor + "><span>" + Content + "</span></td>\n";
                    //2015/6/15 新增每行設定幾個欄位
                    cut++;
                    if (ColumnByRow != 0 && ColumnByRow == cut && Columns.Count != cut)
                    {
                        strBody += "</tr>\n<tr " + showhand + "  " + onClickString + ">\n";
                        cut = 0;
                    }
                }
            }
            if (ColumnByRow > cut)
            {
                for (int i = 0; i < ColumnByRow - cut; i++)
                {
                    strBody += "<td><span>&nbsp;</span></td>";
                }
            }
            strBody += "</tr>\n";
            IdCount++;
        }
        strBody += "</table>\n";
        //--------------------------------------------
        string Result = strPage + strBody;
        return Result;
    }
    //----------------------------------------------------------------------
    private void GetData()
    {
        dataTable = NpoDB.GetDataTableS(sqlCommand, dict);
    }
    //----------------------------------------------------------------------
    private string GetHtmlText(HtmlTable table)
    {
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        table.RenderControl(htw);

        return htw.InnerWriter.ToString();
    }
    //----------------------------------------------------------------------
} //end of NPOGridView
