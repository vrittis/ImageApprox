using System;
using System.Drawing;
using System.Linq;
using imG.Approx;
using imG.Approx.Mutation;

namespace ImageApprox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var processFactory = new ProcessFactory("data/miro.jpg", new Random().Next());
            //processFactory.TargetFactory = new Func<ITarget>(() => new Target("data/static.squarespace.com.jpg", 300));
            Process process = processFactory.Build();
            //process.ShapeFactoryCatalog.DisableAll();
            //process.ShapeFactoryCatalog.Enable("blob");
            //process.ShapeFactoryCatalog.Enable("circle");
            //process.ShapeFactoryCatalog.Enable("line");
            //process.ShapeFactoryCatalog.Enable("bezier");
              
            process.NewGenerationOccured += process_NewGenerationOccured;
            process.BetterFitnessFound += process_BetterFitnessFound;

            do
            {
                process.Mutate();
            } while (process.Generation < 500000);
        }

        private static void process_BetterFitnessFound(Process process)
        {
            //if (process.Evolutions%5 == 0)
            //{
                SaveImage(process.Drawing.Draw(),
                    string.Format("{0}.{1}.{2:000000}.{3:0000000000}.{4:0000000000}.png", process.Target.Name, process.RandomizationProvider.Seed,
                        process.Generation,
                        process.Evolutions,
                        process.DistanceToTarget));
            //}
        }

        private static void process_NewGenerationOccured(Process process)
        {
            if (process.Generation % 100 == 0)
            {
            Console.WriteLine(
                "Generation {0:00000000} : distance to target: {2:0000000000}", process.Generation, "",
                process.DistanceToTarget);
            Console.WriteLine("Shapes {0:000}", process.Drawing.Shapes.Count);
                Console.WriteLine("Factories: {0}", string.Join(" ,", process.ShapeFactoryCatalog.ActiveFactories().Select(f => f.Name).ToArray()));
            Console.SetCursorPosition(0, 0);
            }
        }

        private static void EnableRandomFactory(Process process)
        {
            process.ShapeFactoryCatalog.EnableAll();
            var factories = process.ShapeFactoryCatalog.ActiveFactories();
            var selectedName = factories[process.RandomizationProvider.Next(0, factories.Count)].Name;
            process.ShapeFactoryCatalog.DisableAll();
            process.ShapeFactoryCatalog.Enable(selectedName);
        }


        private static void SaveImage(Bitmap p, string name)
        {
            p.Save(name);
        }
    }
}