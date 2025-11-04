using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stockie.Models;
using Stockie.Services;

namespace Stockie
{
 public class ItemsForm : Form
 {
 private User currentUser;
 private DataGridView dgv;
 private Button btnAdd;
 private Button btnDelete;

 public ItemsForm(User user)
 {
 currentUser = user;
 InitializeComponent();
 LoadItems();
 }

 private void InitializeComponent()
 {
 this.Text = "Items";
 this.Size = new Size(800,600);
 this.StartPosition = FormStartPosition.CenterParent;

 dgv = new DataGridView();
 dgv.Dock = DockStyle.Top;
 dgv.Height =450;
 dgv.ReadOnly = true;
 dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
 this.Controls.Add(dgv);

 btnAdd = new Button();
 btnAdd.Text = "Agregar";
 btnAdd.Location = new Point(10,470);
 btnAdd.Click += BtnAdd_Click;
 this.Controls.Add(btnAdd);

 btnDelete = new Button();
 btnDelete.Text = "Eliminar";
 btnDelete.Location = new Point(110,470);
 btnDelete.Click += BtnDelete_Click;
 this.Controls.Add(btnDelete);

 // Only admins can modify
 bool isAdmin = currentUser != null && currentUser.Role == Models.UserRole.Administrator;
 btnAdd.Enabled = isAdmin;
 btnDelete.Enabled = isAdmin;
 }

 private void LoadItems()
 {
 var items = ItemService.GetAllItems();
 dgv.DataSource = items.Select(i => new { i.Id, i.Nombre, i.Tipo, i.CodigoSn }).ToList();
 }

 private void BtnAdd_Click(object sender, EventArgs e)
 {
 using (var f = new AddItemForm())
 {
 if (f.ShowDialog() == DialogResult.OK)
 {
 ItemService.AddItem(f.Item);
 LoadItems();
 }
 }
 }

 private void BtnDelete_Click(object sender, EventArgs e)
 {
 if (dgv.SelectedRows.Count ==0) return;
 int id = (int)dgv.SelectedRows[0].Cells[0].Value;
 var confirm = MessageBox.Show($"Eliminar item con ID {id}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
 if (confirm == DialogResult.Yes)
 {
 ItemService.DeleteItem(id);
 LoadItems();
 }
 }
 }
}
