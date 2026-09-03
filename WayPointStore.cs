using System.Text;

namespace WayPoints
{

    class WayPointStore
    {
        private WayPoint[] wayPointArray;   // private array - the data store
        private int count;                  // tracks how many waypoints are stored

        // Constructor - creates the array sized exactly to the number of data lines in file
        public WayPointStore(string fileName)
        {
            count = 0;
            int size = countDataLines(fileName);            // count lines first to size array
            wayPointArray = new WayPoint[size];             // create array of correct size
            loadFromFile(fileName);                         // fill array from file
        }

        // Counts lines in file (excluding header and blank lines) to size the array
        private int countDataLines(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            int total = 0;
            for (int i = 1; i < lines.Length; i++)         // skip line 0 (header)
            {
                if (lines[i] != "")
                    total++;
            }
            return total;
        }

        // Reads the CSV file and loads each UNIQUE waypoint into the array
        private void loadFromFile(string fileName)
        {
            string[] linesInFile = File.ReadAllLines(fileName);
            int lineNumber = 0;

            foreach (string line in linesInFile)
            {
                lineNumber++;
                if (lineNumber != 1 && line != "")          // skip header line 1 and blank lines
                {
                    string[] features = line.Split(',');

                    string name        = features[0];
                    string code        = features[1];
                    string latitude    = features[3];
                    string longitude   = features[4];      
                    int elevation      = convertElevationToMeters(features[5]);
                    string description = buildDescription(features);

                    // This ensures UNIQUE waypoints as required by the spec
                    if (SearchByName(name) == null)
                    {
                        wayPointArray[count] = new WayPoint(name, code, latitude, longitude, elevation, description);
                        count++;
                    }
                }
            }
        }

        // Description starts at column index 11, but may itself contain commas
        // so we join all remaining columns (same logic as starter code)
        private string buildDescription(string[] features)
        {
            StringBuilder desc = new StringBuilder();
            int pos = 11;
            while (pos < features.Length)
            {
                if (features[pos] != "" && features[pos] != " ")
                    desc.Append(features[pos] + ",");
                pos++;
            }
            // Remove trailing comma if present
            string result = desc.ToString();
            if (result.EndsWith(","))
                result = result.TrimEnd(',');
            return result;
        }

        // Converts elevation string to integer metres
        private int convertElevationToMeters(string elevationStr)
        {
            char[] unitChars = { 'f', 't', 'M', 'm' };
            if (elevationStr.ToLower().EndsWith("m"))
            {
                return (int)Math.Round(Double.Parse(elevationStr.TrimEnd(unitChars)));
            }
            double elevationFeet = Double.Parse(elevationStr.TrimEnd(unitChars));
            return (int)Math.Round(elevationFeet / 3.281);  
        }

        // Returns how many waypoints are stored
        public int Count()
        {
            return count;
        }

        // Displays ALL waypoints in the array - loops through every element
        public void DisplayAll()
        {
            Console.WriteLine("\n--- All WayPoints (" + count + " total) ---");
            for (int i = 0; i < count; i++)
            {
                wayPointArray[i].Display();
            }
        }

        // Searches array for a waypoint by EXACT name - returns the WayPoint object if found
        // Returns null if not found
        public WayPoint? SearchByName(string name)
        {
            string trimmedName = name.Trim();
            for (int i = 0; i < count; i++)
            {
                if (wayPointArray[i].Name.ToLower() == trimmedName.ToLower())
                    return wayPointArray[i];
            }
            return null;    // not found — null is valid because return type is WayPoint?
        }

        // Searches array for waypoints whose name STARTS WITH the given letters
        // Returns array of matches (partial name / prefix search)
        public WayPoint[] SearchByPartialName(string partialName)
        {
            string trimmedPartial = partialName.Trim();

           
            int matchCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (wayPointArray[i].Name.ToLower().StartsWith(trimmedPartial.ToLower()))
                    matchCount++;
            }


            WayPoint[] results = new WayPoint[matchCount];
            int resultIndex = 0;
            for (int i = 0; i < count; i++)
            {
                if (wayPointArray[i].Name.ToLower().StartsWith(trimmedPartial.ToLower()))
                {
                    results[resultIndex] = wayPointArray[i];
                    resultIndex++;
                }
            }
            return results;
        }

        // Searches for all waypoints at or below a given height (in metres)
        // Returns array of matching WayPoint objects
        public WayPoint[] SearchUnderHeight(int maxHeight)
        {

            int matchCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (wayPointArray[i].Elevation <= maxHeight)
                    matchCount++;
            }


            WayPoint[] results = new WayPoint[matchCount];
            int resultIndex = 0;
            for (int i = 0; i < count; i++)
            {
                if (wayPointArray[i].Elevation <= maxHeight)
                {
                    results[resultIndex] = wayPointArray[i];
                    resultIndex++;
                }
            }
            return results;
        }
    }
}
