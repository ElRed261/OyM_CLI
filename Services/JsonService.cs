using System.Text.Json;
using OyM_CLI.Models;

namespace OyM_CLI.Services;

class JsonService
{
    public void Exportar(string ruta, List<Tarea> tareas)
    {
        var json = JsonSerializer.Serialize(tareas, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ruta, json);
    }

    public List<Tarea>? Importar(string ruta)
    {
        var json = File.ReadAllText(ruta);
        return JsonSerializer.Deserialize<List<Tarea>>(json);
    }
}

