using System.Net.Sockets;
using OyM_CLI.Models;
using OyM_CLI.Repocitorios;
using OyM_CLI.Services;

namespace OyM;
class Program
{
    static void Main(string[] args)
    {
        var JsonService = new JsonService();
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
        if (args.Length < 2)
        {
            Console.WriteLine("Usa oym complete <id> --undo");
            break;
        }
        int idCompletar = int.Parse(args[1]);
        if (args.Length >= 3 && args[2] == "--undo")
            repo.DescompletarTarea(idCompletar);
        else
        repo.CompletarTarea(idCompletar);
       
        Console.WriteLine($"Se completó la tarea. ");
        break;
    
    case "delete":
        if (args.Length < 2)
        {
            Console.WriteLine("Usa oym delete <id>");
            break;
        }
        int idDelete = int.Parse(args[1]);
        repo.EliminarTarea(idDelete);
        Console.WriteLine($"Tarea eliminada con ID: {idDelete}");
    break;
    
    case "export":
                if (args.Length < 2)
                {
                    Console.WriteLine("Usa oym export <archivo.json>");
                    break;
                }
                var tareasExport = repo.ListaTareas();
                JsonService.Exportar(args[1], tareasExport);
                Console.WriteLine($"Exportadas {tareasExport.Count} tareas a {args[1]}");
    break;
    case "import":
                if (args.Length < 2)
                {
                    Console.WriteLine("Usa: oym import <archivo.json>");
                    break;
                }
                var tareasImport = JsonService.Importar(args[1]);
                if (tareasImport == null) break;
                foreach (var t in tareasImport)
                {
                    repo.CrearTarea(t);
                }
                Console.WriteLine($"Importadas {tareasImport.Count} tareas desde {args[1]}");
    break;
    default:
        

    Console.WriteLine($"Comando desconocido: {comando}");
                break;
        }
    }
}