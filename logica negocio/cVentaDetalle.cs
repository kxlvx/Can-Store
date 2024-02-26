using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enciclopedia_canina_store.logica_negocio
{
    class cVentaDetalle
    {
        databaseDataContext db = new databaseDataContext();
        public int Obtener_Nuevo_Codigo_Factura_Venta_Detalle()
        {
            int Nuevo_Codigo = 1;
            var numeros = from fact in db.fac_ven_detalles orderby fact.fac_ven_detalle1 descending select fact.fac_ven_detalle1;
            if (numeros.Count() > 0)
            {
                Nuevo_Codigo = numeros.First() + 1;
            }
            return Nuevo_Codigo;
        }
    }
}
