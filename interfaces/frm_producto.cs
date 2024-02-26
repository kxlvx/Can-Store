using enciclopedia_canina_store.interfaces.reportes;
using enciclopedia_canina_store.logica_negocio;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store
{
    public partial class frm_producto : Form
    {
        cProducto lis;
        databaseDataContext db = new databaseDataContext();

        string ACCION = "";
        int TOTAL_FILAS = 0;
        int FILA_ACTUAL = 0;
        public frm_producto()
        {
            InitializeComponent();
        }
        void cargartabla()
        {
            lis = new cProducto(txt_Buscar.Text);
            dgv_productos.DataSource = lis.buscar();
            dgv_productos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            habilitar(true);
        }
        private void frm_producto_Load(object sender, EventArgs e)
        {
            cargartabla();
            Cargar_Categorias_combo_Box();
            cargar_Fabricante_Combo_Box();

        }
        public void habilitar_textos(Boolean ok)
        {
            txt_id.Enabled = (false);
            txt_nombre.Enabled = (ok);
            txt_descripcion.Enabled = (ok);
            txt_cantidad.Enabled = (ok);
            txt_precio_venta.Enabled = (ok);
            cbx_categoria.Enabled = (ok);
            txt_precio_compra.Enabled = (ok);
            txt_codigo_producto.Enabled = (ok);
            txt_porcent_desc.Enabled = (ok);
            cbx_fabricante.Enabled = (ok);
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
            btn_add_categoria.Enabled = !ok;
            btn_add_fabricante.Enabled = !ok;
        }
        public void limpiar_textos()
        {
            txt_id.Text = "";
            txt_nombre.Text = "";
            txt_descripcion.Text = "";
            txt_cantidad.Text = "";
            txt_precio_venta.Text = "";
            cbx_categoria.SelectedIndex=-1;
            txt_precio_compra.Text = "";
            txt_codigo_producto.Text = "";
            txt_porcent_desc.Text = "";
            cbx_fabricante.SelectedIndex = -1;
        }
        public void PonerFocus_Caja_De_Texto(TextBox caja)
        {
            caja.Focus();
            caja.SelectionStart = caja.Text.Length;
        }
        public Boolean ValidarTexto()
        {
            bool texto_validado = false;
            if (string.IsNullOrEmpty(txt_id.Text))
                MessageBox.Show("No se ha seleccionado el ID");
            else
            {
                if (string.IsNullOrEmpty(txt_codigo_producto.Text))
                {
                    MessageBox.Show("El codigo de producto no se ha ingresado");
                    PonerFocus_Caja_De_Texto(txt_codigo_producto);
                }
                else
                {
                    if (Verificar_Codigo_Producto_Es_Unico() && ACCION == "insertar")
                    {
                        MessageBox.Show("El codigo de producto ingresado ya existe, Solo necesita editarlo");
                    }
                    else
                    {
                        if (!Expresiones_Regulares.Verificar_Letras(txt_nombre.Text.Replace(" ", "")))
                        {
                            MessageBox.Show("El Nombre no se ha ingresado correctamente");
                            PonerFocus_Caja_De_Texto(txt_nombre);
                        }
                        else
                        {
                            if (lis.Verificar_Producto_Existe(txt_nombre.Text, int.Parse(txt_id.Text)))
                            {
                                MessageBox.Show("No se puede insetar dos productos con el mismo nombre");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(txt_descripcion.Text.Replace(" ", "")))
                                {
                                    MessageBox.Show("La descripcion no se ha ingresado correctamente");
                                    txt_descripcion.Focus();
                                    txt_descripcion.SelectionStart = txt_descripcion.Text.Length;
                                }
                                else
                                {
                                    if (!Expresiones_Regulares.Verificar_Numeros(txt_cantidad.Text.Replace(" ", "")))
                                    {
                                        MessageBox.Show("La cantidad  no se ha ingresado correctamente");
                                        PonerFocus_Caja_De_Texto(txt_cantidad);
                                    }
                                    else
                                    {
                                        if (!Expresiones_Regulares.Verificar_Decimal(txt_precio_compra.Text.Replace(" ", "")) && !Expresiones_Regulares.Verificar_Numeros(txt_precio_compra.Text.Replace(" ", "")))
                                        {
                                            MessageBox.Show("El precio de compra no se ha ingresado correctamente");
                                            PonerFocus_Caja_De_Texto(txt_precio_compra);
                                        }
                                        else
                                        {
                                            if (!Expresiones_Regulares.Verificar_Decimal(txt_precio_venta.Text.Replace(" ", "")) && !Expresiones_Regulares.Verificar_Numeros(txt_precio_venta.Text.Replace(" ", "")))
                                            {
                                                MessageBox.Show("El precio de venta no se ha ingresado correctamente");
                                                PonerFocus_Caja_De_Texto(txt_precio_venta);
                                            }
                                            else
                                            {
                                                if (!Expresiones_Regulares.Verificar_Decimal(txt_porcent_desc.Text.Replace(" ", "")) && !Expresiones_Regulares.Verificar_Numeros(txt_porcent_desc.Text.Replace(" ", "")))
                                                {
                                                    MessageBox.Show("El porcentaje de descuento no se ha ingresado correctamente");
                                                    PonerFocus_Caja_De_Texto(txt_precio_venta);
                                                }
                                                else
                                                {
                                                    if (double.Parse(txt_porcent_desc.Text) >= 100)
                                                    {
                                                        MessageBox.Show("El porcentaje de descuento debe ser menor a 100");
                                                        PonerFocus_Caja_De_Texto(txt_porcent_desc);
                                                    }
                                                    else
                                                    {
                                                        if (cbx_categoria.SelectedIndex == -1)
                                                        {
                                                            MessageBox.Show("Seleccione una categoria");
                                                            cbx_categoria.Focus();
                                                        }
                                                        else
                                                        {
                                                            if (cbx_fabricante.SelectedIndex == -1)
                                                            {
                                                                MessageBox.Show("Seleccione un fabricante");
                                                                cbx_fabricante.Focus();
                                                            }
                                                            else
                                                            {
                                                                texto_validado = true;
                                                            }
                                                        }
                                                    }


                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

            }
            return texto_validado;
        }

        private bool Verificar_Codigo_Producto_Es_Unico()
        {
            var productos = from p in db.productos where p.pro_codigo == txt_codigo_producto.Text select p;
            return productos.Count() > 0;
        }

        void Cargar_Categorias_combo_Box()
        {
            cbx_categoria.Items.Clear();
            var cat = from c in db.categorias
                      select c.cat_nombre;
            foreach (var nombre in cat)
            {
                cbx_categoria.Items.Add(nombre);
            }
        }
        void cargar_Fabricante_Combo_Box()
        {
            cbx_fabricante.Items.Clear();
            var fab = from f in db.fabricantes
                      select f.fab_nombre;

            foreach (var nombre in fab)
            {
                cbx_fabricante.Items.Add(nombre);
            }
        }

        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            ACCION = "insertar";
            limpiar_textos();
            habilitar_textos(true);
            habilitar_botones(false);
            txt_id.Text = lis.Obtener_Nuevo_Codigo_Producto().ToString();
            lbl_mensaje.Text = "";
        }
        public Boolean validarPrecioVenta()
        {
            double pv = double.Parse(txt_precio_venta.Text);
            double pc = double.Parse(txt_precio_compra.Text);
            if (pv > pc)
            {
                return true;
            }
            else
            {
                txt_precio_venta.BackColor = Color.DarkRed;
                timer1.Start();
                return false;
            }
        }
        private void btn_guardar_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (ValidarTexto())
            {
                if (validarPrecioVenta())
                {
                    if (ACCION.Equals("editar"))
                    {
                        try
                        {
                            lis = new cProducto("");
                            producto fb = db.productos.Single(producto => producto.pro_id == int.Parse(txt_id.Text));
                            fb.pro_nombre = txt_nombre.Text;
                            fb.pro_descripcion = txt_descripcion.Text;
                            fb.pro_cantidad = int.Parse(txt_cantidad.Text);
                            fb.pro_precio_venta = decimal.Parse(txt_precio_venta.Text);
                            fb.cat_codigo = lis.buscarIdCat(cbx_categoria.Text);
                            fb.pro_precio_compra = decimal.Parse(txt_precio_compra.Text);
                            fb.pro_codigo = txt_codigo_producto.Text;
                            fb.pro_desc_porcen = decimal.Parse(txt_porcent_desc.Text);
                            fb.fab_codigo = lis.buscarIdFab(cbx_fabricante.Text);
                            db.SubmitChanges();
                            cargartabla();
                            msg = "registro actualizado exitosamente";
                            habilitar(true);
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Sucedio un error al editar el registro");
                        }

                    }
                    else
                    {
                        try
                        {
                            lis = new cProducto("");
                            producto fb = new producto();
                            fb.pro_id = int.Parse(txt_id.Text);
                            fb.pro_nombre = txt_nombre.Text;
                            fb.pro_descripcion = txt_descripcion.Text;
                            fb.pro_cantidad = int.Parse(txt_cantidad.Text);
                            fb.pro_precio_venta = decimal.Parse(txt_precio_venta.Text);
                            fb.cat_codigo = lis.buscarIdCat(cbx_categoria.Text);
                            fb.pro_precio_compra = decimal.Parse(txt_precio_compra.Text);
                            fb.pro_codigo = txt_codigo_producto.Text;
                            fb.pro_desc_porcen = decimal.Parse(txt_porcent_desc.Text);
                            fb.fab_codigo = lis.buscarIdFab(cbx_fabricante.Text);
                            db.productos.InsertOnSubmit(fb);
                            db.SubmitChanges();
                            cargartabla();
                            msg = "registro guardado exitosamente";
                            habilitar(true);
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Sucedio un error al insertar el registro");
                        }
                    }
                }
                else
                {
                    msg = "algunos campos no son correctos";
                }
            }
            lbl_mensaje.Text = msg;
            timer1.Start();
        }

        private void btn_editar_Click(object sender, EventArgs e)
        {
            if (ValidarTexto())
            {
                ACCION = "editar";
                habilitar_textos(true);
                txt_nombre.Focus();
                habilitar_botones(false);
                lbl_mensaje.Text = "";
            }
            else
            {
                lbl_mensaje.Text = "seleccione un registro de la tabla";
                timer1.Start();
            }
        }

        private void dgv_productos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                FILA_ACTUAL = dgv_productos.CurrentRow.Index;
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
                txt_id.Text = "";
            lbl_mensaje.Text = "";
        }
        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (ValidarTexto())
            {
                try
                {
                    DialogResult result = MessageBox.Show("Esta seguro de eliminar el producto: " +
                    txt_nombre.Text, "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        producto fb = db.productos.Single(pro => pro.pro_id == int.Parse(txt_id.Text));
                        db.productos.DeleteOnSubmit(fb);
                        db.SubmitChanges();
                        cargartabla();
                        msg = "registro eliminado exitosamente";
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Sucedio un error al eliminar el registro");
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
            txt_precio_venta.BackColor = Color.White;
            timer1.Stop();
        }

        private void btn_add_categoria_Click(object sender, EventArgs e)
        {
            frm_categoria cat = new frm_categoria();

            cat.Show();
        }

        private void btn_add_fabricante_Click(object sender, EventArgs e)
        {
            frm_fabricante fab = new frm_fabricante();
            fab.Show();
        }

        private void cbx_categoria_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void cbx_fabricante_MouseClick(object sender, MouseEventArgs e)
        {
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
            cargartabla();
        }
        private void btn_anterior_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_productos.Rows.Count;
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

        private void Recoger_Datos_De_Fila_Tabla(int filaActual)
        {
            txt_id.Text = dgv_productos.Rows[filaActual].Cells[0].Value.ToString();
            txt_nombre.Text = dgv_productos.Rows[filaActual].Cells[1].Value.ToString();
            txt_descripcion.Text = dgv_productos.Rows[filaActual].Cells[2].Value.ToString();
            txt_cantidad.Text = dgv_productos.Rows[filaActual].Cells[3].Value.ToString();
            txt_precio_compra.Text = dgv_productos.Rows[filaActual].Cells[4].Value.ToString();
            txt_precio_venta.Text = dgv_productos.Rows[filaActual].Cells[5].Value.ToString();
            cbx_categoria.Text = dgv_productos.Rows[filaActual].Cells[6].Value.ToString();
            cbx_fabricante.Text = dgv_productos.Rows[filaActual].Cells[7].Value.ToString();
            txt_codigo_producto.Text = dgv_productos.Rows[filaActual].Cells[8].Value.ToString();
            txt_porcent_desc.Text = dgv_productos.Rows[filaActual].Cells[9].Value.ToString();

        }

        private void btn_siguiente_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_productos.Rows.Count;
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

        private void txt_cantidad_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_cantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Validaciones_Entrada_De_Texto.Validar_Numeros_1_Caracter(sender, e, '@');
            Validaciones_Codigo_Ancii.Ingreso_Numeros(e);
        }

        private void txt_precio_venta_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Numeros_Decimales(e, txt_precio_venta.Text);
        }
        private void txt_precio_compra_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Numeros_Decimales(e, txt_precio_compra.Text);
        }
        private void txt_porcent_desc_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Numeros_Decimales(e, txt_porcent_desc.Text);
        }
        private void txt_precio_venta_TextChanged(object sender, EventArgs e)
        {
            if (txt_precio_venta.Text.Equals(","))
            {
                txt_precio_venta.Text = "0,";
                txt_precio_venta.SelectionStart = txt_precio_venta.Text.Length;
            }
        }
        private void txt_precio_compra_TextChanged(object sender, EventArgs e)
        {
            if (txt_precio_compra.Text.Equals(","))
            {
                txt_precio_compra.Text = "0,";
                txt_precio_compra.SelectionStart = txt_precio_compra.Text.Length;
            }
        }
        private void txt_porcent_desc_TextChanged(object sender, EventArgs e)
        {
            if (txt_porcent_desc.Text.Equals(","))
            {
                txt_porcent_desc.Text = "0,";
                txt_porcent_desc.SelectionStart = txt_porcent_desc.Text.Length;
            }
        }


        private void txt_precio_venta_KeyDown(object sender, KeyEventArgs e)
        {


        }/*
        public static void Validar_Numeros_1_Caracter(object objeto, KeyPressEventArgs tecla, char caracter)
        {
            if (!char.IsControl(tecla.KeyChar) && !char.IsDigit(tecla.KeyChar) && tecla.KeyChar != caracter)
            {
                tecla.Handled = true;
            }

            if (tecla.KeyChar == caracter && (objeto as TextBox).Text.IndexOf(caracter) > -1)
            {
                tecla.Handled = true;
            }
        }
        */
        private void dgv_productos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_reporte_Click(object sender, EventArgs e)
        {
            frm_rpt_productos f = new frm_rpt_productos();
            // f.MdiParent = this;
           // AbrirFormInPanel(f);
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }
    }
}
