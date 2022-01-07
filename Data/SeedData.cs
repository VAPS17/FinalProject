#define TEST_PAGINATION_MEMBERS
//#define TEST_PAGINATION_PROJECTS

using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Data
{
	public class SeedData
	{
		private const string ADMIN_EMAIL = "admin@gmail.com";
		private const string ADMIN_PASS = "AdminPass123$";
		private const string ROLE_ADMINISTRATOR = "admin";
		private const string ROLE_MANAGER = "manager";
		private const string ROLE_MEMBER = "member";

		internal static void Populate(ProjectManaContext projectManaContext)
		{
            /*	
			//Preencher a tabela "State
			if (projectManaContext.State.Any()) return;

			projectManaContext.State.AddRange(
				new State { StateValue = "Not Started" },
				new State { StateValue = "In Progress" },
				new State { StateValue = "Finished" },
				new State { StateValue = "Late" }
				);
			projectManaContext.SaveChanges();

			//Preencher a tabela "P_Task"
			if (projectManaContext.P_Task.Any()) return;

				projectManaContext.P_Task.AddRange(
					new P_Task { P_TaskName = "Teste_1", Comentary = "Ola primeira Tarefa", StateId = 1, ProjectId = 1
					, CreationDate = DateTime.Now.Date, Deadline = DateTime.Now.Date },
					new P_Task { P_TaskName = "Teste_1.2", Comentary = "Ola segunda Tarefa", StateId = 1, ProjectId = 1
					, CreationDate = DateTime.Now.Date, Deadline = DateTime.Now.Date },
					new P_Task { P_TaskName = "Teste_2", Comentary = "Ola primeira Tarefa", StateId = 1, ProjectId = 2
					, CreationDate = DateTime.Now.Date, Deadline = DateTime.Now.Date },
					new P_Task { P_TaskName = "Teste_2.1", Comentary = "Ola segunda Tarefa", StateId = 1, ProjectId = 2
					, CreationDate = DateTime.Now.Date, Deadline = DateTime.Now.Date }
					); ;
				projectManaContext.SaveChanges();
			
#if TEST_PAGINATION_MEMBERS
            Function function = projectManaContext.Function.FirstOrDefault();
            Member member = projectManaContext.Member.FirstOrDefault();

            if (function == null)
            {
                function = new Function { Name = "Anonymous" };
                projectManaContext.Add(function);
            }

            if (member == null)
            {
                for (int i = 1; i <= 1000; i++)
                {
                    projectManaContext.Member.Add(
                        new Member
                        {
                            Name = "Member " + i,
                            Email = "membertest" + i + "@ipg.pt",
			    PhoneNumber = "987654321",
                            EmployeeNumber = "" + i,
                            Function = function
                        });
                }
                projectManaContext.SaveChanges();
            }
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
						*/
        }

		internal static void CreateDefaultAdmin(UserManager<IdentityUser> userManager)
		{
			EnsureUserIsCreatedAsync(userManager, ADMIN_EMAIL, ADMIN_PASS, ROLE_ADMINISTRATOR).Wait();
		}

		private static async Task EnsureUserIsCreatedAsync(UserManager<IdentityUser> userManager, string email, string password, string role)
		{
			var user = await userManager.FindByNameAsync(email);

			if (user == null)
			{
				user = new IdentityUser
				{
					UserName = email,
					Email = email
				};

				await userManager.CreateAsync(user, password);
			}

			if (await userManager.IsInRoleAsync(user, role)) return;
			await userManager.AddToRoleAsync(user, role);
		}

		internal static void PopulateUsers(UserManager<IdentityUser> userManager)
		{
			//EnsureUserIsCreatedAsync(userManager, "john@ipg.pt", "Secret123$", ROLE_CUSTOMER).Wait();
			//EnsureUserIsCreatedAsync(userManager, "mary@ipg.pt", "Secret123$", ROLE_PRODUCT_MANAGER).Wait();
		}
		internal static void CreateRoles(RoleManager<IdentityRole> roleManager)
		{
			EnsureRoleIsCreatedAsync(roleManager, ROLE_ADMINISTRATOR).Wait();
			EnsureRoleIsCreatedAsync(roleManager, ROLE_MANAGER).Wait();
			EnsureRoleIsCreatedAsync(roleManager, ROLE_MEMBER).Wait();
		}

		private static async Task EnsureRoleIsCreatedAsync(RoleManager<IdentityRole> roleManager, string role)
		{
			if (await roleManager.RoleExistsAsync(role)) return;

			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}
}
