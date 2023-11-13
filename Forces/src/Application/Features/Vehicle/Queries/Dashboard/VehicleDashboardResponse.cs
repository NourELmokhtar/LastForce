using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Vehicle.Queries.Dashboard
{
    public class VehicleDashboardResponse
    {
        public int AllVehicleCount { get; set; } = 0;
        public int AllVehicleCountP { get; set; } = 0;
        public int AllVehicleCountS { get; set; } = 0;
        public int OnServiceCount { get; set; } = 0;
        public int OnServiceP { get; set; } = 0;
        public int OnServiceS { get; set; } = 0;
        public int AvilableCount { get; set; } = 0;
        public int AvilableCountP { get; set; } = 0;
        public int AvilableCountS { get; set; } = 0;
        public int NotAvilableCount { get; set; } = 0;
        public int NotAvilableCountP { get; set; } = 0;
        public int NotAvilableCountS { get; set; } = 0;
        public int TrafficViolations { get; set; } = 0;
        public int TrafficViolationsP { get; set; } = 0;
        public int TrafficViolationsS { get; set; } = 0;
        public int BookedCount { get; set; } = 0;
        public int BookedCountP { get; set; } = 0;
        public int BookedCountS { get; set; } = 0;
        public int PassengersRequestCount { get; set; } = 0;
        public int CarrageRequestCount { get; set; } = 0;
        public int AllRequestsCount { get; set; } = 0;
        public int AllRequestsCountP { get; set; } = 0;
        public int AllRequestsCountS { get; set; } = 0;
        public int CompletedRequests { get; set; } = 0;
        public int CompletedRequestsP { get; set; } = 0;
        public int CompletedRequestsS { get; set; } = 0;
        public int PendingRequests { get; set; } = 0;
        public int PendingRequestsP { get; set; } = 0;
        public int PendingRequestsS { get; set; } = 0;
        public int RejectedRequests { get; set; } = 0;
        public int PetrolCount { get; set; } = 0;
        public int DiselCount { get; set; } = 0;


    }
}
