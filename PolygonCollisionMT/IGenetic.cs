using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    public interface IGenetic<T>
    {
        int Score
        {
            get;
        }

        T mutate();
        //List<T> crossover(); 
    }
}
