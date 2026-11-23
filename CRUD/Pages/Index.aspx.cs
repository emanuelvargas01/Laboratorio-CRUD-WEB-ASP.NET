using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace CRUD.Pages
{
    public partial class Index : System.Web.UI.Page
    {
        readonly MySqlConnection con = new MySqlConnection(
    ConfigurationManager.ConnectionStrings["conexion"].ConnectionString
);
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarTabla();
        }
        void CargarTabla()
        {
            using (MySqlConnection con = new MySqlConnection(
                ConfigurationManager.ConnectionStrings["conexion"].ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand("sp_load", con);
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gvusuarios.DataSource = dt;
                gvusuarios.DataBind();
            }
        }
        protected void BtnCreate_Click(object sender, EventArgs e) {
            Response.Redirect("~/Pages/CRUD.aspx?op=C");
        }
        protected void BtnRead_Click(object sender, EventArgs e)
        {
            string id;
            Button btnConsultar =(Button)sender;
            GridViewRow selectedrow =(GridViewRow)btnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Pages/CRUD.aspx?id="+id+"&op=R");

        }
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string id;
            Button btnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)btnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Pages/CRUD.aspx?id=" + id + "&op=U");

        }
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string id;
            Button btnConsultar = (Button)sender;
            GridViewRow selectedrow = (GridViewRow)btnConsultar.NamingContainer;
            id = selectedrow.Cells[1].Text;
            Response.Redirect("~/Pages/CRUD.aspx?id=" + id + "&op=D");

        }
    }
}