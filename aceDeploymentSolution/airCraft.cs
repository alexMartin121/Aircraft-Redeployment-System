namespace aceDeploymentSolution
{
    internal class airCraft
    {
        public string Type;
        public int Range; //km
        public int Size;
        public double RunwayRequirements;
        public string FlightPlan;

        public airCraft(string type)
        {
            Type = type;
            FlightPlan = "Flight Plan: ";
            switch (type)
            {
                case "Typhoon":
                    RunwayRequirements = 700;
                    Range = 2900;
                    Size = 1;
                    break;
                case "C-130J":
                    RunwayRequirements = 910;
                    Range = 5250;
                    Size = 3;
                    break;
            }
        }
    }
}
