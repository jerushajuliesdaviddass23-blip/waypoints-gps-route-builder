namespace WayPoints
{
    class Route
    {
        private string routeName;        // name of this route
        private RouteNode? head;
        private int count;               // number of waypoints in the route

        // Constructor - creates an empty route with a name
        public Route(string routeName)
        {
            this.routeName = routeName;
            this.head      = null;
            this.count     = 0;
        }

        // Returns the route name
        public string RouteName
        {
            get { return routeName; }
        }

        // Returns number of waypoints in the route
        public int Count()
        {
            return count;
        }

        // AddFront - adds a new waypoint at the FRONT of the list
        public void AddFront(WayPoint wp)
        {
            head = new RouteNode(wp, head);     // new node points to old head, becomes new head
            count++;
        }

        // AddEnd - adds a new waypoint at the END of the list
        public void AddEnd(WayPoint wp)
        {
            RouteNode newNode = new RouteNode(wp);

            if (head == null)               // empty list - new node becomes head
            {
                head = newNode;
            }
            else
            {
                RouteNode temp = head;
                while (temp.Next != null)   // traverse to last node
                    temp = temp.Next;
                temp.Next = newNode;        // attach new node at the end
            }
            count++;
        }

        // DisplayRoute - prints all waypoints in the route in required format:
        // Route :routeName:{wp1}{wp2}{wp3}
        public void DisplayRoute()
        {
            Console.Write("Route :" + routeName + ":");
            RouteNode? temp = head;         
            while (temp != null)
            {
                Console.Write(temp.Data.ToString2());   // print each waypoint in {} format
                temp = temp.Next;
            }
            Console.WriteLine();    // newline at end
        }

        // RemoveWayPoint - removes the first waypoint with the given name from the route
        // Traversal pattern: keep track of previous node to re-link the chain
        public void RemoveWayPoint(string name)
        {
            if (head == null)
            {
                Console.WriteLine("Route is empty - nothing to remove.");
                return;
            }

            // Check if head node is the one to remove
            if (head.Data.Name.ToLower() == name.ToLower())
            {
                head = head.Next;           // skip over head - second node becomes new head
                count--;
                Console.WriteLine("Removed: " + name);
                return;
            }

            // Traverse list looking for the node to remove
            RouteNode prev    = head;
            RouteNode? current = head.Next;  

            while (current != null)
            {
                if (current.Data.Name.ToLower() == name.ToLower())
                {
                    prev.Next = current.Next;   // skip over current node - removes it from chain
                    count--;
                    Console.WriteLine("Removed: " + name);
                    return;
                }
                prev    = current;
                current = current.Next;
            }

            Console.WriteLine("WayPoint '" + name + "' not found in route.");
        }

        // InsertAt - inserts a waypoint at a specific position (1 = front, 2 = 2nd, etc.)
        // If position is larger than the list length, appends to end
        // Extended from InsertInOrder concept in Lab 5 Ex.3
        public void InsertAt(int position, WayPoint wp)
        {
            if (position <= 1 || head == null)      // position 1 = add to front
            {
                AddFront(wp);
                return;
            }

            RouteNode newNode  = new RouteNode(wp);
            RouteNode temp     = head;
            int currentPos     = 1;

            // Traverse to node just BEFORE the desired position
            while (temp.Next != null && currentPos < position - 1)
            {
                temp = temp.Next;
                currentPos++;
            }

            // Insert new node between temp and temp.Next
            newNode.Next = temp.Next;
            temp.Next    = newNode;
            count++;
        }

        // ReverseRoute - reverses the order of waypoints in the route
        // Uses three-pointer technique to re-link all nodes in reverse
        public void ReverseRoute()
        {
            RouteNode? prev    = null;           
            RouteNode? current = head;           
            RouteNode? next    = null;           
            while (current != null)
            {
                next          = current.Next;   // save next before overwriting
                current.Next  = prev;           // reverse the link
                prev          = current;        // move prev forward
                current       = next;           // move current forward
            }
            head = prev;   
            Console.WriteLine("Route reversed.");
        }
    }
}
