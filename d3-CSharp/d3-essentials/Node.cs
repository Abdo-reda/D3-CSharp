namespace d3_essentials;

public struct Node
{
    
    //- change the location of the node class
    //- also make it a struct?
    //- also add T for data for each node.
    
    //public delegate double RadiusFunction(Node node, int index, List<Node> nodes);
    public delegate double RadiusFunction(Node node);
    
    static int counter = 0;
    //position (could use vectors)
    public double x { get; set; }
    public double y { get; set; }
    //velocity
    public double vx { get; private set; }
    public double vy { get; private set; }
    //identifier
    public int index { get; private set; }
    //shape
    private double _radius { get; set; }
    public double radius() { return CalcRadius(this); }
    private static RadiusFunction CalcRadius = DefaultRadius;
    //data
    public string name { get; set; }
    //public T data { get; set; }
    
    Node(double x, double y, double radius = 1, string name="placeholder") {
        this.x = x;
        this.y = y;
        this._radius = radius;
        this.index = counter;
        this.name = name + counter;
        counter++;
    }
    
    public static void SetRadiusFunction(RadiusFunction customFunction)
    {
        CalcRadius = customFunction;
    }
    
    private static double DefaultRadius(Node node) {
        return node._radius;
    }
}