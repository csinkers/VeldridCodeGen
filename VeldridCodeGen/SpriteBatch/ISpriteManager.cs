﻿namespace UAlbion.Core.SpriteBatch
{
    public interface ISpriteManager
    {
        SpriteLease Borrow(SpriteKey key, int length, object caller);
        void Cleanup();
        WeakSpriteReference MakeWeakReference(SpriteLease lease, int index);
    }
}