using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store.interfaces
{
    public partial class login : Form
    {
        databaseDataContext db = new databaseDataContext();
        public login()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconminimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user, clave;

            if (string.IsNullOrEmpty(txt_usuario.Text.Replace(" ", "")) || string.IsNullOrEmpty(txt_clave.Text.Replace(" ", "")))
            {
                MessageBox.Show("Los campos no pueden estar vacios");
            }
            else
            {
                try
                {
                    user = txt_usuario.Text;
                    clave = txt_clave.Text;
                    var usuario = from u in db.vendedors
                                  where u.ven_user == user & u.ven_password == clave
                                  select u;

                    string Tipo_Usuario;
                    if (usuario.Count() > 0)
                    {
                        vendedor ven = usuario.First();
                        if (usuario.First().ven_codigo == 1)
                        {
                            Tipo_Usuario = "Administrador";
                        }
                        else
                        {
                            Tipo_Usuario = "Vendedor";
                        }

                        frm_inicio frm = new frm_inicio(ven.ven_codigo, Tipo_Usuario);
                        frm.WindowState = FormWindowState.Maximized;
                        frm.Show();
                        Hide();
                        /*
                        if (usuario.First().ven_codigo == 1)
                        {
                            Habilitar_Menu(true);
                        }
                        else
                        {
                           *
                        }*/
                    }
                    else
                    {
                        MessageBox.Show("Los datos ingresados son incorrectos");
                        txt_clave.Text = txt_usuario.Text = "";
                        txt_usuario.Focus();

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Sucedio un error al momento de conectar con la base de datos, \n asegurece que los datos sean correctos");

                }



            }


        }


        private void login_Load(object sender, EventArgs e)
        {
            //   txt_usuario.Text = "darwin";
            //   txt_clave.Text = "calva";
        }
    }
}
