//I use library SharpGL
using SharpGL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Utility;

namespace DelaunarySample
{
	public partial class Form1: Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private bool _rotate = false;

		private float _rotatePoint = 0;

		private List<PVector> _pointList = new List<PVector>();

		private Delaunay _d;

		private PVector _eyePoint = new PVector(0.5f, 0.5f, 0.25f);

		private List<Triangle> _renderTriangleList = new List<Triangle>();

		private void Form1_Load(object sender, EventArgs e)
		{
			Init();
		}

		private void Init()
		{
			//_pointList = new List<PVector>(SetOuPoint(3));

			_pointList = new List<PVector>(SetRndPoint(10));
			_d = new Delaunay(_pointList);

			_renderTriangleList = new List<Triangle>(_d.OutsideTriangleList);
		}

		private List<PVector> SetRndPoint(int setPointNumber)
		{
			Random rand = new Random(3);
			List<PVector> pointList = new List<PVector>();
			for ( int i = 0; i < setPointNumber; i++ )
			{
				PVector vector = new PVector(
					(float)rand.Next(1, 100) / 100.0f,
					(float)rand.Next(1, 100) / 100.0f,
					(float)rand.Next(1, 100) / 100.0f);
				pointList.Add(vector);
			}
			return pointList;
		}

		private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
		{
			OpenGL gl = this.openGLControl1.OpenGL;

			// Clear The Screen And The Depth Buffe
			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

			// Reset The View
			gl.LoadIdentity();

			MoveGL(gl);
			RenderPoints(gl);

			gl.LoadIdentity();
			MoveGL(gl);

			//render outer triangle

			foreach ( var tri in _renderTriangleList )
			{
				RenderTriangle(gl, tri);
			}

			gl.Flush();

			//rotate
			if ( _rotate )
			{
				_rotatePoint += 3.0f;
			}
		}

		private List<PVector> SetOuPoint(int setPointNumber)
		{
			var rnd = new Random();
			
			List<PVector> pointList = new List<PVector>();
			for ( int k = 0; k < setPointNumber; k++ )
			{
				for ( int i = 0; i < setPointNumber; i++ )
				{
					for ( int j = 0; j < setPointNumber; j++ )
					{
						PVector vector = new PVector(
						(float)( j * 2 + 1 + rnd.NextDouble()) / 10.0f,
						(float)( i * 2 + 1 + rnd.NextDouble()) / 10.0f,
						(float)( k * 2 + 1 + rnd.NextDouble()) / 10.0f * -1.0f);
						pointList.Add(vector);
					}
				}
			}
			return pointList;
		}

		/// <summary>
		/// 図形、視点の移動
		/// </summary>
		/// <param name="gl"></param>
		private void MoveGL(OpenGL gl)
		{
			//gl.LookAt(-10, 10, -10, 0, 0, 0, 0, 1, 0);

			gl.Translate(-0.5f, -0.5f, -3.0f);

			gl.Rotate(_rotatePoint, 0.3f, 1.0f, 0.5f);
		}

		private void RenderTriangleLine(OpenGL gl, Triangle triangle)
		{
			gl.Begin(SharpGL.Enumerations.BeginMode.LineLoop);

			gl.Color(0.0f, 0.0f, 1.0f);			// Blue
			gl.Vertex(triangle.V1.X, triangle.V1.Y, triangle.V1.Z);
			gl.Vertex(triangle.V2.X, triangle.V2.Y, triangle.V2.Z);
			gl.Vertex(triangle.V3.X, triangle.V3.Y, triangle.V3.Z);
			gl.End();
		}

		private void RenderTriangle(OpenGL gl, Triangle triangle)
		{
			gl.Begin(OpenGL.GL_TRIANGLES);					// Start Drawing The Pyramid

			gl.Color(1.0f, 0.0f, 0.0f);			// Red
			gl.Vertex(triangle.V1.X, triangle.V1.Y, triangle.V1.Z);			// Top Of Triangle (Front)
			gl.Color(0.0f, 1.0f, 0.0f);			// Green
			gl.Vertex(triangle.V2.X, triangle.V2.Y, triangle.V2.Z);			// Left Of Triangle (Front)
			gl.Color(0.0f, 0.0f, 1.0f);			// Blue
			gl.Vertex(triangle.V3.X, triangle.V3.Y, triangle.V3.Z);			// Right Of Triangle (Front)

			gl.End();						// Done Drawing The Pyramid
		}

		private void RenderPoints(OpenGL gl)
		{
			gl.PointSize(5.0f);
			gl.Begin(SharpGL.Enumerations.BeginMode.Points);
			gl.Color(0.0f, 1.0f, 0.0f);
			gl.Vertex(_eyePoint.X, _eyePoint.Y, _eyePoint.Z);
			gl.End();

			gl.PointSize(5.0f);
			gl.Begin(SharpGL.Enumerations.BeginMode.Points);

			foreach ( var point in _pointList )
			{
				gl.Color(1.0f, 0.0f, 0.0f);
				gl.Vertex(point.X, point.Y, point.Z);
			}
			gl.End();
		}

		private void rotateButton_Click(object sender, EventArgs e)
		{
			if ( rotateButton.Text == "rotate" )
			{
				rotateButton.Text = "Stop";
				_rotate = true;
			}
			else
			{
				rotateButton.Text = "rotate";
				_rotate = false;
			}
		}
	}
}