using System;
using System.Collections;
using System.Linq;

namespace enciclopedia_canina_store.logica_negocio
{
    public class cProducto
    {
        databaseDataContext db = new databaseDataContext();
        ArrayList lista = new ArrayList();
        public cProducto(string dato)
        {
            var pro = from f in db.productos
                      from c in db.categorias
                      from fab in db.fabricantes
                      where f.cat_codigo == c.cat_codigo
                      & f.fab_codigo == fab.fab_codigo &
                      f.pro_nombre.Contains("" + dato)
                      select new
                      {
                          f.pro_id,
                          f.pro_nombre,
                          f.pro_descripcion,
                          f.pro_cantidad,
                          f.pro_precio_compra,
                          f.pro_precio_venta,
                          c.cat_nombre,
                          fab.fab_nombre,
                          f.pro_codigo,
                          f.pro_desc_porcen

                      };
            foreach (var f in pro)
            {
                lista.Add(f);
            }
        }
        public ArrayList buscar()
        {
            return lista;
        }
        public Boolean Verificar_Producto_Existe(string nombre, int id)
        {

            var resultado = from d in db.productos
                            where d.pro_nombre.Equals(nombre) && d.pro_id != id
                            select d.pro_id;
            return resultado.Count() > 0;
        }
        public int buscarIdCat(string dato)
        {
            int cod = 0;
            var da = from d in db.categorias
                     where d.cat_nombre.Equals(dato)
                     select new
                     {
                         d.cat_codigo
                     };
            foreach (var ca in da)
            {
                cod = ca.cat_codigo;
            }
            return cod;
        }
        public int buscarIdFab(string dato)
        {
            int cod = 0;
            var da = from d in db.fabricantes
                     where d.fab_nombre.Equals(dato)
                     select new
                     {
                         d.fab_codigo
                     };
            foreach (var ca in da)
            {
                cod = ca.fab_codigo;
            }
            return cod;
        }
        /*-------------NUEVOS METODOS---------------------*/
        public int Obtener_Nuevo_Codigo_Producto()
        {
            int Nuevo_Codigo = 1;
            var numeros = from p in db.productos orderby p.pro_id descending select p.pro_id;

            if (numeros.Count() > 0)
            {
                Nuevo_Codigo = numeros.First() + 1;
            }
            return Nuevo_Codigo;
        }
    }

}
