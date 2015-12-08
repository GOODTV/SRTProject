using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core.Utility;

public partial class Master_MasterPage : System.Web.UI.MasterPage
{
    DBProvider dbProvider = new DBProvider();

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
            //if (Session["LoginOK"] == null || Session["LoginOK"].Equals(false))
            //{
            //    Response.Write(" <script> parent.document.location= '../Error_Page.aspx' </script> ");
            //    Response.End();
            //}
            //else
            //{
            //    //Label登入使用者
            //    this.lblLoginUser.Text = Session["LoginName"].ToString() + "　";
                //this.lblFunTitle.Text = Request.QueryString["FunctionName"];
            //}
        //}
        //catch (Exception ex)
        //{
        //    Response.Write(" <script> parent.document.location= '../Error_Page.aspx' </script> ");
        //    Response.End();
        //}
    }

    ///// <summary>
    ///// 登出按鈕
    ///// </summary>
    ///// <param name="sender">The source of the event.</param>
    ///// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    ///// <history>
    ///// 1.Tanya Wu, 2012/8/30, Create
    ///// </history>
    //protected void btnLogOut_Click(object sender, EventArgs e)
    //{
    //    dbProvider.CloseConnection();

    //    Session.Clear();
    //    Session["LoginOK"] = false;
    //    Response.Write(" <script> parent.document.location= '../Login.aspx' </script> ");
    //    Response.End();
    //}
}
