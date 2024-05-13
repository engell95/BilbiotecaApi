using System;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaApi.DbModels;
public class DBSeeder
{
    public static void Seed(BibliotecaDbContext context)
	{
		var guids = new List<string>();
		for (int i = 0; i < 6; i++)
		{
			guids.Add(Guid.NewGuid().ToString());
		}

		//Roles
		if (!context.Roles.Any())
		{

			var roles = new List<IdentityRole>
			{
				new IdentityRole
				{
					Name = "Administrador",
					NormalizedName = "Administrador".ToUpper(),
					Id = guids[4],
					ConcurrencyStamp = guids[4]
				},
				new IdentityRole
				{
					Name = "Estudiante",
					NormalizedName = "Estudiante".ToUpper(),
					Id = guids[5],
					ConcurrencyStamp = guids[5]
				}
			};

			context.AddRange(roles);
		}

		//Usuarios
		if (!context.Users.Any())
		{	

			var userAdmin = new IdentityUser
			{
				UserName = "Administrador",
				Email = "Admin@test.com",
				NormalizedEmail = "Admin@test.com".ToUpper(),
				NormalizedUserName = "Admin".ToUpper(),
				Id =  guids[1]
			};

			var user1 = new IdentityUser
			{
				UserName = "Usuario1",
				Email = "User1@test.com",
				NormalizedEmail = "User1@test.com".ToUpper(),
				NormalizedUserName = "Usuario1".ToUpper(),
				Id = guids[2]
			};

			var user2 = new IdentityUser
			{
				UserName = "Usuario2",
				Email = "User2@test.com",
				NormalizedEmail = "User2@test.com".ToUpper(),
				NormalizedUserName = "Usuario2".ToUpper(),
				Id = guids[3]
			};

			userAdmin.PasswordHash = new PasswordHasher<IdentityUser>()
			.HashPassword(userAdmin,"Superadmin@123");

			user1.PasswordHash = new PasswordHasher<IdentityUser>()
			.HashPassword(user1,"Seguro@123");

			user2.PasswordHash = new PasswordHasher<IdentityUser>()
			.HashPassword(user2,"Seguro@123");

			var users = new List<IdentityUser<string>>{userAdmin,user1,user2};

			context.AddRange(users);
		}
		
		//Roles - Usuario
		if (!context.UserRoles.Any()){
			var userRoles = new List<IdentityUserRole<string>>
			{
				new IdentityUserRole<string>
				{
					RoleId = guids[5],
					UserId = guids[2]
				},
				new IdentityUserRole<string>
				{
					RoleId = guids[4],
					UserId = guids[1]
				},
				new IdentityUserRole<string>
				{
					RoleId = guids[5],
					UserId = guids[3]
				}
			};
			context.AddRange(userRoles);
		}

		//Autores
		if (!context.Autores.Any())
		{
            var Autores = new List<Autor> {
			    new Autor{Nombre = "Octavio Paz",Estado = true },
				new Autor{Nombre = "Carlos Fuentes",Estado = true },
				new Autor{Nombre = "Juan Rulfo" ,Estado = true},
				new Autor{Nombre = "Elena Poniatowska" ,Estado = true},
				new Autor{Nombre = "Laura Esquivel" ,Estado = true},
				new Autor{Nombre = "Carlos Monsiváis" ,Estado = true},
				new Autor{Nombre = "Sergio Pitol" ,Estado = true},
				new Autor{Nombre = "Juan José Arreola" ,Estado = true},
				new Autor{Nombre = "Rosario Castellanos" ,Estado = true},
				new Autor{Nombre = "Alfonso Reyes" },
		    };
            
			context.AddRange(Autores);
		}

		//Editoriales
		if (!context.Editoriales.Any())
		{
            var Autores = new List<Editorial> {
			    new Editorial{Nombre = "Fondo de Cultura Económica (FCE)",Estado = true },
				new Editorial{Nombre = "Editorial Planeta Mexicana",Estado = true },
				new Editorial{Nombre = "Siglo XXI Editores" ,Estado = true},
				new Editorial{Nombre = "Ediciones Castillo" ,Estado = true},
				new Editorial{Nombre = "Editorial Diana" ,Estado = true},
				new Editorial{Nombre = "Editorial Alfaguara México" ,Estado = true}
		    };
            
			context.AddRange(Autores);
		}
		
		//Libros
		if (!context.Libros.Any())
		{
            var Libros = new List<Libro> {
			    new Libro{Nombre = "Corrientes alternas. Antología de verso y prosa",Descripcion="Una antología definitiva para adentrarse en el universo literario del mayor poeta mexicano contemporáneo",Copias = 10,Fecha_Publicacion = DateTime.Now,Id_Editorial=1,Id_Autor=1 ,Estado = true },
				new Libro{Nombre = "El tiempo que nos hizo nos deshace",Descripcion="El tiempo que nos hizo nos deshace se integra en la colección Poesía Portátil como una selección de los versos más representativos de Octavio Paz.",Copias = 5,Fecha_Publicacion = DateTime.Now,Id_Editorial=3,Id_Autor=1 ,Estado = true },
				new Libro{Nombre = "Obra poética (1935-1998)",Descripcion="En este volumen el lector tiene ante sí la completa producción poética de uno de los principales exponentes del género del siglo XX, además de su acercamiento a la poesía universal en calidad de traductor",Copias = 3,Fecha_Publicacion = DateTime.Now,Id_Editorial=4,Id_Autor=1 ,Estado = true },
				new Libro{Nombre = "Libertad bajo palabra",Descripcion="En palabras del propio Octavio Paz este libro se fue haciendo poco a poco a través de los años, sin un plan fijo",Copias = 0,Fecha_Publicacion = DateTime.Now,Id_Editorial=1,Id_Autor=1 ,Estado = true },
				new Libro{Nombre = "Pedro Páramo",Descripcion="Cuando al final de la década de los sesenta la narrativa hispanoamericana alcanzó un prestigio mundial, se volvió la vista atrás en busca de sus 'clásicos'. La figura gigantesca de Rulfo destacó inmediatamente.",Copias = 5,Fecha_Publicacion = DateTime.Now,Id_Editorial=3,Id_Autor=2 ,Estado = true },
				new Libro{Nombre = "El llano en llamas",Descripcion="La presente edición limitada conmemora el 70 aniversario de la publicación de la obra de Juan Rulfo.",Copias = 100,Fecha_Publicacion = DateTime.Now,Id_Editorial=3,Id_Autor=2 ,Estado = true },
				new Libro{Nombre = "Antología personal",Descripcion="Toda la obra de Juan Rulfo (1918-1986) apareció entre 1953 (fecha de publicación de El Llano en llamas) y 1955 (año de la primera edición de Pedro Páramo), entregándose en adelante el genial narrador prácticamente al silencio durante tres décadas, salvo por El gallo de oro (1980).",Copias = 5,Fecha_Publicacion = DateTime.Now,Id_Editorial=3,Id_Autor=2 ,Estado = true },
				new Libro{Nombre = "Lo que yo vi",Descripcion="Las memorias de la gran autora del realismo mágico mexicano, la crónica de una época salpicada de nostalgia por el tiempo ido y de reflexiones sobre la consciencia del presente",Copias = 10,Fecha_Publicacion = DateTime.Now,Id_Editorial=4,Id_Autor=5 ,Estado = true },
				new Libro{Nombre = "Como agua para chocolate",Descripcion="Y así como un poeta juega con las palabras, así ella jugaba a su antojo con los ingredientes y con las cantidades, obteniendo resultados fenomenales.» No siempre tenemos a mano los ingredientes de la felicidad.",Copias = 5,Fecha_Publicacion = DateTime.Now,Id_Editorial=4,Id_Autor=5,Estado = true },
				new Libro{Nombre = "Contradeseo (Mapa de las lenguas)",Descripcion="Una novela para leer con el mismo afán impúdico con que sus personajes transitan los caminos del deseo. «Una novela sobre la cara más oscura de la amistad, los complicados caminos del deseo y las miserias domésticas.",Copias = 8,Fecha_Publicacion = DateTime.Now,Id_Editorial=4,Id_Autor=5,Estado = true },
		    };
            
			context.AddRange(Libros);
		}

		//Prestamos
		if (!context.Prestamos.Any())
		{
            var Prestamos = new List<Prestamo> {
			    new Prestamo{Id_Libro = 1 ,Id_Usuario = guids[2] ,Fecha_Prestamo = DateTime.Now,Fecha_Devolucion_Esperada = DateTime.Now.AddDays(10)},
				new Prestamo{Id_Libro = 2 ,Id_Usuario = guids[2] ,Fecha_Prestamo = DateTime.Now,Fecha_Devolucion_Esperada = DateTime.Now.AddDays(10)},
				new Prestamo{Id_Libro = 2 ,Id_Usuario = guids[3] ,Fecha_Prestamo = DateTime.Now,Fecha_Devolucion_Esperada = DateTime.Now.AddDays(30)},
				new Prestamo{Id_Libro = 1 ,Id_Usuario = guids[3] ,Fecha_Prestamo = DateTime.Now,Fecha_Devolucion_Esperada = DateTime.Now.AddDays(15)},
				new Prestamo{Id_Libro = 4 ,Id_Usuario = guids[2] ,Fecha_Prestamo = DateTime.Now,Fecha_Devolucion_Esperada = DateTime.Now.AddDays(15)},
				new Prestamo{Id_Libro = 3 ,Id_Usuario = guids[3] ,Fecha_Prestamo = DateTime.Now,Fecha_Devolucion_Esperada = DateTime.Now.AddDays(15)},
			};
            
			context.AddRange(Prestamos);
		}

		context.SaveChanges();
	}

}