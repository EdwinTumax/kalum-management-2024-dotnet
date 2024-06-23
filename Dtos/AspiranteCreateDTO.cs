using System.ComponentModel.DataAnnotations;
using KalumManagement.Helpers;

namespace KalumManagement.Dtos
{
    public class AspiranteCreateDTO
    {
        [Required(ErrorMessage = "El campo es requerido")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [PrefixPhone]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [EmailAddress(ErrorMessage = "El correo electronico no es valido")]
        public string Email { get; set; }
        public string Estatus { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string ExamenId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string CarreraId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string JornadaId { get; set; }
    }
}