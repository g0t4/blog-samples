namespace MiscBlogSamples
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class BlogMapThenCollect
	{
		public List<Output> ForLoop()
		{
			var outputs = new List<Output>();
			var inputs = GetInputs();
			for (var i = 0; i < inputs.Count(); i++)
			{
				var input = inputs[i];
				var output = new Output();
				output.Id = Convert.ToInt32(input.Id);
				outputs.Add(output);
			}
			return outputs;
		}

		public List<Output> ForEachLoop()
		{
			var outputs = new List<Output>();
			foreach (var input in GetInputs())
			{
				var output = new Output();
				output.Id = Convert.ToInt32(input.Id);
				outputs.Add(output);
			}
			return outputs;
		}

		private List<Input> GetInputs()
		{
			return new List<Input>();
		}

		public class Input
		{
			public string Id { get; set; }
		}

		public class Output
		{
			public Output()
			{
			}

			public Output(Input input)
			{
				Id = Convert.ToInt32(input.Id);
			}

			public int Id { get; set; }
		}

		public List<Output> FunctionalForEachLoop()
		{
			var outputs = new List<Output>();
			GetInputs()
				.ForEach(input => MapAndAdd(input, outputs));
			return outputs;
		}

		private void MapAndAdd(Input input, List<Output> outputs)
		{
			var output = new Output();
			output.Id = Convert.ToInt32(input.Id);
			outputs.Add(output);
		}

		public List<Output> FunctionalMapThenCollect()
		{
			var outputs = GetInputs()
				.Select(input =>
				{
					var output = new Output();
					output.Id = Convert.ToInt32(input.Id);
					return output;
				})
				.ToList();
			return outputs;
		}

		public List<Output> FunctionalConstructorMapThenCollect()
		{
			var outputs = GetInputs()
				.Select(input => new Output(input))
				.ToList();
			return outputs;
		}

		public List<Output> ForEachWithFilter()
		{
			var outputs = new List<Output>();
			foreach (var input in GetInputs())
			{
				int id;
				if (!Int32.TryParse(input.Id, out id))
				{
					continue;
				}

				var output = new Output();
				output.Id = id;
				outputs.Add(output);
			}
			return outputs;
		}

		public List<Output> FunctionalMapThenFilterThenCollect()
		{
			var outputs = GetInputs()
				.Where(InputHasValidId)
				.Select(input => new Output(input))
				.ToList();
			return outputs;
		}

		private bool InputHasValidId(Input input)
		{
			int id;
			return Int32.TryParse(input.Id, out id);
		}
	}
}