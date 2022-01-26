//#define TEST_PAGINATION_MEMBERS
//#define TEST_PAGINATION_TASKS
//#define TEST_PAGINATION_PROJECTS
//#define TEST_PAGINATION_MEETINGS

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
			State state = projectManaContext.State.FirstOrDefault();
			
			//Preencher a tabela "State
			if (state == null) { 
			//if (projectManaContext.State.Any()) return;

			projectManaContext.State.AddRange(
				new State { StateValue = "Not Started" },
				new State { StateValue = "In Progress" },
				new State { StateValue = "Finished" },
				new State { StateValue = "Late" }
				);
			projectManaContext.SaveChanges();
			}

#if TEST_PAGINATION_TASKS
			P_Task task = projectManaContext.P_Task.FirstOrDefault();
			if (task == null)
			{
				//Preencher a tabela "P_Task"
				//if (projectManaContext.P_Task.Any()) return;

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
			}
#endif

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

			//int year, int month, int day, int hour, int minute, int second
#if TEST_PAGINATION_PROJECTS

			Project project = projectManaContext.Project.FirstOrDefault();

			if(project == null)
            {

							projectManaContext.Project.Add(
								new Project {
									Name = "Montar carro",
									Description = "E preciso primeiro montar a carrocaria",
									ProjectCreatorId = 123,
									StartDate = new DateTime(2022, 1, 25, 5, 26, 40),
									//FinishDate = new DateTime(),
									DecisiveDeliveryDate = new DateTime(2022, 3, 25, 6, 0, 0)
								});

							projectManaContext.Project.Add(
								new Project{
									Name = "Fazer o jogo 2048",
									Description = "E necessario ter numeros",
									ProjectCreatorId = 1234,
									StartDate = new DateTime(2022, 1, 25, 5, 26, 40),
									//FinishDate = new DateTime(),
									DecisiveDeliveryDate = new DateTime(2022, 3, 25, 7, 12, 10)
								});

							projectManaContext.Project.Add(
								new Project{
									Name = "Dadores for us",
									Description = "O site deve ser desenvolvido para dadores de sangue assiduos",
									ProjectCreatorId = 12345,
									StartDate = new DateTime(2022, 1, 25, 5, 26, 40),
									//FinishDate = new DateTime(),
									DecisiveDeliveryDate = new DateTime(2022, 3, 25, 8, 5, 5)
								});

							projectManaContext.Project.Add(
								new Project{
									Name = "Engenharia de Software e Programacao",
									Description = "O site deve ser desenvolvido em c#, arquitetura MVC",
									ProjectCreatorId = 123456,
									StartDate = new DateTime(2021, 1, 25, 9, 26, 40),
									//FinishDate = new DateTime(),
									DecisiveDeliveryDate = new DateTime(2022, 5, 25, 8, 5, 30)
								});

							projectManaContext.SaveChanges();
			}
#endif

#if TEST_PAGINATION_MEETINGS

			Meeting meeting = projectManaContext.Meeting.FirstOrDefault();

			if(meeting == null)
            {

							projectManaContext.Meeting.Add(
								new Meeting {
									Topic = "Recolha de requisitos",
									Description = "A reuniao e feita para recolha dos requisitos de software com o cliente presente",
									ProjectId = 123,
									DateandTime = new DateTime(2022, 2, 27, 9, 9, 0),
								});

							projectManaContext.Meeting.Add(
								new Meeting
								{
									Topic = "Recolha de modelos",
									Description = "A reuniao e feita para recolha dos modelos do projeto",
									ProjectId = 1234,
									DateandTime = new DateTime(2022, 1, 26, 9, 9, 0),
								});

				projectManaContext.Meeting.Add(
								new Meeting{
									Topic = "Recolha de modelos",
									Description = "A reuniao e feita para recolha dos modelos do projeto",
									ProjectId = 12345,
									DateandTime = new DateTime(2022, 1, 26, 9, 9, 0),
								});

							projectManaContext.SaveChanges();
			}
#endif
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
