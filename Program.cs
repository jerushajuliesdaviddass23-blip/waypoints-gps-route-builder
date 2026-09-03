using System;
using System.Collections.Generic;
using WayPoints;

namespace StarterCode_WayPoints
{
    internal class Program
    {
        static string FILE_PATH = ""; //set path to solution directory if using VS (not VS code) then we will use "..\\..\\..\\" as path
        static string fileName = "UK_waypoints.csv"; 

        static void Main(string[] args)
        {
            Console.WriteLine("=== WayPoint Store & Route Testing Interactive Script ===\n");

            WayPointStore store = new WayPointStore(FILE_PATH + fileName);

            Console.WriteLine("--- Checklist: Display All waypoints in array ---");
            store.DisplayAll();
            Console.WriteLine("\nTotal waypoints loaded: " + store.Count());

            Console.WriteLine("\n--- Checklist: Search All WayPoints via name ---");
            Console.Write("Enter exact waypoint name to search (e.g., Ambleside): ");
            string exactName = Console.ReadLine() ?? "";
            WayPoint? exactResult = store.SearchByName(exactName);  
            if (exactResult != null)
            {
                exactResult.Display();
            }
            else
            {
                Console.WriteLine("Waypoint not found.");
            }

            Console.WriteLine("\n--- Checklist: Create a class Route ---");
            Console.Write("Enter name for the route (e.g., Route 1): ");
            string route1Name = Console.ReadLine() ?? "Route 1";    
            Route route1 = new Route(route1Name);
            Console.WriteLine("Route created.");

            Console.WriteLine("\n--- Checklist: Add a waypoint object to end of a Route ---");
            Console.Write("Enter exact name of waypoint to add to the END (e.g., Ambleside): ");
            WayPoint? wpEnd = store.SearchByName(Console.ReadLine() ?? ""); 
            if (wpEnd != null) 
            {
                route1.AddEnd(wpEnd);
            }
            else
            {
                Console.WriteLine("Waypoint not found.");
            }
            route1.DisplayRoute();

            Console.WriteLine("\n--- Checklist: Route methods include AddFront ---");
            Console.Write("Enter exact name of waypoint to add to the FRONT (e.g., Keswick): ");
            WayPoint? wpFront = store.SearchByName(Console.ReadLine() ?? ""); 
            if (wpFront != null) 
            {
                route1.AddFront(wpFront);
            }
            else
            {
                Console.WriteLine("Waypoint not found.");
            }
            route1.DisplayRoute();

            Console.WriteLine("\n--- Checklist: Route method(s) to insert waypoint in a specific position ---");
            Console.Write("Enter exact name of waypoint to insert (e.g., Coniston): ");
            WayPoint? wpInsert = store.SearchByName(Console.ReadLine() ?? ""); 
            Console.Write("Enter position to insert at (e.g., 2): ");
            if (int.TryParse(Console.ReadLine() ?? "", out int position) && wpInsert != null) 
            {
                route1.InsertAt(position, wpInsert);
            }
            else
            {
                Console.WriteLine("Invalid position or waypoint not found.");
            }
            route1.DisplayRoute();

            Console.WriteLine("\n--- Checklist: Route method to remove a waypoint from a route by name ---");
            Console.Write("Enter exact name of the waypoint to remove (e.g., Keswick): ");
            string removeName = Console.ReadLine() ?? "";
            route1.RemoveWayPoint(removeName);
            route1.DisplayRoute();

            Console.WriteLine("\n--- Checklist: Search All WayPoints via partialName ---");
            Console.Write("Enter partial waypoint name to search (e.g., Amb): ");
            string partialName = Console.ReadLine() ?? "";  
            WayPoint[] partialResults = store.SearchByPartialName(partialName);
            if (partialResults != null && partialResults.Length > 0)
            {
                foreach (WayPoint wp in partialResults) wp.Display();
            }
            else
            {
                Console.WriteLine("No waypoints found.");
            }

            Console.WriteLine("\n--- Checklist: Search All Waypoints under a given height ---");
            Console.Write("Enter maximum height in meters (e.g., 50): ");
            if (int.TryParse(Console.ReadLine() ?? "", out int maxHeight))
            {
                WayPoint[] heightResults = store.SearchUnderHeight(maxHeight);
                if (heightResults != null && heightResults.Length > 0)
                {
                    foreach (WayPoint wp in heightResults) wp.Display();
                }
                else
                {
                    Console.WriteLine("No waypoints found under that height.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Skipping search.");
            }

            Console.WriteLine("\n--- Checklist: Reverse a route ---");
            Console.WriteLine("Reversing the route...");
            route1.ReverseRoute();
            route1.DisplayRoute();

            Console.WriteLine("\n--- Checklist: Store multiple routes ---");
            List<Route> routesList = new List<Route>();
            routesList.Add(route1);

            Console.Write("Enter name for a second route (e.g., Route 2): ");
            string route2Name = Console.ReadLine() ?? "Route 2";   
            Route route2 = new Route(route2Name);

            Console.Write("Enter exact name of waypoint to add to Route 2 (e.g., Kendal): ");
            WayPoint? wpR2 = store.SearchByName(Console.ReadLine() ?? ""); 
            if (wpR2 != null) route2.AddEnd(wpR2);

            routesList.Add(route2);

            Console.WriteLine("\nDisplaying all stored routes:");
            foreach (Route r in routesList)
            {
                r.DisplayRoute();
            }

            Console.WriteLine("\n=== All tests completed successfully ===");
        }
    }
}
