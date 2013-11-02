namespace MiscBlogSamples
{
	using System.Collections.Generic;
	using System.Linq;

	public class InvariantsToAvoidNullChecking
	{
		public class Variant
		{
			public class Family
			{
				public List<string> Names;
			}

			public void ConsumerExamples()
			{
				// consumer creating a family
				var family = new Family();
				family.Names = new[] {"John", "Jane"}.ToList();

				// consumer adding a name
				family.Names = family.Names ?? new List<string>();
				family.Names.Add("Baby");

				// consumer searching names
				var searchName = "Bob";
				var hasSomeoneNamed = family.Names != null && family.Names.Contains(searchName);
			}
		}

		public class Invariant
		{
			public class Family
			{
				public readonly List<string> Names;

				public Family(IEnumerable<string> names)
				{
					Names = names.ToList();
				}
			}

			public void ConsumerExamples()
			{
				// consumer creating a family
				var names = new[] { "John", "Jane" };
				var family = new Family(names);

				// consumer adding a name
				family.Names.Add("Baby");

				// consumer searching names
				var searchName = "Bob";
				var hasSomeoneNamed = family.Names.Contains(searchName);
			}
		}

		public class EncapsulatedInvariant
		{
			public class Family
			{
				protected readonly List<string> Names;

				public Family(IEnumerable<string> names)
				{
					Names = names.ToList();
				}

				public void AddName(string name)
				{
					Names.Add(name);
				}

				public bool HasSomeoneNamed(string searchName)
				{
					return Names.Contains(searchName);
				}
			}

			public void ConsumerExamples()
			{
				// consumer creating a family
				var names = new[] { "John", "Jane" };
				var family = new Family(names);

				// consumer adding a name
				family.AddName("Baby");

				// consumer searching names
				var searchName = "Bob";
				var hasSomeoneNamed = family.HasSomeoneNamed(searchName);
			}
		}
	}
}