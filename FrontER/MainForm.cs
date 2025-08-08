using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FrontER
{
    public partial class MainForm : Form
    {
        private readonly HttpClient _http;

        // Crear
        TextBox txtCI, txtPaterno, txtMaterno, txtNombres, txtCelular, txtCorreo, txtCuenta, txtTipo, txtSaldo;
        DateTimePicker dtNac;
        Button btnCrear;

        // Buscar
        TextBox txtBuscarCI;
        Button btnBuscar;
        DataGridView gridBuscar;

        // Baja
        TextBox txtCuentaBaja;
        Button btnBajaCuenta;

        // Reporte
        DateTimePicker dtDesde, dtHasta;
        Button btnReporte;
        DataGridView gridReporte;

        public MainForm(string baseUrl, string user, string pass)
        {
            _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}"));
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

            Text = "BCP FrontEnd";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1000; Height = 650;

            BuildUI();

            // Por si acaso
            gridBuscar.AutoGenerateColumns = true;
            gridReporte.AutoGenerateColumns = true;
        }

        void BuildUI()
        {
            var tabs = new TabControl { Dock = DockStyle.Fill };

            // ===== Crear =====
            var tpCrear = new TabPage("Crear");
            txtCI = T(130, 20, "6851475LP"); L(tpCrear, 20, 25, "CI:"); tpCrear.Controls.Add(txtCI);
            txtPaterno = T(130, 60, "RODRIGUEZ"); L(tpCrear, 20, 65, "Paterno:"); tpCrear.Controls.Add(txtPaterno);
            txtMaterno = T(130, 100, "PEREZ"); L(tpCrear, 20, 105, "Materno:"); tpCrear.Controls.Add(txtMaterno);
            txtNombres = T(130, 140, "EDUARDO"); L(tpCrear, 20, 145, "Nombres:"); tpCrear.Controls.Add(txtNombres);

            dtNac = new DateTimePicker { Left = 130, Top = 180, Width = 200, Value = new DateTime(2000, 1, 1) };
            L(tpCrear, 20, 185, "Fecha Nac.:"); tpCrear.Controls.Add(dtNac);

            txtCelular = T(130, 220, "70000000"); L(tpCrear, 20, 225, "Celular:"); tpCrear.Controls.Add(txtCelular);
            txtCorreo = T(130, 260, "eduardo@example.com"); L(tpCrear, 20, 265, "Correo:"); tpCrear.Controls.Add(txtCorreo);

            txtCuenta = T(520, 20, "000000000001"); L(tpCrear, 410, 25, "Nro Cuenta:"); tpCrear.Controls.Add(txtCuenta);
            txtTipo = T(520, 60, "AHO"); L(tpCrear, 410, 65, "Tipo (AHO/CTE):"); tpCrear.Controls.Add(txtTipo);
            txtSaldo = T(520, 100, "100"); L(tpCrear, 410, 105, "Saldo:"); tpCrear.Controls.Add(txtSaldo);

            btnCrear = new Button { Left = 520, Top = 140, Width = 180, Height = 32, Text = "Crear Cliente + Cuenta" };
            btnCrear.Click += BtnCrear_Click; tpCrear.Controls.Add(btnCrear);

            // ===== Buscar =====
            var tpBuscar = new TabPage("Buscar");
            txtBuscarCI = T(100, 20, "6851475LP"); L(tpBuscar, 20, 25, "CI:"); tpBuscar.Controls.Add(txtBuscarCI);
            btnBuscar = new Button { Left = 320, Top = 18, Width = 100, Height = 30, Text = "Buscar" };
            btnBuscar.Click += BtnBuscar_Click; tpBuscar.Controls.Add(btnBuscar);
            gridBuscar = Grid(20, 60); tpBuscar.Controls.Add(gridBuscar);

            // ===== Baja =====
            var tpBaja = new TabPage("Baja");
            txtCuentaBaja = T(140, 20, "CTA000001"); L(tpBaja, 20, 25, "Cuenta ID:"); tpBaja.Controls.Add(txtCuentaBaja);
            btnBajaCuenta = new Button { Left = 320, Top = 18, Width = 150, Height = 30, Text = "Dar de baja" };
            btnBajaCuenta.Click += BtnBajaCuenta_Click; tpBaja.Controls.Add(btnBajaCuenta);

            // ===== Reporte =====
            var tpRep = new TabPage("Reporte");
            dtDesde = new DateTimePicker { Left = 100, Top = 20, Width = 200, Value = DateTime.Today.AddDays(-7) };
            dtHasta = new DateTimePicker { Left = 420, Top = 20, Width = 200, Value = DateTime.Today };
            L(tpRep, 20, 25, "Desde:"); tpRep.Controls.Add(dtDesde);
            L(tpRep, 340, 25, "Hasta:"); tpRep.Controls.Add(dtHasta);
            btnReporte = new Button { Left = 640, Top = 18, Width = 120, Height = 30, Text = "Generar" };
            btnReporte.Click += BtnReporte_Click; tpRep.Controls.Add(btnReporte);
            gridReporte = Grid(20, 60); tpRep.Controls.Add(gridReporte);

            tabs.TabPages.Add(tpCrear);
            tabs.TabPages.Add(tpBuscar);
            tabs.TabPages.Add(tpBaja);
            tabs.TabPages.Add(tpRep);
            Controls.Add(tabs);
        }

        // ==== Handlers API ====
        async void BtnCrear_Click(object? s, EventArgs e)
        {
            try
            {
                // validación simple de email
                if (!Regex.IsMatch(txtCorreo.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                { MessageBox.Show("Correo inválido"); return; }

                var dto = new
                {
                    carnetVc = txtCI.Text.Trim(),
                    paternoVc = txtPaterno.Text.Trim(),
                    maternoVc = txtMaterno.Text.Trim(),
                    nombresVc = txtNombres.Text.Trim(),
                    fechaNacimientoDt = dtNac.Value,
                    celularIn = int.Parse(txtCelular.Text.Trim()),
                    correoVc = txtCorreo.Text.Trim(),
                    estadoCliente = "ACTIVO",
                    nroCuentaVc = txtCuenta.Text.Trim(),
                    tipoCuentaVc = txtTipo.Text.Trim(),   // "AHO" o "CTE"
                    saldoDc = decimal.Parse(txtSaldo.Text.Trim()),
                    estadoCuenta = "ACTIVO"
                };
                var r = await _http.PostAsJsonAsync("/api/cliente-cuenta", dto);
                MessageBox.Show(r.IsSuccessStatusCode ? "OK" : $"Error: {r.StatusCode}");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        async void BtnBuscar_Click(object? s, EventArgs e)
        {
            try
            {
                var ci = txtBuscarCI.Text.Trim();
                var json = await _http.GetStringAsync($"/api/buscar?ci={Uri.EscapeDataString(ci)}");
                gridBuscar.DataSource = JsonToDataTable(json);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        async void BtnBajaCuenta_Click(object? s, EventArgs e)
        {
            try
            {
                var id = txtCuentaBaja.Text.Trim();
                var r = await _http.PutAsync($"/api/cuentas/{Uri.EscapeDataString(id)}/baja", null);
                MessageBox.Show(r.IsSuccessStatusCode ? "Cuenta dada de baja" : $"Error: {r.StatusCode}");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        async void BtnReporte_Click(object? s, EventArgs e)
        {
            try
            {
                var desde = dtDesde.Value.Date.ToString("yyyy-MM-dd");
                var hasta = dtHasta.Value.Date.ToString("yyyy-MM-dd");
                var json = await _http.GetStringAsync($"/api/reporte?desde={desde}&hasta={hasta}");
                gridReporte.DataSource = JsonToDataTable(json);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // ==== Helpers UI ====
        static Label L(Control parent, int x, int y, string t) { var l = new Label { Left = x, Top = y, AutoSize = true, Text = t }; parent.Controls.Add(l); return l; }
        static TextBox T(int x, int y, string? v = null) => new TextBox { Left = x, Top = y, Width = 240, Text = v ?? "" };
        static DataGridView Grid(int x, int y) => new DataGridView { Left = x, Top = y, Width = 920, Height = 480, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells };

        // ==== Helper: JSON (array de objetos) -> DataTable para el DataGridView
        static DataTable JsonToDataTable(string json)
        {
            var dt = new DataTable();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Array) return dt;

            bool cols = false;
            foreach (var row in root.EnumerateArray())
            {
                if (!cols)
                {
                    foreach (var p in row.EnumerateObject())
                        dt.Columns.Add(p.Name);
                    cols = true;
                }
                var dr = dt.NewRow();
                foreach (var p in row.EnumerateObject())
                    dr[p.Name] = p.Value.ToString();
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
