// Copyright (c) Six Labors and contributors.
// Licensed under the GNU Affero General Public License, Version 3.

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors;

namespace SixLabors.ImageSharp.Drawing.Processing.Processors.Drawing
{
    /// <summary>
    /// Defines a processor to fill <see cref="Image"/> pixels withing a given <see cref="IPath"/>
    /// with the given <see cref="IBrush"/> and blending defined by the given <see cref="ShapeGraphicsOptions"/>.
    /// </summary>
    public class DrawPathProcessor : IImageProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawPathProcessor" /> class.
        /// </summary>
        /// <param name="options">The graphics options.</param>
        /// <param name="pen">The details how to outline the region of interest.</param>
        /// <param name="shape">The shape to be filled.</param>
        public DrawPathProcessor(ShapeGraphicsOptions options, IPen pen, IPath shape)
        {
            this.Shape = shape;
            this.Pen = pen;
            this.Options = options;
        }

        /// <summary>
        /// Gets the <see cref="IBrush"/> used for filling the destination image.
        /// </summary>
        public IPen Pen { get; }

        /// <summary>
        /// Gets the region that this processor applies to.
        /// </summary>
        public IPath Shape { get; }

        /// <summary>
        /// Gets the <see cref="ShapeGraphicsOptions"/> defining how to blend the brush pixels over the image pixels.
        /// </summary>
        public ShapeGraphicsOptions Options { get; }

        /// <inheritdoc />
        public IImageProcessor<TPixel> CreatePixelSpecificProcessor<TPixel>(Configuration configuration, Image<TPixel> source, Rectangle sourceRectangle)
            where TPixel : unmanaged, IPixel<TPixel>
            => new FillRegionProcessor(this.Options, this.Pen.StrokeFill, new ShapePath(this.Shape, this.Pen)).CreatePixelSpecificProcessor(configuration, source, sourceRectangle);
    }
}
