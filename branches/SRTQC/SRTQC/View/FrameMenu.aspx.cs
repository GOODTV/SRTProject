using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Core.Service;

public partial class FrameMenu : BasePage
{
    FunctionListService objFunctionListService = new FunctionListService();

    /// <summary>
    /// Page_Load
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// <history>
    /// 1.Tanya Wu, 2012/8/30, Create
    /// </history>
    protected void Page_Load(object sender, EventArgs e)
    {
        //建立FunctionList
        this.CreateFunctionList();
    }

    /// <summary>
    /// Create Function List.
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2012/9/7, Create
    /// </history>
    protected void CreateFunctionList()
    {
        string funID = string.Empty;

        //Get father node
        DataTable dt = objFunctionListService.GetFunctionFatherNode();

        if (dt.Rows.Count > 0)
        {
            TreeNode node;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                node = new TreeNode();
                funID = dt.Rows[i]["id"].ToString(); // 紀錄功能代號
                node.Text = dt.Rows[i]["function_name"].ToString();
                node.NavigateUrl = dt.Rows[i]["program_url"].ToString() + "?FunctionName=" + dt.Rows[i]["function_name"].ToString();

                //頁面「Axxxxxxx」只有Role為Admin才有權限
                //if (funID.IndexOf('A') == -1 || (funID.IndexOf('A') > -1 && Session["RoleID"].ToString() == "41"))
                //{
                if (dt.Rows[i]["program_url"].ToString() == "") //資料夾(功能群組)
                {
                    node.ImageUrl = "~/App_Themes/image/folder.gif";
                    node.SelectAction = TreeNodeSelectAction.Expand;
                    treeList.Nodes.Add(node);
                    this.NewChildNode(node, funID);  //將此層的node,功能編號,FunctionList傳到下一層
                }
                else //單一功能
                {
                    node.ImageUrl = "~/App_Themes/image/file.gif";
                    node.Target = "content";
                    treeList.Nodes.Add(node);
                }
                //}

            }

        }
    }

    /// <summary>
    /// New Child Node.
    /// </summary>
    /// <history>
    /// 1.Tanya Wu, 2012/9/7, Create
    /// </history>
    protected void NewChildNode(TreeNode newNode, string pFunID)
    {
        string funID;

        //Get child node
        DataTable dt = objFunctionListService.GetChildNode(pFunID);

        if (dt.Rows.Count > 0)
        {
            TreeNode node;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                node = new TreeNode();
                funID = dt.Rows[i]["id"].ToString();
                node.Text = dt.Rows[i]["function_name"].ToString();
                node.NavigateUrl = dt.Rows[i]["program_url"].ToString() + "?FunctionName=" + dt.Rows[i]["function_name"].ToString();

                if (dt.Rows[i]["program_url"].ToString() == "")  //資料夾(功能群組)
                {
                    node.ImageUrl = "~/App_Themes/image/folder.gif";
                    node.SelectAction = TreeNodeSelectAction.Expand;
                    newNode.ChildNodes.Add(node);
                    NewChildNode(node, funID);
                }
                else //單一功能
                {
                    node.ImageUrl = "~/App_Themes/image/file.gif";
                    node.Target = "content";
                    newNode.ChildNodes.Add(node);
                }
            }
        }
    }
}