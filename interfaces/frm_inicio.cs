using enciclopedia_canina_store.interfaces;
using enciclopedia_canina_store.interfaces.reportes;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store
{
    public partial class frm_inicio : Form
    {
        int ID_USUARIO_ACTUAL = 0;
        String TIPO_USUARIO_ACTUAL;
        databaseDataContext db = new databaseDataContext();
        public frm_inicio()
        {
            InitializeComponent();
            hidden = false;
            panelWidth = MenuVertical.Width;
        }
        public frm_inicio(int codigo_vendedor, string tipo_Usuario)
        {
            InitializeComponent();
            hidden = false;
            panelWidth = MenuVertical.Width;

            ID_USUARIO_ACTUAL = codigo_vendedor;
            TIPO_USUARIO_ACTUAL = tipo_Usuario;
            AgregarDatosVendedor(codigo_vendedor);
        }



        private void frm_inicio_Load(object sender, EventArgs e)
        {
            //AbrirFormInPanel(new frm_fondo());
        }

        public void AbrirFormInPanel(Form Formhijo)
        {
            if (panelContenedor.Controls.Count > 0)
                panelContenedor.Controls.RemoveAt(0);
            Form fh = Formhijo;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            fh.Opacity = 0.5;
            panelContenedor.Controls.Add(fh);
            panelContenedor.Tag = fh;

            fh.Show();
        }

        private void productosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormInPanel(new frm_producto());
        }

        private void categoriasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormInPanel(new frm_categoria());
        }

        private void fabricantesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormInPanel(new frm_fabricante());
        }

        private void facturacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_facturacion factura = new frm_facturacion(ID_USUARIO_ACTUAL, lblnombre.Text);
            AbrirFormInPanel(factura);
        }

        private void clientesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormInPanel(new frm_clientes());
        }

        private void comprasToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblhora.Text = DateTime.Now.ToString("hh:mm:ss ");
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer2.Start();
        }

        bool hidden;
        int panelWidth;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                MenuVertical.Width = MenuVertical.Width + 10;
                if (MenuVertical.Width >= panelWidth)
                {
                    timer2.Stop();
                    hidden = false;
                    Refresh();
                }
            }
            else
            {
                MenuVertical.Width = MenuVertical.Width - 10;
                if (MenuVertical.Width <= 0)
                {
                    timer2.Stop();
                    hidden = true;
                    Refresh();
                }
            }
        }

        private void productosToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frm_rpt_productos f = new frm_rpt_productos();
            // f.MdiParent = this;
            AbrirFormInPanel(f);
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }


        private void vendedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_vendedor f = new frm_vendedor(ID_USUARIO_ACTUAL);
            AbrirFormInPanel(f);
            f.WindowState = FormWindowState.Maximized;
            f.Show();

        }

        private void ciudadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_ciudad f = new frm_ciudad();
            AbrirFormInPanel(f);
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void categoriasToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frm_rpt_categoria f = new frm_rpt_categoria();
            // f.MdiParent = this;
            AbrirFormInPanel(f);
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void fabricantesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frm_rpt_fabricantes f = new frm_rpt_fabricantes();
            // f.MdiParent = this;
            AbrirFormInPanel(f);
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void vendedoresToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frm_rpt_vendedores f = new frm_rpt_vendedores();
            // f.MdiParent = this;
            AbrirFormInPanel(f);
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void ciudadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_rpt_ciudad f = new frm_rpt_ciudad();
            // f.MdiParent = this;
            AbrirFormInPanel(f);
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void clientesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frm_rpt_clientes f = new frm_rpt_clientes();
            // f.MdiParent = this;
            AbrirFormInPanel(f);
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }
        private void AgregarDatosVendedor(int codigo_vendedor)
        {
            var usuario = from v in db.vendedors
                          join p in db.personas on v.ven_codigo equals p.per_codigo
                          where v.ven_codigo == codigo_vendedor
                          select new { correo = p.per_email, nombre = p.per_nombre + " " + p.per_apellido };
            lblusuario.Text = TIPO_USUARIO_ACTUAL;
            lblcorreo.Text = usuario.First().correo;
            lblnombre.Text = usuario.First().nombre.ToString();
            if(TIPO_USUARIO_ACTUAL== "Administrador")
            {
                vendedoresToolStripMenuItem.Enabled = true;
                vendedoresToolStripMenuItem1.Enabled = true;

            }
            else
            {
                vendedoresToolStripMenuItem.Enabled = false;
                vendedoresToolStripMenuItem1.Enabled = false;
            }
        }
        
        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login login = new login();
            login.Show();
            this.Dispose();
        }
    }
}
