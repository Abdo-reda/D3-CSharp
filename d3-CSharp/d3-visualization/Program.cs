namespace quadtree_visualization;

static class Program
{
    [STAThread]
    static void Main() {
        ApplicationConfiguration.Initialize();
        Application.Run(new QuadtreeVisualization());
        // Application.EnableVisualStyles();
        // Application.SetCompatibleTextRenderingDefault(false);
        //Application.Run(new QuadtreeVisualization());
    }
}