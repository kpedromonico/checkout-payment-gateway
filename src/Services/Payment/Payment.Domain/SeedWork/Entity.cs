using System;

namespace Payment.Domain.SeedWork
{
    public class Entity
    {

        Guid _Id;

        public virtual Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }
    }
}