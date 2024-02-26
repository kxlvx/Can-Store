using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enciclopedia_canina_store.logica_negocio
{
    class cCategoria
    {
        databaseDataContext db = new databaseDataContext();
        ArrayList lista = new ArrayList();

        public cCategoria()
        {
        }

        public cCategoria(String dato)
        {
            var fab = from f in db.categorias
                      where f.cat_nombre.Contains(dato)
                      select new
                      {
                          f.cat_codigo,
                          f.cat_nombre
                      };
            foreach (var f in fab)
            {
                lista.Add(f);
            }
        }
        public ArrayList buscar()
        {
            return lista;
        }
        public Boolean Existe_Registro(string dato, int id)
        {//Verifica si existe un  registro y si ese registro es diferente al actual
            var f = from d in db.categorias
                    where d.cat_nombre.Equals(dato) && d.cat_codigo != id
                    select d;
            return (f.Count() > 0);
        }

        /*--------------NUEVAS FUNCIONES------------*/
        public int Obtener_Nuevo_Codigo_Categoria()
        {
            int Nuevo_Codigo = 1;
            var numeros = from p in db.categorias orderby p.cat_codigo descending select p.cat_codigo;

            if (numeros.Count() > 0)
            {
                Nuevo_Codigo = numeros.First() + 1;
            }
            return Nuevo_Codigo;
        }
    }
}
