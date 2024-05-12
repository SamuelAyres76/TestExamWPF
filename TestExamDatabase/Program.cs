using System;
using TestExamWPF;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TestExamDatabase
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var db = new MovieClass("OODExam_SamuelAyres"))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Create movies
                        var movie1 = new Movie
                        {
                            Title = "The Banshees of Inisherin",
                            Description = "Set on a remote island off the west coast of Ireland, the movie follows lifelong friends...",
                            Cast = "Colin Farrell, Brendan Gleeson, Barry Keoghan",
                            ImageName = "https://th.bing.com/th/id/OIP.alst-1rFLKuP7u2MtilUoAHaLG?w=186&h=279&c=7&r=0&o=5&pid=1.7"
                        };
                        var movie2 = new Movie
                        {
                            Title = "Drive",
                            Description = "A mysterious Hollywood stuntman and mechanic moonlights as a getaway driver and finds himself in trouble when he helps out his neighbor.",
                            Cast = "Ryan Gosling, Carey Mulligan, Bryan Cranston",
                            ImageName = "https://th.bing.com/th/id/OIP.yc0iZZanYrssgaT6HqnH-AHaLH?w=186&h=279&c=7&r=0&o=5&pid=1.7"
                        };
                        var movie3 = new Movie
                        {
                            Title = "La La Land",
                            Description = "While navigating their careers in Los Angeles, a pianist and an actress fall in love while attempting to reconcile their aspirations for the future.",
                            Cast = "Ryan Gosling, Emma Stone, Rosemarie DeWitt",
                            ImageName = "https://th.bing.com/th/id/OIP.A50ToYFonXKzJ9p9lpi2VAHaLH?w=186&h=279&c=7&r=0&o=5&pid=1.7"

                        };

                        // Add the movies (and implicitly the bookings via navigation property) to the database
                        db.Movies.Add(movie1);
                        db.Movies.Add(movie2);
                        db.Movies.Add(movie3);

                        // Save changes to the database
                        db.SaveChanges();

                        transaction.Commit();
                        Console.WriteLine("Movies added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error occurred: " + ex.Message);
                    }
                }
            }
        }
    }
}
