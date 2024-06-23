using KalumManagement.Dtos;

namespace KalumManagement.Services
{
    public interface IKalumQueueService
    {   
        public Task<bool> CandidateCreateOrderAsync(AspiranteCreateOrderDTO order);
    }
}