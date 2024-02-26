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
    public partial class frm_rpt_categoria : Form
    {
        databaseDataContext db = new databaseDataContext();
        public frm_rpt_categoria()
        {
            InitializeComponent();
        }

        private void frm_rpt_categoria_Load(object sender, EventArgs e)
        {
            var query = from c in db.categorias select c;
            rpt_Categorias reporte = new rpt_Categorias();
            reporte.SetDataSource(query);
            crystalReportViewer1.ReportSource = reporte;

        }
    }
}
