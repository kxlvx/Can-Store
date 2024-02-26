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
    public partial class frm_rpt_fabricantes : Form
    {
        databaseDataContext db = new databaseDataContext();
        public frm_rpt_fabricantes()
        {
            InitializeComponent();
        }

        private void frm_rpt_fabricantes_Load(object sender, EventArgs e)
        {
            var query = from f in db.fabricantes select f;
            rpt_Fabricante reporte = new rpt_Fabricante();
            reporte.SetDataSource(query);
            crystalReportViewer1.ReportSource = reporte;
        }
    }
}
