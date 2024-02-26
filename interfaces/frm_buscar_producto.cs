using enciclopedia_canina_store.logica_negocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace enciclopedia_canina_store.interfaces
{
    public partial class frm_buscar_producto : Form
    {

        public frm_facturacion formularioPadre;
        databaseDataContext db = new databaseDataContext();
        Boolean PRODUCTO_SELECCIONADO = false;
        String ID_PRODUCTO = "";
        String NOMBRE_PROD = "";
        String PRECIO_PROD = "";
        String CODIGO_PROD = "";
        String CANTIDAD_PROD = "";


        public frm_buscar_producto(frm_facturacion form)
        {
            InitializeComponent();
            formularioPadre = form;
        }
        cProducto lis;
        void CargarTabla()
        {
            lis = new cProducto(txt_Buscar.Text);
           
            dgv_productos.DataSource = lis.buscar();
            dgv_productos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void frm_buscar_producto_Load(object sender, EventArgs e)
        {
            CargarTabla();
        }

        private void dgv_productos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            try
            {
                DataGridViewRow fila = dgv_productos.CurrentRow;
                ID_PRODUCTO = fila.Cells[0].Value.ToString();
                NOMBRE_PROD = fila.Cells[1].Value.ToString();
                PRECIO_PROD = fila.Cells[5].Value.ToString();
                CODIGO_PROD = fila.Cells[8].Value.ToString();
                CANTIDAD_PROD = fila.Cells[3].Value.ToString();
                txt_cantidad.BackColor = Color.OrangeRed;
                PRODUCTO_SELECCIONADO = true;
                txt_cantidad.Focus();
                lbl_producto.Text = "Producto: " + NOMBRE_PROD;
                timer1.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio una excepcion al seleccionar el producto");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txt_cantidad.BackColor = Color.White;
            lbl_mensaje.Text = "";
            timer1.Stop();
        }
        private void txt_cantidad_KeyPress(object sender, KeyPressEventArgs tecla)
        {

            try
            {
                if (!(char.IsNumber(tecla.KeyChar)) && (tecla.KeyChar != (char)Keys.Back)
                           && (tecla.KeyChar != (char)Keys.Enter) && (tecla.KeyChar != (char)Keys.Return)
                           && (tecla.KeyChar != (char)Keys.Delete))
                {
                    MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tecla.Handled = true;
                    return;
                }
                else
                {
                    if (tecla.KeyChar == (int)Keys.Enter)
                    {

                        if (PRODUCTO_SELECCIONADO)
                        {
                            if (int.Parse(txt_cantidad.Text.Trim()) <= int.Parse(CANTIDAD_PROD))//Solo se agrega a latabla si la cantidad seleccionada es menor al stock actual
                            {
                                if ((formularioPadre.dgv_product_fact.Rows.Count == 1))
                                {
                                    try
                                    { //Se calcaula el total 
                                        double total = int.Parse(txt_cantidad.Text.Trim()) * Double.Parse(PRECIO_PROD);
                                 
                                        //En el formulario padre se agrega una fila con los datos seleccionados
                                        formularioPadre.dgv_product_fact.Rows.Add(ID_PRODUCTO, CODIGO_PROD, NOMBRE_PROD, PRECIO_PROD, txt_cantidad.Text.Trim(), total);
                                      
                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show("Sucedio un error al seleccionar el producto");
                                    }
                                }
                                else
                                {
                                    Boolean existe = false;
                                   
                                    for (int fila = 0; fila < formularioPadre.dgv_product_fact.Rows.Count - 1; fila++)
                                    {
                                        if (formularioPadre.dgv_product_fact.Rows[fila].Cells[0].Value.ToString().Trim() == ID_PRODUCTO.Trim())
                                        {
                                            double total = int.Parse(txt_cantidad.Text) * double.Parse(PRECIO_PROD);
                                            formularioPadre.dgv_product_fact.Rows[fila].Cells[3].Value =PRECIO_PROD;
                                            formularioPadre.dgv_product_fact.Rows[fila].Cells[4].Value = txt_cantidad.Text;
                                            formularioPadre.dgv_product_fact.Rows[fila].Cells[5].Value = total.ToString();;
                                            //linea de codigo para salir del ciclo FOR
                                            fila = formularioPadre.dgv_product_fact.Rows.Count + 1;
                                            existe = true;
                                        }
                                    }
                                    if (!existe)
                                    {
                                        double tot = int.Parse(txt_cantidad.Text) * Double.Parse(PRECIO_PROD);                                 
                                        formularioPadre.dgv_product_fact.Rows.Add(ID_PRODUCTO, CODIGO_PROD, NOMBRE_PROD, PRECIO_PROD, txt_cantidad.Text, tot);
                                    }                                  
                                }
                                Agregar_Subtotal_A_Formulario_Padre();
                                MessageBox.Show("Producto agregado a la lista de compras");
                            }
                            else
                            {
                                MessageBox.Show($"Solo se puede seleccionar {CANTIDAD_PROD} productos");
                                txt_cantidad.Focus();
                                txt_cantidad.SelectionStart = txt_cantidad.TextLength;
                            }

                        }
                        else
                        {
                            lbl_mensaje.Text = "Seleccione un producto";
                            timer1.Start();
                        }
                        
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio una excepcion al seleccionar la cantidad");
            }


        }

        private void Agregar_Subtotal_A_Formulario_Padre()
        {
            double subtotal = 0;
            int total_filas= formularioPadre.dgv_product_fact.Rows.Count - 1;
            for (int i = 0; i < total_filas; i++)
            {
               subtotal+= double.Parse(formularioPadre.dgv_product_fact.Rows[i].Cells[5].Value.ToString());
            }
            formularioPadre.txt_subtotal.Text = "" + subtotal;         
        }

        private void iconminimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void txt_Buscar_TextChanged(object sender, EventArgs e)
        {
            CargarTabla();
        }

        private void txt_cantidad_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbl_producto_Click(object sender, EventArgs e)
        {

        }

        private void lbl_mensaje_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void dgv_productos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
