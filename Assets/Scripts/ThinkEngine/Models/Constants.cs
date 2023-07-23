namespace ThinkEngine.Models {
    public static class Constants {
        
        public const int FLOAT_TO_INT_MULTIPLIER = 1000;
        
        
        public static int FloatToInt(float value) {
            return (int) (value * FLOAT_TO_INT_MULTIPLIER);
        }
        
        public static float IntToFloat(int value) {
            return (float) value / FLOAT_TO_INT_MULTIPLIER;
        }
        
        
    }
}
