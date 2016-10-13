using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Pyxis.Models.Cache;

namespace Pyxis.Migrations
{
    [DbContext(typeof(CacheContext))]
    [Migration("20161012172023_AddReferencedAtToCacheFile")]
    partial class AddReferencedAtToCacheFile
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("Pyxis.Models.Cache.CacheFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Path");

                    b.Property<DateTime>("ReferencedAt");

                    b.Property<long>("Size");

                    b.HasKey("Id");

                    b.ToTable("CacheFiles");
                });
        }
    }
}
