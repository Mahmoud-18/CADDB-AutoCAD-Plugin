using System.Data.SqlClient;
using System.Data;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace CAD_Database
{
    public class DBLoadUtil
    {
     
        public void LoadLines()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor editor = doc.Editor;
            SqlConnection conn = DBUtil.GetConnection();
            
            try
            {
               
                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    TypedValue[] typedValues = new TypedValue[1];
                    typedValues.SetValue(new TypedValue(0, "Line"), 0);
                    SelectionFilter filter = new SelectionFilter(typedValues);

                    PromptSelectionResult sp =editor.SelectAll(filter);

                    if (sp.Status == PromptStatus.OK)
                    {
                        double StartPointX = 0.0, StartPointY = 0.0, EndPointX = 0.0, EndPointY = 0.0;
                        String Layer = "", LineType = "", Color = "";
                        double Length = 0.0;
                        Line line = new Line();
                        SelectionSet ss = sp.Value;

                        String sql = @"INSERT INTO dbo.Lines(StartPointX,StartPointY,EndPointX,EndPointY,layer,Color,Linetype,Length,Created)
                                     Values (@StartPointX,@StartPointY,@EndPointX,@EndPointY,@layer,@Color,@Linetype,@Length,@Created)";
                        conn.Open();

                        foreach (SelectedObject obj in ss) 
                        {
                            line = trans.GetObject(obj.ObjectId,OpenMode.ForRead) as Line;
                            StartPointX = line.StartPoint.X;
                            StartPointY = line.StartPoint.Y;
                            EndPointX = line.EndPoint.X;
                            EndPointY = line.EndPoint.Y;
                            Layer = line.Layer;
                            LineType = line.Linetype;
                            Color = line.Color.ToString();
                            Length = line.Length;

                            SqlCommand cmd = new SqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@StartPointX", StartPointX);
                            cmd.Parameters.AddWithValue("@StartPointY", StartPointY);
                            cmd.Parameters.AddWithValue("@EndPointX", EndPointX);
                            cmd.Parameters.AddWithValue("@EndPointY", EndPointY);
                            cmd.Parameters.AddWithValue("@layer", Layer);
                            cmd.Parameters.AddWithValue("@Linetype", LineType);
                            cmd.Parameters.AddWithValue("@Color", Color);
                            cmd.Parameters.AddWithValue("@Length", Length);
                            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                            cmd.ExecuteNonQuery();
                            
                        }                      
                        editor.WriteMessage("Done. Lines exported Successfully!");
                    }
                    else
                    {
                        editor.WriteMessage("No Object selected.");
                    }
                   
                    
                }
            }

            catch (System.Exception ex)
            {
                editor.WriteMessage(ex.Message);
            }
            finally 
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
           
        }
        public void LoadPolyLines()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor editor = doc.Editor;

            SqlConnection conn = DBUtil.GetConnection();
            
            try
            {
                

                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    TypedValue[] typedValues = new TypedValue[1];
                    typedValues.SetValue(new TypedValue(0, "LWPOLYLINE"), 0);
                    SelectionFilter filter = new SelectionFilter(typedValues);

                    PromptSelectionResult sp = editor.SelectAll(filter);

                    if (sp.Status == PromptStatus.OK)
                    {
                        String layer = "", linetype = "";
                        String coordinates = "";
                        double length = 0.0;
                        bool isClosed = false;
                        Polyline pline = new Polyline();
                        SelectionSet ss = sp.Value;

                        String sql = @"INSERT INTO dbo.Plines(Layer,Linetype,Length,Coordinates,IsClosed,Created)
                                     Values (@Layer,@Linetype,@Length,@Coordinates,@IsClosed,@Created)";
                        conn.Open();

                        foreach (SelectedObject obj in ss)
                        {
                            pline = trans.GetObject(obj.ObjectId, OpenMode.ForRead) as Polyline;                          
                            layer = pline.Layer;
                            linetype = pline.Linetype;                       
                            length = pline.Length;                            
                            isClosed = pline.Closed;

                            var vCount = pline.NumberOfVertices;
                            Point2d point;
                            for (int i = 0; i <= vCount-1; i++)
                            {
                                point = pline.GetPoint2dAt(i);
                                coordinates += point[0].ToString() + "," + point[1].ToString();
                                if (i < vCount-1)
                                    coordinates += "," ;
                            }

                            SqlCommand cmd = new SqlCommand(sql, conn);                           
                            cmd.Parameters.AddWithValue("@Layer", layer);
                            cmd.Parameters.AddWithValue("@LineType", linetype);                            
                            cmd.Parameters.AddWithValue("@Length", length);
                            cmd.Parameters.AddWithValue("@Coordinates", coordinates);
                            cmd.Parameters.AddWithValue("@IsClosed", isClosed);
                            cmd.Parameters.AddWithValue("@Created", DateTime.Now);

                            cmd.ExecuteNonQuery();
                        }
                        editor.WriteMessage("Done. Polylines exported Successfully!");
                    }
                    else
                    {
                        editor.WriteMessage("No Object selected.");
                    }
                   
                    
                }
            }

            catch (System.Exception ex)
            {
                editor.WriteMessage(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            
        }

        public void LoadBlocksNoAttributes()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor editor = doc.Editor;
            SqlConnection conn = DBUtil.GetConnection();
            
            try
            {
                

                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    TypedValue[] typedValues = new TypedValue[1];
                    typedValues.SetValue(new TypedValue(0, "INSERT"), 0);
                    SelectionFilter filter = new SelectionFilter(typedValues);

                    PromptSelectionResult sp = editor.SelectAll(filter);

                    if (sp.Status == PromptStatus.OK)
                    {
                        double insptX = 0.0, insptY = 0.0;
                        String blkname = "", layer = "";
                        double rotation = 0.0;
                        string inspt = "";
                        BlockReference block;
                        SelectionSet ss = sp.Value;

                        String sql = @"INSERT INTO dbo.BlockNoAttribute(InsertionPoint,BlockName,Layer,Rotation,Created)
                                     Values (@InsertionPoint,@BlockName,@Layer,@Rotation,@Created)";
                        conn.Open();

                        foreach (SelectedObject obj in ss)
                        {
                            block = trans.GetObject(obj.ObjectId, OpenMode.ForRead) as BlockReference;
                            if (block.AttributeCollection.Count == 0 & !block.Name.Contains("*"))
                            {
                                insptX = block.Position.X;
                                insptY = block.Position.Y;
                                inspt = insptX.ToString() + "," + insptY.ToString();
                                layer = block.Layer;
                                blkname = block.Name;
                                rotation = block.Rotation;

                                SqlCommand cmd = new SqlCommand(sql, conn);
                                cmd.Parameters.AddWithValue("@InsertionPoint", inspt);
                                cmd.Parameters.AddWithValue("@Layer", layer);
                                cmd.Parameters.AddWithValue("@BlockName", blkname);
                                cmd.Parameters.AddWithValue("@Rotation", rotation);
                                cmd.Parameters.AddWithValue("@Created", DateTime.Now);

                                cmd.ExecuteNonQuery();
                            }

                        }

                        editor.WriteMessage("Done. Blocks exported Successfully!");
                    }
                    else
                    {
                        editor.WriteMessage("No Object selected.");
                    }
                    
                   
                }
            }

            catch (System.Exception ex)
            {
                editor.WriteMessage(ex.Message);               
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            
        }
        
    }
}
