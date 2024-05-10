using MasterDetail.Models;

namespace MasterDetail.ViewModels
{
    public class ConfiguracionVM
    {
        public byte[] FileData { get; set; }

        public List<Configuracion> Configuraciones { get; set; }

    }
}
