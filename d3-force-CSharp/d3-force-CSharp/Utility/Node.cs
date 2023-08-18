namespace d3_force_CSharp.Utility;

public class Node
{
    //position (could use vectors)
    public double x;
    public double y;
    //velocity
    public double vx;
    public double vy; 

    Node(double x, double y) {
        this.x = x;
        this.y = y;
    }
}