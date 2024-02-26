using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace enciclopedia_canina_store.interfaces.reportes
{
    public partial class frm_rpt_factura : Form
    {
        databaseDataContext db = new databaseDataContext();
        int ID_FACTURA = 1;
        public frm_rpt_factura()
        {
            InitializeComponent();
        }
        public frm_rpt_factura(int id_factura)
        {
            InitializeComponent();
            ID_FACTURA = id_factura;
        }
        private void frm_rpt_factura_Load(object sender, EventArgs e)
        {
            /*
            int codigo = 22;
            var query = from fac in db.factura_venta
                        join cli in db.cliente on fac.cli_codigo equals cli.cli_codigo
                        join per in db.persona on cli.cli_codigo equals per.per_codigo
                        let Detalle_Factura = from fac_det in db.fac_ven_detalle
                                              join pro in db.producto on fac_det.pro_id equals pro.pro_id
                                              where fac_det.fac_codigo == codigo
                                              select new
                                              {
                                                  pro.pro_codigo,
                                                  pro.pro_nombre,
                                                  precio_unidad = fac_det.fv_puv.ToString(),
                                                  cantidad = fac_det.fv_cantidad.ToString(),
                                                  fac_det.fac_codigo,
                                                  precio_total = (fac_det.fv_puv * fac_det.fv_cantidad).ToString(),
                                              }
                        where fac.fac_codigo == codigo
                        select new
                        {
                            fac.fac_numero,
                            per_nombre = per.per_nombre + " " + per.per_apellido,
                            per.per_direccion,
                            per.per_telefono,
                            per.per_ruc,
                            total_pagar = fac.fac_total.ToString(),
                            Detalle_Factura
                        };
            foreach (var item in query)
            {
                foreach (var i in item.Detalle_Factura)
                {
                    MessageBox.Show(i.pro_nombre);
                }
            }

            */
            try
            {
                var query = from fac in db.factura_ventas
                            join cli in db.clientes on fac.cli_codigo equals cli.cli_codigo
                            join per in db.personas on cli.cli_codigo equals per.per_codigo

                            join fac_det in db.fac_ven_detalles on fac.fac_codigo equals fac_det.fac_codigo
                            join pro in db.productos on fac_det.pro_id equals pro.pro_id
                            where fac.fac_codigo == ID_FACTURA
                            select new
                            {
                                fac.fac_numero,
                                per_nombre = per.per_nombre + " " + per.per_apellido,
                                per.per_direccion,
                                per.per_telefono,
                                per.per_ruc,
                                pro.pro_codigo,
                                pro.pro_nombre,
                                //  precio_unidad= fac_det.fv_puv,cantidad=fac_det.fv_cantidad
                                precio_unidad = fac_det.fv_puv.ToString(),
                                cantidad = fac_det.fv_cantidad.ToString(),
                                precio_total = (fac_det.fv_puv * fac_det.fv_cantidad).ToString(),
                                total_pagar = fac.fac_total.ToString(),
                                fac_det.fac_ven_detalle1

                            };

                // MessageBox.Show(" Hay " + query.Count() + " resultados");
                rpt_FacturaVenta reporte = new rpt_FacturaVenta();
                reporte.SetDataSource(query);
                crystalReportViewer1.ReportSource = reporte;

            }
            catch (Exception)
            {
                MessageBox.Show("Sucedio un error al comunicarse con el servidor");
            }
        }
    }
}
