using d3_force_CSharp.Utility;

namespace d3_force_CSharp.Forces;

/*
 The Center force simulate the center of gravity of a system of nodes.
    - By Default the center is the coordinates 0,0 but you can change them.
    - Also, this force doesn't take account any weight for the nodes (all nodes have the same mass and they are all shifted in the same effect and same displacement).
 */
public class Center
{
    public double x { get; set; }
    public double y { get; set; }
    public double strength { get; set; }
    private List<Node> nodes;
    
    public Center(List<Node> nodes, double x = 0f, double y = 0f, double strength = 1.0f) {
        this.x = x;
        this.y = y;
        this.strength = strength;
        this.nodes = nodes;
    }
    
    public void force() {
        int i;
        double sx = 0.0f;
        double sy = 0.0f;
        int n = nodes.Count;
        Node curNode;
        
        for (i = 0; i < n; ++i) {
            curNode = nodes[i];
            sx += curNode.x;
            sy += curNode.y;
        }

        for (sx = (sx / n - x) * strength, sy = (sy / n - y) * strength, i = 0; i < n; ++i) {
            curNode = nodes[i];
            curNode.x -= sx;
            curNode.y -= sy;
        }
    }
    
    public void setNodes(List<Node> nodes) {
        this.nodes = nodes;
    }
}