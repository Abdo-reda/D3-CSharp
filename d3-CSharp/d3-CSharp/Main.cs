
// This class is used for testing purposes

using System.Numerics;
using d3_essentials;
using d3_quadtree_CSharp;

Quadtree tempQuadtree = new Quadtree();
tempQuadtree.Add(new Vector2(20, 40));



/* TODO:
 - Make the output of the project be a CLASS not a window applicaiton.
 - The Node List should be static and not passed to each constructor ... right?
 - The Node List could be an array instead of a list. This will make it more performant.
 - Should I use floats or doubles throughout the projects?
  - float are less precise, generally more faster, and take less memory!
 - Should I replace the Vector2 with a Node struct or with my Own Point Struct that uses doubles instead !
 
*/