using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TrafiguraAssessment.Domain.Entities;

public partial class AssessmentDBContext : DbContext
{
    public AssessmentDBContext()
    {
    }

    public AssessmentDBContext(DbContextOptions<AssessmentDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TradeTransaction> TradeTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=Noorie\\NOORIETECH;user=buraaqsa;password=tr!@6p&ZrM#6Ve;database=AssessmentDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TradeTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__TradeTra__55433A4B29E96304");

            entity.HasIndex(e => e.TradeId, "IX_TradeTxn_TradeID");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Action).HasMaxLength(10);
            entity.Property(e => e.BuySell).HasMaxLength(4);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SecurityCode).HasMaxLength(10);
            entity.Property(e => e.TradeId).HasColumnName("TradeID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
