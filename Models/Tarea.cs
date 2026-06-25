namespace OyM_CLI.Models;

public enum Prioridad
{
    Baja,
    Media,
    Alta
}

public class Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Materia { get; set; }

    public Tarea (String titulo, String materia)
    {
        Titulo = titulo;
        Materia = materia;
    }
    
    public Prioridad Prioridad { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaCierre { get; set; }
    public DateTime? FechaCompletado { get; set; }
    public bool Completado { get; set; }
    
}