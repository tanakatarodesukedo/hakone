using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    /// <summary>
    /// 履歴表示画面
    /// </summary>
    public partial class PlayerHistoryForm : Form
    {
        /// <summary>
        /// 大学コード
        /// </summary>
        public string UnivCode { get; set; }

        /// <summary>
        /// 学内連番
        /// </summary>
        public int InternalNo { get; set; }

        /// <summary>
        /// 選手名
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// DB接続文字列
        /// </summary>
        private string connStr = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayerHistoryForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PlayerHistoryForm_Load(object sender, EventArgs e)
        {
            lblPlayerName.Text = PlayerName + " の記録推移";

            LoadHistory();
        }

        /// <summary>
        /// 履歴読み込み
        /// </summary>
        private void LoadHistory()
        {
            string sql = @"
                SELECT 
                    RECORD_DATE,
                    BEST_10K,
                    BEST_HALF
                FROM 
                    PLAYER_INFO_HISTORY
                WHERE 
                    UNIV_CODE = :univCode
                    AND INTERNAL_NO = :internalNo
                ORDER BY 
                    RECORD_DATE
            ";

            using (var conn = new OracleConnection(connStr))
            {
                conn.Open();
                var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":univCode", UnivCode);
                cmd.Parameters.Add(":internalNo", InternalNo);

                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                dgvHistory.DataSource = dt;

                dgvHistory.Columns["RECORD_DATE"].HeaderText = "日時";
                dgvHistory.Columns["BEST_10K"].HeaderText = "1万m";
                dgvHistory.Columns["BEST_HALF"].HeaderText = "ハーフ";

                DrawChart(dt);
            }
        }

        /// <summary>
        /// グラフ描画
        /// </summary>
        /// <param name="dt">dt</param>
        private void DrawChart(DataTable dt)
        {
            chart1.Series.Clear();

            var s10k = new Series("1万m");
            s10k.ChartType = SeriesChartType.Line;
            s10k.MarkerStyle = MarkerStyle.Circle;

            var sHalf = new Series("ハーフ");
            sHalf.ChartType = SeriesChartType.Line;
            sHalf.MarkerStyle = MarkerStyle.Circle;

            foreach (DataRow row in dt.Rows)
            {
                var date = Convert.ToDateTime(row["RECORD_DATE"]);

                // --- 1万m mm:ss.xx ---
                double sec10k = Parse10k(row["BEST_10K"].ToString());
                s10k.Points.AddXY(date, sec10k);

                // --- ハーフ hh:mm:ss ---
                double secHalf = ParseHalf(row["BEST_HALF"].ToString());
                sHalf.Points.AddXY(date, secHalf);
            }

            chart1.Series.Add(s10k);
            chart1.Series.Add(sHalf);

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy/MM/dd";
            chart1.ChartAreas[0].AxisY.Title = "秒";
        }

        /// <summary>
        /// 1万mタイムを秒に変換
        /// </summary>
        /// <param name="time">タイム</param>
        /// <returns>秒</returns>
        private double Parse10k(string time)
        {
            if (string.IsNullOrWhiteSpace(time)) return double.NaN;

            var parts = time.Split(':', '.'); // mm, ss, xx
            int mm = int.Parse(parts[0]);
            int ss = int.Parse(parts[1]);
            int xx = int.Parse(parts[2]);

            return mm * 60 + ss + xx / 100.0;
        }

        /// <summary>
        /// ハーフタイムを秒に変換
        /// </summary>
        /// <param name="time">タイム</param>
        /// <returns>秒</returns>
        private double ParseHalf(string time)
        {
            if (string.IsNullOrWhiteSpace(time)) return double.NaN;

            var parts = time.Split(':'); // hh, mm, ss
            int hh = int.Parse(parts[0]);
            int mm = int.Parse(parts[1]);
            int ss = int.Parse(parts[2]);

            return hh * 3600 + mm * 60 + ss;
        }
    }
}

