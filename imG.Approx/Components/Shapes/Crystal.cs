//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using imG.Approx.Components.BuildingBlocks;
//using imG.Approx.Mutation;
//using Color = imG.Approx.Components.BuildingBlocks.Color;

//namespace imG.Approx.Components.Shapes
//{
    

//    public class Crystal : IShape
//    {
    

//        public Facet Facet { get; set; }

//        public Crystal()
//        {
//            Facets = new List<Facet>();
//        }

//        public Crystal(Crystal source)
//        {
//            Facets = source.Facets.Select(f => f.Clone()).ToList();
//        }

//        public IShape Clone()
//        {
//            return new Crystal(this);
//        }

//        public IMutable[] MutableComponents { get; }
//        public void Draw(Graphics g)
//        {
            
//        }

//        public void InitializeComponents(Process process)
//        {

//        }
//    }
//}
