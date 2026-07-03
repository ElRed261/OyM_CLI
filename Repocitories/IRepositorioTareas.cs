using OyM_CLI.Models;

namespace OyM_CLI.Repocitorios
{
    public interface IRepositorioTareas
    {
        Tarea? ObtenerTarea(int id);
        List<Tarea> ListaTareas();
        Tarea CrearTarea(Tarea tarea);
        void CompletarTarea(int id);
        void EliminarTarea(int id);
        void DescompletarTarea(int id);

    }
}
