namespace Game.Data
{

    public class GameConfig
    {
        public static class PlayerSettings
        {
            public static float PlayerSpeed = 7f;
            public static int[] StackLimit = { 24, 8, 10, 12, 14 };
        }

        public static class WorkerSettings
        {
            public static float[] WorkerSpeed = { 2, 2.5f, 3, 3.5f, 4 };
            public static int[] StackLimit = { 4, 6, 8, 10, 12 };
        }

        public static class CustomerSettings
        {
            public static float CustomerSpeed = 2f;
            public static int CustomersPerCounter = 3;
            public static float IdleTime = 5f;
        }

        public static class CounterSettings
        {
            public static int ProductionQTY;
            public static float ProductionSpeed = 0.1f;
        }
    }
}