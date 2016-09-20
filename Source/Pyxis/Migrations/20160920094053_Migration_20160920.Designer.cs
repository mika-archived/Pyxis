using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Pyxis.Models.Cache;

namespace Pyxis.Migrations
{
    [DbContext(typeof(CacheContext))]
    [Migration("20160920094053_Migration_20160920")]
    partial class Migration_20160920
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Pyxis.Models.Cache.CacheFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Path");

                    b.Property<int>("Size");

                    b.HasKey("Id");

                    b.ToTable("CacheFiles");
                });
        }
    }
}
