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
    public partial class frm_rpt_ciudad : Form
    {
        databaseDataContext db = new databaseDataContext();
        public frm_rpt_ciudad()
        {
            InitializeComponent();
        }

        private void frm_rpt_ciudad_Load(object sender, EventArgs e)
        {
            var query = from c in db.ciudads select c;
            rpt_Ciudades reporte = new rpt_Ciudades();
            reporte.SetDataSource(query);
            crystalReportViewer1.ReportSource = reporte;

        }
    }
}
