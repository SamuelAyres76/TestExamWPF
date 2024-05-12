using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestExamWPF
{
    public partial class MainWindow : Window
    {
        private MovieClass db;

        public MainWindow()
        {
            InitializeComponent();
            db = new MovieClass();
            LoadMovies(); // Call the method to load movies when the window is initialized
        }

        // LINQ Query.
        // Method to load movies from the database and display them in the Listbox
        private void LoadMovies()
        {
            try
            {
                var movies = db.Movies.ToList(); // Retrieve all movies from the database
                listBoxMovies.ItemsSource = movies; // Set the list of movies as the ItemsSource of the Listbox
                listBoxMovies.DisplayMemberPath = "Title"; // Specify the property to display in the Listbox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Displaying Movies: " + ex.Message); // Display an error message if loading movies fails
            }
        }

        // Selection Changed.
        // Event handler for when the selection in the Listbox changes
        private void ListBoxMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMovie = listBoxMovies.SelectedItem as Movie;

            if (selectedMovie != null)
            {
                textBoxSynopsis.Text = selectedMovie.Description; // Display the description of the selected movie
                // Calculate available seats
                var bookings = db.Bookings.Where(b => b.MovieID == selectedMovie.MovieID).ToList(); // Retrieve bookings for the selected movie
                textBoxAvailableSeats.Text = bookings.FirstOrDefault()?.NumberOfTicketsLeft.ToString(); // Display the number of available seats
            }
        }

        // Date Changed.
        // Event handler for when the selected date in the DatePicker changes
        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMovie = listBoxMovies.SelectedItem as Movie;
            if (selectedMovie != null && DatePickerForMovies.SelectedDate.HasValue)
            {
                var selectedDate = DatePickerForMovies.SelectedDate.Value.Date;
                var booking = selectedMovie.Bookings.FirstOrDefault(b => b.BookingDate.Date == selectedDate);
                if (booking != null)
                {
                    textBoxAvailableSeats.Text = booking.NumberOfTicketsLeft.ToString();
                }
                else
                {
                    textBoxAvailableSeats.Text = "100";
                }
            }
        }

        // Button Clicked to Book Seats.
        // Event handler for when the "Book Seats" button is clicked
        private void ButtonBookSeats_Click(object sender, RoutedEventArgs e)
        {
            var selectedMovie = listBoxMovies.SelectedItem as Movie;

            try
            {
                var requiredSeats = int.Parse(textBoxRequiredSeats.Text);
                var bookingDate = DatePickerForMovies.SelectedDate.Value.Date;

                // Retrieve existing booking or create a new one
                var existingBooking = db.Bookings.FirstOrDefault(b => b.MovieID == selectedMovie.MovieID && b.BookingDate == bookingDate);

                if (existingBooking != null)
                {
                    if (requiredSeats > existingBooking.NumberOfTicketsLeft)
                    {
                        throw new Exception("Not enough seats available.");
                    }
                    existingBooking.NumberOfTicketsLeft -= requiredSeats; // Update the existing booking
                }
                else
                {
                    // Create a new booking if there is no existing booking for that date
                    var newBooking = new Booking
                    {
                        BookingDate = bookingDate,
                        NumberOfTicketsLeft = 100 - requiredSeats, // Start with 100 tickets, then subtract the required seats
                        MovieID = selectedMovie.MovieID
                    };
                    db.Bookings.Add(newBooking); // Add the new booking to the database
                }

                db.SaveChanges(); // Save changes to the database
                UpdateAvailableSeats(); // Refresh the display of available seats
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Booking Seats: " + ex.Message); // Display an error message if booking seats fails
            }
        }

        // Update Available Seats.
        // Method to update the display of available seats
        private void UpdateAvailableSeats()
        {
            var selectedMovie = listBoxMovies.SelectedItem as Movie;
            if (selectedMovie != null && DatePickerForMovies.SelectedDate.HasValue)
            {
                var selectedDate = DatePickerForMovies.SelectedDate.Value.Date;
                var booking = selectedMovie.Bookings.FirstOrDefault(b => b.BookingDate.Date == selectedDate);
                if (booking != null)
                {
                    textBoxAvailableSeats.Text = booking.NumberOfTicketsLeft.ToString();
                }
                else
                {
                    textBoxAvailableSeats.Text = "100";
                }
            }
        }
    }
}
