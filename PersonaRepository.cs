using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace CRUD_WPF
{
    public class PersonaRepository
    {
        private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=CrudWpfDb;Trusted_Connection=True;TrustServerCertificate=True;";


        public void ProbarConexion()
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
        }

        public List<Persona> ObtenerTodos()
        {
            var lista = new List<Persona>();

            using var conn = new SqlConnection(ConnectionString);

            using var cmd = new SqlCommand("SELECT Id, Nombre, Edad From Personas ORDER BY Id", conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Persona
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Edad = reader.GetInt32(2)
                });
            }

            return lista;

        }

        public void Insertar(Persona p)
        {
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(
                "INSERT INTO Personas (Nombre, Edad) VALUES (@Nombre, @Edad)", conn);

            cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@Edad", p.Edad);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public int Actualizar(Persona p)
        {
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(
                "UPDATE Personas SET Nombre = @Nombre, Edad = @Edad WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@Edad", p.Edad);
            cmd.Parameters.AddWithValue("@Id", p.Id);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Eliminar(int id)
        {
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand("DELETE FROM Personas WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }

    
}
