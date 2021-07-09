using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd.Mappers
{
    interface IMapper
    {
        public interface IMapper<T>
        {
            T GetByID(int id);
            void Save(T t);
            void Delete(T t);
        }
    }
}
