namespace WebApplication4.Test
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
        public string LicensePlate { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
    }
}
