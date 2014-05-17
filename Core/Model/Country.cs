namespace Koalite.EFSample.Model
{
    public class Country
    {
        protected Country() { /*EF*/ }

        public Country(string name)
        {
            Check.Require(!string.IsNullOrWhiteSpace(name), "Name is required");

            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}
