﻿using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    // To be generated
    public readonly partial struct Vertex2DTextured
    {
        public static VertexLayoutDescription Layout = new(
            new VertexElementDescription("vPosition", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("vTextCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2));
    }
}
