namespace d3_essentials;

public struct Node //could use a factory if I want to have an optional or default T data type.
{
    
    //- change the location of the node class
    //- also make it a struct?
    //- also add T for data for each node.
    
    //public delegate double RadiusFunction(Node node, int index, List<Node> nodes);
    public delegate double RadiusFunction(Node node);
    static int counter = 0; //should this be moved to the factory? note Node that are created with the same generic T will have the same counter.
    
    //position (could use vector2)
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
    // public T? data { get; set; } //TODO: maybe implement a generic T data type. however, I will have to modify anyone that uses Node struct, any forces, quadtree, etc.
    
    public Node(double x, double y, double radius = 1, string name="node") {
        this.x = x;
        this.y = y;
        this._radius = radius;
        this.index = counter;
        this.name = name + counter;
        counter++;
        // this.data = data; T? data = default(T)
    } 
    
    public static void SetRadiusFunction(RadiusFunction customFunction)
    {
        CalcRadius = customFunction;
    }
    
    private static double DefaultRadius(Node node) {
        return node._radius;
    }
}