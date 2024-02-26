using enciclopedia_canina_store.interfaces.reportes;
using enciclopedia_canina_store.logica_negocio;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store
{
    public partial class frm_categoria : Form
    {
        cCategoria controlador=new cCategoria();
        databaseDataContext db = new databaseDataContext();
        string ACCION = "";
        int TOTAL_FILAS = 0;
        int FILA_ACTUAL = 0;
        public frm_categoria()
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
                var query = from f in db.categorias
                            select new
                            {
                               CODIGO= f.cat_codigo,
                               NOMBRE= f.cat_nombre
                            };
                dgv_categoria_prod.DataSource = query;
                habilitar(true);
                dgv_categoria_prod.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al cargar la tabla");
            }
        }

        private void frm_categoria_Load(object sender, EventArgs e)
        {
            habilitar_textos(false);
            CargarTabla();
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
        public void limpiar_textos()
        {
            txt_codigo.Text = ("");
            txt_nombre_cat.Text = ("");
        }
        public Boolean ValidarTexto()
        {
            bool texto_validado = false;
            if (string.IsNullOrEmpty(txt_codigo.Text.Replace(" ", "")))
                 MessageBox.Show("No se ha seleccionado el codigo");
            else
            {
                if (string.IsNullOrEmpty(txt_nombre_cat.Text.Replace(" ", "")))
                {
                    MessageBox.Show("Asegurece de ingresar un nombre de categoria");
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

        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            ACCION = "insertar";
            limpiar_textos();
            habilitar_textos(true);
            habilitar_botones(false);
            txt_codigo.Text = controlador.Obtener_Nuevo_Codigo_Categoria().ToString();
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
                    if (!controlador.Existe_Registro(txt_nombre_cat.Text,int.Parse(txt_codigo.Text)))
                    {
                        categoria fb = new categoria();
                        fb.cat_codigo = int.Parse(txt_codigo.Text);
                        fb.cat_nombre = txt_nombre_cat.Text;
                        db.categorias.InsertOnSubmit(fb);
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
                
                if (!controlador.Existe_Registro(txt_nombre_cat.Text,int.Parse(txt_codigo.Text)))
                {
                    categoria fb = db.categorias.Single(categoria => categoria.cat_codigo == int.Parse(txt_codigo.Text));
                    fb.cat_nombre = txt_nombre_cat.Text;
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

        private void dgv_categoria_prod_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                FILA_ACTUAL = dgv_categoria_prod.CurrentRow.Index;
                Recoger_Datos_De_Fila_Tabla(FILA_ACTUAL);
                ACCION = "editar";
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al seleccionar el registro");
            }
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            habilitar(true);
            if (ACCION == "insertar")
                txt_codigo.Text = "";
            lbl_mensaje.Text = "";
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (ValidarTexto())
            {
                DialogResult result = MessageBox.Show("Esta seguro de eliminar el fabricante: " +
                    txt_nombre_cat.Text, "Advertencia", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        db = new databaseDataContext();
                        categoria fb = db.categorias.Single(categoria => categoria.cat_codigo == int.Parse(txt_codigo.Text));
                        db.categorias.DeleteOnSubmit(fb);
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

        private void txt_Buscar_TextChanged(object sender, EventArgs e)
        {
            Filtrar_Categoria(txt_Buscar.Text);
        }

        private void Filtrar_Categoria(string nombre)
        {
            try
            {
            var query = from p in db.categorias
                        where System.Data.Linq.SqlClient.SqlMethods.Like(p.cat_nombre, "%" + nombre + "%")
                        select new
                        {
                            CODIGO = p.cat_codigo,
                            NOMBRE = p.cat_nombre
                        };
            dgv_categoria_prod.DataSource = query;
            habilitar(true);
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrio un inconveniente al filtrar los registros");
            }
        }

        private void dgv_categoria_prod_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btn_anterior_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_categoria_prod.Rows.Count;
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
            txt_codigo.Text = dgv_categoria_prod.Rows[fila].Cells[0].Value.ToString();
            txt_nombre_cat.Text = dgv_categoria_prod.Rows[fila].Cells[1].Value.ToString();
        }

        private void btn_siguiente_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_categoria_prod.Rows.Count;
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

        private void btn_reporte_Click(object sender, EventArgs e)
        {
            frm_rpt_categoria r = new frm_rpt_categoria();
            // AbrirFormInPanel(f);
            r.WindowState = FormWindowState.Maximized;
            // this.Parent.
            r.Show();
        }

        private void dgv_categoria_prod_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
