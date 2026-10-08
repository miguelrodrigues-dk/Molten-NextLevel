using HelpDeskMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskMvc.Data

{
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

//A tabela que guarda o catalogo da MOLTEN
    public DbSet<Filme> Filmes { get; set; }


  public DbSet<MoltenUsuario> Usuarios { get; set; }

}



}




