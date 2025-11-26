using System;
using System.Collections.Generic;

namespace csaspn_COLL.Controllers
{
    public class Trip
    {
        public string Destination { get; set; } = string.Empty;
        public DateTime TravelDate { get; set; }
        public int Travelers { get; set; }
        public bool IsBooked { get; set; }
        public List<string> Attractions { get; set; } = new List<string>();
    }

    public class TripController
    {
        private static readonly List<Trip> trips = new List<Trip>();

        public IReadOnlyList<Trip> GetTrips()
        {
            return trips;
        }

        public void AddTrip(string destination, DateTime travelDate, int travelers, bool isBooked, List<string> attractions)
        {
            var trip = new Trip
            {
                Destination = destination,
                TravelDate = travelDate,
                Travelers = travelers,
                IsBooked = isBooked,
                Attractions = attractions ?? new List<string>()
            };

            trips.Add(trip);
        }
    }
}
