using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Core.Utility;
using Core.DataAccess;

namespace Core.Service
{
    public class FunctionListService
    {
        DBProvider dbProvider = new DBProvider();
        SqlConnection conn = new SqlConnection();
        FunctionListDao objFunctionListDao = new FunctionListDao();

        /// <summary>
        /// Get Father Node.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public DataTable GetFunctionFatherNode()
        {
            return dbProvider.OpenDataTableNonParameter(objFunctionListDao.GetFunctionFatherNode());
        }

        /// <summary>
        /// Get Child Node.
        /// </summary>
        /// <param name="parent_id">父類別ID.</param>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public DataTable GetChildNode(string parent_id)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.Add("@parent_id", SqlDbType.NVarChar).Value = parent_id;

            return dbProvider.OpenDataTable(objFunctionListDao.GetChildNode(), cmd);
        }
    }
}
