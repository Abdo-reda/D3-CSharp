using System.Numerics;
using d3_quadtree_CSharp;

namespace quadtree_visualization;


public partial class QuadtreeVisualization : Form
{
    private Quadtree quadtree;
    private System.ComponentModel.IContainer components = null;

    public QuadtreeVisualization(): base() {
        Console.WriteLine("Creating Quadtree ...");
        InitializeComponent();
        List<Vector2> points = new List<Vector2>();
        Random rnd = new Random();
        /*for (int i=0; i<rnd.Next(15,25); i++) {
            int tempX = rnd.Next(0,200);
            int tempY = rnd.Next(0,200);
            Console.WriteLine(tempX + ", " + tempY);
            points.Add(new Vector2(tempX, tempY));
        }*/
        // points.Add(new Vector2(0, 0));
        // points.Add(new Vector2(100, 100));
        points.Add(new Vector2(40, 30));
        // points.Add(new Vector2(-150, -120));
        // points.Add(new Vector2(20, 120));
        // points.Add(new Vector2(80, 160));
        // points.Add(new Vector2(40, 20));
        this.quadtree = new Quadtree(points, 2);
        Console.WriteLine("Created Quadtree Successfully");
        Console.WriteLine("It has " + this.quadtree.Size() + " quads");
        this.Click += handleClick;
    }

    protected override void OnPaint(PaintEventArgs e) {
        base.OnPaint(e);
        DrawQuadtree(e.Graphics);
    }
    
    private void DrawQuadtree(Graphics g) {
        
        g.Clear(Color.Azure);
        
        this.quadtree.Visit((Quad curQuad) => {
            Console.WriteLine("executing visit callback");
            // Console.WriteLine(g.VisibleClipBounds.Height - (curQuad.y1 - curQuad.y0) - curQuad.y0);
            RectangleF rootQuad = new RectangleF(
                (float) curQuad.x0 + g.VisibleClipBounds.Width/2,
                (float) (g.VisibleClipBounds.Height/2 - curQuad.y1), 
                (float) (curQuad.x1 - curQuad.x0),
                (float) (curQuad.y1 - curQuad.y0)
            );
            g.DrawRectangle(Pens.Crimson, rootQuad);
        });
        
        
        foreach (Vector2 point in quadtree.Data()) {
            PointF pointPosition = new PointF(point.X + g.VisibleClipBounds.Width/2, g.VisibleClipBounds.Height/2 - point.Y);
            g.FillEllipse(Brushes.Black, pointPosition.X - 2, pointPosition.Y - 2, 4, 4);
        }
    }
    
    
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }

        base.Dispose(disposing);
    }
    
    private void InitializeComponent() {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 800);
        this.Text = "QuadTree Visualization";
    }

    private void handleClick(object sender, EventArgs e) {
        MouseEventArgs me = (MouseEventArgs) e;
        using (Graphics g = this.CreateGraphics()) {
            Vector2 newPoint = new Vector2((me.X - g.VisibleClipBounds.Width / 2), (g.VisibleClipBounds.Height / 2 - me.Y));
            Console.WriteLine("Clicked" +  newPoint.X + "," +  newPoint.Y);
            quadtree.Add(newPoint);
            this.DrawQuadtree(g);
        }
    }
}