using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Interfaces.Entities
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
