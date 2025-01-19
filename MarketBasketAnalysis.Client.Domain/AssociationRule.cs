using System;

namespace MarketBasketAnalysis.Client.Domain
{
    public sealed class AssociationRule : IEquatable<AssociationRule>
    {
        #region Fields and Properties

        private readonly int _handSidePairCount;
        private readonly int _transactionCount;

        public AssociationRulePart LeftHandSide { get; }

        public AssociationRulePart RightHandSide { get; }

        public int Count => _handSidePairCount;

        public double Support => (double)_handSidePairCount / _transactionCount;

        public double Confidence => Support / LeftHandSide.Support;

        public double Lift => Confidence / RightHandSide.Support;

        public double Conviction => (1 - RightHandSide.Support) / (1 - Confidence);

        public double YuleQCoefficient => CalculateOnContingencyTable((a, b, c, d) =>
            (a * d - b * c) / (a * d + b * c));

        public double PhiCorrelationCoefficient => CalculateOnContingencyTable((a, b, c, d) =>
            (a * d - b * c) / Math.Sqrt((a + b) * (a + c) * (b + d) * (c + d)));

        public double ChiSquaredTestStatistic => CalculateOnContingencyTable((a, b, c, d) =>
            (a + b + c + d) * Math.Pow(a * d - b * c, 2) / ((a + b) * (a + c) * (b + d) * (c + d)));

        #endregion

        #region Constructors

        public AssociationRule(Item leftHandSideItem, Item rightHandSideItem, int leftHandSideCount,
            int rightHandSideCount, int handSidePairCount, int transactionCount)
        {
            ValidateParameters(leftHandSideItem, rightHandSideItem, leftHandSideCount, rightHandSideCount,
                handSidePairCount, transactionCount);

            LeftHandSide = new AssociationRulePart(leftHandSideItem, leftHandSideCount, transactionCount);
            RightHandSide = new AssociationRulePart(rightHandSideItem, rightHandSideCount, transactionCount);

            _handSidePairCount = handSidePairCount;
            _transactionCount = transactionCount;
        }

        #endregion

        #region Methods

        private static void ValidateParameters(Item leftHandSideItem, Item rightHandSideItem,
            int leftHandSideCount, int rightHandSideCount, int handSidePairCount, int transactionCount)
        {
            if (leftHandSideItem == null)
                throw new ArgumentNullException(nameof(leftHandSideItem));

            if (rightHandSideItem == null)
                throw new ArgumentNullException(nameof(rightHandSideItem));

            if (leftHandSideItem.Equals(rightHandSideItem))
                throw new ArgumentException("Items of left and right hand sides cannot be the the same.");

            if (transactionCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transactionCount), transactionCount,
                    "Transaction count must be greater than zero.");
            }

            if (leftHandSideCount > transactionCount)
            {
                throw new ArgumentOutOfRangeException(nameof(leftHandSideCount), leftHandSideCount,
                    "Left hand side count cannot be greater than transaction count.");
            }

            if (rightHandSideCount > transactionCount)
            {
                throw new ArgumentOutOfRangeException(nameof(rightHandSideCount), rightHandSideCount,
                    "Right hand side count cannot be greater than transaction count.");
            }

            if (handSidePairCount > Math.Min(leftHandSideCount, rightHandSideCount))
            {
                throw new ArgumentOutOfRangeException(nameof(handSidePairCount), handSidePairCount,
                    "Hand side pair count cannot be greater than the minimum of left hand side count and right hand side count.");
            }
        }

        private double CalculateOnContingencyTable(Func<double, double, double, double, double> func)
        {
            var a = _handSidePairCount;
            var b = LeftHandSide.Count - _handSidePairCount;
            var c = RightHandSide.Count - _handSidePairCount;
            var d = _transactionCount - a - b - c;

            return func(a, b, c, d);
        }

        public override int GetHashCode() =>
            LeftHandSide.GetHashCode() * 397 ^ RightHandSide.GetHashCode();

        public override bool Equals(object obj) =>
            Equals(obj as AssociationRule);

        public bool Equals(AssociationRule other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return LeftHandSide.Equals(other.LeftHandSide) &&
                   RightHandSide.Equals(other.RightHandSide);
        }

        public override string ToString() =>
            $"{LeftHandSide} -> {RightHandSide}";

        #endregion
    }
}
