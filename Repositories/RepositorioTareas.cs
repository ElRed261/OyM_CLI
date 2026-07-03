using System.Collections.Concurrent;
using Microsoft.Data.Sqlite;
using OyM_CLI.Models;

namespace OyM_CLI.Repositories;

public class RepositorioTareas : IRepositorioTareas
{
    private const string conection = "Data Source=OyM.db";

    public RepositorioTareas()
    {
        using var conexion = new SqliteConnection(conection);
        conexion.Open();
        var comando = conexion.CreateCommand();
        comando.CommandText =@"
        CREATE TABLE IF NOT EXISTS Tareas (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Titulo TEXT NOT NULL,
            Materia TEXT NOT NULL,
            Prioridad TEXT NOT NULL,
            FechaCreacion TEXT NOT NULL,
            FechaCierre TEXT,
            Completado INTEGER NOT NULL DEFAULT 0,
            FechaCompletado TEXT
        )
    ";
        
        comando.ExecuteNonQuery();
        conexion.Close();
    }

    public Tarea CrearTarea(Tarea tarea)
    {
        using var conexion = new SqliteConnection(conection);
        conexion.Open();

        var comando = conexion.CreateCommand();
        comando.CommandText =
            @" INSERT INTO Tareas (Titulo, Materia, Prioridad, FechaCreacion, FechaCierre, Completado) VALUES ($titulo, $materia, $prioridad, $fechaCreacion, $fechaCierre, $completado)";
        comando.Parameters.AddWithValue("$titulo", tarea.Titulo);
        comando.Parameters.AddWithValue("$materia", tarea.Materia);
        comando.Parameters.AddWithValue("$prioridad", tarea.Prioridad.ToString());
        comando.Parameters.AddWithValue("$fechaCreacion", tarea.FechaCreacion.ToString("o"));
        comando.Parameters.AddWithValue("$fechaCierre", (object?)tarea.FechaCierre?.ToString("o") ?? DBNull.Value);
        comando.Parameters.AddWithValue("$completado", tarea.Completado ? 1 : 0);
        
        comando.ExecuteNonQuery();
        comando.CommandText = "SELECT last_insert_rowid()";
        tarea.Id = (int) (long) comando.ExecuteScalar();
        conexion.Close();
        return tarea;
        

    }
    public Tarea? ObtenerTarea(int id)
    {
        using var conexion = new SqliteConnection(conection);
        conexion.Open();

        var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Titulo, Materia, Prioridad, FechaCreacion, FechaCierre, Completado, FechaCompletado FROM Tareas WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);

        using var lector = comando.ExecuteReader();
        if (lector.Read())
        {
            return new Tarea(
                    lector.GetString(1),
                    lector.GetString(2)
                )
                {
                    Id = lector.GetInt32(0),
                    Prioridad = Enum.Parse<Prioridad>(lector.GetString(3)),
                    FechaCreacion = DateTime.Parse(lector.GetString(4)),
                    FechaCierre = lector.IsDBNull(5) ? null : DateTime.Parse(lector.GetString(5)),
                    Completado = lector.GetInt32(6) == 1,
                    FechaCompletado = lector.IsDBNull(7) ? null : DateTime.Parse(lector.GetString(7))
                };
        }

        return null;
    }

    public List<Tarea> ListaTareas()
    {
        var tareas = new List<Tarea>();
        using var conexion = new SqliteConnection(conection);
        conexion.Open();
        
        var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Titulo, Materia, Prioridad, FechaCreacion, FechaCierre, Completado, FechaCompletado FROM Tareas";
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            tareas.Add(new Tarea(
                    lector.GetString(1),
                    lector.GetString(2)
                )
                {
                    Id = lector.GetInt32(0),
                    Prioridad = Enum.Parse<Prioridad>(lector.GetString(3)),
                    FechaCreacion = DateTime.Parse(lector.GetString(4)),
                    FechaCierre = lector.IsDBNull(5) ? null : DateTime.Parse(lector.GetString(5)),
                    Completado = lector.GetInt32(6) == 1,
                    FechaCompletado = lector.IsDBNull(7) ? null : DateTime.Parse(lector.GetString(7))
                }
            );
        }
        return tareas;
    }

    public void CompletarTarea(int id)
    {
        using var conexion = new SqliteConnection(conection);
        conexion.Open();
        
        var comando = conexion.CreateCommand();
        comando.CommandText ="UPDATE Tareas SET Completado = 1, FechaCompletado = $fecha WHERE Id = $id";
        comando.Parameters.AddWithValue("$fecha", DateTime.Now.ToString("o"));
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    public void EliminarTarea(int id)
    {
        using var conexion = new SqliteConnection(conection);
        conexion.Open();
        var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Tareas WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }
    public void DescompletarTarea(int id)
    {
        using var conexion = new SqliteConnection(conection);
        conexion.Open();

        var comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Tareas SET Completado = 0, FechaCompletado = NULL WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }
}