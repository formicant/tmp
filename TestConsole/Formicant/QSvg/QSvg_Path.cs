using System.Collections.Generic;
using System.Linq;

namespace Formicant
{
	public partial class QSvg
	{
		protected QElement Path(QPath qPath)
		{
			return E("path")
				[A("d", qPath)]
			;
		}
		protected QElement Path(IEnumerable<QPath.QPathNode> nodes)
		{
			return Path(new QPath(nodes));
		}

		protected QPath M(Vector2 point)
		{
			return new QPath(new QPath.QPathNodeMove(point, true));
		}
		protected QPath M(double x, double y)
		{
			return new QPath(new QPath.QPathNodeMove(new Vector2(x, y), true));
		}


		protected class QPath
		{
			public QPath()
			{
				Nodes = Enumerable.Empty<QPathNode>();
			}

			public QPath(QPathNode node)
			{
				Nodes = Enumerable.Repeat(node, 1);
			}

			public QPath(IEnumerable<QPathNode> nodes)
			{
				Nodes = nodes;
			}

			public IEnumerable<QPathNode> Nodes { get; private set; }

			#region Node shorthand methods

			public QPath z
			{
				get { return new QPath(Nodes.Append(new QPathNodeClose())); }
			}

			public QPath m(Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeMove(point, false)));
			}
			public QPath m(double x, double y)
			{
				return m(new Vector2(x, y));
			}

			public QPath M(Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeMove(point, true)));
			}
			public QPath M(double x, double y)
			{
				return M(new Vector2(x, y));
			}

			public QPath l(Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeLine(point, false)));
			}
			public QPath l(double x, double y)
			{
				return l(new Vector2(x, y));
			}

			public QPath L(Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeLine(point, true)));
			}
			public QPath L(double x, double y)
			{
				return L(new Vector2(x, y));
			}

			public QPath h(double x)
			{
				return new QPath(Nodes.Append(new QPathNodeHorizontal(x, false)));
			}

			public QPath H(double x)
			{
				return new QPath(Nodes.Append(new QPathNodeHorizontal(x, true)));
			}

			public QPath v(double y)
			{
				return new QPath(Nodes.Append(new QPathNodeVertical(y, false)));
			}

			public QPath V(double y)
			{
				return new QPath(Nodes.Append(new QPathNodeVertical(y, true)));
			}

			public QPath c(Vector2 tangent1, Vector2 tangent2, Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeCurve(tangent1, tangent2, point, false)));
			}
			public QPath c(double x1, double y1, double x2, double y2, double x, double y)
			{
				return c(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x, y));
			}

			public QPath C(Vector2 tangent1, Vector2 tangent2, Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeCurve(tangent1, tangent2, point, true)));
			}
			public QPath C(double x1, double y1, double x2, double y2, double x, double y)
			{
				return C(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x, y));
			}

			public QPath s(Vector2 tangent2, Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeSmoothCurve(tangent2, point, false)));
			}
			public QPath s(double x2, double y2, double x, double y)
			{
				return s(new Vector2(x2, y2), new Vector2(x, y));
			}

			public QPath S(Vector2 tangent2, Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeSmoothCurve(tangent2, point, true)));
			}
			public QPath S(double x2, double y2, double x, double y)
			{
				return S(new Vector2(x2, y2), new Vector2(x, y));
			}

			public QPath q(Vector2 tangent, Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeQuadraticCurve(tangent, point, false)));
			}
			public QPath q(double x1, double y1, double x, double y)
			{
				return q(new Vector2(x1, y1), new Vector2(x, y));
			}

			public QPath Q(Vector2 tangent, Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeQuadraticCurve(tangent, point, true)));
			}
			public QPath Q(double x1, double y1, double x, double y)
			{
				return Q(new Vector2(x1, y1), new Vector2(x, y));
			}

			public QPath t(Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeSmoothQuadraticCurve(point, false)));
			}
			public QPath t(double x, double y)
			{
				return t(new Vector2(x, y));
			}

			public QPath T(Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeSmoothQuadraticCurve(point, true)));
			}
			public QPath T(double x, double y)
			{
				return T(new Vector2(x, y));
			}

			public QPath a(Vector2 radii, double rotation, bool largeArc, bool sweep, Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeArc(radii, rotation, largeArc, sweep, point, false)));
			}
			public QPath a(double radius, bool largeArc, bool sweep, Vector2 point)
			{
				return a(new Vector2(radius, radius), 0, largeArc, sweep, point);
			}
			public QPath a(double rx, double ry, double rotation, bool largeArc, bool sweep, double x, double y)
			{
				return a(new Vector2(rx, ry), rotation, largeArc, sweep, new Vector2(x, y));
			}
			public QPath a(double radius, bool largeArc, bool sweep, double x, double y)
			{
				return a(new Vector2(radius, radius), 0, largeArc, sweep, new Vector2(x, y));
			}

			public QPath A(Vector2 radii, double rotation, bool largeArc, bool sweep, Vector2 point)
			{
				return new QPath(Nodes.Append(new QPathNodeArc(radii, rotation, largeArc, sweep, point, true)));
			}
			public QPath A(double rx, double ry, double rotation, bool largeArc, bool sweep, double x, double y)
			{
				return A(new Vector2(rx, ry), rotation, largeArc, sweep, new Vector2(x, y));
			}

			#endregion

			#region Path nodes

			public abstract class QPathNode
			{
				protected QPathNode(bool absolute)
				{
					Absolute = absolute;
				}

				public bool Absolute { get; private set; }

				public abstract char Char { get; }

				public abstract IEnumerable<double> Values { get; }
			}

			public abstract class QPathNodePoint : QPathNode
			{
				protected QPathNodePoint(Vector2 point, bool absolute)
					: base(absolute)
				{
					Point = point;
				}

				public Vector2 Point { get; private set; }
			}

			public class QPathNodeClose : QPathNode
			{
				public QPathNodeClose()
					: base(false) { }

				public override char Char { get { return 'z'; } }

				public override IEnumerable<double> Values
				{
					get { return Enumerable.Empty<double>(); }
				}
			}

			public class QPathNodeMove : QPathNodePoint
			{
				public QPathNodeMove(Vector2 point, bool absolute = true)
					: base(point, absolute) { }

				public override char Char { get { return 'm'; } }

				public override IEnumerable<double> Values
				{
					get { return new[] { Point.X, Point.Y }; }
				}
			}

			public class QPathNodeLine : QPathNodePoint
			{
				public QPathNodeLine(Vector2 point, bool absolute = true)
					: base(point, absolute) { }

				public override char Char { get { return 'l'; } }

				public override IEnumerable<double> Values
				{
					get { return new[] { Point.X, Point.Y }; }
				}
			}

			public class QPathNodeHorizontal : QPathNode
			{
				public QPathNodeHorizontal(double x, bool absolute = true)
					: base(absolute)
				{
					X = x;
				}

				public override char Char { get { return 'h'; } }

				public double X { get; private set; }

				public override IEnumerable<double> Values
				{
					get { return new[] { X }; }
				}
			}

			public class QPathNodeVertical : QPathNode
			{
				public QPathNodeVertical(double y, bool absolute = true)
					: base(absolute)
				{
					Y = y;
				}

				public override char Char { get { return 'v'; } }

				public double Y { get; private set; }

				public override IEnumerable<double> Values
				{
					get { return new[] { Y }; }
				}
			}

			public class QPathNodeCurve : QPathNodePoint
			{
				public QPathNodeCurve(Vector2 tangent1, Vector2 tangent2, Vector2 point, bool absolute = true)
					: base(point, absolute)
				{
					Tangent1 = tangent1;
					Tangent2 = tangent2;
				}

				public Vector2 Tangent1 { get; private set; }
				public Vector2 Tangent2 { get; private set; }

				public override char Char { get { return 'c'; } }

				public override IEnumerable<double> Values
				{
					get { return new[] { Tangent1.X, Tangent1.Y, Tangent2.X, Tangent2.Y, Point.X, Point.Y }; }
				}
			}

			public class QPathNodeSmoothCurve : QPathNodePoint
			{
				public QPathNodeSmoothCurve(Vector2 tangent2, Vector2 point, bool absolute = true)
					: base(point, absolute)
				{
					Tangent2 = tangent2;
				}

				public Vector2 Tangent2 { get; private set; }

				public override char Char { get { return 's'; } }

				public override IEnumerable<double> Values
				{
					get { return new[] { Tangent2.X, Tangent2.Y, Point.X, Point.Y }; }
				}
			}

			public class QPathNodeQuadraticCurve : QPathNodePoint
			{
				public QPathNodeQuadraticCurve(Vector2 tangent, Vector2 point, bool absolute = true)
					: base(point, absolute)
				{
					Tangent = tangent;
				}

				public Vector2 Tangent { get; private set; }

				public override char Char { get { return 'q'; } }

				public override IEnumerable<double> Values
				{
					get { return new[] { Tangent.X, Tangent.Y, Point.X, Point.Y }; }
				}
			}

			public class QPathNodeSmoothQuadraticCurve : QPathNodePoint
			{
				public QPathNodeSmoothQuadraticCurve(Vector2 point, bool absolute = true)
					: base(point, absolute) { }

				public override char Char { get { return 't'; } }

				public override IEnumerable<double> Values
				{
					get { return new[] { Point.X, Point.Y }; }
				}
			}

			public class QPathNodeArc : QPathNodePoint
			{
				public QPathNodeArc(Vector2 radii, double rotation, bool largeArc, bool sweep, Vector2 point, bool absolute = true)
					: base(point, absolute)
				{
					Radii = radii;
					Rotation = rotation;
					LargeArc = largeArc;
					Sweep = sweep;
				}

				public Vector2 Radii { get; private set; }
				public double Rotation { get; private set; }
				public bool LargeArc { get; private set; }
				public bool Sweep { get; private set; }

				public override char Char { get { return 'a'; } }

				public override IEnumerable<double> Values
				{
					get { return new[] { Radii.X, Radii.Y, Rotation, LargeArc ? 1 : 0, Sweep ? 1 : 0, Point.X, Point.Y }; }
				}
			}

			#endregion
		}
	}
}
