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

			public void Creative()
			{
				var family = new Family();
				family.Names = new[] {"John", "Jane"}.ToList();
			}

			public void AddFamilyMember(Family family)
			{
				family.Names = family.Names ?? new List<string>();
				family.Names.Add("Baby");
			}

			public void CheckForFamilyMember(Family family)
			{
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

			public void Creative()
			{
				var names = new[] {"John", "Jane"};
				var family = new Family(names);
			}

			public void AddFamilyMember(Family family)
			{
				family.Names.Add("Baby");
			}

			public void CheckForFamilyMember(Family family)
			{
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

			public void Creative()
			{
				var names = new[] {"John", "Jane"};
				var family = new Family(names);
			}

			public void AddFamilyMember(Family family)
			{
				family.AddName("Baby");
			}

			public void CheckForFamilyMember(Family family)
			{
				var searchName = "Bob";
				var hasSomeoneNamed = family.HasSomeoneNamed(searchName);
			}
		}
	}
}