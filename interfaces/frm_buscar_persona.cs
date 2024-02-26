using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace enciclopedia_canina_store.interfaces
{
    public partial class frm_buscar_persona : Form
    {
        
        databaseDataContext db = new databaseDataContext();
        public frm_buscar_persona()
        {
            InitializeComponent();
          
        }

        private void frm_buscar_persona_Load(object sender, EventArgs e)
        {
            CargarTabla();
        }

        private void CargarTabla()
        {
            var query = from p in db.personas select p;
            //  lis = new cProducto(txt_Buscar.Text);
            dgv_persona.DataSource = query;
            dgv_persona.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgv_persona_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

           
            DataGridViewRow fila = dgv_persona.CurrentRow;
            //formulario_padre.txt_codigo.Text = fila.Cells[0].Value.ToString();
            this.Dispose();


        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void iconminimizar_Click(object sender, EventArgs e)
        {
           // this.WindowState = FormWindowState.Minimized;
        }
    }
}
