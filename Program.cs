using System.Net.Sockets;
using OyM_CLI.Models;
using OyM_CLI.Repocitorios;

namespace OyM;
class Program
{
    static void Main(string[] args)
    {

        var repo = new RepositorioTareas();
        if (args.Length == 0)
        {
            Console.WriteLine("Uso:(comando) (argumento)");
            Console.WriteLine("Comadnos: add, list, complete, delete, export, import");
            return;
        }

        var comando = args[0].ToLower();
        switch (comando)
        {
            case "add":
                var titulo = args[1];
                var materia = "";
                var prioridad = Prioridad.Media;
                DateTime? fechaCierre = null;

                for (int i = 2; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-m":
                            materia = args[++i];
                            break;
                        case "-p":
                            prioridad = Enum.Parse<Prioridad>(args[++i], true);
                            break;
                        case "-c":
                            fechaCierre = DateTime.Parse(args[++i]);
                            break;
                    }
                   
                }

                var tarea = repo.CrearTarea(new Tarea(titulo, materia)
                {
                    Prioridad = prioridad,
                    FechaCierre = fechaCierre
                });
                Console.WriteLine($"Tarea creada con ID:  {tarea.Id}");
                break;
            
    case "list":
                var tareas = repo.ListaTareas();
                foreach (var t in tareas)
                {
                    Console.WriteLine($"[{t.Id}] {t.Titulo} - {t.Materia} - {t.Prioridad} - {(t.Completado ? "✓" : "○")}");

                }
    break;
    case "complete":
    break;
    case "delete":
    break;
    case "export":
    break;
    case "import":
    break;
    default:
        

    Console.WriteLine($"Comando desconocido: {comando}");
                break;
        }
    }
}