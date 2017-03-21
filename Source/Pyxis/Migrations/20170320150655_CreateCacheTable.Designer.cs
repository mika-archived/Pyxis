using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Pyxis.Models.Database;

namespace Pyxis.Migrations
{
    [DbContext(typeof(CacheContext))]
    [Migration("20170320150655_CreateCacheTable")]
    partial class CreateCacheTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Pyxis.Models.Database.Cache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Path");

                    b.Property<ulong>("Size");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Caches");
                });
        }
    }
}
