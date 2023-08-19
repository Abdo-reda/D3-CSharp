namespace d3_force_CSharp.Utility;

public class Utility
{
    private static long a = 1664525;
    private static long c = 1013904223;
    private static long m = 4294967296; 
    private static long s = 1;
    
    public static double jiggle(double random) {
        return (random - 0.5) * 1e-6; //is random ... a random number? random()
    }
    
    //Linear Congreuential Generator (LCG) is a simple random number generator using a linear equation, it generates a number between 0-1
    //https://en.wikipedia.org/wiki/Linear_congruential_generator#Parameters_in_common_use
    public static double lcg() {
        s = (a * s + c) % m;
        return s*1.0 / m;
    }
    
}