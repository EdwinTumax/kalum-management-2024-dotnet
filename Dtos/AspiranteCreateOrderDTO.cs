namespace KalumManagement.Dtos
{
    public class AspiranteCreateOrderDTO
    {
        public string OrderId {get;set;}
        public DateTime OrderDate {get;set;}
        public string Status {get;set;}
        public DataCreateOrderDTO Data {get;set;}
        
    }
}