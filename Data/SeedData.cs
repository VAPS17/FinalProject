#define TEST_PAGINATION_MEMBERS
//#define TEST_PAGINATION_PROJECTS

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
			/*
			//Preencher a tabela "State
			if (projectManaContext.State.Any()) return;

			projectManaContext.State.AddRange(
				new State { StateValue = "On Going" },
				new State { StateValue = "In Progress" },
				new State { StateValue = "Conclued" }
				);
			projectManaContext.SaveChanges();
			*/
			//Preencher a tabela "P_Task"
			/*if (projectManaContext.P_Task.Any()) return;

			projectManaContext.P_Task.AddRange(
				new P_Task { P_TaskName = "Teste_1", Comentary = "Ola primeira Tarefa", StateId = 1, ProjectId = 1 },
				new P_Task { P_TaskName = "Teste_1.2", Comentary = "Ola segunda Tarefa", StateId = 2, ProjectId = 1 },
				new P_Task { P_TaskName = "Teste_2", Comentary = "Ola primeira Tarefa", StateId = 1, ProjectId = 2 },
				new P_Task { P_TaskName = "Teste_2.1", Comentary = "Ola segunda Tarefa", StateId = 3, ProjectId = 2 }
				) ;*/
			projectManaContext.SaveChanges();

#if TEST_PAGINATION_MEMBERS
			Function function = projectManaContext.Function.FirstOrDefault();

			if (function == null) {
				function = new Function { Name = "Anonymous" };
				projectManaContext.Add(function);
			}

			for (int i = 1; i <= 1000; i++) {
				projectManaContext.Member.Add(
					new Member {
						Name = "Member " + i,
						Email = "membertest" + i + "@ipg.pt",
						EmployeeNumber = "" + i,
						Function = function 
					});
			}
			projectManaContext.SaveChanges();
#endif

#if TEST_PAGINATION_PROJECTS
			for (int i = 1; i <= 1000; i++) {
				projectManaContext.Project.Add(
					new Project {
						Name = "Project " + i,
						Description = "ola" + i + "bomdia",
						ProjectCreator = "antonio" + i,
						NumberEmployees = "1" + i,
                        StartDate = 12 / 2 / 2021 14:12,
                        FinishDate = 0,
						DecisiveDeliveryDate = 
					}
			projectManaContext.SaveChanges();
#endif
		}
	}
}
