﻿using System;
using System.Linq;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Func;

namespace Somnium.Core.Double
{
    public class ActivateNerveCell : NerveCell<double>
    {

        private Func<double, double> activateFuc;

        public ActivateNerveCell(DataSize datasize) : base(datasize)
        {
            Weight = DenseMatrix.CreateRandom(datasize.RowCount, datasize.ColumnCount, new ContinuousUniform());
            Bias = new ContinuousUniform().Median;
            ActivateFuc = Activate.Sigmoid;
        }


        public sealed override Func<double, double> ActivateFuc
        {
            get { return activateFuc; }
            set
            {
                activateFuc = value;
                DeltaActivateFuc = MathNet.Numerics.Differentiate.FirstDerivativeFunc(ActivateFuc);
            }
        }

        public override Func<double, double> DeltaActivateFuc { set; get; }

        public override double Weighted(Matrix<double> inputLayer)
        {
           return inputLayer.PointwiseMultiply(Weight).Enumerate().Sum();
        }

        public override double Activated(Matrix<double> inputLayer)
        {
            return ActivateFuc(inputLayer.PointwiseMultiply(Weight).Enumerate().Sum() + Bias);
        }
    }
}