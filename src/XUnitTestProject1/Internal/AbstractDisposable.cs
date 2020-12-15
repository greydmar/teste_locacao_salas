using System;
using Nito.Disposables;

namespace locacao.tests.Internal
{
    public abstract class AbstractDisposable : SingleDisposable<AbstractDisposable.NoopContext>
    {
        public sealed class NoopContext { }

        protected AbstractDisposable()
            : base(new NoopContext())
        {
        }
        protected sealed override void Dispose(NoopContext context)
        {
            OnDispose(IsDisposing);
        }

        protected virtual void OnDispose(bool disposing) { }

        protected virtual void EnsureNotDisposed()
        {
            if (IsDisposed || IsDisposing)
                throw new ObjectDisposedException($"{this.GetType().FullName}{this.GetHashCode()}");
        }

    }
}