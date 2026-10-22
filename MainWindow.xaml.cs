using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
namespace CRUD_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PersonaRepository _repo = new();

        private Persona? _seleccionada;

        public MainWindow()
        {
            InitializeComponent();

            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                dgPersonas.ItemsSource = _repo.ObtenerTodos();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar datos:\n" + ex.Message);
            }
        }

        private Persona? LeerFormulario()
        {
            string nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Escribe un nombre.");
                return null;
            }

            if (!int.TryParse(txtEdad.Text, out int edad) || edad < 0 || edad > 150)
            {
                MessageBox.Show("La edad debe ser un número entre 0 y 150.");
                return null;
            }
            return new Persona { Nombre = nombre, Edad = edad };
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtEdad.Clear();
            dgPersonas.SelectedItem = null;
            _seleccionada = null;
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            _seleccionada = dgPersonas.SelectedItem as Persona;

            if (_seleccionada != null)
            {
                txtNombre.Text = _seleccionada.Nombre;
                txtEdad.Text = _seleccionada.Edad.ToString();
            }
        }

        private void Limpiar_btn(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private void Update_btn(object sender, RoutedEventArgs e)
        {
            if (_seleccionada == null)
            {
                MessageBox.Show("Selecciona una fila de la tabla primero.");
                return;
            }

            Persona? persona = LeerFormulario();
            if (persona == null) return;

            persona.Id = _seleccionada.Id;

            try
            {
                int filas = _repo.Actualizar(persona);
                if (filas == 0)
                    MessageBox.Show("Esa persona ya no existe en la base de datos.");

                CargarDatos();
                LimpiarFormulario();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al actualizar:\n" + ex.Message);
            }
        }

        private void Delete_btn(object sender, RoutedEventArgs e)
        {
            if (_seleccionada == null)
            {
                MessageBox.Show("Selecciona una fila de la tabla primero.");
                return;
            }

            var respuesta = MessageBox.Show(
                $"¿Eliminar a {_seleccionada.Nombre} ({_seleccionada.Edad} años)?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (respuesta != MessageBoxResult.Yes) return;

            try
            {
                int filas = _repo.Eliminar(_seleccionada.Id);
                if (filas == 0)
                    MessageBox.Show("Esa persona ya no existe en la base de datos.");

                CargarDatos();
                LimpiarFormulario();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al eliminar:\n" + ex.Message);
            }
        }

        private void Agregar_btn(object sender, RoutedEventArgs e)
        {
            Persona? persona = LeerFormulario();
            if (persona == null) return;

            try
            {
                _repo.Insertar(persona);
                CargarDatos();
                LimpiarFormulario();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al agregar:\n" + ex.Message);
            }
        }
    }
}