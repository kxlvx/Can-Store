using enciclopedia_canina_store.interfaces.reportes;
using enciclopedia_canina_store.logica_negocio;
using System;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store.interfaces
{
    public partial class frm_vendedor : Form
    {
        databaseDataContext db = new databaseDataContext();
        string ACCION = "";
        int ID_USUARIO_ACTUAL = 0;
        int TOTAL_FILAS = 0;
        int FILA_ACTUAL = 0;
        public frm_vendedor(int Id_Usuario_Actual)
        {
            InitializeComponent();
            ID_USUARIO_ACTUAL = Id_Usuario_Actual;
        }

        private void frm_vendedor_Load(object sender, EventArgs e)
        {
            habilitar_textos(false);
            CargarTabla();
            Cagar_Combo_Box_Ciudad();
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

        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            ACCION = "insertar";

            limpiar_textos();
            habilitar_textos(true);
            habilitar_botones(false);
            lbl_mensaje.Text = "";
            txt_codigo.Text = Obtener_Nuevo_Codigo_Persona().ToString();
        }

        private void btn_editar_Click(object sender, EventArgs e)
        {
            if (ValidarTexto())
            {
                ACCION = "editar";
                habilitar_textos(true);
                txt_sueldo.Focus();
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



        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (ValidarTexto())
            {
                if (!Verdedor_Es_Usuario_Actual())
                {
                    DialogResult result = MessageBox.Show("Esta seguro de eliminar el Vendedor: " +
                    txt_nombre.Text, "Advertencia", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            db = new databaseDataContext();
                            vendedor ven = db.vendedors.Single(vend => vend.ven_codigo == int.Parse(txt_codigo.Text));
                            db.vendedors.DeleteOnSubmit(ven);
                            db.SubmitChanges();
                            persona fb = db.personas.Single(persona => persona.per_codigo == int.Parse(txt_codigo.Text));
                            db.personas.DeleteOnSubmit(fb);
                            db.SubmitChanges();
                            CargarTabla();
                            msg = "registro eliminado exitosamente";
                            limpiar_textos();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Sucedio un error al eliminar el registro, intente nuevamente");
                        }
                    }

                }
                else
                {
                    msg = ("No puede eliminar al usuario actual");
                }
            }
            else
            {
                msg = "seleccione un registro de la tabla";
            }
            lbl_mensaje.Text = msg;
            timer1.Start();

        }

        private bool Verdedor_Es_Usuario_Actual()
        {
            return int.Parse(txt_codigo.Text) == ID_USUARIO_ACTUAL;
        }

        private void btn_reportes_Click(object sender, EventArgs e)
        {
            frm_rpt_vendedores frm = new frm_rpt_vendedores();
            // frm.MdiParent = this.MdiParent;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
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

        private void habilitar_textos(bool ok)
        {
            txt_codigo.Enabled = (false);
            txt_contrasena.Enabled = (ok);
            txt_sueldo.Enabled = (ok);
            txt_user.Enabled = (ok);
            txt_nombre.Enabled = (ok);
            txt_apellido.Enabled = (ok);
            txt_direccion.Enabled = (ok);
            cb_ciudad.Enabled = (ok);
            txt_email.Enabled = (ok);
            txt_cedula_ruc.Enabled = (ok);
            txt_telefono.Enabled = (ok);

        }

        private void limpiar_textos()
        {
            txt_codigo.Text = "";//
            txt_contrasena.Text = "";//
            txt_Buscar.Text = "";
            txt_sueldo.Text = "";//
            txt_user.Text = "";//
            txt_nombre.Text = "";
            txt_apellido.Text = "";
            txt_direccion.Text = "";
            cb_ciudad.Text = "";
            txt_email.Text = "";
            txt_cedula_ruc.Text = "";
            txt_telefono.Text = "";

        }
        private void CargarTabla()
        {
            try
            {
                var query = from v in db.vendedors
                            join p in db.personas on v.ven_codigo equals p.per_codigo
                            join c in db.ciudads on p.ciu_codigo equals c.ciu_codigo
                            select new
                            {
                                p.per_codigo,
                                p.per_nombre,
                                p.per_apellido,
                                p.per_ruc,
                                p.per_direccion,
                                p.per_telefono,
                                p.per_email,
                                v.ven_sueldo,
                                v.ven_user,
                                v.ven_password,
                                c.ciu_nombre
                            };
                dgv_vendedor.DataSource = query;
                habilitar(true);
                dgv_vendedor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al cargar la tabla");
            }

        }
        private bool GuardarRegistro()
        {
            string sms = "";
            bool ingreso_correcto = false;
            if (Existe_Persona_Cedula(txt_cedula_ruc.Text))
            {
                sms = ($"El usuario con la cedula {txt_cedula_ruc.Text} ya esta registrado");
                lbl_mensaje.Text = sms;
                timer1.Start();
            }
            else
            {
                try
                {
                    db = new databaseDataContext();
                    persona per = new persona();
                    per.per_codigo = int.Parse(txt_codigo.Text);
                    per.per_nombre = txt_nombre.Text;
                    per.per_apellido = txt_apellido.Text;
                    per.per_ruc = txt_cedula_ruc.Text;
                    per.per_direccion = txt_direccion.Text;
                    per.per_telefono = txt_telefono.Text;
                    per.ciu_codigo = buscarIdCiudad(cb_ciudad.SelectedItem.ToString());
                    per.per_email = txt_email.Text;
                    db.personas.InsertOnSubmit(per);//Insertar temporalmente

                    vendedor v = new vendedor();
                    v.ven_codigo = int.Parse(txt_codigo.Text);
                    v.ven_sueldo = Convert.ToDecimal(txt_sueldo.Text);
                    DateTime fecha = DateTime.Now.Date;
                    v.ven_fecha_ingreso = fecha;
                    v.ven_estado = false;
                    v.ven_user = txt_user.Text;
                    v.ven_password = txt_contrasena.Text;
                    db.vendedors.InsertOnSubmit(v);
                    db.SubmitChanges();//Commit 
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
            string sms = "";
            bool edicion_corecta = true;
            try
            {
                db = new databaseDataContext();
                persona per = db.personas.Single(persona => persona.per_codigo == int.Parse(txt_codigo.Text));
                per.per_nombre = txt_nombre.Text;
                per.per_apellido = txt_apellido.Text;
                per.per_ruc = txt_cedula_ruc.Text;
                per.per_direccion = txt_direccion.Text;
                per.per_telefono = txt_telefono.Text;
                per.ciu_codigo = buscarIdCiudad(cb_ciudad.SelectedItem.ToString());
                per.per_email = txt_email.Text;
                db.SubmitChanges();
                //guardar datos en la tabla vendedor
                vendedor ven = db.vendedors.Single(v => v.ven_codigo == int.Parse(txt_codigo.Text));
                //ven.ven_codigo = int.Parse(txt_codigo.Text);
                ven.ven_sueldo = Convert.ToDecimal(txt_sueldo.Text);
                DateTime fecha = DateTime.Now.Date;
                ven.ven_fecha_ingreso = fecha;
                ven.ven_estado = false;
                ven.ven_user = txt_user.Text;
                ven.ven_password = txt_contrasena.Text;
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

        private bool Existe_Persona_Cedula(string cedula)
        {
            try
            {
            var per = from p in db.personas
                      where p.per_ruc.Equals(cedula)
                      select p;
            return per.Count() > 0;
            }
            catch (System.Data.SqlClient.SqlException)
            {

                MessageBox.Show("Sucedio una exception al interactuar con la base de datos");
            }
            return false;

        }

        private int Obtener_Nuevo_Codigo_Persona()
        {
            int indice = 1;
            var numeros = from p in db.personas select p.per_codigo;

            if (numeros.Count() > 0)
            {

                foreach (int i in numeros)
                {
                    indice = i;
                }
                indice++;
            }
            return indice;
        }
        public int buscarIdCiudad(string dato)
        {
            var codigo = from d in db.ciudads
                         where d.ciu_nombre.Equals(dato)
                         select d.ciu_codigo;

            return codigo.First();
        }
        public string buscarNombreCiudad(int id)
        {
            var codigo = from d in db.ciudads
                         where d.ciu_codigo == id
                         select d.ciu_nombre;

            return codigo.First();
        }

        private void dgv_vendedor_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btn_anterior_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_vendedor.Rows.Count;
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
            txt_codigo.Text = dgv_vendedor.Rows[fila].Cells[0].Value.ToString();
            txt_nombre.Text = dgv_vendedor.Rows[fila].Cells[1].Value.ToString();
            txt_apellido.Text = dgv_vendedor.Rows[fila].Cells[2].Value.ToString();
            txt_cedula_ruc.Text = dgv_vendedor.Rows[fila].Cells[3].Value.ToString();
            txt_direccion.Text = dgv_vendedor.Rows[fila].Cells[4].Value.ToString();
            txt_telefono.Text = dgv_vendedor.Rows[fila].Cells[5].Value.ToString();
            txt_email.Text = dgv_vendedor.Rows[fila].Cells[6].Value.ToString();
            txt_sueldo.Text = dgv_vendedor.Rows[fila].Cells[7].Value.ToString();
            txt_user.Text = dgv_vendedor.Rows[fila].Cells[8].Value.ToString();
            txt_contrasena.Text = dgv_vendedor.Rows[fila].Cells[9].Value.ToString();
            cb_ciudad.SelectedItem = dgv_vendedor.Rows[fila].Cells[10].Value.ToString();
        }

        private void btn_siguiente_Click(object sender, EventArgs e)
        {
            try
            {
                TOTAL_FILAS = dgv_vendedor.Rows.Count;
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

        private void dgv_vendedor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txt_Buscar_TextChanged(object sender, EventArgs e)
        {
            Filtrar_Vendedor(txt_Buscar.Text);
        }

        private void Filtrar_Vendedor(string nombre)
        {
            var query = from v in db.vendedors
                        join p in db.personas on v.ven_codigo equals p.per_codigo
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
                            p.per_email,
                            v.ven_sueldo,
                            v.ven_user,
                            v.ven_password,
                            c.ciu_nombre
                        };
            dgv_vendedor.DataSource = query;
            habilitar(true);
        }

        private void dgv_vendedor_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                FILA_ACTUAL = dgv_vendedor.CurrentRow.Index;
                Recoger_Datos_De_Fila_Tabla(FILA_ACTUAL);
                ACCION = "editar";
            }
            catch (Exception)
            {

                MessageBox.Show("Sucedio un error al seleccionar el registro");
            }
        }
        public void PonerFocus_Caja_De_Texto(TextBox caja)
        {
            caja.Focus();
            caja.SelectionStart = caja.Text.Length;
        }
        private bool ValidarTexto()
        {
            bool texto_validado = false;
            if (string.IsNullOrEmpty(txt_codigo.Text)) { }
            // MessageBox.Show("No se ha seleccionado el codigo");
            else
            {
                if (!Expresiones_Regulares.Verificar_Decimal(txt_sueldo.Text.Replace(" ", "")) && !Expresiones_Regulares.Verificar_Numeros(txt_sueldo.Text.Replace(" ", "")))
                {
                    MessageBox.Show("El sueldo no se ha ingresado correctamente");
                    PonerFocus_Caja_De_Texto(txt_sueldo);
                }
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
                                                if (string.IsNullOrEmpty(txt_user.Text.Replace(" ", "")))
                                                {
                                                    MessageBox.Show("Ingrese un nombre de usuario");
                                                    PonerFocus_Caja_De_Texto(txt_user);
                                                }
                                                else
                                                {
                                                    if (string.IsNullOrEmpty(txt_contrasena.Text))
                                                    {
                                                        MessageBox.Show("Ingrese unacontraseña para el usuario");
                                                        PonerFocus_Caja_De_Texto(txt_contrasena);
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

            }

            return texto_validado;
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

        private void txt_cedula_ruc_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Ingreso_Numeros(e);
        }

        private void txt_nombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validaciones_Codigo_Ancii.Ingreso_Letras(e);
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

        private void txt_sueldo_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validaciones_Codigo_Ancii.Numeros_Decimales(e, txt_sueldo.Text);
        }

        private void txt_sueldo_TextChanged(object sender, EventArgs e)
        {
            if (txt_sueldo.Text == ",")
            {
                txt_sueldo.Text = "0,";
                // txt_Numeros_Decimales.Select(txt_Numeros_Decimales.Text.Length, 0);
                txt_sueldo.SelectionStart = txt_sueldo.Text.Length;

            }
        }

        private void txt_cedula_ruc_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
