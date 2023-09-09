using System.Collections;
using System.Numerics;
using d3_essentials;

namespace d3_quadtree_CSharp;

//I could look into an existing implementation of quadtrees

/*
 My structure will be as follows:
    - Quadtree class that contains the root quad
    - A quad class that has boundaries and four children quads and a list of nodes/points.
    - A node/point struct or class that has a position and a radius.
 */


/*
 * This implementation assumes that every quadrant is a square.
 */

//TODO: 
    //- should the quadtree take vector2 points? node? or just x,y?

public class Quadtree
{
    private Quad root;

    public delegate void VisitCallbackSignature(Quad quad);
    private VisitCallbackSignature VisitCallback;

    public Quadtree(List<Vector2> points,  int pointLimit = 1, double x0 = 0, double y0 = 0, double x1 = 100, double y1 = 100) {
        this.root = new Quad(x0, y0, x1, y1);
        Quad.SetPointLimit(pointLimit);
        this.AddAll(points);
    }
    
    public Quadtree(int pointLimit = 1, double x0 = 0, double y0 = 0, double x1 = 100, double y1 = 100) {
        this.root = new Quad(x0, y0, x1, y1);
        Quad.SetPointLimit(pointLimit);
    }
    
    /// <summary>
    /// Expands the quadtree to cover the point and adds it to the quadtree. 
    /// </summary>
    /// <param name="point"></param>
    public void Add(Vector2 point) {
        Cover(point);
        Quad curQuad = this.root;

        while(curQuad.quadrants[this.DetermineQuad(point, curQuad.meanX(), curQuad.meanY())] != null) {
            curQuad = curQuad.quadrants[this.DetermineQuad(point, curQuad.meanX(), curQuad.meanY())];
        }

        if (!curQuad.ContainsExclusive(point)) {
            while (!curQuad.hasCapacity()) {
                int quadrant = this.DetermineQuad(point, curQuad.meanX(), curQuad.meanY());
                curQuad = curQuad.MakeChild(quadrant);
            }
        }
        
        curQuad.AddPoint(point);
    }
    
    /// <summary>
    /// Adds a list of points to the quadtree.
    /// </summary>
    /// <param name="points"> List </param>
    public void AddAll(List<Vector2> points) {
        foreach (Vector2 point in points) {
            this.Add(point);
        }
    }
    
    /// <summary>
    /// Modifies the quadtree boundaries to cover the given point/node by doubling the boundaries and creating new quads.
    /// </summary>
    /// <param name="point"> Vector2 </param>
    public void Cover(Vector2 point) {
        double x0 = this.root.x0, x1 = this.root.x1, y0 = this.root.y0, y1 = this.root.y1;
        double xRange = x1 - x0, yRange = y1 - y0;
        double x = point.X, y = point.Y;

        while (x0 > x || x >= x1 || y0 > y || y >= y1) {
            int childQuadrant = 0; //the quadran that the root quad will be moved to.
            xRange *= 2;
            yRange *= 2;
            
            //TODO: use bitwise operators instead of if statements. (more optimized?)
            if (x < x0 && y < y0) {
                //quad2
                childQuadrant = 0; //quad2 is parent
                x0 = x1 - xRange;
                y0 = y1 - yRange;
            } else if (x < x0) {
                //quad1
                childQuadrant = 3; //quad 1 is parent
                x0 = x1 - xRange;
                y1 = y0 + yRange;
            } else if (y < y0) {
                //quad3
                childQuadrant = 1; //quad 3 is parent
                x1 = x0 + xRange;
                y0 = y1 - yRange;
            } else {
                //quad0
                childQuadrant = 2; //quad 0 is parent
                x1 = x0 + xRange;
                y1 = y0 + yRange;
            }
            Quad extendedQuad = new Quad(x0, y0, x1, y1);
            if (!root.isLeaf()) {
                extendedQuad.quadrants[childQuadrant] = this.root;
                this.root.parent = extendedQuad;
            } else {
                extendedQuad.CopyExclusivePoints(this.root);
            }
            extendedQuad.CopyInclusivePoints(this.root);
            this.root = extendedQuad;
        }
    }

    private int DetermineQuad(Vector2 point, double xm, double ym) {
        int quadrant;
        double x = point.X, y = point.Y;
        if (x < xm && y < ym) {
            quadrant = 2;
        } else if (x < xm) {
            quadrant = 1;
        } else if (y < ym) {
            quadrant = 3;
        } else {
            quadrant = 0;
        }
        
        return quadrant;
    }
    
    
    /// <summary>
    /// Removes the given point from the quadtree.
    /// </summary>
    /// <param name="point"> Vector2 </param>
    public bool Remove(Vector2 point) {
        Quad curQuad = this.root;
        if (!curQuad.ContainsInclusive(point)) {
            Console.WriteLine("Invalid Point, point does not exist in quadtree.");
            return false;
        }

        while(curQuad.quadrants[this.DetermineQuad(point, curQuad.meanX(), curQuad.meanY())] != null) {
            curQuad = curQuad.quadrants[this.DetermineQuad(point, curQuad.meanX(), curQuad.meanY())];
        }
        
        curQuad.RemovePoint(point);
        return true;
    }
    
    /// <summary>
    /// Removes all points from the quadtree.
    /// </summary>
    public void RemoveAll() {
        Vector2[] points = this.GetPoints();
        foreach (Vector2 point in points) {
            this.Remove(point);
        }
    }
    
    /// <summary>
    /// Loops over all the quads in the quadtree and executes the given callback function.
    /// </summary>
    /// <param name="VisitCallback"></param>
    public void Visit(VisitCallbackSignature VisitCallback) {
        this.VisitCallback = VisitCallback;
        VisitAfter(this.root);
    }
    
    private void VisitAfter(Quad curQuad) {
        this.VisitCallback(curQuad);
        if (!curQuad.isLeaf()) {
            for (int i = 0; i < curQuad.quadrants.Length; i++) {
                if (curQuad.quadrants[i] != null) {
                    VisitAfter(curQuad.quadrants[i]);
                }
            }
        }
    }
    
    /// <summary>
    /// Performs proximity search for the nearest point to the given point. Returns null if no point is in the given range
    /// </summary>
    /// <param name="point"></param>
    /// <returns> </returns>
    public Vector2? Find(Vector2 targetPoint, double range = -1) {

        if (range < 0) {
            // range = Double.PositiveInfinity; //wtf I can do this?
            range = this.root.x1 - this.root.x0;
        }
        
        double x = targetPoint.X, y = targetPoint.Y;
        double searchX0 = x - range, searchY0 = y - range, searchX1 = x + range, searchY1 = y + range;  //boundaries of the search area
        
        Vector2? data = null;
        Quad curQuad;
        Stack<Quad> quadStack = new Stack<Quad>();
        quadStack.Push(this.root);
        
        //don't like this, I think it can be improved.
        while (quadStack.Count > 0) {
            curQuad = quadStack.Pop();
            
            if ( (curQuad.x0 > searchX1) ||
                 (curQuad.y0 > searchY1) ||
                 (curQuad.x1 < searchX0) ||
                 (curQuad.y1 < searchY0)
                ) continue;

            if ( curQuad.InclusivePoints.Length > 0) { //&& curQuad.isLeaf()?
                //assume that the quad has at least one point
                double dx, dy, distance; //memory vs readability!
                foreach (Vector2 curPoint in curQuad.InclusivePoints) {
                    dx = x - curPoint.X;
                    dy = y - curPoint.Y;
                    distance = dx * dx + dy * dy;
                    if (distance < range*range) {
                        range = Math.Sqrt(distance);
                        searchX0 = x - range;
                        searchY0 = y - range;
                        searchX1 = x + range; 
                        searchY1 = y + range;
                        data = curPoint;
                    }
                }
            } else {
                int priorityQuad = DetermineQuad(targetPoint, curQuad.meanX(), curQuad.meanY());
                if (curQuad.quadrants[priorityQuad] != null) quadStack.Push(curQuad.quadrants[priorityQuad]!);
                for (int i = 0; i < curQuad.quadrants.Length; i++) {
                    if (curQuad.quadrants[i] != null) {
                        if (i != priorityQuad) {
                            quadStack.Push(curQuad.quadrants[i]!);
                        }
                    }
                }
            }         
                
        }
            
        return data;
    }
    
    /// <summary>
    ///  Performs a deep clone of the quadtree and returns it.
    /// </summary>
    /// <returns></returns>
    public Quadtree Clone() {
        Quadtree quadtreeCpy = (Quadtree) this.MemberwiseClone();
        quadtreeCpy.root = this.root.Clone();
        return quadtreeCpy;
    }

    /// <summary>
    /// Returns a Vector4 representing the bounds of the quadtree.  
    /// </summary>
    /// <returns> float (minX, minY, maxX, maxY) </returns>
    public Vector4 Bounds() {
        return new Vector4(
            (float) this.root.x0, 
            (float) this.root.y0, 
            (float) this.root.x1, 
            (float) this.root.y1
        );
    }

    /// <summary>
    /// Returns a list of all points in the quadtree.
    /// </summary>
    /// <returns> Vector2[] points  </returns>
    public Vector2[] GetPoints() {
        return this.root.InclusivePoints;
    }   
    
    /// <summary>
    /// Returns the number of quads in the quadtree.
    /// </summary>
    /// <returns> int count </returns>
    public int QuadCount() {
        int count = 0;
        this.Visit((Quad quad) => {
            count++;
        });
        return count;
    }

    /// <summary>
    /// Returns the number of points in the quadtree.
    /// </summary>
    /// <returns> int count </returns>
    public int Size() {
        return this.root.InclusivePoints.Length;
    }

    //TODO: implement this after I integreate the node struct
    public void Data() {
        //this will loop over all the quads, access their nodes and get their data. Array of data for each node I guess ... 
        this.Visit((Quad quad) => {
            
        });
    }
    
    
}