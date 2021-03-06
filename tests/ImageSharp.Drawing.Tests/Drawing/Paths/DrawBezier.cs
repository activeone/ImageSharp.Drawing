// Copyright (c) Six Labors and contributors.
// Licensed under the GNU Affero General Public License, Version 3.

using System.Linq;
using System.Numerics;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing.Processing.Processors.Drawing;
using SixLabors.ImageSharp.Drawing.Tests.Processing;
using SixLabors.ImageSharp.Drawing.Tests.TestUtilities;
using Xunit;

namespace SixLabors.ImageSharp.Drawing.Tests.Drawing.Paths
{
    public class DrawBezier : BaseImageOperationsExtensionTest
    {
        IPen pen = Pens.Solid(Color.HotPink, 2);

        PointF[] points = new PointF[]{
            new PointF(10, 10),
            new PointF(20, 20),
            new PointF(20, 50),
            new PointF(50, 10)
            };

        private void VerifyPoints(PointF[] expectedPoints, IPath path)
        {
            var innerPath = Assert.IsType<Path>(path);
            var segment = Assert.Single(innerPath.LineSegments);
            var bezierSegment = Assert.IsType<CubicBezierLineSegment>(segment);
            Assert.Equal(expectedPoints, bezierSegment.ControlPoints.ToArray());

            var simplePath = Assert.Single(path.Flatten());
            Assert.False(simplePath.IsClosed);
        }

        [Fact]
        public void Pen()
        {
            this.operations.DrawBeziers(new ShapeGraphicsOptions(), this.pen, this.points);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.NotEqual(this.shapeOptions, processor.Options.ShapeOptions);
            this.VerifyPoints(this.points, processor.Shape);
            Assert.Equal(this.pen, processor.Pen);
        }

        [Fact]
        public void PenDefaultOptions()
        {
            this.operations.DrawBeziers(this.pen, this.points);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.Equal(this.shapeOptions, processor.Options.ShapeOptions);
            this.VerifyPoints(this.points, processor.Shape);
            Assert.Equal(this.pen, processor.Pen);
        }

        [Fact]
        public void BrushAndThickness()
        {
            this.operations.DrawBeziers(new ShapeGraphicsOptions(), this.pen.StrokeFill, 10, this.points);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.NotEqual(this.shapeOptions, processor.Options.ShapeOptions);
            this.VerifyPoints(this.points, processor.Shape);
            Assert.Equal(this.pen.StrokeFill, processor.Pen.StrokeFill);
            Assert.Equal(10, processor.Pen.StrokeWidth);
        }

        [Fact]
        public void BrushAndThicknessDefaultOptions()
        {
            this.operations.DrawBeziers(this.pen.StrokeFill, 10, this.points);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.Equal(this.shapeOptions, processor.Options.ShapeOptions);
            this.VerifyPoints(this.points, processor.Shape);
            Assert.Equal(this.pen.StrokeFill, processor.Pen.StrokeFill);
            Assert.Equal(10, processor.Pen.StrokeWidth);
        }

        [Fact]
        public void ColorAndThickness()
        {
            this.operations.DrawBeziers(new ShapeGraphicsOptions(), Color.Red, 10, this.points);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.NotEqual(this.shapeOptions, processor.Options.ShapeOptions);
            this.VerifyPoints(this.points, processor.Shape);
            var brush = Assert.IsType<SolidBrush>(processor.Pen.StrokeFill);
            Assert.Equal(Color.Red, brush.Color);
            Assert.Equal(10, processor.Pen.StrokeWidth);
        }

        [Fact]
        public void ColorAndThicknessDefaultOptions()
        {
            this.operations.DrawBeziers(Color.Red, 10, this.points);

            DrawPathProcessor processor = this.Verify<DrawPathProcessor>();

            Assert.Equal(this.shapeOptions, processor.Options.ShapeOptions);
            this.VerifyPoints(this.points, processor.Shape);
            var brush = Assert.IsType<SolidBrush>(processor.Pen.StrokeFill);
            Assert.Equal(Color.Red, brush.Color);
            Assert.Equal(10, processor.Pen.StrokeWidth);
        }
    }
}
