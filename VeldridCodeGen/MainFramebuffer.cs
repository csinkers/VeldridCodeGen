﻿using UAlbion.Core.SpriteBatch;
using Veldrid;

namespace UAlbion.Core
{
    public class MainFramebuffer : FramebufferHolder
    {
        public MainFramebuffer() : base(0, 0) { }
        protected override Framebuffer CreateFramebuffer(IVeldridInitEvent e) 
            => e.Device.SwapchainFramebuffer;
    }
}