using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    /// <summary>
    /// 選手編集画面
    /// </summary>
    public partial class PlayerEditorForm : Form
    {
        /// <summary>
        /// DB接続文字列
        /// </summary>
        private string connStr = "User Id=hakone; Password=hakone0719; Data Source=localhost:1521/ORCL;";

        public string Mode { get; set; }     // モード（新規登録："NEW", 編集："EDIT", 削除："DEL"）

        public string UnivCode { get; set; }     // 大学コード

        public int InternalNo { get; set; }     // 学内連番

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayerEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PlayerEditorForm_Load(object sender, EventArgs e)
        {
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

                if (Mode == "NEW")
                {
                    cmbUniversity.SelectedIndex = -1;
                }
                else
                {
                    cmbUniversity.SelectedValue = UnivCode;
                    cmbUniversity.Enabled = false;

                    btnDelete.Visible = true;
                }
            }
        }

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 選手名必須
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("選手名は必須です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPlayerName.Focus();
                return;
            }

            // 大学名必須（ComboBox）
            if (cmbUniversity.SelectedIndex < 0)
            {
                MessageBox.Show("大学名を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbUniversity.Focus();
                return;
            }

            var result = MessageBox.Show(
                "この内容で登録してよろしいですか？",   // メッセージ
                "確認",                                     // タイトル
                MessageBoxButtons.YesNo,                    // ボタン
                MessageBoxIcon.Question                     // アイコン
            );

            if (result == DialogResult.No)
            {
                return; // 保存キャンセル
            }

            // 保存 → 親へ OK を返す
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// キャンセルボタン押下
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // キャンセル → 親へ Cancel を返す
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "削除してよろしいですか？",   // メッセージ
                "確認",                                     // タイトル
                MessageBoxButtons.YesNo,                    // ボタン
                MessageBoxIcon.Question                     // アイコン
            );

            if (result == DialogResult.No)
            {
                return; // 削除キャンセル
            }

            // 削除 → 親へ OK を返す
            this.Mode = "DEL";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
