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

    public Quadtree(List<Vector2> points, double x0, double y0, double x1, double y1) {
        this.root = new Quad(x0, y0, x1, y1);
        // this.root.nodes = nodes;
        // I will probably have to create a function to add nodes to the quadtree.
    }
    
    
    void Add(Vector2 point) {
        Cover(point);
        
    }

    void AddAll() {
        
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
            Quad extendedQuad = new Quad();
            xRange *= 2;
            yRange *= 2;
            if (x < x0 && y < y0) {
                extendedQuad.BL = this.root;
                x0 = x1 - xRange;
                y0 = y1 - yRange;
            } else if (x < x0) {
                extendedQuad.TL = this.root;
                x0 = x1 - xRange;
                y1 = y0 + yRange;
            } else if (x >= x1 && y >= y1) {
                extendedQuad.TR = this.root;
                x1 = x0 + xRange;
                y1 = y0 + yRange;
            } else {
                extendedQuad.BR = this.root;
                x1 = x0 + xRange;
                y0 = y1 - yRange;
            }
            
            extendedQuad.SetBoundaries(x0, y0, x1, y1);
            this.root = extendedQuad;
        }
    }
    
    void Data() {
        
    }
    
    void Extent() {
        
    }
    
    void Find() {
        
    }
    
    void Remove() {
        
    }
    
    void RemoveAll() {
        
    }
    
    Quad Root() {
        return this.root;
    }
    
    void Size() {
        
    }
    
    void Visit() {
        
    }
    
    void VisitAfter() {
        
    }
    
    void X() {
        
    }
    
    void Y() {
        
    }
    
    void Copy() {
        
    }

    void LeafCopy() {
        
    }
    
}