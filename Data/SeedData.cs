#define DADOS_FICTICIOS
//#define TEST_PAGINATION_TASKS


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
			if (state == null)
			{
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

#if DADOS_FICTICIOS
			Function function = projectManaContext.Function.FirstOrDefault();
			Function function2;
			Member member = projectManaContext.Member.FirstOrDefault();

			if (function == null)
			{
				function = new Function { Name = "Programmer" };
				function2 = new Function { Name = "Tester" };
				projectManaContext.Add(function);
				projectManaContext.Add(function2);


				member = new Member
				{
					Name = "Victor Seguro",
					Email = "victors@ipg.pt",
					PhoneNumber = "987654321",
					EmployeeNumber = "1",
					Function = function
				};
				projectManaContext.Member.Add(member);

				Member member2 =
				new Member
				{
					Name = "Gonçalo Silva",
					Email = "goncalos@ipg.pt",
					PhoneNumber = "987567821",
					EmployeeNumber = "2",
					Function = function2
				};
				projectManaContext.Member.Add(member2);
				Member member3 =
				new Member
				{
					Name = "Daniel Carmona",
					Email = "danielc@ipg.pt",
					PhoneNumber = "988567821",
					EmployeeNumber = "3",
					Function = function
				};
				projectManaContext.Member.Add(member3);

				projectManaContext.SaveChanges();
			


			//int year, int month, int day, int hour, int minute, int second

			 



				Project project =
					new Project
					{
						Name = "Montar carro",
						Description = "E preciso primeiro montar a carrocaria",
						ProjectCreatorId = 3,
						StartDate = new DateTime(2022, 1, 25, 5, 26, 40),
									//FinishDate = new DateTime(),
									DecisiveDeliveryDate = new DateTime(2022, 3, 25, 6, 0, 0)
						,
						StateId = 2
					}; 
					projectManaContext.Project.Add(project);
					
					Project project2=
					new Project
					{
						Name = "Fazer o jogo 2048",
						Description = "E necessario ter numeros",
						ProjectCreatorId = 3,
						StartDate = new DateTime(2022, 1, 25, 5, 26, 40),
						//FinishDate = new DateTime(),
						DecisiveDeliveryDate = new DateTime(2022, 3, 25, 7, 12, 10)
						,
						StateId = 2
					};
					projectManaContext.Project.Add(project2); 

					Project project3=
					new Project
					{
						Name = "Dadores for us",
						Description = "O site deve ser desenvolvido para dadores de sangue assiduos",
						ProjectCreatorId = 3,
						StartDate = new DateTime(2022, 1, 25, 5, 26, 40),
									//FinishDate = new DateTime(),
									DecisiveDeliveryDate = new DateTime(2022, 3, 25, 8, 5, 5)
						,
						StateId = 2
					};
					projectManaContext.Project.Add(project3);

					Project project4=
					new Project
					{
						Name = "Engenharia de Software e Programacao",
						Description = "O site deve ser desenvolvido em c#, arquitetura MVC",
						ProjectCreatorId = 3,
						StartDate = new DateTime(2021, 1, 25, 9, 26, 40),
									//FinishDate = new DateTime(),
									DecisiveDeliveryDate = new DateTime(2022, 5, 25, 8, 5, 30)
						,
						StateId = 2
					};
					projectManaContext.Project.Add(project4);
					projectManaContext.SaveChanges();

				projectManaContext.MemberProject.Add(
					new MemberProject
					{
						ProjectId=project.ProjectId,
						MemberId=member3.MemberId
					}
					);
				projectManaContext.MemberProject.Add(
					new MemberProject
					{
						ProjectId = project2.ProjectId,
						MemberId = member3.MemberId
					}
					);
				projectManaContext.MemberProject.Add(
					new MemberProject
					{
						ProjectId = project3.ProjectId,
						MemberId = member3.MemberId
					}
					);
				projectManaContext.MemberProject.Add(
					new MemberProject
					{
						ProjectId = project4.ProjectId,
						MemberId = member3.MemberId
					}
					);
				projectManaContext.SaveChanges();
			
			


			Meeting meeting = projectManaContext.Meeting.FirstOrDefault();

			

				projectManaContext.Meeting.Add(
					new Meeting
					{
						Topic = "Recolha de requisitos",
						Description = "A reuniao e feita para recolha dos requisitos de software com o cliente presente",
						ProjectId = project.ProjectId,
						DateandTime = new DateTime(2022, 2, 27, 9, 9, 0),

					});

				projectManaContext.Meeting.Add(
					new Meeting
					{
						Topic = "Recolha de modelos",
						Description = "A reuniao e feita para recolha dos modelos do projeto",
						ProjectId = project2.ProjectId,
						DateandTime = new DateTime(2022, 1, 26, 9, 9, 0),
					});

				projectManaContext.Meeting.Add(
								new Meeting
								{
									Topic = "Recolha de modelos",
									Description = "A reuniao e feita para recolha dos modelos do projeto",
									ProjectId = project3.ProjectId,
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
			EnsureUserIsCreatedAsync(userManager, "victors@ipg.pt", "Secret123$", ROLE_MEMBER).Wait();
			EnsureUserIsCreatedAsync(userManager, "goncalos@ipg.pt", "Secret123$", ROLE_MEMBER).Wait();
			EnsureUserIsCreatedAsync(userManager, "danielc@ipg.pt", "Secret123$", ROLE_MANAGER).Wait();
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
