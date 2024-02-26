using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enciclopedia_canina_store.logica_negocio
{
    class cCiudad
    {
        databaseDataContext db = new databaseDataContext();
        public cCiudad()
        {
        }
        public int Obtener_Nuevo_Codigo_Ciudad()
        {
            int Nuevo_Codigo = 1;
            var numeros = from c in db.ciudads orderby c.ciu_codigo descending  select c.ciu_codigo;
            if (numeros.Count() > 0)
            {
                Nuevo_Codigo = numeros.First() + 1;
            }
            return Nuevo_Codigo;
        }

        public Boolean Existe_Registro(string dato, int id)
        {//Verifica si existe un  registro y si ese registro es diferente al actual
            var f = from d in db.ciudads
                    where d.ciu_nombre.Equals(dato) && d.ciu_codigo != id
                    select d;
            return (f.Count() > 0);
        }
    }
}
