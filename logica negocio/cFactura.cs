using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enciclopedia_canina_store.logica_negocio
{
    class cFactura
    {
        databaseDataContext db = new databaseDataContext();
        public cFactura()
        {

        }
        /*--------------------NUEVAS FUNCIONES----------------------------*/
        public int Obtener_Nuevo_Codigo_Factura()
        {
            int Nuevo_Codigo = 1;
            var numeros = from fact in db.factura_ventas orderby fact.fac_codigo descending select fact.fac_codigo;
            if (numeros.Count() > 0)
            {
                Nuevo_Codigo = numeros.First() + 1;
            }
            return Nuevo_Codigo;
        }
    }
}
