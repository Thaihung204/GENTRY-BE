using System;
using System.Collections.Generic;
using System.Text;

namespace GENTRY.Models.Interfaces
{
    public interface IEntity<T> : IEntity
    {
        new T Id { get; set; }
    }
}
