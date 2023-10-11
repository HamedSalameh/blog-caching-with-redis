namespace cache_example.domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Product(int id, string name, string description, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Description: {Description}, Price: {Price}";
        }
    }
}