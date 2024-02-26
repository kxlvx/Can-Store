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
    public partial class frm_rpt_clientes : Form
    {
        databaseDataContext db = new databaseDataContext();
        public frm_rpt_clientes()
        {
            InitializeComponent();
        }

        private void frm_rpt_clientes_Load(object sender, EventArgs e)
        {

            var query = from c in db.clientes
                        join p in db.personas on c.cli_codigo equals p.per_codigo
                        join ciu in db.ciudads on p.ciu_codigo equals ciu.ciu_codigo
                        orderby p.per_nombre
                        select new
                        {
                            p.per_ruc,
                            p.per_nombre,
                            p.per_apellido,
                            p.per_direccion,
                            p.per_telefono,
                            p.per_email,
                            c.cli_tipo,
                            c.cli_saldo
                        };
            rpt_Clientes reporte = new rpt_Clientes();
            reporte.SetDataSource(query);
            crystalReportViewer1.ReportSource = reporte;


        }
    }
}
