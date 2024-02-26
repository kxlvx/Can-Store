using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace enciclopedia_canina_store.logica_negocio
{
    class Validaciones_Entrada_De_Texto
    {
        /*
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
        }*/
        public static void Ingreso_Solo_Letras(KeyPressEventArgs tecla)
        {
            if (!(char.IsLetter(tecla.KeyChar)) && (tecla.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tecla.Handled = true;
                return;
            }
        }
    }
}
