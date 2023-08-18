using d3_force_CSharp.Utility;

namespace d3_force_CSharp.Forces;

/*
 The Collide force simulate .... 
 */
public class Collide
{
    
    public delegate double RadiusFunction(Node node, int index, List<Node> nodes);
    
    public double radii { get; set; }
    public double strength { get; set; }
    public int iterations { get; set; }
    private List<Node> nodes;
    
    public double radius { get; set; }
    private RadiusFunction CalcRadius;
    private double Radius(Node node, int index, List<Node> nodes)
    {
        return CalcRadius(node, index, nodes);
    }
    
    public Collide(List<Node> nodes, double radius = 1f, double strength = 1.0f, int iterations = 1) {
        this.radius = radius;
        this.strength = strength;
        this.iterations = iterations;
        this.nodes = nodes;
        CalcRadius = DefaultRadius;
    }
    
    public Collide(RadiusFunction radiusFunc, List<Node> nodes, double strength = 1.0f, int iterations = 1) {
        this.strength = strength;
        this.iterations = iterations;
        this.nodes = nodes;
        CalcRadius = radiusFunc;
    }
    
    public void force() {
        int i;
        int n = nodes.Count;
        Node curNode;
        //tree
        double xi;
        double yi;
        double ri;
        double ri2;

        for (int k=0; k<iterations; ++k) {
            //create a quadtree
            for (i=0; i<n; ++i) {
                curNode = nodes[i];
                //ri = radii[curNode.index], ri2 = ri * ri;
                xi = curNode.x + curNode.vx;
                yi = curNode.y + curNode.vy;
                //tree.visit (apply)
                /*ri = curNode.r + radius;
                ri2 = ri * ri;
                curNode.x = Math.Max(radius, Math.Min(width - radius, curNode.x));
                curNode.y = Math.Max(radius, Math.Min(height - radius, curNode.y));*/
            }
        }
    }
    
    void initialize(List<Node> nodes) {
        this.nodes = nodes;
    }
    
    public void SetRadiusFunction(RadiusFunction customFunction)
    {
        CalcRadius = customFunction;
    }
    
    private double DefaultRadius(Node node, int index, List<Node> nodes)
    {
        return radius;
    }
    
    
}