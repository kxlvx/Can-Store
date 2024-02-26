using enciclopedia_canina_store.interfaces.reportes;
using enciclopedia_canina_store.logica_negocio;
using System;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store
{
    public partial class frm_clientes : Form
    {
        cCliente controlador = new cCliente();
        databaseDataContext db = new databaseDataContext();
        string ACCION = "";
        int TOTAL_FILAS = 0;
        int FILA_ACTUAL = 0;
        public frm_clientes()
        {
            InitializeComponent();
        }
        void CargarTabla()
        {
            try
            {
                var query = from cli in db.clientes
                            join p in db.personas on cli.cli_codigo equals p.per_codigo
                            join c in db.ciudads on p.ciu_codigo equals c.ciu_codigo
                            select new
                            {
                                p.per_codigo,
                                p.per_nombre,
                                p.per_apellido,
                                p.per_ruc,
                                p.per_direccion,
                                p.per_telefono,
                                c.ciu_nombre,
                                p.per_email,
                                cli.cli_tipo,
                                cli.cli_saldo
                            };
                dgv_clientes.DataSource = query;
                habilitar(true);
                dgv_clientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al cargar la tabla");
            }
        }
        private void frm_clientes_Load(object sender, EventArgs e)
        {
            habilitar_textos(false);
            CargarTabla();
            Cagar_Combo_Box_Ciudad();
            Agregar_Clientes_Combo_Box();

        }

        private void Agregar_Clientes_Combo_Box()
        {
            cbx_tipo_cli.Items.Clear();
            cbx_tipo_cli.Items.Add("Actual");
            cbx_tipo_cli.Items.Add("Potencial");

        }


        private void Cagar_Combo_Box_Ciudad()
        {
            cb_ciudad.Items.Clear();
            var ciudades = from ciu in db.ciudads select ciu;
            foreach (var item in ciudades)
            {
                cb_ciudad.Items.Add(item.ciu_nombre);
            }
        }
        public void habilitar_textos(Boolean ok)
        {
            txt_codigo.Enabled = (false);
            txt_nombre.Enabled = (ok);
            txt_apellido.Enabled = (ok);
            txt_direccion.Enabled = (ok);
            cb_ciudad.Enabled = (ok);
            txt_email.Enabled = (ok);
            cbx_tipo_cli.Enabled = (ok);
            txt_cedula_ruc.Enabled = (ok);
            txt_telefono.Enabled = (ok);
            txt_saldo.Enabled = (ok);
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
            txt_codigo.Text = "";
            txt_nombre.Text = "";
            txt_apellido.Text = "";
            txt_direccion.Text = "";
            cb_ciudad.SelectedIndex = -1;
            cbx_tipo_cli.SelectedIndex = -1;
            txt_email.Text = "";
            cbx_tipo_cli.Text = "";
            txt_cedula_ruc.Text = "";
            txt_telefono.Text = "";
            txt_saldo.Text = "";
        }
        public void PonerFocus_Caja_De_Texto(TextBox caja)
        {
            caja.Focus();
            caja.SelectionStart = caja.Text.Length;
        }
        private bool ValidarTexto()
        {
            bool texto_validado = false;
            if (string.IsNullOrEmpty(txt_codigo.Text))
                MessageBox.Show("No se ha seleccionado el codigo");
            else
            {
                if (!Expresiones_Regulares.Verificar_Cedula(txt_cedula_ruc.Text.Replace(" ", "")))
                {
                    MessageBox.Show("La cedula no se ha ingresado correctamente");
                    PonerFocus_Caja_De_Texto(txt_cedula_ruc);
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
                        if (!Expresiones_Regulares.Verificar_Letras(txt_apellido.Text.Replace(" ", "")))
                        {
                            MessageBox.Show("El Apellido no se ha ingresado correctamente");
                            PonerFocus_Caja_De_Texto(txt_apellido);
                        }

                        else
                        {
                            if (string.IsNullOrEmpty(txt_direccion.Text.Replace(" ", "")))
                            {
                                MessageBox.Show("El campo dirección no puede  estar  vacio");
                                txt_direccion.Focus();
                                txt_direccion.SelectionStart = txt_direccion.Text.Length;
                            }
                            else
                            {
                                if (cb_ciudad.SelectedIndex == -1)
                                    MessageBox.Show("Asegurece de seleccionar una ciudad");
                                else
                                {
                                    if (!Expresiones_Regulares.Verificar_Telefono(txt_telefono.Text.Replace(" ", "")))
                                    {
                                        MessageBox.Show("Asegurece de ingresar un telefono valido");
                                        PonerFocus_Caja_De_Texto(txt_telefono);
                                    }

                                    else
                                    {
                                        if (!Expresiones_Regulares.Verificar_Correo(txt_email.Text.Replace(" ", "")))
                                        {
                                            MessageBox.Show("Asegurece de ingresar un correo valido");
                                            PonerFocus_Caja_De_Texto(txt_email);
                                        }
                                        else
                                        {
                                            if (cbx_tipo_cli.SelectedIndex == -1)
                                                MessageBox.Show("Asegurece de seleccionar un tipo de cliente");
                                            else
                                            {
                                                if (!Expresiones_Regulares.Verificar_Decimal(txt_saldo.Text.Replace(" ", "")) && !Expresiones_Regulares.Verificar_Numeros(txt_saldo.Text.Replace(" ", "")))
                                                {
                                                    MessageBox.Show("El saldo  no se ha ingresado correctamente");
                                                    PonerFocus_Caja_De_Texto(txt_saldo);
                                                }
                                                else
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

            return texto_validado;
        }
        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            ACCION = "insertar";

            limpiar_textos();
            habilitar_textos(true);
            habilitar_botones(false);
            // registros = dgv_clientes.Rows.Count;
            txt_codigo.Text = controlador.Obtener_Nuevo_Codigo_Cliente().ToString();
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
            bool ingreso_correcto = false;
            string sms = "";
            if (controlador.Existe_Persona_Cedula(txt_cedula_ruc.Text))
            {
                sms = ($"El cliente  con cedula {txt_cedula_ruc.Text} ya existe");
                lbl_mensaje.Text = sms;
                timer1.Start();
            }
            else
            {
                try
                {
                    db = new databaseDataContext();
                    persona fb = new persona();
                    fb.per_codigo = int.Parse(txt_codigo.Text);
                    fb.per_nombre = txt_nombre.Text;
                    fb.per_apellido = txt_apellido.Text;
                    fb.per_direccion = txt_direccion.Text;
                    fb.ciu_codigo = buscarIdCiudad(cb_ciudad.SelectedItem.ToString());
                    fb.per_email = txt_email.Text;
                    fb.per_ruc = txt_cedula_ruc.Text;
                    fb.per_telefono = txt_telefono.Text;
                    db.personas.InsertOnSubmit(fb);

                    cliente cli = new cliente();
                    cli.cli_codigo = int.Parse(txt_codigo.Text);
                    cli.cli_saldo = txt_saldo.Text;
                    cli.cli_tipo = cbx_tipo_cli.Text;
                    db.clientes.InsertOnSubmit(cli);
                    db.SubmitChanges();
                    sms = "registro guardado exitosamente";
                    ingreso_correcto = true;
                    lbl_mensaje.Text = sms;
                    timer1.Start();
                }
                catch (Exception)
                {
                    MessageBox.Show("Sucedio la excepcion al insertar el registro");
                }
            }
            return ingreso_correcto;
        }

        private bool EditarRegistro()
        {
            bool edicion_corecta = true;
            string sms = "";
            try
            {
                db = new databaseDataContext();
                persona fb = db.personas.Single(persona => persona.per_codigo == int.Parse(txt_codigo.Text));
                fb.per_nombre = txt_nombre.Text;
                fb.per_apellido = txt_apellido.Text;
                fb.per_direccion = txt_direccion.Text;
                fb.ciu_codigo = buscarIdCiudad(cb_ciudad.SelectedItem.ToString()); ;
                fb.per_email = txt_email.Text;
                fb.per_ruc = txt_cedula_ruc.Text;
                fb.per_telefono = txt_telefono.Text;
                db.SubmitChanges();
                //guardar datos en la tabla cliente
                cliente cli = db.clientes.Single(cliente => cliente.cli_codigo == int.Parse(txt_codigo.Text));
                cli.cli_saldo = txt_saldo.Text;
                cli.cli_tipo = cbx_tipo_cli.Text;
                db.SubmitChanges();
                sms = ("Registro editado correctamente");
                lbl_mensaje.Text = sms;
                timer1.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Asegurece de seleccionar un una ciudad de la lista");
                edicion_corecta = false;
            }
            return edicion_corecta;
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

        private void dgv_clientes_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                FILA_ACTUAL = dgv_clientes.CurrentRow.Index;
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
                DialogResult result = MessageBox.Show("Esta seguro de eliminar el Cliente: " +
                    txt_nombre.Text, "Advertencia", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        db = new databaseDataContext();
                        cliente cli = db.clientes.Single(cliente => cliente.cli_codigo == int.Parse(txt_codigo.Text));
                        db.clientes.DeleteOnSubmit(cli);
                        db.SubmitChanges();
                        persona fb = db.personas.Single(persona => persona.per_codigo == int.Parse(txt_codigo.Text));
                        db.personas.DeleteOnSubmit(fb);
                        db.SubmitChanges();
                        CargarTabla();
                        msg = "registro eliminado exitosamente";
                        limpiar_textos();
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        MessageBox.Show("No se puede eliminar, el cliente tiene facturas asociadas: ");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Sucedio un error inespeado al intentar eliminar el regitro");
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
            Filtrar_Clientes(txt_Buscar.Text);
        }

        private void Filtrar_Clientes(string nombre)
        {
            var query = from cli in db.clientes
                        join p in db.personas on cli.cli_codigo equals p.per_codigo
                        join c in db.ciudads on p.ciu_codigo equals c.ciu_codigo
                        where SqlMethods.Like(p.per_nombre, "%" + nombre + "%")
                        select new
                        {
                            p.per_codigo,
                            p.per_nombre,
                            p.per_apellido,
                            p.per_ruc,
                            p.per_direccion,
                            p.per_telefono,
                            c.ciu_nombre,
                            p.per_email,
                            cli.cli_tipo,
                            cli.cli_saldo
                        };
            dgv_clientes.DataSource = query;
            habilitar(true);
        }

        private void btn_anterior_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_clientes.Rows.Count;
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

        private void btn_siguiente_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_clientes.Rows.Count;
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
        private void Recoger_Datos_De_Fila_Tabla(int fila)
        {
            txt_codigo.Text = dgv_clientes.Rows[fila].Cells[0].Value.ToString();
            txt_nombre.Text = dgv_clientes.Rows[fila].Cells[1].Value.ToString();
            txt_apellido.Text = dgv_clientes.Rows[fila].Cells[2].Value.ToString();
            txt_cedula_ruc.Text = dgv_clientes.Rows[fila].Cells[3].Value.ToString();
            txt_direccion.Text = dgv_clientes.Rows[fila].Cells[4].Value.ToString();
            txt_telefono.Text = dgv_clientes.Rows[fila].Cells[5].Value.ToString();
            cb_ciudad.SelectedItem = dgv_clientes.Rows[fila].Cells[6].Value.ToString();
            txt_email.Text = dgv_clientes.Rows[fila].Cells[7].Value.ToString();
            cbx_tipo_cli.SelectedItem = dgv_clientes.Rows[fila].Cells[8].Value.ToString();
            txt_saldo.Text = dgv_clientes.Rows[fila].Cells[9].Value.ToString();
        }
        private void btn_reportes_Click(object sender, EventArgs e)
        {

            frm_rpt_clientes frm = new frm_rpt_clientes();
            // frm.MdiParent = this.MdiParent;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dgv_clientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public int buscarIdCiudad(string dato)
        {
            var codigo = from d in db.ciudads
                         where d.ciu_nombre.Equals(dato)
                         select d.ciu_codigo;

            return codigo.First();
        }

        private void txt_saldo_TextChanged(object sender, EventArgs e)
        {
            if (txt_saldo.Text == ",")
            {
                txt_saldo.Text = "0,";
                // txt_Numeros_Decimales.Select(txt_Numeros_Decimales.Text.Length, 0);
                txt_saldo.SelectionStart = txt_saldo.Text.Length;

            }
        }

        private void txt_cedula_ruc_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Ingreso_Numeros(e);
        }

        private void txt_nombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Entrada_De_Texto.Ingreso_Solo_Letras(e);
        }

        private void txt_apellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Entrada_De_Texto.Ingreso_Solo_Letras(e);
        }

        private void txt_telefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Ingreso_Numeros(e);
        }

        private void txt_saldo_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Numeros_Decimales(e, txt_saldo.Text);
        }
    }
}
