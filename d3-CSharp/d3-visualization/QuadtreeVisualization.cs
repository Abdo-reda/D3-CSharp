using System.Numerics;
using d3_quadtree_CSharp;

namespace quadtree_visualization;


public partial class QuadtreeVisualization : Form
{
    private Quadtree quadtree;

    public QuadtreeVisualization(): base() {
        Console.WriteLine("Creating Quadtree ...");
        InitializeComponent();
        List<Vector2> points = new List<Vector2>();
        Random rnd = new Random();
        for (int i=0; i<rnd.Next(15,25); i++) {
            int tempX = rnd.Next(-100,200);
            int tempY = rnd.Next(-100,200);
            Console.WriteLine(tempX + ", " + tempY);
            points.Add(new Vector2(tempX, tempY));
        }
        // points.Add(new Vector2(50, -20));
        this.quadtree = new Quadtree(points, 1);
        Console.WriteLine("Created Quadtree Successfully");
    }

    protected override void OnPaint(PaintEventArgs e) {
        base.OnPaint(e);
        DrawQuadtree(e.Graphics);
    }
    
    private void DrawQuadtree(Graphics g) {
        
        Console.WriteLine("before visit callback");
        this.quadtree.Visit((Quad curQuad) => {
            Console.WriteLine("executing visit callback");
            // Console.WriteLine(g.VisibleClipBounds.Height - (curQuad.y1 - curQuad.y0) - curQuad.y0);
            RectangleF rootQuad = new RectangleF(
                (float) curQuad.x0 + g.VisibleClipBounds.Width/2,
                (float) curQuad.y0 + g.VisibleClipBounds.Height/2, 
                (float) (curQuad.x1 - curQuad.x0),
                (float) (curQuad.y1 - curQuad.y0)
            );
            g.DrawRectangle(Pens.Crimson, rootQuad);
        });
        
        
        foreach (Vector2 point in quadtree.GetAllPoints()) {
            PointF pointPosition = new PointF(point.X + g.VisibleClipBounds.Width/2, point.Y + g.VisibleClipBounds.Height/2);
            g.FillEllipse(Brushes.Black, pointPosition.X - 2, pointPosition.Y - 2, 4, 4);
        }
    }
    
    private System.ComponentModel.IContainer components = null;
    
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }

        base.Dispose(disposing);
    }
    
    private void InitializeComponent() {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "QuadTree Visualization";
    }
}