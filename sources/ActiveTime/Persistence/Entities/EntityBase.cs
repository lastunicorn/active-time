using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.Persistence.Entities
{
    public class EntityBase
    {
        /// <summary>
        /// Gets or sets an integer value that uniquely identifies the comment.
        /// </summary>
        public virtual int Id { get; set; }
    }
}
