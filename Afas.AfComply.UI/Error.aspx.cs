using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

public partial class _Error : System.Web.UI.Page
{

    private ILog Log = LogManager.GetLogger(typeof(_Error));
    
    /// <summary>
    /// 01) Page_Load function. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // nop
    }
}