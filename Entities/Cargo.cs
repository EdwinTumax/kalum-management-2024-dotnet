namespace KalumManagement.Entities
{
    public class Cargo
    {
        public string CargoId {get;set;}
        public string Description {get;set;}
        public string Prefijo {get;set;}
        public double Monto {get;set;}
        public bool GeneraMora {get;set;}
        public int PorcentajeMora {get;set;}        
        public virtual List<CuentasXCobrar> CuentasXCobrar {get;set;}
    }
}