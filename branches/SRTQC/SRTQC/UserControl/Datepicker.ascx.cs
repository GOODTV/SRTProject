using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_Datepicker : System.Web.UI.UserControl
{
    /// <summary>
    /// 日期內容
    /// </summary>
    public string Text
    {
        get
        { return this.TextBox1.Text; }
        set
        { this.TextBox1.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.TextBox1.Attributes.Add("readonly", "true");
        }
        
    }
}