namespace aceDeploymentSolution
{
    internal class airBase
    {
        public int AirCraftCapacity;
        public int AircraftSpaceUsed;
        public List<airCraft> Aircraft = new List<airCraft>();
        public string Name;
        public int PersonellCapacity;
        public int Personell;
        public double Longitude;
        public double Latitude;
        public double RunwaySize;


        public airBase(string name, double runwaySize, int aircraftCapacity, int personellCapacity, int personell, double latitude, double longitude)
        {
            Name = name;
            RunwaySize = runwaySize;
            AirCraftCapacity = aircraftCapacity;
            Aircraft.Capacity = aircraftCapacity; //sets list size
            AircraftSpaceUsed = 0; //Defaults
            PersonellCapacity = personellCapacity;
            Personell = personell;
            Longitude = longitude;
            Latitude = latitude;
        }

        public void addAircraft(airCraft airplane)
        {
            Aircraft.Add(airplane);
            AircraftSpaceUsed = AircraftSpaceUsed + airplane.Size;
        }
        public void removeAircraft(airCraft airplane)
        {
            Aircraft.Remove(airplane);
            AircraftSpaceUsed = AircraftSpaceUsed - airplane.Size;
        }
    }
}
