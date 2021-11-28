using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Data
{
	public class SeedData
	{
		internal static void Populate(ProjectManaContext context)
		{
			if (context.State.Any()) return;

			context.State.AddRange(
				new State { StateValue = "On Going" },
				new State { StateValue = "In Progress"},
				new State { StateValue = "Conclued"}
		);

			context.SaveChanges();
		}
	}
}
