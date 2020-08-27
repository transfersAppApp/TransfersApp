using System;
using System.Collections.Generic;
using System.Text;

namespace TransfersBL.Infrastruture
{
    public class BaseModel<T>
    {
        public bool IsDeleted { get; set; } = false;
        public T Id { get; set; }
    }


}
