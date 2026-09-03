namespace WayPoints
{
    // Class to store all data for a single WayPoint
    // Follows same pattern as Node class from Lab 5 (BinTree/LinkedList)
    class WayPoint
    {
        // Private data members - encapsulation (same approach as Link class in Lab 5)
        private string name;
        private string code;
        private string latitude;
        private string longitude;
        private int elevation;
        private string description;

        // Constructor - takes all waypoint data from the CSV file
        public WayPoint(string name, string code, string latitude, string longitude, int elevation, string description)
        {
            this.name        = name;
            this.code        = code;
            this.latitude    = latitude;
            this.longitude   = longitude;
            this.elevation   = elevation;
            this.description = description;
        }

        // Properties (get/set) - same pattern as Data property in Lab BinTree Node
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Latitude
        {
            get { return latitude; }
        }

        public string Longitude
        {
            get { return longitude; }
        }

        public int Elevation
        {
            get { return elevation; }
        }

        public string Description
        {
            get { return description; }
        }

        // Display a single waypoint in the required format:
        // {Name, Code, pos[Longitude,Latitude], h:Elevationm, Description}
        public void Display()
        {
            Console.Write("{" + name + ", " + code);
            Console.Write(", pos[" + longitude + "," + latitude + "]");
            Console.Write(", h:" + elevation + "m");
            if (description != "")
                Console.Write(", " + description);
            Console.WriteLine("}");
        }

        // Returns formatted string version - used when displaying routes
        public string ToString2()
        {
            string desc = (description != "") ? ", " + description : "";
            return "{" + name + ", " + code + ", pos[" + longitude + "," + latitude + "], h:" + elevation + "m" + desc + "}";
        }
    }
}
// comment