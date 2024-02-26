using enciclopedia_canina_store.interfaces;
using enciclopedia_canina_store.interfaces.reportes;
using enciclopedia_canina_store.logica_negocio;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store
{
    public partial class frm_facturacion : Form
    {
        databaseDataContext db;
        cVentaDetalle fac_ven_det = new cVentaDetalle();
        public frm_facturacion(int id_vendedor, string Nombre)
        {
            InitializeComponent();
            txt_cod_vendedor.Text = Convert.ToString(id_vendedor);//El id de vendedor y nombre son recogidos de inicio
            txt_vendedor.Text = Nombre;
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }
        private void frm_facturacion_Load(object sender, EventArgs e)
        {
            habilitar(true);
            txt_cedula_ruc.Enabled = false;
            cFactura fct = new cFactura();
            txt_cod_factura.Text = fct.Obtener_Nuevo_Codigo_Factura().ToString();
            txt_num_fact.Text = "FACT - " + txt_cod_factura.Text;
            //Estos dos botones estan desabilitados, no se puede seleccionar productos, sin antes seleccionar el vendedor
            btn_Agregar.Enabled = false;
            btnCancelar.Enabled = false;
            txt_desc.Enabled = false;
            txt_total.Enabled = false;
            txt_vendedor.Enabled = false;
            txt_pago_client.Enabled = false;

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
        public void habilitar_textos(Boolean ok)
        {

            txt_codigo.Enabled = (false);
            txt_nombre.Enabled = (ok);
            txt_apellido.Enabled = (ok);
            txt_direccion.Enabled = (ok);
            txt_ciudad.Enabled = (ok);
            txt_telefono.Enabled = (ok);
            txt_email.Enabled = (ok);
            txt_cod_factura.Enabled = (ok);
            txt_num_fact.Enabled = (ok);
            txt_cod_vendedor.Enabled = (ok);
            txt_vendedor.Enabled = ok;
        }
        public void habilitar_botones(Boolean ok)
        {
            btn_nuevo.Enabled = (ok);
            //hacen lo contrario de los otros botones
            btn_cancelar.Enabled = (!ok);
        }
        public void limpiar_textos()
        {
            txt_codigo.Text = "";
            txt_nombre.Text = "";
            txt_apellido.Text = "";
            txt_direccion.Text = "";
            txt_ciudad.Text = "";
            txt_telefono.Text = "";
            txt_email.Text = "";
            txt_desc.Text = "";
            txt_subtotal.Text = "";
            txt_subtotal_iva.Text = "";
            txt_total.Text = "";
            txt_pago_client.Text = "";
            txt_cambio.Text = "";
        }
        public Boolean validarTexto()
        {
            if (txt_codigo.Text.Equals("") ||
            txt_nombre.Text.Equals("") ||
            txt_apellido.Text.Equals("") ||
            txt_direccion.Text.Equals("") ||
            txt_ciudad.Text.Equals("") ||
            txt_telefono.Text.Equals("") ||
            txt_email.Text.Equals("") ||
            txt_cod_factura.Text.Equals("") ||
            txt_num_fact.Text.Equals("") ||
            txt_cod_vendedor.Text.Equals("") ||
            txt_vendedor.Text.Equals(""))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            txt_cedula_ruc.Enabled = true;
            txt_cedula_ruc.BackColor = Color.OrangeRed;
            // btn_Agregar.Enabled = false;
            // btnCancelar.Enabled = false;
            // txt_desc.Enabled = true;
            // txt_total.Enabled = true;
            //txt_pago_client.Enabled = true;
            habilitar_botones(false);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            txt_cedula_ruc.BackColor = Color.White;
            txt_cod_vendedor.BackColor = Color.White;
            lbl_mensaje.Text = "";
            timer1.Stop();
        }

        private void txt_cedula_ruc_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Ingreso_Numeros(e);
            if (e.KeyChar == (int)Keys.Enter)
            {
                db = new databaseDataContext();
                cCliente cli = new cCliente("");
                if (cli.Existe_Persona(txt_cedula_ruc.Text))
                {
                    try
                    {
                        var f = from d in db.personas
                                from ciu in db.ciudads
                                where d.ciu_codigo == ciu.ciu_codigo
                                & d.per_ruc.Equals(txt_cedula_ruc.Text)
                                select new
                                {
                                    d.per_codigo,
                                    d.per_nombre,
                                    d.per_apellido,
                                    d.per_direccion,
                                    ciu.ciu_nombre,
                                    d.per_telefono,
                                    d.per_email
                                };
                        var p = f.First();
                        txt_codigo.Text = "" + (p.per_codigo);
                        txt_nombre.Text = p.per_nombre;
                        txt_apellido.Text = p.per_apellido;
                        txt_direccion.Text = p.per_direccion;
                        txt_ciudad.Text = p.ciu_nombre;
                        txt_telefono.Text = p.per_telefono;
                        txt_email.Text = p.per_email;
                        //Se habilitan los botones de agrgar o cancelar productos
                        btn_Agregar.Enabled = true;
                        btnCancelar.Enabled = true;
                        txt_desc.Enabled = true;
                        txt_pago_client.Enabled = true;
                        txt_cedula_ruc.Enabled = false;
                        txt_total.Text = "0";
                        txt_pago_client.Text = "0";
                        txt_desc.Text = "0";
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Sucedio un error al extraer los datos del cliente \n Intente de nuevo");
                    }
                }
                else
                {
                    lbl_mensaje.Text = "Cliente no registrado";
                    limpiar_textos();
                    //Se desabilitan los botones de agrgar o cancelar productos
                    btn_Agregar.Enabled = false;
                    btnCancelar.Enabled = false;
                    txt_desc.Enabled = false;
                    txt_pago_client.Enabled = false;
                    timer1.Start();
                }
            }
        }

        private void txt_cod_vendedor_KeyPress(object sender, KeyPressEventArgs caracter)
        {
            /*
            if (!(char.IsNumber(caracter.KeyChar)) && (caracter.KeyChar != (char)Keys.Back)
                && (caracter.KeyChar != (char)Keys.Enter) && (caracter.KeyChar != (char)Keys.Return)
                && (caracter.KeyChar != (char)Keys.Delete))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                caracter.Handled = true;
                return;
            }
            else
            {
                if (caracter.KeyChar == (int)Keys.Enter)
                {
                    cVendedor ven = new cVendedor();
                    if (ven.existe(txt_cod_vendedor.Text))
                    {
                        db = new databaseDataContext();
                        var f = from d in db.persona
                                from v in db.vendedor
                                where d.per_codigo == int.Parse(txt_cod_vendedor.Text)
                                & d.per_codigo == v.ven_codigo
                                select new
                                {
                                    d.per_nombre
                                };
                        foreach (var p in f)
                        {
                            txt_vendedor.Text = p.per_nombre;
                        }
                    }
                    else
                    {
                        lbl_mensaje.Text = "Vendedor no registrado";
                        timer1.Start();
                    }
                }
            }*/
        }

        private void dgv_product_fact_CurrentCellChanged(object sender, EventArgs e)
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

        private void txt_subtotal_TextChanged(object sender, EventArgs e)
        {
            double IVA = 0.12;
            double valor = (double.Parse("0" + txt_subtotal.Text) * IVA);
            txt_subtotal_iva.Text = string.Format("{0:n2}", (Math.Truncate(valor * 100) / 100));

            valor = (double.Parse("0" + txt_subtotal.Text) + double.Parse("0" + txt_subtotal_iva.Text));
            txt_desc.Text = string.Format("{0:n2}", (Math.Truncate((valor * 0.02) * 100) / 100));
            txt_total.Text = string.Format("{0:n2}", (Math.Truncate((valor - (valor * 0.02)) * 100) / 100));
        }

        private void txt_cedula_ruc_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            habilitar(true);
            txt_cedula_ruc.Enabled = false;
            txt_cedula_ruc.Text = "";
            limpiar_textos();
            txt_desc.Enabled = false;
            txt_pago_client.Enabled = false;
        }

        private void txt_cod_vendedor_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_desc_TextChanged(object sender, EventArgs e)
        {
            if (txt_desc.Text.Equals(","))//al ingresar ,3
            {
                txt_desc.Text = "0,";
                txt_desc.SelectionStart = txt_desc.TextLength;
            }
        }

        private void txt_pago_client_TextChanged(object sender, EventArgs e)
        {
            if (txt_pago_client.Text.Equals(","))//al ingresar ,3
            {
                txt_pago_client.Text = "0,";
                txt_pago_client.SelectionStart = txt_pago_client.TextLength;
            }
        }

        private void txt_pago_client_KeyPress(object sender, KeyPressEventArgs tecla)
        {
            Validaciones_Codigo_Ancii.Numeros_Decimales(tecla, txt_pago_client.Text);

            if (tecla.KeyChar == (int)Keys.Enter)
            {


                if (!Expresiones_Regulares.Verificar_Decimal(txt_pago_client.Text) && !Expresiones_Regulares.Verificar_Numeros(txt_pago_client.Text))
                {
                    MessageBox.Show("Asegurece de ingresar datotos válidos\n Ejemplo: 15,50");
                }
                else
                {
                    double cambio = double.Parse(txt_pago_client.Text) - double.Parse(txt_total.Text);
                    if (cambio < 0)
                    {
                        txt_cambio.Text = "saldo insuficiente";
                    }
                    else
                    {
                        txt_cambio.Text = string.Format("{0:n2}", (Math.Truncate(cambio * 100) / 100));
                        DialogResult res = MessageBox.Show("confirmar pago\nGuardar Factura", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if ((int)res == (int)DialogResult.Yes)
                        {
                            int total_filas = dgv_product_fact.Rows.Count - 1;
                            if (total_filas == 0)
                            {
                                MessageBox.Show("Seleccione al menos un producto");
                            }
                            else
                            {
                                if (Factura_Guardada_Correctamente())
                                {
                                    if (guardarDetalleFactura())
                                    {
                                        Actualizar_Stock();
                                        limpiar_textos();
                                        MessageBox.Show("Factura almacenada correctamente");
                                        DialogResult imprimir = MessageBox.Show("Desea imprimir la factura", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                        if ((int)imprimir == (int)DialogResult.Yes)
                                        {
                                            frm_rpt_factura f = new frm_rpt_factura(int.Parse(txt_cod_factura.Text));
                                            f.Show();
                                        }
                                        dgv_product_fact.Rows.Clear();
                                        cFactura fct = new cFactura();
                                        txt_cod_factura.Text = fct.Obtener_Nuevo_Codigo_Factura().ToString();
                                        txt_num_fact.Text = "fact-" + txt_cod_factura.Text;

                                        /*Se desabilitan los campos */
                                        btnCancelar.Enabled = false;
                                        btn_Agregar.Enabled = false;
                                        txt_cedula_ruc.Text = "";
                                        txt_cedula_ruc.Focus();
                                        txt_cedula_ruc.SelectionStart = txt_cedula_ruc.TextLength;

                                        txt_pago_client.Enabled = false;
                                        txt_desc.Enabled = false;
                                        habilitar_botones(true);
                                    }
                                    else
                                    {
                                        MessageBox.Show("No fue posible almacenar la factura");
                                    }
                                }
                            }


                        }
                    }
                }
            }

        }

        private bool Factura_Guardada_Correctamente()
        {

            bool insercion_correcta = false;
            try
            {
                db = new databaseDataContext();
                factura_venta fv = new factura_venta();
                fv.fac_codigo = int.Parse(txt_cod_factura.Text);
                fv.fac_numero = txt_num_fact.Text;
                fv.fac_fecha = dt_fecha.Value.Date;
                fv.cli_codigo = int.Parse(txt_codigo.Text);
                fv.ven_codigo = int.Parse(txt_cod_vendedor.Text);
                fv.fac_subtotal = decimal.Parse(txt_subtotal.Text);
                fv.fac_subiva = decimal.Parse(txt_subtotal_iva.Text);
                fv.fac_total = decimal.Parse(txt_total.Text);
                fv.fac_descuento = decimal.Parse(txt_desc.Text);
                fv.fac_estado = true;
                db.factura_ventas.InsertOnSubmit(fv);
                // db.SubmitChanges();
                insercion_correcta = true;
            }
            catch (Exception)
            {
                MessageBox.Show("No fue posible almacenar la factura, asegurece de ingresar datos correctos");
            }
            return insercion_correcta;
        }

        bool guardarDetalleFactura()
        {
            int ID_Venta_Detalle = fac_ven_det.Obtener_Nuevo_Codigo_Factura_Venta_Detalle();
            bool guardado_correcto = false;
            try
            {
                int total_filas = dgv_product_fact.Rows.Count - 1;
                for (int fila = 0; fila < total_filas; fila++)
                {
                    fac_ven_detalle fd = new fac_ven_detalle();
                    fd.fac_ven_detalle1 = ID_Venta_Detalle;
                    fd.fac_codigo = int.Parse(txt_cod_factura.Text);
                    fd.pro_id = int.Parse(dgv_product_fact.Rows[fila].Cells[0].Value.ToString());
                    fd.fv_cantidad = int.Parse(dgv_product_fact.Rows[fila].Cells[4].Value.ToString());
                    fd.fv_puv = decimal.Parse(dgv_product_fact.Rows[fila].Cells[3].Value.ToString());
                    fd.fv_estado = true;
                    fd.fv_descuento = decimal.Parse(txt_desc.Text);
                    db.fac_ven_detalles.InsertOnSubmit(fd);
                    ID_Venta_Detalle++;
                }
                db.SubmitChanges();
                guardado_correcto = true;

            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio una excepcion al guardar el detalle");
            }
            return guardado_correcto;
        }
        bool Actualizar_Stock()
        {
            bool aactualizacio_exitosa = false;
            try
            {
                db = new databaseDataContext();
                int total_filas = dgv_product_fact.Rows.Count - 1;
                for (int i = 0; i < total_filas; i++)
                {
                    int Id_Actualizar = int.Parse(dgv_product_fact.Rows[i].Cells[0].Value.ToString());

                    var resultado = from p in db.productos where p.pro_id == Id_Actualizar select p;
                    foreach (var item in resultado)
                    {
                        item.pro_cantidad = item.pro_cantidad - int.Parse(dgv_product_fact.Rows[i].Cells[4].Value.ToString());
                    }
                    db.SubmitChanges();
                }
                aactualizacio_exitosa = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al actualizar los productos");
            }
            return aactualizacio_exitosa;
        }
        /*
        public static void AllowNumber(object sender, KeyPressEventArgs e, char cSymbol)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != cSymbol)
            {
                e.Handled = true;
            }

            if (e.KeyChar == cSymbol && (sender as TextBox).Text.IndexOf(cSymbol) > -1)
            {
                e.Handled = true;
            }
        }
        */
        private void btn_reporte_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Expresiones_Regulares.Verificar_Numeros(txt_Reporte.Text))
                {
                    MessageBox.Show("Ingrese el codigo de factura para ver el reporte");
                    txt_Reporte.Focus();
                    txt_Reporte.SelectionStart = txt_Reporte.TextLength;
                }
                else
                {
                    int id = int.Parse(txt_Reporte.Text);
                    frm_rpt_factura frm = new frm_rpt_factura(id);
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Show();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al abrir el formulario. Intente de nuevo");

            }



        }

        private void btn_Agregar_Click(object sender, EventArgs e)
        {
            frm_buscar_producto frm = new frm_buscar_producto(this);
            frm.Show();

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow fila = dgv_product_fact.CurrentRow;
                string dato = fila.Cells[1].Value.ToString();
                DialogResult res = MessageBox.Show("esta seguro de eliminar el producto: " + dato, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    txt_subtotal.Text = "" + (double.Parse(txt_subtotal.Text) - double.Parse(fila.Cells[5].Value.ToString()));
                    dgv_product_fact.Rows.RemoveAt(dgv_product_fact.CurrentRow.Index);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Asegurece de seleccionar un registro");
            }

        }

        private void txt_desc_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Numeros_Decimales(e, txt_desc.Text);
        }

        private void txt_Reporte_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Ingreso_Numeros(e);
        }
    }
}
