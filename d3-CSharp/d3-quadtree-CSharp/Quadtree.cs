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
    
    
    public void Add(Vector2 point) {
        Cover(point);
        Quad curQuad = this.root;

        while(curQuad.quadrants[this.determineQuad(point, curQuad.meanX(), curQuad.meanY())] != null) {
            curQuad = curQuad.quadrants[this.determineQuad(point, curQuad.meanX(), curQuad.meanY())];
        }

        if (!curQuad.ContainsExclusive(point)) {
            while (!curQuad.hasCapacity()) {
                int quadrant = this.determineQuad(point, curQuad.meanX(), curQuad.meanY());
                curQuad = curQuad.MakeChild(quadrant);
            }
        }
        
        curQuad.AddPoint(point);
    }

    public void AddAll(List<Vector2> points) {
        foreach (Vector2 point in points) {
            this.Add(point);
        }
    }

    /*
     * Modifies the quadtree boundaries to cover the given point/node by doubling the boundaries and creating new quads.
     * Also, appearently this implementation assumes that every quadrant is a square.
     */
    void Cover(Vector2 point) {
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

    private int determineQuad(Vector2 point, double xm, double ym) {
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
    
    void Extent() {
        
    }
    
    void Find() {
        
    }
    
    public void Remove(Vector2 point) {
        Quad curQuad = this.root;
        if (!curQuad.ContainsInclusive(point)) return;

        while(curQuad.quadrants[this.determineQuad(point, curQuad.meanX(), curQuad.meanY())] != null) {
            curQuad = curQuad.quadrants[this.determineQuad(point, curQuad.meanX(), curQuad.meanY())];
        }
        
        curQuad.RemovePoint(point);
    }
    
    void RemoveAll() {
        
    }
    
    public void Visit(VisitCallbackSignature VisitCallback) {
        Quad curQuad = this.root;
        this.VisitCallback = VisitCallback;
        VisitAfter(curQuad);
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
    
    void X() {
        
    }
    
    void Y() {
        
    }
    
    void Copy() {
        
    }

    void LeafCopy() {
        
    }

    /*public Vector4 Bounds() {
        return new Vector4(
             this.root.x0, 
             this.root.y0, 
            this.root.x1, 
            this.root.y1);
    }*/

    /*
     * Returns a list of all the points in the quadtree.
     *  - This could be implemented using visit method and adding a callback that pushes the exclusive points to a list.
     */
    public Vector2[] Data() {
        return this.root.InclusivePoints;
    }
    
    public int Size() {
        int size = 0;
        this.Visit((Quad quad) => {
            size++;
        });
        return size;
    }
    
    
}