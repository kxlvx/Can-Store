using enciclopedia_canina_store.interfaces.reportes;
using enciclopedia_canina_store.logica_negocio;
using System;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store.interfaces
{
    public partial class frm_ciudad : Form
    {

        int TOTAL_FILAS = 0;
        int FILA_ACTUAL = 0;
        string ACCION = "";
        cCiudad controlador = new cCiudad();
        databaseDataContext db = new databaseDataContext();
        public frm_ciudad()
        {

            InitializeComponent();
        }
        void CargarTabla()
        {
            try
            {
                var query = from c in db.ciudads
                            select new
                            {
                                CODIGO = c.ciu_codigo,
                                NOMBRE = c.ciu_nombre
                            };
                dgv_ciudad.DataSource = query;
                habilitar(true);
                dgv_ciudad.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al cargar la tabla");
            }
        }
        public void habilitar_textos(Boolean ok)
        {
            txt_codigo.Enabled = (false);
            txt_nombre_cat.Enabled = (ok);
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


        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            ACCION = "insertar";

            limpiar_textos();
            habilitar_textos(true);
            habilitar_botones(false);
            lbl_mensaje.Text = "";
            txt_codigo.Text = controlador.Obtener_Nuevo_Codigo_Ciudad().ToString();
        }
        public void limpiar_textos()
        {
            txt_codigo.Text = ("");
            txt_nombre_cat.Text = ("");
        }

        private void btn_editar_Click(object sender, EventArgs e)
        {
            if (ValidarTexto())
            {
                ACCION = "editar";
                habilitar_textos(true);
                txt_nombre_cat.Focus();
                habilitar_botones(false);
                lbl_mensaje.Text = "";
            }
            else
            {
                lbl_mensaje.Text = "seleccione un registro de la tabla";
                timer1.Start();
            }
        }
        private bool ValidarTexto()
        {

            bool texto_validado = false;
            if (string.IsNullOrEmpty(txt_codigo.Text.Replace(" ", "")))
                MessageBox.Show("No se ha seleccionado el codigo");
            else
            {
                if (string.IsNullOrEmpty(txt_nombre_cat.Text.Replace(" ", "")))
                {
                    MessageBox.Show("Asegurece de ingresar un nombre de ciudad");
                    txt_nombre_cat.Focus();
                    txt_nombre_cat.SelectionStart = txt_nombre_cat.TextLength;
                }
                else
                {
                    texto_validado = true;
                }
            }

            return texto_validado;
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            habilitar(true);
            if (ACCION == "insertar")
                txt_codigo.Text = "";
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
        private bool EditarRegistro()
        {
            string msg = "";
            bool edicion_corecta = false;
            try
            {
                db = new databaseDataContext();
                if (!controlador.Existe_Registro(txt_nombre_cat.Text, int.Parse(txt_codigo.Text)))
                {
                    ciudad ci = db.ciudads.Single(ciu => ciu.ciu_codigo == int.Parse(txt_codigo.Text));
                    ci.ciu_nombre = txt_nombre_cat.Text;
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

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (ValidarTexto())
            {
                DialogResult result = MessageBox.Show("Esta seguro de eliminar la Ciudad: " +
                    txt_nombre_cat.Text, "Advertencia", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        db = new databaseDataContext();
                        ciudad ven = db.ciudads.Single(vend => vend.ciu_codigo == int.Parse(txt_codigo.Text));
                        db.ciudads.DeleteOnSubmit(ven);
                        db.SubmitChanges();
                        CargarTabla();
                        msg = "registro eliminado exitosamente";
                        limpiar_textos();
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

        private void btn_reporte_Click(object sender, EventArgs e)
        { 
            
            frm_rpt_ciudad r = new frm_rpt_ciudad();
            // AbrirFormInPanel(f);
            r.WindowState = FormWindowState.Maximized;
            // this.Parent.
            r.Show();
        }

        private void btn_anterior_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_ciudad.Rows.Count;
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
            txt_codigo.Text = dgv_ciudad.Rows[fila].Cells[0].Value.ToString();
            txt_nombre_cat.Text = dgv_ciudad.Rows[fila].Cells[1].Value.ToString();

        }
        private void btn_siguiente_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_ciudad.Rows.Count;
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

        private void dgv_ciudad_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txt_Buscar_TextChanged(object sender, EventArgs e)
        {
            Filtrar_Ciudad(txt_Buscar.Text);
        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void iconminimizar_Click(object sender, EventArgs e)
        {
           // WindowState = FormWindowState.Minimized;
        }

        private void dgv_ciudad_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                FILA_ACTUAL = dgv_ciudad.CurrentRow.Index;
                Recoger_Datos_De_Fila_Tabla(FILA_ACTUAL);
                ACCION = "editar";
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al seleccionar el registro");
            }
        }

        private void frm_ciudad_Load(object sender, EventArgs e)
        {
            habilitar_textos(false);
            CargarTabla();
        }
        private bool GuardarRegistro()
        {
            string msg;
            bool ingreso_correcto = false;
            try
            {
                db = new databaseDataContext();
                if (!controlador.Existe_Registro(txt_nombre_cat.Text, int.Parse(txt_codigo.Text)))
                {
                    ciudad ciu = new ciudad();
                    ciu.ciu_codigo = int.Parse(txt_codigo.Text);
                    ciu.ciu_nombre = txt_nombre_cat.Text;
                    db.ciudads.InsertOnSubmit(ciu);
                    db.SubmitChanges();//Commit 
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

        private void Filtrar_Ciudad(string nombre)
        {
            try
            {
                var query = from c in db.ciudads
                            where SqlMethods.Like(c.ciu_nombre, "%" + nombre + "%")
                            select new
                            {
                                CÓDIGO = c.ciu_codigo,
                                NOMBRE = c.ciu_nombre
                            };
                dgv_ciudad.DataSource = query;
                habilitar(true);
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrio un inconveniente al filtrar los registros");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_mensaje.Text = "";
            timer1.Stop();
        }
    }
}
