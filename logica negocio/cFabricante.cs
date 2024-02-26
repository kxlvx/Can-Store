using System;
using System.Collections;
using System.Linq;

namespace enciclopedia_canina_store.logica_negocio
{
    class cFabricante
    {
        databaseDataContext db = new databaseDataContext();
       // ArrayList lista = new ArrayList();
        public cFabricante()
        {

        }
/*
        public cFabricante(string dato)
        {
            var fab = from f in db.fabricantes
                      where f.fab_nombre.Contains(dato)
                      select new
                      {
                          f.fab_codigo,
                          f.fab_nombre
                      };
            foreach (var f in fab)
            {
                lista.Add(f);
            }
        }
        public ArrayList Obtener_Lista()
        {
            return lista;
        }
        */
        public Boolean Existe_Registro(string dato)
        {
            Boolean existe = false;
            var f = from d in db.fabricantes
                    where d.fab_nombre.Equals(dato)
                    select new
                    {
                        d.fab_codigo
                    };
            foreach (var p in f)
            {
                existe = true;
            }
            return existe;
        }
        /*-----------NUEVAS FUNCIONES ----------------*/
        public int Obtener_Nuevo_Codigo_Fabricante()
        {
            int Nuevo_Codigo = 1;
            var numeros = from p in db.fabricantes orderby p.fab_codigo descending select p.fab_codigo;

            if (numeros.Count() > 0)
            {
                Nuevo_Codigo = numeros.First() + 1;
            }
            return Nuevo_Codigo;
        }

        public Boolean Existe_Registro(string dato, int id)
        {//Verifica si existe un  registro y si ese registro es diferente al actual
            var f = from d in db.fabricantes
                    where d.fab_nombre.Equals(dato) && d.fab_codigo != id
                    select d;
            return (f.Count() > 0);
        }
    }
}
