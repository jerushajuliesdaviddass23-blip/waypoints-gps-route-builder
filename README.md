# WayPoints GPS Route Builder

**Unit:** 6G7V0008 Algorithms & Data Structures  
**Student ID:** 25867550  
**University:** Manchester Metropolitan University

A C# console application that demonstrates core data structures and algorithms by building a GPS route management system. This project implements a waypoint store (using arrays) and a custom linked-list route planner with comprehensive search and manipulation capabilities.

---

## Overview

The application serves as a backend tool for a GPS route application. It loads UK waypoints from a CSV file, organises them in an optimised array structure, and allows users to build and edit routes using a custom singly linked list implementation.

**Key Features:**
- Load 2,445+ UK GPS waypoints from CSV
- Search waypoints by exact name, partial name, or elevation
- Create and manage multiple routes
- Add/remove/insert waypoints in routes
- Reverse route direction
- Custom linked list implementation from scratch

---

## Project Structure

```
waypoints-gps-route-builder/
├── src/
│   ├── WayPoints.csproj           # Project configuration
│   ├── Program.cs                 # Main entry point with interactive console tests
│   ├── WayPoint.cs                # Single waypoint data class
│   ├── WayPointStore.cs           # Array-based waypoint store
│   ├── Route.cs                   # Custom linked list for routes
│   └── RouteNode.cs               # Individual linked list node
├── data/
│   └── UK_waypoints.csv           # 2,445 UK GPS waypoint locations
├── docs/
│   ├── COMPLEXITY_ANALYSIS.md     # Big-O analysis of all methods
│   └── DATA_STRUCTURES.md         # Justification of design choices
├── README.md                       # This file
├── .gitignore                      # Git ignore patterns
└── LICENSE                         # License file (MIT recommended)
```

---

## Getting Started

### Requirements

- **Visual Studio 2022** (or Visual Studio Code with C# extensions)
- **.NET 6.0+**
- UK_waypoints.csv in the project directory

### Running the Application

**Visual Studio 2022:**

1. Open Visual Studio
2. File → Open → Project/Solution
3. Navigate to `WayPoints.csproj` and open it
4. Ensure `UK_waypoints.csv` is in the same folder as the `.csproj` file
5. Press **F5** or click the green ▶ **Start** button

**If CSV file is not found:**
- Right-click `UK_waypoints.csv` in Solution Explorer
- Properties → set **Copy to Output Directory** to **"Copy if newer"**
- Run again

### Interactive Console Guide

Once running, follow the on-screen prompts:

1. **Display all waypoints** – See all 2,445 locations loaded
2. **Search by exact name** – Find a specific waypoint (e.g., "Ambleside")
3. **Create a route** – Give your route a name
4. **Add waypoints** – Add to the front, end, or specific position
5. **Remove waypoints** – Remove by name from your route
6. **Search partial name** – Find waypoints starting with a prefix
7. **Search by elevation** – Find locations below a certain height
8. **Reverse route** – Flip the direction of travel
9. **Manage multiple routes** – Store and display several routes

---

## Class Documentation

### `WayPoint.cs`

Represents a single GPS waypoint.

**Properties:**
- `Name` – Location name (e.g., "Ambleside")
- `Code` – Unique identifier code
- `Latitude` – Decimal latitude
- `Longitude` – Decimal longitude
- `Elevation` – Height in metres
- `Description` – Optional location details

**Key Methods:**
- `Display()` – Console output in spec format
- `ToString2()` – Formatted string for route display

---

### `WayPointStore.cs`

Stores all waypoints in a fixed-size array.

**Key Methods:**
- `SearchByName(string name)` – O(n) exact match search
- `SearchByPartialName(string prefix)` – O(n) prefix search
- `SearchUnderHeight(int maxHeight)` – O(n) elevation filter
- `DisplayAll()` – O(n) display all waypoints
- `Count()` – Return number of loaded waypoints

**Design:** Array allocated to exact size via `countDataLines()` — no resizing overhead.

---

### `Route.cs`

Custom singly linked list for managing ordered waypoints in a route.

**Key Methods:**
- `AddFront(WayPoint wp)` – O(1) add to start
- `AddEnd(WayPoint wp)` – O(n) add to end
- `InsertAt(int position, WayPoint wp)` – O(n) insert at position
- `RemoveWayPoint(string name)` – O(n) remove by name
- `ReverseRoute()` – O(n) reverse direction
- `DisplayRoute()` – O(n) display route in spec format

**Design:** Each node holds a reference to an existing `WayPoint` — no data duplication.

---

### `RouteNode.cs`

Single node in the linked list chain.

**Properties:**
- `Data` – Reference to a `WayPoint` object
- `Next` – Nullable reference to next node in chain

---

## Complexity Analysis

| Method | Complexity | Notes |
|--------|-----------|-------|
| `SearchByName()` | O(n) | Linear scan through array |
| `SearchByPartialName()` | O(n) | Two-pass: count, then collect |
| `SearchUnderHeight()` | O(n) | Two-pass filtering |
| `AddFront()` | O(1) | Single pointer update |
| `AddEnd()` | O(n) | Must traverse to tail first |
| `InsertAt()` | O(n) | Traverse to position, then rewire |
| `RemoveWayPoint()` | O(n) | Search + unlink |
| `ReverseRoute()` | O(n) | Single pass, three pointers |
| `loadFromFile()` | **O(n²)** | Most expensive: outer loop calls SearchByName() for uniqueness check |

**Why O(n²) for loading is acceptable:**
- Runs once at startup
- 2,445 entries = ~3M comparisons
- Completes in <50ms on modern hardware
- Code clarity and correctness prioritised over micro-optimisation

---

## Design Decisions

### Array for WayPointStore (vs. LinkedList or BST)

✅ **Chosen: Array**

**Why:**
- Contiguous memory → excellent cache locality for `DisplayAll()`
- O(1) random access by index
- Exact sizing via `countDataLines()` eliminates resizing
- Client specified array format for mobile transfer
- Read-heavy workload (load once, search many)

❌ **Not LinkedList:** O(n) access to any element; pointer overhead
❌ **Not BST:** Would reorder by name; routes need user-defined order; over-engineered for 2,445 entries

### Linked List for Route (vs. Array or BST)

✅ **Chosen: Custom Singly Linked List**

**Why:**
- Routes edited constantly (add/remove/reorder stops)
- Insert/delete anywhere = O(1) pointer updates (vs. O(n) array shifts)
- Preserves exact insertion order (critical for GPS)
- Assignment explicitly requested linked list

❌ **Not Array:** Inserting mid-route requires shifting; deletion leaves gaps
❌ **Not BST:** Would sort by waypoint name; defeats purpose of custom route ordering

---

## Future Optimisations

### Reduce loadFromFile() to O(n)
Replace `SearchByName()` call with `HashSet<string>` lookup:
```csharp
HashSet<string> seenNames = new HashSet<string>();
if (seenNames.Add(name))  // Add returns false if already present
{
    wayPointArray[count++] = new WayPoint(...);
}
```
**Impact:** O(n²) → O(n); saves ~3M comparisons on load.

### Handle 50,000+ waypoints
Add optional BST (SortedDictionary) for search acceleration:
- Keep array for display/transfer
- Index BST by name for O(log n) lookups
- ~16 comparisons vs. 25,000 → 1,500× speedup

### Route Optimisations
- Add distance calculations between consecutive waypoints
- Implement A* pathfinding to auto-generate optimal routes
- Store routes persistently to JSON/XML

---

## Testing

The application includes comprehensive interactive testing via `Program.cs`:

- ✅ Load and display 2,445 waypoints
- ✅ Search by exact name
- ✅ Search by partial name
- ✅ Filter by elevation
- ✅ Create routes and manipulate waypoints
- ✅ Store multiple routes simultaneously
- ✅ Reverse route direction

Run through all tests by following the console prompts. A sample run is stored in `output.txt`.

---

## References

- Cormen, T.H. *et al.* (2009) *Introduction to Algorithms*. 3rd ed. MIT Press.
- Sedgewick, R. and Wayne, K. (2011) *Algorithms*. 4th ed. Addison-Wesley.
- Goodrich, M.T., Tamassia, R. and Goldwasser, M.H. (2014) *Data Structures and Algorithms in Java*. 6th ed. Wiley.
- Microsoft (2024) *C# Documentation — Collections*. [https://docs.microsoft.com/dotnet/csharp](https://docs.microsoft.com/dotnet/csharp)

---

## License

MIT License — Feel free to use this code for educational purposes.

---

## Author

**Student ID:** 25867550  
**Unit:** 6G7V0008 Algorithms & Data Structures  
**Institution:** Manchester Metropolitan University
