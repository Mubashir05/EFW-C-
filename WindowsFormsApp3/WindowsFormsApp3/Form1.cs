using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog() { Filter= "JPEG|*.jpg" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pic.Image = Image.FromFile(ofd.FileName);
                    Employee obj = employeeBindingSource.Current as Employee;
                    if (obj != null)
                        obj.ImageUrl = ofd.FileName;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using(ModelContext db = new ModelContext())
            {
                employeeBindingSource.DataSource = db.EmpList.ToList();
            }
            metroPanel1.Enabled = false;
            Employee obj = employeeBindingSource.Current as Employee;
            if (obj != null)
                pic.Image = Image.FromFile(obj.ImageUrl); 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            pic.Image = null;
            metroPanel1.Enabled = true;
            employeeBindingSource.Add(new Employee());
            employeeBindingSource.MoveLast();
            textName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            metroPanel1.Enabled = true;
            textName.Focus();
            Employee obj = employeeBindingSource.Current as Employee;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            metroPanel1.Enabled = false;
            employeeBindingSource.ResetBindings(false);
            Form1_Load(sender, e);  
        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Employee obj = employeeBindingSource.Current as Employee;
            if (obj != null)
                pic.Image = Image.FromFile(obj.ImageUrl);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MetroFramework.MetroMessageBox.Show(this,"Are you sure you wanna delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (ModelContext db = new ModelContext())
                {
                    Employee obj = employeeBindingSource.Current as Employee;
                    if(obj != null)
                    {
                        if (db.Entry<Employee>(obj).State == EntityState.Detached)
                            db.Set<Employee>().Attach(obj);
                        db.Entry<Employee>(obj).State = EntityState.Deleted;
                        db.SaveChanges();
                        MetroFramework.MetroMessageBox.Show(this, "Successfully Deleted");
                        employeeBindingSource.RemoveCurrent();
                        metroPanel1.Enabled = false;
                        pic.Image = null;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (ModelContext db = new ModelContext())
            {
                Employee obj = employeeBindingSource.Current as Employee;
                if(obj != null)
                {
                    if (db.Entry<Employee>(obj).State == EntityState.Detached)
                        db.Set<Employee>().Attach(obj);
                    if (obj.EmpID == 0)
                        db.Entry<Employee>(obj).State = EntityState.Added;
                    else
                        db.Entry<Employee>(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    MetroFramework.MetroMessageBox.Show(this, "Saved Successfully");
                    metroGrid1.Refresh();
                    metroPanel1.Enabled = false;
                }
            }
        }
    }
}
