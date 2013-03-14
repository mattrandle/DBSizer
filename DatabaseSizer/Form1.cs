using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseSizer.Helpers;
using DatabaseSizer.Sizer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.MessageBox;
using log4net.Config;

namespace DatabaseSizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
            XmlConfigurator.Configure();

            this.cbAuthenticationType.SelectedIndex = 0;
        }

        private void btnProduceSpreadsheet_Click(object sender, EventArgs e)
        {
            Database db = null;
            SqlConnectionDetails connectionInfo = null;
            this.TryCatch(() =>
                {
                    connectionInfo = this.GetSqlConnectionDetails();
                    db = GetSMODatabase(connectionInfo);
                },
                          ex =>
                              {
                                  var box = new ExceptionMessageBox(ex);
                                  box.Show(this);
                              });

            if (db != null)
                this.CreateSpreadsheet(connectionInfo, db);
        }

        private void CreateSpreadsheet(SqlConnectionDetails connectionInfo, Database db)
        {
            db.Tables.Refresh();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var progressForm = CreateProgressForm(db, cancellationTokenSource);

            var progressReporter = new ProgressReporter();
            Action progressAction = () => progressReporter.ReportProgress(() => progressForm.progressBar1.PerformStep());

            var colLenService = this.CreateColLengthService(connectionInfo);

            Task.Factory.
                 StartNew(() =>
                     {
                         var si = new SizingInfoFromDmo(colLenService);
                         var sizingData = si.CreateSqlDatabaseFromSMO(db, progressAction, cancellationToken);

                         var ssc = new SpreadSheetCreator();
                         ssc.CreateSpreadSheet(sizingData, progressAction, cancellationToken);
                     }).
                 ContinueWith(task =>
                     {
                         progressReporter.ReportProgress(progressForm.Close);
                         if (task.IsFaulted)
                         {
                             progressReporter.ReportProgress(() =>
                                 {
                                     if (task.Exception != null)
                                     {
                                         MessageBox.Show(task.Exception.InnerException.ToString(),
                                                         "Failed creating spreadsheet");
                                     }
                                     else
                                     {
                                         MessageBox.Show("Failed creating spreadsheet");
                                     }
                                 });
                         }
                     });

            progressForm.ShowDialog(this);
        }

        private IColumnLengthService CreateColLengthService(SqlConnectionDetails connectionInfo)
        {
            IColumnLengthService colLenService;
            if (this.rbAverages.Checked)
                colLenService = new ColumnLengthServiceActual(connectionInfo.ConnectionString);
            else
                colLenService = new ColumnLengthService100();
            return colLenService;
        }

        private static Progress CreateProgressForm(Database db, CancellationTokenSource cancellationTokenSource)
        {
            var progressForm = new Progress
                {
                    progressBar1 =
                        {
                            Step = 1, 
                            Maximum = (db.Tables.Count*2)
                        }
                };
            progressForm.button1.Click += (src, ev) => cancellationTokenSource.Cancel();
            return progressForm;
        }

        private static Database GetSMODatabase(SqlConnectionDetails connectionInfo)
        {
            ServerConnection serverConnection;
            if (connectionInfo.UseSqlAuthentication != null && !connectionInfo.UseSqlAuthentication.Value)
            {
                serverConnection = new ServerConnection(connectionInfo.ServerName);
            }
            else
            {
                serverConnection = new ServerConnection(connectionInfo.ServerName, connectionInfo.Username,
                                                        connectionInfo.Password);
            }

            var sqlServer = new Server(serverConnection);
            var db = new Database(sqlServer, connectionInfo.DefaultDatabase);

            return db;
        }

        private SqlConnectionDetails GetSqlConnectionDetails()
        {
            SqlConnectionDetails sqlConnectionInfo;
            if (this.cbAuthenticationType.SelectedIndex == 0)
            {
                sqlConnectionInfo = new SqlConnectionDetails(this.edServerName.Text, this.edDatabaseName.Text);
            }
            else
            {
                sqlConnectionInfo = new SqlConnectionDetails(this.edServerName.Text, this.edUsername.Text,
                                                             this.edPassword.Text, this.edDatabaseName.Text);
            }

            return sqlConnectionInfo;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbAuthenticationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbAuthenticationType.SelectedIndex == 0)
            {
                this.edUsername.Enabled = false;
                this.edPassword.Enabled = false;
            }
            else
            {
                this.edUsername.Enabled = true;
                this.edPassword.Enabled = true;
            }
        }
    }
}