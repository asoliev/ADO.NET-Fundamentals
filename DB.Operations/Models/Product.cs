namespace DB.Operations.Models
{
    public class Product : IEquatable<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Product);
        }

        public bool Equals(Product other)
        {
            return other != null &&
                   Name == other.Name &&
                   Description == other.Description &&
                   Weight == other.Weight &&
                   Height == other.Height &&
                   Width == other.Width &&
                   Length == other.Length;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description, Weight, Height, Width, Length);
        }
    }
}