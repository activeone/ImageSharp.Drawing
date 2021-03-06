// Copyright (c) Six Labors and contributors.
// Licensed under the GNU Affero General Public License, Version 3.

using System;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Drawing.Tests.TestUtilities.ReferenceCodecs;
using IOPath = System.IO.Path;

namespace SixLabors.ImageSharp.Drawing.Tests
{
    public static partial class TestEnvironment
    {
        private static readonly Lazy<Configuration> ConfigurationLazy = new Lazy<Configuration>(CreateDefaultConfiguration);

        internal static Configuration Configuration => ConfigurationLazy.Value;

        internal static IImageDecoder GetReferenceDecoder(string filePath)
        {
            IImageFormat format = GetImageFormat(filePath);
            return Configuration.ImageFormatsManager.FindDecoder(format);
        }

        internal static IImageEncoder GetReferenceEncoder(string filePath)
        {
            IImageFormat format = GetImageFormat(filePath);
            return Configuration.ImageFormatsManager.FindEncoder(format);
        }

        internal static IImageFormat GetImageFormat(string filePath)
        {
            string extension = IOPath.GetExtension(filePath);

            return Configuration.ImageFormatsManager.FindFormatByFileExtension(extension);
        }

        private static void ConfigureCodecs(
            this Configuration cfg,
            IImageFormat imageFormat,
            IImageDecoder decoder,
            IImageEncoder encoder,
            IImageFormatDetector detector)
        {
            cfg.ImageFormatsManager.SetDecoder(imageFormat, decoder);
            cfg.ImageFormatsManager.SetEncoder(imageFormat, encoder);
            cfg.ImageFormatsManager.AddImageFormatDetector(detector);
        }

        private static Configuration CreateDefaultConfiguration()
        {
            var cfg = new Configuration(
                new JpegConfigurationModule(),
                new GifConfigurationModule(),
                new TgaConfigurationModule()
            );

            // Magick codecs should work on all platforms
            IImageEncoder pngEncoder = IsWindows ? (IImageEncoder)SystemDrawingReferenceEncoder.Png : new PngEncoder();
            IImageEncoder bmpEncoder = IsWindows ? (IImageEncoder)SystemDrawingReferenceEncoder.Bmp : new BmpEncoder();

            cfg.ConfigureCodecs(
                PngFormat.Instance,
                MagickReferenceDecoder.Instance,
                pngEncoder,
                new PngImageFormatDetector());

            cfg.ConfigureCodecs(
                BmpFormat.Instance,
                IsWindows ? (IImageDecoder)SystemDrawingReferenceDecoder.Instance : MagickReferenceDecoder.Instance,
                bmpEncoder,
                new BmpImageFormatDetector());

            return cfg;
        }
    }
}
