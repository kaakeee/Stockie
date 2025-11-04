using System;
using System.Drawing;
using System.Windows.Forms;
using Stockie.Models;

namespace Stockie
{
 public class AddItemForm : Form
 {
 public Item Item { get; private set; }
 private TextBox txtNombre;
 private TextBox txtTipo;
 private TextBox txtCodigoSn;
 private Button btnOk;
 private Button btnCancel;

 public AddItemForm()
 {
 InitializeComponent();
 }

 private void InitializeComponent()
 {
 this.Text = "Agregar Item";
 this.Size = new Size(400,250);
 this.StartPosition = FormStartPosition.CenterParent;

 var lblNombre = new Label() { Text = "Nombre:", Location = new Point(10,20) };
 txtNombre = new TextBox() { Location = new Point(100,18), Width =250 };
 var lblTipo = new Label() { Text = "Tipo:", Location = new Point(10,60) };
 txtTipo = new TextBox() { Location = new Point(100,58), Width =250 };
 var lblCodigo = new Label() { Text = "Codigo SN:", Location = new Point(10,100) };
 txtCodigoSn = new TextBox() { Location = new Point(100,98), Width =250 };

 btnOk = new Button() { Text = "OK", Location = new Point(100,150) };
 btnOk.Click += BtnOk_Click;
 btnCancel = new Button() { Text = "Cancel", Location = new Point(200,150) };
 btnCancel.Click += (s,e) => this.DialogResult = DialogResult.Cancel;

 this.Controls.Add(lblNombre);
 this.Controls.Add(txtNombre);
 this.Controls.Add(lblTipo);
 this.Controls.Add(txtTipo);
 this.Controls.Add(lblCodigo);
 this.Controls.Add(txtCodigoSn);
 this.Controls.Add(btnOk);
 this.Controls.Add(btnCancel);
 }

 private void BtnOk_Click(object sender, EventArgs e)
 {
 if (string.IsNullOrWhiteSpace(txtNombre.Text))
 {
 MessageBox.Show("Nombre es requerido", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
 return;
 }

 Item = new Item
 {
 Nombre = txtNombre.Text.Trim(),
 Tipo = string.IsNullOrWhiteSpace(txtTipo.Text) ? null : txtTipo.Text.Trim(),
 CodigoSn = string.IsNullOrWhiteSpace(txtCodigoSn.Text) ? null : txtCodigoSn.Text.Trim()
 };

 this.DialogResult = DialogResult.OK;
 }
 }
}
