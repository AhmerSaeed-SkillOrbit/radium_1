namespace POSMainForm
{
    partial class frmReportDailySalesByStaff
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer2 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.dataSet2 = new POSMainForm.DataSet2();
            this.dataSet2BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsReportC = new POSMainForm.dsReportC();
            this.dataSet2BindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet2BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsReportC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet2BindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer2
            // 
            reportDataSource1.Name = "dsDRByInvoice";
            reportDataSource1.Value = this.dataSet2BindingSource1;
            this.reportViewer2.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer2.LocalReport.ReportEmbeddedResource = "POSMainForm.rptDailySalesReportByInvoiceNo.rdlc";
            this.reportViewer2.Location = new System.Drawing.Point(12, 8);
            this.reportViewer2.Name = "reportViewer2";
            this.reportViewer2.ServerReport.BearerToken = null;
            this.reportViewer2.Size = new System.Drawing.Size(422, 324);
            this.reportViewer2.TabIndex = 0;
            this.reportViewer2.Load += new System.EventHandler(this.reportViewer2_Load);
            // 
            // dataSet2
            // 
            this.dataSet2.DataSetName = "DataSet2";
            this.dataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataSet2BindingSource
            // 
            this.dataSet2BindingSource.DataSource = this.dataSet2;
            this.dataSet2BindingSource.Position = 0;
            // 
            // dsReportC
            // 
            this.dsReportC.DataSetName = "dsReportC";
            this.dsReportC.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataSet2BindingSource1
            // 
            this.dataSet2BindingSource1.DataSource = this.dataSet2;
            this.dataSet2BindingSource1.Position = 0;
            // 
            // frmReportDailySalesByStaff
            // 
            this.ClientSize = new System.Drawing.Size(437, 348);
            this.Controls.Add(this.reportViewer2);
            this.Name = "frmReportDailySalesByStaff";
            this.Load += new System.EventHandler(this.frmReportDailySalesByStaff_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet2BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsReportC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet2BindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource DailySalesByInvoiceBindingSource;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer2;
        private System.Windows.Forms.BindingSource dataSet2BindingSource;
        private DataSet2 dataSet2;
        private System.Windows.Forms.BindingSource dataSet2BindingSource1;
        private dsReportC dsReportC;
    }
}