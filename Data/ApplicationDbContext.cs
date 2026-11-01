using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<ShowSeat> ShowSeats { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Movie configuration
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId);

                entity.Property(m => m.Title)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(m => m.Description)
                    .HasMaxLength(500);

                entity.Property(m => m.DurationMinutes)
                    .IsRequired();

                entity.Property(m => m.ReleaseDate)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(m => m.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.Genre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Movies_DurationMinutes",
                    "[DurationMinutes] > 0"));
            });

            // Theatre configuration
            modelBuilder.Entity<Theatre>(entity =>
            {
                entity.HasKey(t => t.TheatreId);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(t => t.Location)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            // Screen configuration
            modelBuilder.Entity<Screen>(entity =>
            {
                entity.HasKey(s => s.ScreenId);

                entity.Property(s => s.ScreenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne<Theatre>()
                    .WithMany()
                    .HasForeignKey(s => s.TheatreId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(s => s.SeatId);

                entity.Property(s => s.RowName)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(s => s.SeatNumber)
                    .IsRequired();

                entity.Property(s => s.SeatType)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne<Screen>()
                    .WithMany()
                    .HasForeignKey(s => s.ScreenId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new
                {
                    s.ScreenId,
                    s.RowName,
                    s.SeatNumber
                }).IsUnique();

                entity.ToTable(t =>
                    t.HasCheckConstraint(
                        "CK_Seats_SeatNumber",
                        "[SeatNumber] > 0"));
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.HasKey(s => s.ShowId);

                entity.Property(s => s.StartTime)
                    .IsRequired();

                entity.Property(s => s.EndTime)
                    .IsRequired();

                entity.Property(s => s.TicketPrice)
                    .HasPrecision(10, 2)
                    .IsRequired();

                entity.HasOne<Movie>()
                    .WithMany()
                    .HasForeignKey(s => s.MovieId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Screen>()
                    .WithMany()
                    .HasForeignKey(s => s.ScreenId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Shows_EndTime",
                        "[EndTime] > [StartTime]");

                    t.HasCheckConstraint(
                        "CK_Shows_TicketPrice",
                        "[TicketPrice] >= 0");
                });
            });
            modelBuilder.Entity<ShowSeat>(entity =>
            {
                entity.HasKey(ss => ss.ShowSeatId);

                entity.Property(ss => ss.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne<Show>()
                    .WithMany()
                    .HasForeignKey(ss => ss.ShowId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Seat>()
                    .WithMany()
                    .HasForeignKey(ss => ss.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(ss => new { ss.ShowId, ss.SeatId })
                    .IsUnique();

                entity.ToTable(t =>
                    t.HasCheckConstraint(
                        "CK_ShowSeats_Status",
                        "[Status] IN ('Available', 'Booked')"));

                entity.HasOne(ss => ss.Show)
                .WithMany()
    .HasForeignKey(ss => ss.ShowId)
    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ss => ss.Seat)
                    .WithMany()
                    .HasForeignKey(ss => ss.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}