namespace aceDeploymentSolution
{
    internal class Program
    {
        public static List<airBase> airBases = new List<airBase>(10);
        public static List<airBase> sortedAirBases = new List<airBase>(10);
        public static int dangerZone;
        public static double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            // Radius of the Earth in kilometers
            double R = 6371.0;

            // Convert latitude and longitude from degrees to radians
            lat1 = ToRadians(lat1);
            lon1 = ToRadians(lon1);
            lat2 = ToRadians(lat2);
            lon2 = ToRadians(lon2);

            // Calculate the differences in coordinates
            double dlat = lat2 - lat1;
            double dlon = lon2 - lon1;

            // Haversine formula
            double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance
            double distance = R * c;

            return distance;
        }
        public static double ToRadians(double degree)
        {
            return degree * Math.PI / 180.0;
        }
        public static bool CanLand(airCraft airplane, airBase currentAirbase, airBase targetAirbase)
        {
            double distanceToTarget = Haversine(targetAirbase.Latitude, targetAirbase.Longitude, currentAirbase.Latitude, currentAirbase.Longitude);
            if (airplane.Range > distanceToTarget //Airbase in range
                && distanceToTarget > dangerZone //Not in dangerzone
                && (targetAirbase.AircraftSpaceUsed < targetAirbase.AirCraftCapacity)  //Has space for aircraft
                && (targetAirbase.RunwaySize > airplane.RunwayRequirements)) //Runway Size is long enough
            {
                //Check it would not bring it over the space limit
                if (airplane.Size + targetAirbase.AircraftSpaceUsed <= targetAirbase.AirCraftCapacity)
                {
                    return true;
                }
            }
            return false;

        }
        public static void PrintFlightPlans()
        {
            foreach (airBase airbase in airBases)
            {
                foreach (airCraft a in airbase.Aircraft)
                {
                    Console.WriteLine("\n" + a.Type + " " + a.FlightPlan);
                }
            }
        }
        public static void PrintAirbases()
        {
            foreach (airBase airbase in airBases)
            {
                Console.Write("\nName: " + airbase.Name + "\nStaff: " + airbase.Personell + "/" + airbase.PersonellCapacity + "\nDistance from Main Base: " + Haversine(airbase.Latitude, airbase.Longitude, 1.5, 2.5) + "\nAircraft: ");
                foreach (airCraft a in airbase.Aircraft)
                {
                    Console.Write(a.Type + ", ");
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {

            // Create airbases using the constructor
            airBase airBase1 = new airBase("Air Base 1", 3005, 54, 867, 0, 0.0, 0.0);
            airBases.Add(airBase1);
            airBase airBase2 = new airBase("Air Base 2", 2784, 30, 135, 0, 2.0, -3.0);
            airBases.Add(airBase2);
            airBase airBase3 = new airBase("Air Base 3", 3192, 20, 902, 536, -3.0, -4.0);
            airBases.Add(airBase3);
            airBase airBase4 = new airBase("Main Base", 2638, 44, 2057, 1500, 1.5, 2.5); //Main air base
            airBases.Add(airBase4);
            airBase airBase5 = new airBase("Air Base 5", 2876, 39, 774, 276, -4.0, -4.0);
            airBases.Add(airBase5);
            airBase airBase6 = new airBase("Air Base 6", 3081, 58, 489, 23, 0.5, 4.0);
            airBases.Add(airBase6);
            airBase airBase7 = new airBase("Air Base 7", 600, 27, 899, 12, 1.0, -3.0);
            airBases.Add(airBase7);
            airBase airBase8 = new airBase("Air Base 8", 1914, 50, 204, 87, -5.0, 2.0);
            airBases.Add(airBase8);
            airBase airBase9 = new airBase("Air Base 9", 1000, 33, 845, 23, 4.5, -4.0);
            airBases.Add(airBase9);
            airBase airBase10 = new airBase("Air Base 10", 2556, 28, 50, 4, -2.5, 2.5);
            airBases.Add(airBase10);

            //Set Aircraft
            for (int i = 0; i < 3; i++)
            {
                airBase4.addAircraft(new airCraft("C-130J"));
            }
            for (int i = 0; i < 20; i++)
            {
                airBase4.addAircraft(new airCraft("Typhoon"));
            }


            //Example Situation - Rehouse all aircraft to the nearest base
            Console.WriteLine("\nPlease input the criteria below for rebasing");
            Console.Write("Do not rebase units within (default 0, in km):");
            dangerZone = Convert.ToInt32(Console.ReadLine());
            Console.Write("Should units rebase to the airbase with the most avaliable space (Y/N): ");
            char rebaseToLargestCapactity = Convert.ToChar(Console.Read());

            if(char.ToUpper(rebaseToLargestCapactity) == 'Y') //Rebase to airbase with most space first, so sort the list in that order
            {
                sortedAirBases.OrderByDescending(o => (o.AirCraftCapacity - o.AircraftSpaceUsed)).ToList();
            }
            Console.WriteLine("Before:");
            PrintAirbases();
            Console.WriteLine("\nAfter:");
            bool airCraftMoved = true;
            while (airBase4.Aircraft.Any() == true && airCraftMoved == true) //While there are still aircraft in the main airbase
            {
                foreach (airBase airBase in airBases.ToList())//Cycle through each airBase in the list to first move all aircraft out of the main base
                {
                    if (airBase != airBase4) // Dont include the selected airbase 
                    {
                        foreach (airCraft airPlane in airBase4.Aircraft.ToList()) //Cycle Through each aircraft housed in the main base
                        {
                            if (CanLand(airPlane, airBase4, airBase) == true)
                            {
                                if (airBase.Personell > 20) //Cannot house aircraft on an airstrip with no personell
                                {
                                    airPlane.FlightPlan = airPlane.FlightPlan + "Fly from " + airBase4.Name + " to " + airBase.Name + " ";
                                    if (airPlane.Type == "C-130J") //C-130Js can transport troops
                                    {
                                        if (airBase4.Personell > 90)
                                        {
                                            airBase.Personell = airBase.Personell + 90;
                                            airBase4.Personell = airBase4.Personell - 90;
                                            airPlane.FlightPlan = airPlane.FlightPlan + "Transporting 90 Troops";
                                        }
                                        else //No more troops can be transported
                                        {
                                            airPlane.FlightPlan = airPlane.FlightPlan + "Transporting " + airBase4.Personell + " Troops";
                                            airBase.Personell = airBase.Personell + airBase4.Personell;
                                            airBase4.Personell = 0;
                                        }
                                        //Add airplane to this airbase and remove it from targeted one
                                        airBase.addAircraft(airPlane);
                                        airBase4.removeAircraft(airPlane);
                                        airCraftMoved = true;
                                    }
                                    else
                                    {
                                        //Add airplane to this airbase and remove it from targeted one
                                        airBase.addAircraft(airPlane);
                                        airBase4.removeAircraft(airPlane);
                                        airCraftMoved = true;
                                    }
                                }

                            }
                            else
                            {
                                airCraftMoved = false; //Flag for checking that any aircraft have been transported.
                            }
                        }
                    }
                }
            }
            bool end = false;
            if (airCraftMoved == false)
            {
                Console.WriteLine("Aircraft cannot be transported - No airbases in range");
            }
            else
            {
                PrintAirbases();
                while (airBase4.Personell > 0 && end == false) //Loop until there are no troops left
                {
                    int tempPersonell = airBase4.Personell;
                    Console.WriteLine("\nNew Rotation");
                    foreach (airBase airbase in airBases.ToList()) //Go through each airbase
                    {
                        foreach (airCraft airPlane in airbase.Aircraft.ToList()) //Each aircraft in this airbase
                        {
                            if (airPlane.Type == "C-130J") //C-130Js can transport troops
                            {
                                //In range
                                if (CanLand(airPlane, airbase, airBase4) == true)
                                {
                                    //Space for personelll
                                    if (airbase.PersonellCapacity - airbase.Personell > 0)
                                    {
                                        if (airBase4.Personell > 90) //There are still troops at main base
                                        {
                                            if (airbase.PersonellCapacity - airbase.Personell > 90) //Full plane can be taken
                                            {
                                                airPlane.FlightPlan = airPlane.FlightPlan + "\nFly from " + airbase.Name + " to " + airBase4.Name + ", Pickup 90 Troops" + " and return to " + airbase.Name + " ";
                                                airbase.Personell = airbase.Personell + 90;
                                                airBase4.Personell = airBase4.Personell - 90;
                                            }
                                            else //Full plane cannot be taken
                                            {
                                                int remainingSpace = airbase.PersonellCapacity - airbase.Personell;
                                                airPlane.FlightPlan = airPlane.FlightPlan + "\nFly from " + airbase.Name + " to " + airBase4.Name + ", Pickup " + remainingSpace + " Troops" + " and return to " + airbase.Name + " ";
                                                airbase.Personell = airbase.Personell + remainingSpace;
                                                airBase4.Personell = airBase4.Personell - remainingSpace;
                                            }
                                        }
                                        else //No more troops can be transported
                                        {
                                            airPlane.FlightPlan = airPlane.FlightPlan + "\nFly from " + airbase.Name + " to " + airBase4.Name + ", Pickup " + airBase4.Personell + " Troops" + " and return to " + airbase.Name + " ";
                                            airbase.Personell = airbase.Personell + airBase4.Personell;
                                            airBase4.Personell = 0;
                                            break;
                                        }
                                    }
                                    else //There are c-130s, but no more space for troops
                                    {
                                        //Find a new base and rehouse the c-130 there
                                        foreach (airBase newairbase in airBases.ToList()) //Go through each airbase
                                        {
                                            if (newairbase != airBase4) //Do not send c-130s back to the main base, rehouse first and then transport
                                            {
                                                //Try to see if you can land the c-130 at the new airbase and it has space for troops and the new base is in range of the main base
                                                if (CanLand(airPlane, airbase, newairbase) && (newairbase.PersonellCapacity - newairbase.Personell > 0)
                                                    && (Haversine(newairbase.Latitude, newairbase.Longitude, airBase4.Latitude, airBase4.Longitude) < airPlane.Range)
                                                    && (Haversine(newairbase.Latitude, newairbase.Longitude, airBase4.Latitude, airBase4.Longitude) > dangerZone))
                                                {
                                                    newairbase.addAircraft(airPlane); // Move the c-130
                                                    airbase.removeAircraft(airPlane); //Remove the c-130
                                                    airPlane.FlightPlan = airPlane.FlightPlan + "\nFly from " + airbase.Name + " to " + newairbase.Name + " for relocation.";
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }

                    PrintAirbases();
                    PrintFlightPlans();
                    if (tempPersonell == airBase4.Personell)
                    {
                        end = true;
                        Console.WriteLine("No more space to transport troops");
                    }
                }
            }
            Console.Read();
        }
    }
}
