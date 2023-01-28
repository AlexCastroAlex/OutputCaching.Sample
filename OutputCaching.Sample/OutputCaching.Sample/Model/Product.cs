namespace OutputCaching.Sample.Model
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; init; }
        public decimal Price { get; init; }
    }
}
