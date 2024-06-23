namespace KalumManagement.Dtos
{
    public class AspiranteListDTO
    {
        public string NoExpediente {get;set;}
        public string Apellidos {get;set;}
        public string Nombres {get;set;}
        public string Direccion {get;set;}
        public string Telefono {get;set;}
        public string Email {get;set;}
        public string Estatus {get;set;}
        public CarreraTecnicaListDTO CarreraTecnica {get;set;}
        public JornadaListDTO Jornada {get;set;}
        public ExamenAdmisionListDTO ExamenAdmision {get;set;}
    }
}