using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DataAccess
{
    public class FunctionListDao
    {
        /// <summary>
        /// Get Father Node.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create        
        /// </history>
        public String GetFunctionFatherNode()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" SELECT id, function_name, program_url  ");
            strSQL.Append(" FROM dbo.srtmcs_functionlist ");
            strSQL.Append(" WHERE parent_node IS NULL ");

            return strSQL.ToString();
        }

        /// <summary>
        /// Get Child Node.
        /// </summary>
        /// <history>
        /// 1.Tanya Wu, 2013/11/26, Create
        /// </history>
        public String GetChildNode()
        {
            StringBuilder strSQL = new StringBuilder();

            strSQL.Append(" SELECT id, function_name, program_url  ");
            strSQL.Append(" FROM dbo.srtmcs_functionlist ");
            strSQL.Append(" WHERE parent_node = @parent_id ");

            return strSQL.ToString();
        }
    }
}
