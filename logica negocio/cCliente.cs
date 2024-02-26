using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enciclopedia_canina_store.logica_negocio
{
    class cCliente
    {
        databaseDataContext db = new databaseDataContext();
        ArrayList lista = new ArrayList();
        public cCliente(string dato)
        {
            var pro = from p in db.personas
                      from ciu in db.ciudads
                      from cli in db.clientes
                      where p.ciu_codigo == ciu.ciu_codigo
                      & cli.cli_codigo == p.per_codigo
                      & (p.per_ruc.Contains(dato)||p.per_apellido.Contains(dato))
                      select new
                      {
                          p.per_codigo,
                          p.per_nombre,
                          p.per_apellido,
                          p.per_ruc,
                          p.per_direccion,
                          p.per_telefono,
                          ciu.ciu_nombre,
                          p.per_email,
                          cli.cli_tipo,
                          cli.cli_saldo
                      };
            foreach (var f in pro)
            {
                lista.Add(f);
            }
        }

        public cCliente()
        {
        }

        public ArrayList buscar()
        {
            return lista;
        }
        public Boolean Existe_Persona(string dato)
        {
            var f = from d in db.personas
                    where d.per_ruc.Equals(dato) select d;
            return f.Count() > 0;

        }
        public ArrayList buscarCliente(string dato)
        {
            ArrayList cli = new ArrayList();
            var f = from d in db.personas
                    from ciu in db.ciudads
                    where d.ciu_codigo==ciu.ciu_codigo 
                    & d.per_ruc.Equals(dato)
                    select new
                    {
                        d.per_codigo,
                        d.per_nombre,
                        d.per_apellido,
                        d.per_direccion,
                        ciu.ciu_nombre,
                        d.per_telefono,
                        d.per_email
                    };
            foreach (var p in f)
            {
                cli.Add(p);
            }
            return cli;
        }
        public int buscarIdCiudad(string dato)
        {
            int cod = 0;
            var da = from d in db.ciudads
                     where d.ciu_nombre.Equals(dato)
                     select new
                     {
                         d.ciu_codigo
                     };
            foreach (var ca in da)
            {
                cod = ca.ciu_codigo;
            }
            return cod;
        }
        /*--------------------NUEVAS FUNCIONES----------------------------*/
        public int Obtener_Nuevo_Codigo_Cliente()
        {
            int Nuevo_Codigo = 1;
            var numeros = from p in db.personas orderby p.per_codigo descending select p.per_codigo;
            if (numeros.Count() > 0)
            {
                Nuevo_Codigo = numeros.First() + 1;
            }
            return Nuevo_Codigo;
        }
        public bool Existe_Persona_Cedula(string cedula)
        {
            var per = from p in db.personas
                      where p.per_ruc.Equals(cedula)
                      select p;
            return per.Count() > 0;

        }
    }
}
