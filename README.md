# 6G7V0008 Waypoints — GPS Route Builder
Student ID: 25867550  
Unit: 6G7V0008 Algorithms & Data Structures  

---

## What This Is

This is my C# console application for the Waypoints assignment. The idea is that a client needs a backend tool for a GPS route app so I built classes to store a file of UK waypoints in an array and let users build and edit routes using a linked list I made myself. You can search waypoints, create routes, add and remove stops, reverse the route and store multiple routes all tested through an interactive console.

---

## How to Open and Run (Visual Studio)

1. Open **Visual Studio 2022**
2. Click **File → Open → Project/Solution**
3. Browse to the project folder and open **WayPoints.csproj**
4. Make sure **UK_waypoints.csv** is in the same folder as the `.csproj` file
5. Press **F5** or click the green ▶ Start button to build and run
6. The console will ask you to type waypoint names and route names ,just follow what it says

> **If you get a file not found error for the CSV:** right-click `UK_waypoints.csv` in Solution Explorer → Properties → set **Copy to Output Directory** to **Copy if newer** → run again

---

## Files and What They Do

| File | What it does |
|------|-------------|
| `Waypoint.cs` | My class for a single waypoint. Has private fields for name, code, latitude, longitude, elevation and description. Includes a constructor, public properties and a `Display()` method that prints in the required format. |
| `WayPointStore.cs` | Stores ALL waypoints in a private array. Reads the CSV file, skips the header and blank lines, checks for duplicates before adding. Has methods to display everything and search by name, partial name, and height. |
| `Route.cs` | My custom linked list class for a GPS route. Wraps a chain of `RouteNode` objects and has all the add, remove, insert, display and reverse methods the spec asks for. |
| `Routenode.cs` | One node in the linked list. Holds a reference to a `WayPoint` and a nullable pointer to the next node. |
| `Program.cs` | The main program. Tests everything interactively using `Console.ReadLine()` you type waypoint names and positions and it shows the result each time. |
| `UK_waypoints.csv` | The real waypoints file from the assignment  2,445 UK GPS locations. |
| `output.txt` | A saved copy of one full run through the program showing all features working. |

---

## Everything I Implemented

### WayPointStore - array of WayPoints

The first thing I wrote was `countDataLines()` so the array gets created at exactly the right size , it counts how many real data lines are in the file before allocating anything so there is no resizing ever needed.

- **Unique waypoints** - before adding each one, `SearchByName()` checks if it already exists. If it does, it gets skipped.
- `DisplayAll()` — loops through every element and calls `Display()` on each one
- `SearchByName(name)` — scans the array for an exact match, case-insensitive, returns `null` if not found
- `SearchByPartialName(prefix)` — two-pass approach: first counts matches, then collects them into a `WayPoint[]` array
- `SearchUnderHeight(maxH)` — same two-pass approach, returns all waypoints at or below the given height in metres

### Route - my own custom LinkedList

I built this from scratch using the Week 5 lab linked list as my starting point. Each node in the chain holds a reference to a `WayPoint` that already exists in the array — no data is duplicated.

- `AddFront(wp)` — creates a new node pointing to the old head, becomes the new head. O(1), no traversal needed.
- `AddEnd(wp)` — walks the whole chain to find the last node then attaches the new one. O(n).
- `InsertAt(position, wp)` — position 1 goes to the front. If the position is bigger than the list length it just appends to the end. Extended from the InsertInOrder idea in the Week 5 lab.
- `RemoveWayPoint(name)` — checks the head first, then uses a `prev` and `current` two-pointer approach to find and unlink the matching node.
- `DisplayRoute()` — prints the full route in the format the spec asked for: `Route :routeName:{wp1}{wp2}...`
- `ReverseRoute()` — single pass with three pointers (`prev`, `current`, `next`) to flip every link direction. Covered in the Week 10 lecture.

I also stored two separate routes in a `List<Route>` to show multiple routes working at the same time.

---

## Why I Made These Choices

**Array for the waypoint store:** The client specifically asked for an array because it is more compact than a Dictionary and easier to transfer to mobile GPS devices. It also made sense for how this data gets used , you load it once at startup and mostly just read it after that. The contiguous memory layout helps with cache performance when displaying everything. I did consider a `Dictionary<string, WayPoint>` since that gives O(1) lookups, but it roughly doubles the memory overhead and loses index access, which is not a good trade-off for a dataset that barely changes.

**Custom linked list for routes:** Routes get edited constantly  by adding stops, removing them, moving things around. With a linked list you just update a couple of pointers rather than shifting array elements around. The spec also specifically asked for a linked list here so it was the obvious pick. Each node only stores a reference to a `WayPoint` that already lives in the array, so there is no duplication of data.

---

## Complexity of the Most Complex Method

After going through all my methods, `loadFromFile()` in `WayPointStore.cs` is the most expensive. The outer `foreach` loop runs once per line in the file  O(n). But inside that loop I call `SearchByName()` which scans the whole array also O(n). Two nested linear operations gives:

**Overall complexity: O(n²)**

For n = 2,445 waypoints that works out to roughly 2.99 million comparisons in the worst case. In practice it finishes in under 50ms because it only runs once at startup and the dataset is small. If the file ever grew to 50,000+ entries I would replace the `SearchByName()` call with a `HashSet<string>` lookup which would drop the whole load from O(n²) down to O(n) a one-line fix.

---

## My Development Steps

1. Tested the provided starter code first to understand how the CSV reading worked
2. Created `Waypoint.cs` — private fields, constructor, `Display()` in the format the spec shows
3. Created `WayPointStore.cs` — wrote `countDataLines()` first so the array size is known before allocating
4. Added `loadFromFile()` with the uniqueness check inside the loop
5. Added `DisplayAll()` and `SearchByName()`, tested both were giving correct output in `Program.cs`
6. Added `SearchByPartialName()` and `SearchUnderHeight()` — tested with a few different inputs to make sure the two-pass logic was collecting the right results
7. Created `Routenode.cs` — kept it simple, private data and nullable next pointer following the same pattern as the lab Node class
8. Created `Route.cs` — started with `AddFront()`, `AddEnd()` and `DisplayRoute()` and tested these were working before I moved on
9. Added `RemoveWayPoint()` — the prev/current pattern from the lab exercises made this fairly straightforward once I worked out the head-removal edge case
10. Added `InsertAt()` — I extended the InsertInOrder idea from Week 5 and added the fallback to end if the position is too large
11. Added `ReverseRoute()` — three-pointer technique from the lectures
12. Wired all the tests into an interactive `Program.cs` with clear console prompts for each feature, ran it all the way through and saved the output to `output.txt`
13. Went back through every method to work out their Big-O for the report, then identified `loadFromFile()` as O(n²) and did the full line-by-line analysis in Table 2

---

## References

- Cormen, T.H. et al. (2009) *Introduction to Algorithms*. 3rd ed. MIT Press.
- Sedgewick, R. and Wayne, K. (2011) *Algorithms*. 4th ed. Addison-Wesley.
- Goodrich, M.T., Tamassia, R. and Goldwasser, M.H. (2014) *Data Structures and Algorithms in Java*. 6th ed. Wiley.
- McLean, D. (2025) 6G7V0008 Lecture Notes: Weeks 4–10. MMU Moodle.
- Microsoft (2024) *C# Documentation — Collections*. Available at: https://docs.microsoft.com/dotnet/csharp
