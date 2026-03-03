using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestiPro.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GestiPro.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração das entidades
        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Idade).IsRequired();

            // Cascade delete: ao remover uma pessoa, deleta todas as transações também
            entity.HasMany(p => p.Transacoes)
                  .WithOne(t => t.Pessoa)
                  .HasForeignKey(t => t.IdPessoa)
                  .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Descricao).IsRequired().HasMaxLength(400);
            entity.Property(c => c.Finalidade).IsRequired();
        });

        
        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Descricao).IsRequired().HasMaxLength(400);
            entity.Property(t => t.Valor).IsRequired().HasPrecision(18, 2);
            entity.Property(t => t.Tipo).IsRequired();

            // Restrict Delete: não remover categorias ao deletar transações
            entity.HasOne(t => t.Categoria)
                  .WithMany(c => c.Transacoes)
                  .HasForeignKey(t => t.IdCategoria)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

}
