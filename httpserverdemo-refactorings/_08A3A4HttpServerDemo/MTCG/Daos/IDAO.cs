using _08A3A4HttpServerDemo.MTCG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Daos
{
    public interface IDAO<T>
    {
        T Create(T t);

        T Read(T user);

        T Update(T t);

        void Delete(T t);

        List<T> getAll();
    }
}
