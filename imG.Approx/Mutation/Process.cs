using System;
using imG.Approx.Components;
using imG.Approx.Components.Shapes.Factories;

namespace imG.Approx.Mutation
{
    public class Process
    {
        public delegate void MutationOccuredEventhandler(Process process);

        public delegate void NewGenerationOccuredEventHandler(Process process);

        internal Process(IRandomizationProvider randomizationProvider,
            IMutationDescriptionCatalog mutationDescriptionCatalog, ITarget target,
            IShapeFactoryCatalog shapeFactoryCatalog)
        {
            if (randomizationProvider == null)
                throw new ArgumentNullException("randomizationProvider");

            if (mutationDescriptionCatalog == null)
                throw new ArgumentNullException("mutationDescriptionCatalog");

            if (target == null)
                throw new ArgumentNullException("target");

            if (shapeFactoryCatalog == null)
                throw new ArgumentNullException("shapeFactoryCatalog");

            RandomizationProvider = randomizationProvider;
            MutationDescriptionCatalog = mutationDescriptionCatalog;
            Target = target;
            ShapeFactoryCatalog = shapeFactoryCatalog;

            Generation = 0;
        }

        public IRandomizationProvider RandomizationProvider { get; private set; }
        public IMutationDescriptionCatalog MutationDescriptionCatalog { get; private set; }
        public ITarget Target { get; private set; }
        public IShapeFactoryCatalog ShapeFactoryCatalog { get; private set; }

        internal Drawing CurrentDrawing { get; set; }


        public Drawing Drawing
        {
            get
            {
                if (CurrentDrawing == null)
                {
                    InitializeDrawingAndDistance();
                }
                return CurrentDrawing;
            }
        }

        internal ulong? CurrentDistanceToTarget { get; set; }

        public ulong DistanceToTarget
        {
            get
            {
                if (!CurrentDistanceToTarget.HasValue)
                {
                    InitializeDrawingAndDistance();
                }
                return CurrentDistanceToTarget.Value;
            }
        }

        public uint Generation { get; private set; }
        public uint Evolutions { get; internal set; }
        public event NewGenerationOccuredEventHandler NewGenerationOccured;
        public event MutationOccuredEventhandler BetterFitnessFound;


        internal void InitializeDrawingAndDistance()
        {
            if (CurrentDrawing == null)
            {
                CurrentDrawing = new Drawing(Target.Working.Width, Target.Working.Height);
                CurrentDistanceToTarget = Target.DistanceTo(CurrentDrawing, ulong.MaxValue);
            }
        }

        public void Mutate()
        {
            Generation++;
            Drawing newDrawing = Drawing.Clone();
            var mutagen = new Mutagen();
            Mutagen.SelectedMutation selectedMutation = mutagen.SelectMutation(this, newDrawing);
            selectedMutation.Description.Mutate(this, selectedMutation.Target);
            ulong newDistanceToTarget = Target.DistanceTo(newDrawing, DistanceToTarget);
            if (newDistanceToTarget < DistanceToTarget)
            {
                Evolutions++;
                CurrentDrawing = newDrawing;
                CurrentDistanceToTarget = newDistanceToTarget;
                if (BetterFitnessFound != null)
                {
                    BetterFitnessFound(this);
                }
            }
            if (NewGenerationOccured != null)
            {
                NewGenerationOccured(this);
            }
        }
    }
}