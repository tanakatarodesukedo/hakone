using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    /// <summary>
    /// 選手一覧画面
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// DB接続文字列
        /// </summary>
        private readonly string connStr = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        /// <summary>
        /// 1万m正規表現
        /// </summary>
        private readonly string regex10k = @"^(?:[0-5]?\d):[0-5]\d\.\d{2}$";

        /// <summary>
        /// ハーフ正規表現
        /// </summary>
        private readonly string regexHalf = @"^(?:[0-3]):[0-5]\d:[0-5]\d$";

        // BackgroundWorkerを使うために宣言
        private BackgroundWorker backgroundWorker;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // BackgroundWorkerの設定
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true; // プログレス報告を有効にする
            backgroundWorker.WorkerSupportsCancellation = true; // キャンセルをサポートする

            // イベントハンドラの登録
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            SearchPlayers();

            using (var conn = new OracleConnection(connStr))
            {
                conn.Open();
                var cmd = new OracleCommand(
                    "SELECT UNIV_CODE, UNIV_NAME FROM UNIVERSITY_MST ORDER BY UNIV_CODE", conn);

                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                cmbUniversity.DataSource = dt;
                cmbUniversity.DisplayMember = "UNIV_NAME";
                cmbUniversity.ValueMember = "UNIV_CODE";
                cmbUniversity.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// 検索ボタン押下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // プログレスバーをリセット
            progressBar1.Value = 0;
            labelStatus.Text = "処理開始中...";

            // BackgroundWorkerの実行
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync(); // バックグラウンドで処理開始
            }

            SearchPlayers();
        }

        // バックグラウンドで処理を行う
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                // キャンセルされた場合に処理を中断
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                // プログレスの更新
                backgroundWorker.ReportProgress(i);

                // 進行状況に応じて遅延を追加（例: 50ミリ秒）
                Thread.Sleep(50);
            }
        }

        // プログレスバーを更新
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage; // プログレスバーに進行状況を反映
            labelStatus.Text = $"処理中... {e.ProgressPercentage}%"; // ラベルに進行状況を表示
        }

        // 処理が完了したときの処理
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                labelStatus.Text = "処理がキャンセルされました。";
            }
            else if (e.Error != null)
            {
                labelStatus.Text = "エラーが発生しました: " + e.Error.Message;
            }
            else
            {
                labelStatus.Text = "処理が完了しました！";
            }
        }

        /// <summary>
        /// 選手情報検索
        /// </summary>
        private void SearchPlayers()
        {
            if (!ValidateTimeInputs())
            {
                return;
            }

            using (var conn = new OracleConnection(connStr))
            {
                conn.Open();

                string sql = $@"
                    SELECT 
                        p.PLAYER_NAME,
                        p.BEST_10K,
                        p.BEST_HALF,
                        u.UNIV_NAME,
                        p.UNIV_CODE,
                        p.INTERNAL_NO
                    FROM
                        PLAYER_INFO p
                        INNER JOIN UNIVERSITY_MST u ON p.UNIV_CODE = u.UNIV_CODE
                    WHERE
                        1=1
                        {(cmbUniversity.SelectedIndex == -1 ? string.Empty : "AND p.UNIV_CODE   = :univCode")}
                        {(string.IsNullOrWhiteSpace(txtPlayerName.Text) ? string.Empty : "AND p.PLAYER_NAME LIKE '%' || :playerName || '%'")}
                        {(IsMaskedTimeEmpty(mtbBest10kFrom) ? string.Empty : "AND p.BEST_10K >= :best10kFrom")}
                        {(IsMaskedTimeEmpty(mtbBest10kTo) ? string.Empty : "AND p.BEST_10K <= :best10kTo")}
                        {(IsMaskedTimeEmpty(mtbBestHalfFrom) ? string.Empty : "AND p.BEST_HALF >= :bestHalfFrom")}
                        {(IsMaskedTimeEmpty(mtbBestHalfTo) ? string.Empty : "AND p.BEST_HALF <= :bestHalfTo")}
                    ORDER BY
                        TO_NUMBER(SUBSTR(p.BEST_10K,1,2)) * 60
                            + TO_NUMBER(SUBSTR(p.BEST_10K,4,2))
                            + TO_NUMBER(SUBSTR(p.BEST_10K,7,2)) / 100";

                var cmd = new OracleCommand(sql, conn);

                if (cmbUniversity.SelectedIndex > -1)
                {
                    cmd.Parameters.Add(new OracleParameter(":univCode",
                        cmbUniversity.SelectedIndex == -1 ? DBNull.Value : cmbUniversity.SelectedValue));
                }

                if (!string.IsNullOrWhiteSpace(txtPlayerName.Text))
                {
                    cmd.Parameters.Add(new OracleParameter(":playerName",
                        string.IsNullOrWhiteSpace(txtPlayerName.Text) ? (object)DBNull.Value : txtPlayerName.Text));
                }

                if (!IsMaskedTimeEmpty(mtbBest10kFrom))
                {
                    cmd.Parameters.Add(new OracleParameter(":best10kFrom",
                        string.IsNullOrWhiteSpace(mtbBest10kFrom.Text) ? (object)DBNull.Value : mtbBest10kFrom.Text));
                }

                if (!IsMaskedTimeEmpty(mtbBest10kTo))
                {
                    cmd.Parameters.Add(new OracleParameter(":best10kTo",
                        string.IsNullOrWhiteSpace(mtbBest10kTo.Text) ? (object)DBNull.Value : mtbBest10kTo.Text));
                }

                if (!IsMaskedTimeEmpty(mtbBestHalfFrom))
                {
                    cmd.Parameters.Add(new OracleParameter(":bestHalfFrom",
                        string.IsNullOrWhiteSpace(mtbBestHalfFrom.Text) ? (object)DBNull.Value : mtbBestHalfFrom.Text));
                }

                if (!IsMaskedTimeEmpty(mtbBestHalfTo))
                {
                    cmd.Parameters.Add(new OracleParameter(":bestHalfTo",
                        string.IsNullOrWhiteSpace(mtbBestHalfTo.Text) ? (object)DBNull.Value : mtbBestHalfTo.Text));
                }

                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                dgvPlayers.DataSource = dt;

                dgvPlayers.Columns["PLAYER_NAME"].HeaderText = "選手名";
                dgvPlayers.Columns["BEST_10K"].HeaderText = "1万m";
                dgvPlayers.Columns["BEST_HALF"].HeaderText = "ハーフ";
                dgvPlayers.Columns["UNIV_NAME"].HeaderText = "大学名";

                dgvPlayers.Columns["UNIV_CODE"].Visible = false;
                dgvPlayers.Columns["INTERNAL_NO"].Visible = false;
            }
        }

        /// <summary>
        /// タイムの入力値検証
        /// </summary>
        /// <returns>入力値が正常かどうか</returns>
        private bool ValidateTimeInputs()
        {
            if (!IsMaskedTimeEmpty(mtbBest10kFrom) && !Regex.IsMatch(mtbBest10kFrom.Text, regex10k))
            {
                MessageBox.Show("1万m(From)のタイムが不正です（mm:ss.xx）。");
                mtbBest10kFrom.Focus();
                return false;
            }

            if (!IsMaskedTimeEmpty(mtbBest10kTo) && !Regex.IsMatch(mtbBest10kTo.Text, regex10k))
            {
                MessageBox.Show("1万m(To)のタイムが不正です（mm:ss.xx）。");
                mtbBest10kTo.Focus();
                return false;
            }

            if (!IsMaskedTimeEmpty(mtbBestHalfFrom) && !Regex.IsMatch(mtbBestHalfFrom.Text, regexHalf))
            {
                MessageBox.Show("ハーフ(From)のタイムが不正です（hh:mm:ss）。");
                mtbBestHalfFrom.Focus();
                return false;
            }

            if (!IsMaskedTimeEmpty(mtbBestHalfTo) && !Regex.IsMatch(mtbBestHalfTo.Text, regexHalf))
            {
                MessageBox.Show("ハーフ(To)のタイムが不正です（hh:mm:ss）。");
                mtbBestHalfTo.Focus();
                return false;
            }

            return true;
        }


        /// <summary>
        /// MaskedTextBoxが空かどうか判定
        /// </summary>
        /// <param name="mtb">MaskedTextBox</param>
        /// <returns>空かどうか</returns>
        private bool IsMaskedTimeEmpty(MaskedTextBox mtb)
        {
            return !mtb.MaskFull && string.IsNullOrWhiteSpace(mtb.Text.Replace(":", "").Replace(".", "").Replace("-", ""));
        }

        /// <summary>
        /// 追加ボタン押下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = new PlayerEditorForm();
            dlg.Mode = "NEW";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // 子画面で保存されたデータを取得（仮に以下のように受け取る）
                var playerName = dlg.txtPlayerName.Text;
                var time10000 = dlg.mtbBest10k.Text;
                var timeHalf = dlg.mtbBestHalf.Text;
                var universityId = dlg.cmbUniversity.SelectedValue.ToString();

                InsertPlayer(playerName, time10000, timeHalf, universityId);

                SearchPlayers();
            }
        }

        /// <summary>
        /// 選手情報登録
        /// </summary>
        /// <param name="playerName">選手名</param>
        /// <param name="time10000">1万m記録</param>
        /// <param name="timeHalf">ハーフ記録</param>
        /// <param name="univCode">大学コード</param>
        private void InsertPlayer(string playerName, string time10000, string timeHalf, string univCode)
        {
            string sqlMaxInternalNo = @"
                SELECT NVL(MAX(INTERNAL_NO), 0) + 1
                FROM PLAYER_INFO
                WHERE UNIV_CODE = :univCode
            ";

            string sqlInsert = @"
                INSERT INTO PLAYER_INFO (UNIV_CODE, INTERNAL_NO, PLAYER_NAME, BEST_10K, BEST_HALF)
                VALUES (:univCode, :internalNo, :playerName, :best10k, :bestHalf)
            ";

            string sqlInsertHistory = @"
                INSERT INTO PLAYER_INFO_HISTORY (HISTORY_ID, UNIV_CODE, INTERNAL_NO, BEST_10K, BEST_HALF,
                    RECORD_DATE, RECORD_TYPE, UPDATED_BY, REASON)
                VALUES (PLAYER_INFO_HISTORY_SEQ.NEXTVAL, :univCode, :internalNo, :best10k, :bestHalf,
                SYSDATE, 'INSERT', 'SYSTEM', '初')
            ";

            using (var con = new OracleConnection(connStr))  // OracleConnectionを使う
            {
                con.Open();

                // トランザクション開始
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        // 最大のINTERNAL_NOを取得する
                        using (var cmdMax = new OracleCommand(sqlMaxInternalNo, con))
                        {
                            cmdMax.Parameters.Add("univCode", OracleDbType.Varchar2).Value = univCode;
                            var maxInternalNo = cmdMax.ExecuteScalar();  // 最大のINTERNAL_NOを取得

                            // 新しいINTERNAL_NOを設定
                            int internalNo = Convert.ToInt32(maxInternalNo);

                            // データ挿入
                            using (var cmdInsert = new OracleCommand(sqlInsert, con))
                            {
                                cmdInsert.Parameters.Add("univCode", OracleDbType.Varchar2).Value = univCode;
                                cmdInsert.Parameters.Add("internalNo", OracleDbType.Int32).Value = internalNo;
                                cmdInsert.Parameters.Add("playerName", OracleDbType.Varchar2).Value = playerName;
                                cmdInsert.Parameters.Add("best10k", OracleDbType.Varchar2).Value = time10000;
                                cmdInsert.Parameters.Add("bestHalf", OracleDbType.Varchar2).Value = timeHalf;

                                // INSERT 実行
                                cmdInsert.ExecuteNonQuery();
                            }

                            // データ挿入
                            using (var cmdInsert = new OracleCommand(sqlInsertHistory, con))
                            {
                                cmdInsert.Parameters.Add("univCode", OracleDbType.Varchar2).Value = univCode;
                                cmdInsert.Parameters.Add("internalNo", OracleDbType.Int32).Value = internalNo;
                                cmdInsert.Parameters.Add("best10k", OracleDbType.Varchar2).Value = time10000;
                                cmdInsert.Parameters.Add("bestHalf", OracleDbType.Varchar2).Value = timeHalf;

                                // INSERT 実行
                                cmdInsert.ExecuteNonQuery();
                            }
                        }

                        // コミット
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        // エラーが発生した場合はロールバック
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// グリッドのセルダブルクリック
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvPlayers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // ヘッダーは無視

            // 選択行のデータを取得
            var row = dgvPlayers.Rows[e.RowIndex];
            string univCode = row.Cells["UNIV_CODE"].Value.ToString();
            int internalNo = Convert.ToInt32(row.Cells["INTERNAL_NO"].Value);
            string playerName = row.Cells["PLAYER_NAME"].Value.ToString();
            string best10k = row.Cells["BEST_10K"].Value?.ToString();
            string bestHalf = row.Cells["BEST_HALF"].Value?.ToString();

            // 編集画面を生成
            var frmEdit = new PlayerEditorForm();
            frmEdit.Mode = "EDIT";

            // 編集画面にデータを渡す
            frmEdit.UnivCode = univCode;
            frmEdit.InternalNo = internalNo;
            frmEdit.txtPlayerName.Text = playerName;
            frmEdit.mtbBest10k.Text = best10k;
            frmEdit.mtbBestHalf.Text = bestHalf;

            // ダイアログ表示
            if (frmEdit.ShowDialog() == DialogResult.OK)
            {
                if (frmEdit.Mode == "EDIT")
                {
                    // 子画面で保存されたデータを取得（仮に以下のように受け取る）
                    var updPlayerName = frmEdit.txtPlayerName.Text;
                    var updBest10k = frmEdit.mtbBest10k.Text;
                    var updBestHalf = frmEdit.mtbBestHalf.Text;

                    UpdatePlayer(internalNo, updPlayerName, updBest10k, updBestHalf, univCode);
                }
                else
                {
                    DeletePlayer(internalNo, univCode);
                }

                // 更新があった場合は一覧を再読み込み
                SearchPlayers();
            }
        }

        /// <summary>
        /// 選手情報更新
        /// </summary>
        /// <param name="internalNo">学内連番（対象行を特定）</param>
        /// <param name="playerName">選手名</param>
        /// <param name="time10000">1万m記録</param>
        /// <param name="timeHalf">ハーフ記録</param>
        /// <param name="univCode">大学コード</param>
        private void UpdatePlayer(int internalNo, string playerName, string time10000, string timeHalf, string univCode)
        {
            string sqlUpdate = @"
                UPDATE PLAYER_INFO
                SET PLAYER_NAME = :playerName,
                    BEST_10K   = :best10k,
                    BEST_HALF  = :bestHalf
                WHERE UNIV_CODE   = :univCode
                    AND INTERNAL_NO = :internalNo
            ";

            string sqlUpdateHistory = @"
                INSERT INTO PLAYER_INFO_HISTORY (HISTORY_ID, UNIV_CODE, INTERNAL_NO, BEST_10K, BEST_HALF,
                    RECORD_DATE, RECORD_TYPE, UPDATED_BY, REASON)
                VALUES (PLAYER_INFO_HISTORY_SEQ.NEXTVAL, :univCode, :internalNo, :best10k, :bestHalf,
                SYSDATE, 'UPDATE', 'SYSTEM', '自己新')
            ";

            using (var con = new OracleConnection(connStr))
            {
                con.Open();

                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new OracleCommand(sqlUpdate, con))
                        {
                            cmd.Parameters.Add("playerName", OracleDbType.Varchar2).Value = playerName;
                            cmd.Parameters.Add("best10k", OracleDbType.Varchar2).Value = time10000;
                            cmd.Parameters.Add("bestHalf", OracleDbType.Varchar2).Value = timeHalf;
                            cmd.Parameters.Add("univCode", OracleDbType.Varchar2).Value = univCode;
                            cmd.Parameters.Add("internalNo", OracleDbType.Int32).Value = internalNo;

                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new OracleCommand(sqlUpdateHistory, con))
                        {
                            cmd.Parameters.Add("univCode", OracleDbType.Varchar2).Value = univCode;
                            cmd.Parameters.Add("internalNo", OracleDbType.Int32).Value = internalNo;
                            cmd.Parameters.Add("best10k", OracleDbType.Varchar2).Value = time10000;
                            cmd.Parameters.Add("bestHalf", OracleDbType.Varchar2).Value = timeHalf;

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 選手情報削除
        /// </summary>
        /// <param name="internalNo">学内連番（対象行を特定）</param>
        /// <param name="univCode">大学コード</param>
        private void DeletePlayer(int internalNo, string univCode)
        {
            string sqlDelete = @"
                DELETE FROM PLAYER_INFO
                WHERE UNIV_CODE   = :univCode
                    AND INTERNAL_NO = :internalNo
            ";

            using (var con = new OracleConnection(connStr))
            {
                con.Open();

                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new OracleCommand(sqlDelete, con))
                        {
                            cmd.Parameters.Add("univCode", OracleDbType.Varchar2).Value = univCode;
                            cmd.Parameters.Add("internalNo", OracleDbType.Int32).Value = internalNo;

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// クリアボタン押下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbUniversity.SelectedIndex = -1;
            txtPlayerName.Clear();
            mtbBest10kFrom.Clear();
            mtbBest10kTo.Clear();
            mtbBestHalfFrom.Clear();
            mtbBestHalfTo.Clear();

            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync(); // バックグラウンド処理をキャンセル
            }
        }
    }
}
