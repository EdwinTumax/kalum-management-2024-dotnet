using AutoMapper;
using KalumManagement.Entities;

namespace KalumManagement.Dtos
{
    public class AutoMapperProfiles :  Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CarreraTecnica,CarreraTecnicaListDTO>();
            CreateMap<CarreraTecnicaCreateOrUpdateDTO, CarreraTecnica>();
            CreateMap<Aspirante,AspiranteListDTO>();
            CreateMap<Jornada,JornadaListDTO>();
            CreateMap<ExamenAdmision,ExamenAdmisionListDTO>();
            CreateMap<AspiranteCreateDTO,DataCreateOrderDTO>();
        }
    }
}