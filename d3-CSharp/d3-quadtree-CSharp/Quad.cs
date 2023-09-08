using System.Numerics;
using d3_essentials;

namespace d3_quadtree_CSharp;

public class Quad
{
    //Maybe later this value can local for each quad and remove the internal method below.
    private static int POINT_LIMIT = 1; 
    
    //I could remove this, I think it serves no pupose
    private List<Vector2> inclusivePoints = new List<Vector2>(); //points that reside in the quad or in the children quads.
        
    private List<Vector2> exclusivePoints = new List<Vector2>(); //points that are in the quad but not in the children quads. Should never exceed the Point_Limit
    
    //use vector2 or a point struct for boundaries.
        //public Vector2 min;
        //public Vector2 max;
    
    //boundaries of the quad (min and max x and y)
    public double x0;
    public double y0;
    public double x1;
    public double y1;
    
    //I could determine whether the quad is a leaf or not if the points count is equal to the limit_point or if the four children are null.
    
    //TODO: maybe make this an array of Quads? more efficent?
    //Top Right, Top Left, Bottom Left, Bottom Right
    public Quad?[] quadrants = new Quad?[4];
    
    //while convienient, the reference to the parent quad may not be necessary.
    public Quad? parent;
    //maybe create functions that will create the above child quads so that it ensures that the boundaries are correct (each child is a quarter of the parent quad).

    public Vector2[] InclusivePoints {
        get { return inclusivePoints.ToArray(); }
    }

    
    
    public Quad(double x0 = 0, double y0 = 0, double x1 = 100, double y1 = 100) {
        if (!(x0<x1 && y0<y1)) {
            // throw new ArgumentException("Invalid values for boundaries, make sure that x0 < x1 & y0 < y1.");
            Console.WriteLine("Invalid values for boundaries, make sure that x0 < x1 & y0 < y1. For now default values will be used.");
        }
        
        this.x0 = x0;
        this.y0 = y0;
        this.x1 = x1;
        this.y1 = y1;
    }

    public void SetBoundaries(double x0, double y0, double x1, double y1) {
        if (!(x0<x1 && y0<y1)) {
            Console.WriteLine("Invalid values for boundaries, make sure that x0 < x1 & y0 < y1.");
            return;
        }
        
        this.x0 = x0;
        this.y0 = y0;
        this.x1 = x1;
        this.y1 = y1;
    }

    public bool isLeaf() {
        //I can either check children or point count
        for (int i=0; i<quadrants.Length; i++) {
            if (quadrants[i] != null) return false;
        }

        return true;
    }

    public bool hasCapacity() {
        if (exclusivePoints.Count < POINT_LIMIT) return true;
        return false;
    }

    internal static void SetPointLimit(int pointLimit) {
        POINT_LIMIT = pointLimit;
    }
    
    public Quad MakeChild(int quadrant) {
        if (this.quadrants[quadrant] != null) {
            Console.WriteLine("Child already exists at quadrant " + quadrant + ".");
            //throw new ArgumentException("Child already exists at quadrant " + quadrant + ".");
            return this.quadrants[quadrant]!;
        }
        
        double childX0 = this.x0, childY0 = this.y0, childX1 = this.x1, childY1 = this.y1;
        double mx = this.meanX(), my = this.meanY();
        switch (quadrant) {
            case 0:
                childX0 = mx;
                childY0 = my;
                break;
            case 1:
                childX1 = mx;
                childY0 = my;
                break;
            case 2:
                childX1 = mx;
                childY1 = my;
                break;
            case 3:
                childX0 = mx;
                childY1 = my;
                break;
        }
        Quad child = new Quad(childX0, childY0, childX1, childY1);
        this.quadrants[quadrant] = child;
        child.parent = this;
        this.PropogatePoints(quadrant);
        return child;
    }

    public void AddPoint(Vector2 point) {
        if (!this.InBoundary(point)) {
            Console.WriteLine("Invalid Point, point is not within the boundaries of the quad.");
            return;
        }

        if (exclusivePoints.Count == POINT_LIMIT) {
            Console.WriteLine("Invalid Point, point limit has been exceeded.");
            return; 
        }
        
        exclusivePoints.Add(point);
        this.AddToParent(point);
    }
    
    private void AddToParent(Vector2 point) {
        this.inclusivePoints.Add(point);
        this.parent?.AddToParent(point);
    }

    public void RemovePoint(Vector2 point) {
        this.exclusivePoints.Remove(point);
        this.RemoveFromParent(point);
    }

    private void RemoveFromParent(Vector2 point) {
        Quad? tempParent = this.parent;
        this.inclusivePoints.Remove(point);
        if (this.isLeaf()) {
            this.parent?.quadrants.SetValue(null, Array.IndexOf(this.parent.quadrants, this));
            this.parent = null;
        }
        tempParent?.RemoveFromParent(point);
    }

    //make these fields
    public double meanX() {
        return (x1 + x0) / 2;
    }

    public double meanY() {
        return (y1 + y0) / 2;
    }
    
    public bool InBoundary(Vector2 point) {
        if (point.X >= x0 && point.X < x1 && point.Y >= y0 && point.Y < y1) return true;
        return false;
    }

    //This will propogate the points to the child quadrant.
    private void PropogatePoints(int quadrant) {
        for (int i=0; i<exclusivePoints.Count; i++) {
            if (this.quadrants[quadrant]!.InBoundary(exclusivePoints[i])) {
                this.quadrants[quadrant]!.exclusivePoints.Add(exclusivePoints[i]);
                this.quadrants[quadrant]!.inclusivePoints.Add(exclusivePoints[i]);
                exclusivePoints.RemoveAt(i);
                i--;
            }
        }
    }
    
    public void CopyInclusivePoints(Quad childQuad) {
        this.inclusivePoints = new List<Vector2>(childQuad.inclusivePoints);
    }
    
    public void CopyExclusivePoints(Quad childQuad) {
        this.exclusivePoints = new List<Vector2>(childQuad.exclusivePoints);
    }

    public bool ContainsExclusive(Vector2 point) {
        return exclusivePoints.Contains(point);
    }

    public bool ContainsInclusive(Vector2 point) {
        return inclusivePoints.Contains(point);
    }
    
}