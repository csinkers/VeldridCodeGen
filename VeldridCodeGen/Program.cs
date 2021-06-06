﻿using System.IO;
using System.Reflection;
using UAlbion.Api;
using UAlbion.Api.Visual;
using UAlbion.Core.SpriteBatch;
using UAlbion.Core.Visual;
using Veldrid;

namespace UAlbion.Core
{
    class Program
    {
        static void Main()
        {
            var binDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var exchange = new EventExchange();
            var textureSource = new TextureSource();
            var layoutSource = new ResourceLayoutSource();
            var fileSystem = new FileSystem();
            var shaderCache = new ShaderCache(Path.Combine(binDir, "Shaders"));
            var framebuffer = new MainFramebuffer();

            var palette = BuildPalette();
            var scene = new SpriteScene(palette, framebuffer);
            var engine = new Engine(GraphicsBackend.Direct3D11, false, scene);

            exchange
                .Register<IFileSystem>(fileSystem)
                .Register<IResourceLayoutSource>(layoutSource)
                .Register<ITextureSource>(textureSource)
                .Register<IShaderCache>(shaderCache)
                .Attach(framebuffer)
                .Attach(scene)
                .Attach(engine)
                ;

            engine.Run();
        }

        static SimpleTexture<uint> BuildPalette()
        {
            var palette = new SimpleTexture<uint>(new AssetId(0, "Palette"), 256, 1);
            var pixels = palette.MutablePixelData;
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = ApiUtil.PackColor((byte) i, (byte) i, (byte) i, 0xff);
            pixels[0] = 0; // 0 is transparent
            return palette;
        }
    }
}
