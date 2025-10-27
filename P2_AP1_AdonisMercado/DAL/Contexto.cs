using Microsoft.EntityFrameworkCore;
using P2_AP1_AdonisMercado.Models;

namespace P2_AP1_AdonisMercado.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public DbSet<Registros> Registros { get; set; }
}