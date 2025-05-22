using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AskIT.Models;

public partial class WhatsAppDbContext : DbContext
{
    public WhatsAppDbContext()
    {
    }

    public WhatsAppDbContext(DbContextOptions<WhatsAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<KnowledgeBase> KnowledgeBases { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KnowledgeBase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Knowledg__3214EC07F13AE917");

            entity.ToTable("KnowledgeBase");

            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC0762414054");

            entity.Property(e => e.NeedsHumanSupport).HasDefaultValue(false);
            entity.Property(e => e.SenderType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.KnowledgeBase).WithMany(p => p.Messages)
                .HasForeignKey(d => d.KnowledgeBaseId)
                .HasConstraintName("FK__Messages__Knowle__5FB337D6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
