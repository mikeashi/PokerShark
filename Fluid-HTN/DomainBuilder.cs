﻿using FluidHTN.Factory;

namespace FluidHTN
{
    /// <summary>
    ///     A simple domain builder for easy use when one just need the core functionality
    ///     of the BaseDomainBuilder. This class is sealed, so if you want to extend the
    ///     functionality of the domain builder, extend BaseDomainBuilder instead.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DomainBuilder<T, TWorldStateEntry> : BaseDomainBuilder<DomainBuilder<T, TWorldStateEntry>, T, TWorldStateEntry>
        where T : IContext<TWorldStateEntry>
    {
        // ========================================================= CONSTRUCTION

        public DomainBuilder(string domainName) : base(domainName, new DefaultFactory())
        {
        }

        public DomainBuilder(string domainName, IFactory factory) : base(domainName, factory)
        {
        }
    }
}
