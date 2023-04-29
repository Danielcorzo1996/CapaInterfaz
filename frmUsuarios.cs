using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaNegocio;
using CapaEntidad;
using CapaInterfaz.Utilidades;

namespace CapaInterfaz
{
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {

            EstadoUsu.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            EstadoUsu.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            EstadoUsu.DisplayMember = "Texto";
            EstadoUsu.ValueMember = "Value";
            EstadoUsu.SelectedIndex = 0;

            List<Roles> ListaRol = new CN_Roles().listarRoles();

            foreach(Roles role in ListaRol)
            {
                RolUsu.Items.Add(new OpcionCombo() { Valor = role.IdRol, Texto = role.Descripcion });

            }
            RolUsu.DisplayMember = "Texto";
            RolUsu.ValueMember = "Value";
            RolUsu.SelectedIndex = 0;

            foreach(DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && columna.Name != "BtnSeleccionar")
                {
                    BusquedaUsu.Items.Add(new OpcionCombo() {  Valor = columna.Name, Texto = columna.HeaderText});
                }

            }

            BusquedaUsu.DisplayMember = "Texto";
            BusquedaUsu.ValueMember = "Value";
            BusquedaUsu.SelectedIndex = 0;

            //LISTA LOS USUARIOS
            List<Usuarios> ListaUsuarios = new CN_Usuarios().listarUsuarios();

            foreach (Usuarios item in ListaUsuarios)
            {
                dgvData.Rows.Add(new object[] {"",item.IdUsuario,item.Documento,
                                            item.Nombres,item.Apellidos,
                                            item.Correo,item.Clave,
                                            item.ORoles.IdRol,
                                            item.ORoles.Descripcion,
                                            item.Estado == true ? "Activo" : "No Activo",
                                            item.Estado == true ? 1 : 0 

                });

            }

        }

        private void btnGuardarUsuario_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
                
            Usuarios objusuario = new Usuarios();
            {

                objusuario.IdUsuario = Convert.ToInt32(txtId.Text);
                objusuario.Documento = NumeroDocumentoUsu.Text;
                objusuario.Nombres = NombresUsu.Text;
                objusuario.Apellidos = ApellidosUsu.Text;
                objusuario.Correo = CorreoUsu.Text;
                objusuario.Clave = ClaveUsu.Text;
                objusuario.ORoles = new Roles() { IdRol = Convert.ToInt32(((OpcionCombo)RolUsu.SelectedItem).Valor) };
                objusuario.Estado = Convert.ToInt32(((OpcionCombo)EstadoUsu.SelectedItem).Valor) == 1 ? true : false;
                }

            if (objusuario.IdUsuario == 0)
            {
                int idusuariogenerado = new CN_Usuarios().RegistrarUsuario(objusuario, out mensaje);

                if (idusuariogenerado != 0)
                {

                    dgvData.Rows.Add(new object[] {"",idusuariogenerado,NumeroDocumentoUsu.Text,
                                            NombresUsu.Text,ApellidosUsu.Text,
                                            CorreoUsu.Text,ClaveUsu.Text,
                                            ((OpcionCombo)RolUsu.SelectedItem).Valor.ToString(),
                                            ((OpcionCombo)RolUsu.SelectedItem).Texto.ToString(),
                                            ((OpcionCombo)EstadoUsu.SelectedItem).Texto.ToString(),
                                            ((OpcionCombo)EstadoUsu.SelectedItem).Valor.ToString()

                });

                    limpiar();

                }
                else
                {
                    MessageBox.Show(mensaje);
                }

            }
            else
            {
                bool resultado = new CN_Usuarios().EditarUsuario(objusuario, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtId.Text;
                    row.Cells["Documento"].Value = NumeroDocumentoUsu.Text;
                    row.Cells["Nombres"].Value = NombresUsu.Text;
                    row.Cells["Apellidos"].Value = ApellidosUsu.Text;
                    row.Cells["Correo"].Value = CorreoUsu.Text;
                    row.Cells["Clave"].Value = ClaveUsu.Text;
                    row.Cells["IdRol"].Value = ((OpcionCombo)RolUsu.SelectedItem).Valor.ToString();
                    row.Cells["Rol"].Value = ((OpcionCombo)RolUsu.SelectedItem).Texto.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)EstadoUsu.SelectedItem).Texto.ToString();
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)EstadoUsu.SelectedItem).Valor.ToString();

                    limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }

            }


            

        }

        private void limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "0";
            NumeroDocumentoUsu.Text = "";
            NombresUsu.Text = "";
            ApellidosUsu.Text = "";
            CorreoUsu.Text = "";
            ClaveUsu.Text = "";
            RolUsu.SelectedIndex = 0;
            EstadoUsu.SelectedIndex = 0;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.check.Width;
                var h = Properties.Resources.check.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check, new Rectangle (x, y, w, h));   
                e.Handled = true;
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "BtnSeleccionar")
            {
                int indice = e.RowIndex;

                if( indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.Rows[indice].Cells["Id"].Value.ToString(); 
                    NumeroDocumentoUsu.Text = dgvData.Rows[indice].Cells["Documento"].Value.ToString();
                    NombresUsu.Text = dgvData.Rows[indice].Cells["Nombres"].Value.ToString();
                    ApellidosUsu.Text = dgvData.Rows[indice].Cells["Apellidos"].Value.ToString();
                    CorreoUsu.Text = dgvData.Rows[indice].Cells["Correo"].Value.ToString();
                    ClaveUsu.Text = dgvData.Rows[indice].Cells["Clave"].Value.ToString();
                    
                    foreach (OpcionCombo oc in RolUsu.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["IdRol"].Value))
                        {
                            int indice_combo = RolUsu.Items.IndexOf(oc);
                            RolUsu.SelectedIndex = indice_combo;
                            break;
                        }

                    }

                    foreach (OpcionCombo oc in EstadoUsu.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = EstadoUsu.Items.IndexOf(oc);
                            EstadoUsu.SelectedIndex = indice_combo;
                            break;
                        }

                    }
                }
            }
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(txtId.Text) != 0)
            {
                if(MessageBox.Show("Esta seguro de eliminar el usuario.","Mensaje", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;

                    Usuarios objusuario = new Usuarios();
                    {
                        objusuario.IdUsuario = Convert.ToInt32(txtId.Text);
                    };

                    bool resultado = new CN_Usuarios().EliminarUsuario(objusuario, out mensaje);
                   
                    if(resultado)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                    }
                    else
                    {
                        MessageBox.Show(mensaje,"Mensaje",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)BusquedaUsu.SelectedItem).Valor.ToString();

            if(dgvData.Rows.Count > 0)
            {
                foreach(DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBuscar.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }else
                    {
                        row.Visible = false;
                    }
                }
            }
        }

        private void btnLimpiarBusc_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            NumeroDocumentoUsu.Text = "";
            NombresUsu.Text = "";
            ApellidosUsu.Text = "";
            CorreoUsu.Text = "";
            ClaveUsu.Text = "";
        }
    }
}
