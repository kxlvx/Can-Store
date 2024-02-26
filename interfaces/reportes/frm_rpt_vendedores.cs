using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace enciclopedia_canina_store.interfaces.reportes
{
    public partial class frm_rpt_vendedores : Form
    {
        public frm_rpt_vendedores()
        {
            InitializeComponent();
        }

        private void frm_rpt_vendedores_Load(object sender, EventArgs e)
        {
            databaseDataContext db = new databaseDataContext();
            var query = from v in db.vendedors
                        join p in db.personas on v.ven_codigo equals p.per_codigo
                        orderby p.per_nombre
                        select new
                        {
                            p.per_ruc,
                           per_nombre=p.per_nombre+" "+p.per_apellido,
                            p.per_direccion,
                            p.per_telefono,
                            p.per_email,
                            v.ven_user
                        };
            rpt_Vendedores reporte = new rpt_Vendedores();
            reporte.SetDataSource(query);
            crystalReportViewer1.ReportSource = reporte;

        }
    }
}
