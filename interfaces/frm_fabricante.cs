using enciclopedia_canina_store.interfaces.reportes;
using enciclopedia_canina_store.logica_negocio;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store
{
    public partial class frm_fabricante : Form
    {
        cFabricante controlador=new cFabricante();
        databaseDataContext db=new databaseDataContext();
        int TOTAL_FILAS = 0;
        int FILA_ACTUAL = 0;
        string ACCION = "";
        public frm_fabricante()
        {
            InitializeComponent();

        }
        /// <summary>
        /// metodo para actualizar los registros de las tabla Fabricante
        /// </summary>
        void CargarTabla()
        {
            try
            {
                var query = from f in db.fabricantes
                            select new
                            {
                                CODIGO = f.fab_codigo,
                                NOMBRE = f.fab_nombre
                            };
                dgv_fabricantes.DataSource = query;
                habilitar(true);
                dgv_fabricantes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al cargar la tabla");
            }
        }
        private void frm_fabricante_Load(object sender, EventArgs e)
        {
            habilitar_textos(false);
            CargarTabla();
            //dgv_fabricantes.DataSource= db.fabricantes;                       
        }
        public void habilitar_textos(Boolean ok)
        {
            txt_codigo.Enabled = (false);
            txt_nombre_fabricante.Enabled = (ok);
            //txtNombre.setEnabled(ok);
        }
        public void habilitar(Boolean ok)
        {
            //deshabilitar textos
            habilitar_textos(!ok);
            //habilitar botones
            habilitar_botones(ok);
            //visualizar el registro actual
            // ver_registro(Registro);
        }
        public void habilitar_botones(Boolean ok)
        {
            btn_nuevo.Enabled = (ok);
            btn_editar.Enabled = (ok);
            btn_eliminar.Enabled = (ok);
            btn_anterior.Enabled = (ok);
            btn_siguiente.Enabled = (ok);
            //hacen lo contrario de los otros botones
            btn_guardar.Enabled = (!ok);
            btn_cancelar.Enabled = (!ok);
        }
        public void limpiar_textos()
        {
            txt_codigo.Text = ("");
            txt_nombre_fabricante.Text = ("");
        }
        public Boolean ValidarTexto()
        {
            bool texto_validado = false;
            if (string.IsNullOrEmpty(txt_codigo.Text.Replace(" ", "")))
                MessageBox.Show("No se ha seleccionado el codigo");
            else
            {
                if (string.IsNullOrEmpty(txt_nombre_fabricante.Text.Replace(" ", "")))
                {
                    MessageBox.Show("Asegurece de ingresar un nombre de Fabricante");
                    txt_nombre_fabricante.Focus();
                    txt_nombre_fabricante.SelectionStart = txt_nombre_fabricante.TextLength;
                }
                else
                {
                    texto_validado = true;
                }
            }

            return texto_validado;

        }

        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            ACCION = "insertar";
            limpiar_textos();
            habilitar_textos(true);
            habilitar_botones(false);
            txt_codigo.Text = controlador.Obtener_Nuevo_Codigo_Fabricante().ToString();
            lbl_mensaje.Text = "";
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (ValidarTexto())
            {
                if (ACCION.Equals("editar"))
                {
                    if (EditarRegistro())
                        CargarTabla();

                }
                else
                {
                    if (GuardarRegistro())
                        CargarTabla();
                }
            }
        }

        private bool GuardarRegistro()
        {
            string msg;
            bool ingreso_correcto = false;
            try
            {
                db = new databaseDataContext();
                if (!controlador.Existe_Registro(txt_nombre_fabricante.Text, int.Parse(txt_codigo.Text)))
                {
                    fabricante fb = new fabricante();
                    fb.fab_codigo = int.Parse(txt_codigo.Text);
                    fb.fab_nombre = txt_nombre_fabricante.Text;
                    db.fabricantes.InsertOnSubmit(fb);
                    db.SubmitChanges();
                    msg = "registro guardado exitosamente";
                    ingreso_correcto = true;
                }
                else
                {
                    msg = "el nombre ya existe";
                }
                lbl_mensaje.Text = msg;
                timer1.Start();
            }
            catch (Exception)
            {

                MessageBox.Show("Sucedio un error al ingresar el registro");
            }
            return ingreso_correcto;
        }

        private bool EditarRegistro()
        {
            string msg = "";
            bool edicion_corecta = false;
            try
            {
                db = new databaseDataContext();
                if (!controlador.Existe_Registro(txt_nombre_fabricante.Text,int.Parse(txt_codigo.Text)))
                {
                    fabricante fb = db.fabricantes.Single(fabricante => fabricante.fab_codigo == int.Parse(txt_codigo.Text));
                    fb.fab_nombre = txt_nombre_fabricante.Text;
                    db.SubmitChanges();
                    msg = "registro actualizado exitosamente";
                    edicion_corecta = true;
                }
                else
                {
                    msg = "el nombre ya existe";
                }
                lbl_mensaje.Text = msg;
                timer1.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al editar el registro");
            }
            return edicion_corecta;
        }
        private void btn_editar_Click(object sender, EventArgs e)
        {
            if (ValidarTexto())
            {
                ACCION = "editar";
                habilitar_textos(true);
                txt_nombre_fabricante.Focus();
                habilitar_botones(false);
                lbl_mensaje.Text = "";
            }
            else
            {
                lbl_mensaje.Text = "seleccione un registro de la tabla";
                timer1.Start();
            }
        }



        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            habilitar(true);
            if (ACCION == "insertar")
                txt_codigo.Text = "";
            lbl_mensaje.Text = "";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (ValidarTexto())
            {
                DialogResult result = MessageBox.Show("Esta seguro de eliminar el fabricante: " +
                    txt_nombre_fabricante.Text, "Advertencia", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        db = new databaseDataContext();
                        fabricante fb = db.fabricantes.Single(fab => fab.fab_codigo == int.Parse(txt_codigo.Text.Replace(" ","")));
                        db.fabricantes.DeleteOnSubmit(fb);
                        db.SubmitChanges();
                        CargarTabla();
                        msg = "registro eliminado exitosamente";
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("El registro no se elimino,existen productos asociados a el");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Sucedio un error inesperado al eliminar el producto");

                    }
                }

            }
            else
            {
                msg = "seleccione un registro de la tabla";
            }
            lbl_mensaje.Text = msg;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_mensaje.Text = "";
            timer1.Stop();
        }

        private void iconminimizar_Click(object sender, EventArgs e)
        {
           WindowState = FormWindowState.Minimized;
        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void Filtrar_Fabricante(string nombre)
        {
            var query = from p in db.fabricantes
                        where System.Data.Linq.SqlClient.SqlMethods.Like(p.fab_nombre, "%" + nombre + "%")
                        select new
                        {
                            CODIGO = p.fab_codigo,
                            NOMBRE = p.fab_nombre
                        };
            dgv_fabricantes.DataSource = query;
            habilitar(true);
        }

        private void btn_anterior_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_fabricantes.Rows.Count;
                if (TOTAL_FILAS > 0)
                {
                    if (FILA_ACTUAL > 0)
                    {
                        Recoger_Datos_De_Fila_Tabla(FILA_ACTUAL--);
                    }
                    else
                    {//entra cuando fila actual es 0
                        Recoger_Datos_De_Fila_Tabla(FILA_ACTUAL);
                        FILA_ACTUAL = TOTAL_FILAS - 1;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al recoger datos del registro");
            }
        }

        private void Recoger_Datos_De_Fila_Tabla(int fila)
        {
            txt_codigo.Text = dgv_fabricantes.Rows[fila].Cells[0].Value.ToString();
            txt_nombre_fabricante.Text = dgv_fabricantes.Rows[fila].Cells[1].Value.ToString();
        }

        private void btn_siguiente_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_fabricantes.Rows.Count;
                if (TOTAL_FILAS > 0)
                {
                    if (FILA_ACTUAL < TOTAL_FILAS)
                    {
                        Recoger_Datos_De_Fila_Tabla(FILA_ACTUAL++);
                    }
                    else
                    {
                        FILA_ACTUAL = 0;
                        Recoger_Datos_De_Fila_Tabla(FILA_ACTUAL);
                        FILA_ACTUAL++;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al recoger datos del registro");
            }
        }

        private void dgv_fabricantes_CellMouseDoubleClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                FILA_ACTUAL = dgv_fabricantes.CurrentRow.Index;
                Recoger_Datos_De_Fila_Tabla(FILA_ACTUAL);
                ACCION = "editar";
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al seleccionar el registro");
            }
        }

        private void btn_reporte_Click(object sender, EventArgs e)
        {
            frm_rpt_fabricantes frm = new frm_rpt_fabricantes();
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void dgv_fabricantes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txt_Buscar_TextChanged_1(object sender, EventArgs e)
        {
            Filtrar_Fabricante(txt_Buscar.Text);
        }
    }
}
