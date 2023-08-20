using System.Numerics;
using d3_essentials;

namespace d3_quadtree_CSharp;

public class Quad
{
    public List<Vector2> points; //only one point? 
    
    //could turn this into a vector2 or a point struct.
        //public Vector2 min;
        //public Vector2 max;
    
    //boundaries of the quad (min and max x and y)
    public double x0;
    public double y0;
    public double x1;
    public double y1;
    
    //Top Right, Top Left, Bottom Left, Bottom Right
    public Quad TR;
    public Quad TL;
    public Quad BL;
    public Quad BR;

    
    
    public Quad(double x0 = 0, double y0 = 0, double x1 = 1, double y1 = 1) {
        if (!(x0<x1 && y0<y1)) {
            throw new ArgumentException("Invalid values for boundaries, make sure that x0 < x1 & y0 < y1.");
        }
        
        this.x0 = x0;
        this.y0 = y0;
        this.x1 = x1;
        this.y1 = y1;
    }

    public void SetBoundaries(double x0, double y0, double x1, double y1) {
        if (!(x0<x1 && y0<y1)) {
            throw new ArgumentException("Invalid values for boundaries, make sure that x0 < x1 & y0 < y1.");
        }
        
        this.x0 = x0;
        this.y0 = y0;
        this.x1 = x1;
        this.y1 = y1;
    }
    
    
}