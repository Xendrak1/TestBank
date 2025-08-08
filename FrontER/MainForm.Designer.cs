using System.Drawing;
using System.Windows.Forms;

namespace FrontER
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblOk;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblOk = new Label();

            // MainForm
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 200);
            Controls.Add(lblOk);
            Name = "MainForm";
            Text = "BCP FrontEnd";

            // lblOk
            lblOk.AutoSize = true;
            lblOk.Location = new Point(20, 20);
            lblOk.Text = "Login OK. Aquí va la UI (Crear/Buscar/Baja/Reporte).";
        }
    }
}
