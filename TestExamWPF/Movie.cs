using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExamWPF
{
    // Represents a movie entity
    public class Movie
    {
        public int MovieID { get; set; } // Unique identifier for the movie
        public string Title { get; set; } // Title of the movie
        public string Description { get; set; } // Description of the movie
        public string ImageName { get; set; } // Name of the image associated with the movie
        public string Cast { get; set; } // Cast of the movie

        // Navigation property to represent the relationship between Movie and Booking entities
        public virtual ICollection<Booking> Bookings { get; set; }

        // Constructor to initialize the Bookings collection
        public Movie()
        {
            Bookings = new List<Booking>();
        }

        // Method to add a booking for the movie on a specified date
        public Booking AddBooking(DateTime date)
        {
            var booking = Bookings.FirstOrDefault(b => b.BookingDate.Date == date.Date);
            if (booking == null)
            {
                // Create a new booking if no booking exists for the specified date
                booking = new Booking
                {
                    BookingDate = date.Date,
                    NumberOfTicketsLeft = 100, // default ticket count
                    MovieID = this.MovieID, // Set the foreign key MovieID
                    Movie = this // Set the navigation property Movie
                };
                Bookings.Add(booking); // Add the booking to the collection
            }
            return booking;
        }

        // Method to book tickets for the movie on a specified date
        public string BookTickets(DateTime date, int ticketCount)
        {
            var booking = AddBooking(date); // Add a booking for the specified date
            if (booking.NumberOfTicketsLeft >= ticketCount)
            {
                // If there are enough tickets left, book the tickets and return a success message
                booking.NumberOfTicketsLeft -= ticketCount;
                return "Booking successful. Tickets left: " + booking.NumberOfTicketsLeft;
            }
            else
            {
                // If there are not enough tickets left, return a failure message
                return "Not enough tickets available. Tickets left: " + booking.NumberOfTicketsLeft;
            }
        }
    }

    // Represents a booking entity
    public class Booking
    {
        public int BookingID { get; set; } // Unique identifier for the booking
        public DateTime BookingDate { get; set; } // Date of the booking
        public int NumberOfTicketsLeft { get; set; } // Number of tickets left for the booking
        public int MovieID { get; set; } // Foreign key for the associated movie
        public virtual Movie Movie { get; set; } // Navigation property to represent the associated movie
    }

    // Read information from the database using a LINQ query and display movie titles in the Listbox to the left of the screen.
    // Represents the database context for managing movies and bookings
    public class MovieClass : DbContext
    {
        // Constructor to specify the database name
        public MovieClass(string dbName) : base(dbName) { }

        // Default constructor to use a default database name
        public MovieClass() : this("OODExam_SamuelAyres") { }

        // DbSet properties to represent the collections of movies and bookings in the database
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
