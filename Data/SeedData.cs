//#define TEST_PAGINATION_MEMBERS

using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Data
{
	public class SeedData
	{
		internal static void Populate(ProjectManaContext projectManaContext)
		{
#if TEST_PAGINATION_MEMBERS
			for (int i = 1; i <= 1000; i++) {
				projectManaContext.Member.Add(
					new Member {
						Name = "Member " + i,
						Email = "membertest"+i + "@ipg.pt",
						EmployeeNumber=""+i
					}
			if (context.State.Any()) return;

			context.State.AddRange(
				new State { StateValue = "On Going" },
				new State { StateValue = "In Progress"},
				new State { StateValue = "Conclued"}
				);
			}

			projectManaContext.SaveChanges();
#endif
		}
	}
}