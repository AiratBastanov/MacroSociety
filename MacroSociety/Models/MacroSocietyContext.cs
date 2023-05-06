using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source = SQL8002.site4now.net; Initial Catalog = db_a98760_macrosociety; User Id = db_a98760_macrosociety_admin; Password = 2060AmdPC!");
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

                entity.Property(e => e.Id).ValueGeneratedNever();

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
