using d3_essentials;

namespace d3_quadtree_CSharp;

public class Quad
{
    public Node node;
    //Top Right, Top Left, Bottom Left, Bottom Right
    public Quad TR;
    public Quad TL;
    public Quad BL;
    public Quad BR;

    Quad(Node node, Quad TR, Quad TL, Quad BL, Quad BR) {
        this.node = node;
        this.TR = TR;
        this.TL = TL;
        this.BL = BL;
        this.BR = BR;
    }
}