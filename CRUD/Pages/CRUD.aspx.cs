
using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using System.Web.UI;

namespace CRUD.Pages
{
    public partial class CRUD : System.Web.UI.Page
    {
        private string cadena = ConfigurationManager
            .ConnectionStrings["conexion"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string id = Request.QueryString["id"];
                string op = Request.QueryString["op"];

                if (!string.IsNullOrEmpty(id))
                {
                    int idUsuario;

                    if (int.TryParse(id, out idUsuario))
                    {
                        CargarDatos(idUsuario);
                    }
                }

                if (!string.IsNullOrEmpty(op))
                {
                    switch (op)
                    {
                        case "C":
                            lbltitulo.Text = "Ingresar nuevo usuario";
                            BtnCreate.Visible = true;
                            break;

                        case "R":
                            lbltitulo.Text = "Consulta de usuario";
                            break;

                        case "U":
                            lbltitulo.Text = "Modificar usuario";
                            BtnUpdate.Visible = true;
                            break;

                        case "D":
                            lbltitulo.Text = "Eliminar Usuario";
                            BtnDelete.Visible = true;
                            break;
                    }
                }
            }
        }

        // CARGAR DATOS DEL USUARIO
        void CargarDatos(int id)
        {
            using (MySqlConnection con = new MySqlConnection(cadena))
            {
                MySqlCommand cmd = new MySqlCommand("sp_read", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_Id", MySqlDbType.Int32).Value = id;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    tbnombre.Text = row["Nombre"].ToString();
                    tbedad.Text = row["Edad"].ToString();
                    tbemail.Text = row["Correo"].ToString();

                    DateTime fecha = Convert.ToDateTime(
                        row["Fecha_Nacimiento"]
                    );

                    tbdate.Text = fecha.ToString("yyyy-MM-dd");
                }
            }
        }

        // OBTENER FECHA
        DateTime ObtenerFecha()
        {
            DateTime fecha;

            bool valido = DateTime.TryParseExact(
                tbdate.Text,
                new[] { "yyyy-MM-dd", "dd/MM/yyyy" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fecha
            );

            if (!valido)
            {
                throw new Exception("La fecha ingresada no es válida.");
            }

            return fecha;
        }

        // CREAR USUARIO
        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(cadena))
            {
                MySqlCommand cmd = new MySqlCommand("sp_create", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_Nombre", MySqlDbType.VarChar).Value = tbnombre.Text;
                cmd.Parameters.Add("p_Edad", MySqlDbType.Int32).Value = Convert.ToInt32(tbedad.Text);
                cmd.Parameters.Add("p_Correo", MySqlDbType.VarChar).Value = tbemail.Text;
                cmd.Parameters.Add("p_Fecha", MySqlDbType.Date).Value = ObtenerFecha();

                con.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("~/Pages/Index.aspx");
        }

        // ACTUALIZAR USUARIO
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            using (MySqlConnection con = new MySqlConnection(cadena))
            {
                MySqlCommand cmd = new MySqlCommand("sp_update", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_Id", MySqlDbType.Int32).Value = id;
                cmd.Parameters.Add("p_Nombre", MySqlDbType.VarChar).Value = tbnombre.Text;
                cmd.Parameters.Add("p_Edad", MySqlDbType.Int32).Value = Convert.ToInt32(tbedad.Text);
                cmd.Parameters.Add("p_Correo", MySqlDbType.VarChar).Value = tbemail.Text;
                cmd.Parameters.Add("p_Fecha", MySqlDbType.Date).Value = ObtenerFecha();

                con.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("~/Pages/Index.aspx");
        }

        // ELIMINAR USUARIO
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            using (MySqlConnection con = new MySqlConnection(cadena))
            {
                MySqlCommand cmd = new MySqlCommand("sp_delete", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_Id", MySqlDbType.Int32).Value = id;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("~/Pages/Index.aspx");
        }

        // VOLVER
        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Index.aspx");
        }
    }
}
