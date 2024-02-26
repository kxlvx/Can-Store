using System.Text.RegularExpressions;

namespace enciclopedia_canina_store.logica_negocio
{
    class Expresiones_Regulares
    {
        public static bool Verificar_Decimal(string cadena)
        {
            Regex patron = new Regex("^[0-9]+[,][0-9]+$");
            return patron.IsMatch(cadena);
        }
        public static bool Verificar_Numeros(string cadena)
        {
            Regex patron = new Regex("^[0-9]+$");
            return patron.IsMatch(cadena);
        }
        public static bool Verificar_Letras(string cadena)
        {
            Regex patron = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚ]+$");
            return patron.IsMatch(cadena);
        }
        public static bool Contiene_Etiqueta_Html(string cadena)
        {
            Regex patron = new Regex(@"\<[^\>]+\>");
            return patron.IsMatch(cadena);
        }
        public static bool Verificar_Cedula(string cadena)
        {
            Regex patron = new Regex(@"^[0-9]{10}$");
            return patron.IsMatch(cadena);
        }
        public static bool Verificar_Telefono(string cadena)
        {
            Regex patron = new Regex(@"^[0-9]{7,10}$");
            return patron.IsMatch(cadena);
        }
        public static bool Verificar_Correo(string palabra)
        {
            /*Obtenido de https://www.w3.org/TR/html5/forms.html#valid-e-mail-address  */
            Regex patron = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
            return patron.IsMatch(palabra);
        }
    }
}





