using System;
using System.Linq;

namespace enciclopedia_canina_store.logica_negocio
{
    class cVendedor
    {
        databaseDataContext db = new databaseDataContext();

        public cVendedor()
        {

        }
        public Boolean existe(string dato)
        {
            Boolean existe = false;
            if (!dato.Equals(""))
            {
                var f = from d in db.vendedors
                        where d.ven_codigo == int.Parse(dato)
                        select new
                        {
                            d.ven_codigo
                        };
                foreach (var p in f)
                {
                    existe = true;
                }
            }
            return existe;
        }

    }
}
