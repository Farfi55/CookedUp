namespace CookedUp.ThinkEngine.Models {
    public static class Converter {
        
        public const int FLOAT_TO_INT_MULTIPLIER = 1000;
        
        
        public static int FloatToInt(float value) {
            return (int) (value * FLOAT_TO_INT_MULTIPLIER);
        }
        
        public static int FloatToInt(double value) {
            return (int) (value * FLOAT_TO_INT_MULTIPLIER);
        }
        
        public static float IntToFloat(int value) {
            return (float) value / FLOAT_TO_INT_MULTIPLIER;
        }
        
        
    }
}
