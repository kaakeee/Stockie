using System;
using System.Drawing;
using System.Windows.Forms;
using Stockie.Models;
using Stockie.Services;

namespace Stockie
{
 public class MainForm : Form
 {
 private Panel leftMenuPanel;
 private Button btnToggleMenu;
 private FlowLayoutPanel menuItemsPanel;
 private Label lblTitle;
 private MonthCalendar monthCalendar;
 private User currentUser;

 public MainForm(User user)
 {
 currentUser = user;
 InitializeComponent();
 }

 public MainForm() : this(null) { }

 private void InitializeComponent()
 {
 this.Text = "Stockie";
 this.StartPosition = FormStartPosition.CenterScreen;
 this.WindowState = FormWindowState.Maximized;

 // Title label at top
 lblTitle = new Label();
 lblTitle.Text = "Stockie";
 lblTitle.Font = new Font("Segoe UI",20F, FontStyle.Bold);
 lblTitle.Dock = DockStyle.Top;
 lblTitle.Height =60;
 lblTitle.TextAlign = ContentAlignment.MiddleCenter;
 this.Controls.Add(lblTitle);

 // Left menu panel
 leftMenuPanel = new Panel();
 leftMenuPanel.Width =220;
 leftMenuPanel.Dock = DockStyle.Left;
 leftMenuPanel.BackColor = Color.FromArgb(240,240,240);
 leftMenuPanel.Padding = new Padding(5);
 this.Controls.Add(leftMenuPanel);

 // Toggle button to collapse/expand
 btnToggleMenu = new Button();
 btnToggleMenu.Text = "?";
 btnToggleMenu.Width =40;
 btnToggleMenu.Height =40;
 btnToggleMenu.Location = new Point(10,10);
 btnToggleMenu.Click += BtnToggleMenu_Click;
 leftMenuPanel.Controls.Add(btnToggleMenu);

 // Menu items container
 menuItemsPanel = new FlowLayoutPanel();
 menuItemsPanel.FlowDirection = FlowDirection.TopDown;
 menuItemsPanel.WrapContents = false;
 menuItemsPanel.Location = new Point(10,60);
 menuItemsPanel.Width = leftMenuPanel.Width -20;
 menuItemsPanel.Height = this.ClientSize.Height -80;
 menuItemsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
 leftMenuPanel.Controls.Add(menuItemsPanel);

 // Add main menu buttons (Items should appear above Configuracion)
 AddMenuButton("Inventario");
 AddMenuButton("Transferencias");
 AddMenuButton("Envios");
 AddMenuButton("Consulta de Stat");
 AddMenuButton("Reportes");
 AddMenuButton("Items", OpenItems);
 AddMenuButton("Configuracion");

 // Center calendar
 monthCalendar = new MonthCalendar();
 monthCalendar.MaxSelectionCount =1;
 monthCalendar.Dock = DockStyle.Fill;
 monthCalendar.Anchor = AnchorStyles.None;

 // Create a panel for center content so it doesn't overlap title
 var centerPanel = new Panel();
 centerPanel.Dock = DockStyle.Fill;
 centerPanel.Padding = new Padding(20);
 centerPanel.Controls.Add(monthCalendar);
 this.Controls.Add(centerPanel);

 // Ensure z-order: title on top, then left menu and center
 this.Controls.SetChildIndex(lblTitle,0);
 }

 private void AddMenuButton(string text)
 {
 AddMenuButton(text, (s,e)=> MessageBox.Show($"{text} clicked", "Menu", MessageBoxButtons.OK, MessageBoxIcon.Information));
 }

 private void AddMenuButton(string text, EventHandler onClick)
 {
 var btn = new Button();
 btn.Text = text;
 btn.Width = menuItemsPanel.Width -10;
 btn.Height =40;
 btn.Margin = new Padding(0,5,0,0);
 btn.TextAlign = ContentAlignment.MiddleLeft;
 btn.BackColor = Color.White;
 btn.FlatStyle = FlatStyle.Flat;
 btn.Click += onClick;
 menuItemsPanel.Controls.Add(btn);
 }

 private void OpenItems(object sender, EventArgs e)
 {
 // Open the items management form; pass current user so it can enforce permissions
 using (var itemsForm = new ItemsForm(currentUser))
 {
 itemsForm.ShowDialog();
 }
 }

 private void BtnToggleMenu_Click(object sender, EventArgs e)
 {
 if (leftMenuPanel.Width >60)
 {
 // Collapse
 leftMenuPanel.Width =60;
 menuItemsPanel.Visible = false;
 }
 else
 {
 // Expand
 leftMenuPanel.Width =220;
 menuItemsPanel.Visible = true;
 }
 }
 }
}
