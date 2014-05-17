namespace Koalite.EFSample.Model
{
    public class Person
    {
	    protected Person() {  /*EF*/ }

        public Person(string name)
        {
	        Check.Require(!string.IsNullOrWhiteSpace(name), "Name is required");
	        Name = name;
		}

        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}