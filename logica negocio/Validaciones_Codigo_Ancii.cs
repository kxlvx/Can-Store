using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace enciclopedia_canina_store.logica_negocio
{
    class Validaciones_Codigo_Ancii
    {
        public static void Ingreso_Numeros(KeyPressEventArgs tecla)
        {
            int letra = tecla.KeyChar;
            if ((letra >= 32 && letra <= 47) || (letra >= 58 && letra <= 255))
            {
                MessageBox.Show("Solo se permiten numeros");
                tecla.Handled = true;
            }

        }
        public static void Numeros_Decimales(KeyPressEventArgs tecla, string cadena)
        {
            // int punto = 46;
            int coma = ',';
            int letra = tecla.KeyChar;
            if ((letra >= 32 && letra <= 43) || (letra >= 45 && letra <= 47) || (letra >= 58 && letra <= 255))
            {
                MessageBox.Show("Solo se permiten numeros en formato decimal, Ejemplo: 12,50");
                tecla.Handled = true;//controla = verdadero
            }
            else
            {
                if (cadena.Contains(",") && letra == coma)
                {
                    MessageBox.Show("Un numero no puede tener mas de dos comas");
                    tecla.Handled = true;
                }
            }
        }
    }
}
