using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using PegasusDataAccess;

public partial class Sales_TreeView : System.Web.UI.Page
{
    DataAccessClass objda = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            PopulateMenuDataTable();
        }
    }
    

    private void PopulateMenuDataTable()
    {
        DataTable dt = new DataTable();
         dt= objda.return_DataTable("select Inv_Product_CategoryMaster.Category_Id,Inv_Product_CategoryDetail.Parent_Id,Inv_Product_CategoryMaster.Category_Name from Inv_Product_CategoryMaster inner join Inv_Product_CategoryDetail on Inv_Product_CategoryMaster.Category_Id=Inv_Product_CategoryDetail.Ref_Id order by Category_Id ");
          CreateTreeViewDataTable(dt, 0, null);
    }

    private void CreateTreeViewDataTable(DataTable dt, int parentID, TreeNode parentNode)
    {
        DataRow[] drs = dt.Select("Parent_Id = " + parentID.ToString());
        foreach (DataRow i in drs)
        {
            TreeNode newNode = new TreeNode(i["Category_Name"].ToString(), i["Category_Id"].ToString());
            if (parentNode == null)
            {
                tvMenu.Nodes.Add(newNode);
            }
            else
            {
                parentNode.ChildNodes.Add(newNode);
            }
            CreateTreeViewDataTable(dt, Convert.ToInt32(i["Category_Id"]), newNode);
        }
        tvMenu.DataBind();  
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //List<MenuMaster> m = new List<MenuMaster>();
        //foreach (TreeNode i in tvMenu.CheckedNodes)
        //{
        //    m.Add(new MenuMaster { MenuID = Convert.ToInt32(i.Value), MenuName = i.Text });
        //}

        //GridView1.DataSource = m;
        //GridView1.DataBind();
    }

}