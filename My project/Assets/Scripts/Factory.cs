using System;
using Zenject;

public class Factory
{
    DiContainer container;

    public Factory(DiContainer container) => this.container = container;

    public object Create(Type type ) => container.Resolve(type);
}
