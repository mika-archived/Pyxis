using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Pyxis.Models.Caching;

namespace Pyxis.Migrations
{
    [DbContext(typeof(CacheContext))]
    [Migration("20161108020313_CreateCacheFile")]
    partial class CreateCacheFile
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("Pyxis.Models.Caching.CacheFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Category");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Path");

                    b.Property<long>("Size");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("CacheFiles");
                });
        }
    }
}
