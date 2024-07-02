using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;

#nullable disable

namespace MacroSociety.Models
{
    public partial class MacroSocietyContext : DbContext
    {
        public MacroSocietyContext()
        {
        }

        public MacroSocietyContext(DbContextOptions<MacroSocietyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CommentForPost> CommentForPosts { get; set; }
        public virtual DbSet<FriendList> FriendLists { get; set; }
        public virtual DbSet<FriendRequest> FriendRequests { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupInvitation> GroupInvitations { get; set; }
        public virtual DbSet<GroupMember> GroupMembers { get; set; }
        public virtual DbSet<GroupPost> GroupPosts { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSong> UserSongs { get; set; }
        public virtual DbSet<CheckVersion> CheckVersions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("database");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<CommentForPost>(entity =>
            {
                entity.ToTable("CommentForPost");

                entity.Property(e => e.NameUserComment)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TextComment)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.IdFriendPostNavigation)
                    .WithMany(p => p.CommentForPosts)
                    .HasForeignKey(d => d.IdFriendPost)
                    .HasConstraintName("FK_CommentForPost_Post");
            });
            modelBuilder.Entity<CheckVersion>(entity =>
            {
                entity.ToTable("CheckVersion");

                entity.Property(e => e.LastVersion)
                    .IsRequired()
                    .HasMaxLength(50);            
            });

            modelBuilder.Entity<FriendList>(entity =>
            {
                entity.ToTable("FriendList");

                entity.Property(e => e.Friendname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdFriendnameNavigation)
                    .WithMany(p => p.FriendListIdFriendnameNavigations)
                    .HasForeignKey(d => d.IdFriendname)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friendlist_Users");

                entity.HasOne(d => d.IdUsernameNavigation)
                    .WithMany(p => p.FriendListIdUsernameNavigations)
                    .HasForeignKey(d => d.IdUsername)
                    .HasConstraintName("FK_Friendlist_Users1");
            });

            modelBuilder.Entity<FriendRequest>(entity =>
            {
                entity.Property(e => e.FutureFriend)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.FriendRequests)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Friend_Requests_Users");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.HasIndex(e => e.IdUser, "FK_List_Level_User")
                    .IsUnique();

                entity.Property(e => e.LevelCompleted).HasDefaultValueSql("((1))");

                entity.Property(e => e.NameUser)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserReceivedGamePrize)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDefaultValueSql("('Нет')");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithOne(p => p.Game)
                    .HasForeignKey<Game>(d => d.IdUser)
                    .HasConstraintName("FK_List_Level_User_Users");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Groups_Users");
            });

            modelBuilder.Entity<GroupInvitation>(entity =>
            {
                entity.Property(e => e.InvitationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ResponseDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupInvitations)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupInvitations_Groups");

                entity.HasOne(d => d.InvitedByNavigation)
                    .WithMany(p => p.GroupInvitationInvitedByNavigations)
                    .HasForeignKey(d => d.InvitedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.InvitedUser)
                    .WithMany(p => p.GroupInvitationInvitedUsers)
                    .HasForeignKey(d => d.InvitedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.UserId });

                entity.Property(e => e.JoinDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupMembers_Groups");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupMembers_Users");
            });

            modelBuilder.Entity<GroupPost>(entity =>
            {
                entity.Property(e => e.PostContent).IsRequired();

                entity.Property(e => e.PostDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPosts)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupPosts_Groups");

                entity.HasOne(d => d.PostedByNavigation)
                    .WithMany(p => p.GroupPosts)
                    .HasForeignKey(d => d.PostedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupPosts_Users");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.ToTable("Like");

                entity.Property(e => e.NameUserLike)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.WhosePost)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdFriendPostNavigation)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.IdFriendPost)
                    .HasConstraintName("FK_Like_Post1");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.NameUser)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Post_Users");               
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.IsOnline)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.Money)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Photo).IsRequired();

                entity.Property(e => e.SubscriptionGame)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.SubscriptionMusic)
                    .IsRequired()
                    .HasMaxLength(3);
            });

            modelBuilder.Entity<UserSong>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.SongId })
                    .HasName("PK__User_Son__4D2535A5BBB3E0DF");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.SongId).HasColumnName("Song_Id");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.UserSongs)
                    .HasForeignKey(d => d.SongId)
                    .HasConstraintName("FK__User_Song__Song___3BCADD1B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSongs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__User_Song__User___3AD6B8E2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
