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
    public partial class frm_rpt_productos : Form
    {
        databaseDataContext db = new databaseDataContext();
        public frm_rpt_productos()
        {
            InitializeComponent();
        }

        private void frm_rpt_productos_Load(object sender, EventArgs e)
        {
            var query = from p in db.productos
                        select new
                        { p.pro_id, p.pro_nombre, p.pro_descripcion, p.pro_codigo };


            rpt_Productos reporte = new rpt_Productos();
            reporte.SetDataSource(query);
            crystalReportViewer1.ReportSource = reporte;

        }
    }
}
