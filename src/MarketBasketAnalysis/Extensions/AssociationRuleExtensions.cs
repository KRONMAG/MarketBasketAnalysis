using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MarketBasketAnalysis.Extensions
{
    /// <summary>
    /// Defines set operations on sequences of association rules.
    /// </summary>
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static class AssociationRuleExtensions
    {
        #region Methods

        /// <summary>
        /// Computes the difference between two sequences of association rules.
        /// </summary>
        /// <param name="first">The first sequence of association rules.</param>
        /// <param name="second">The second sequence of association rules.</param>
        /// <param name="ignoreLinkDirection">
        /// A value indicating whether the direction of links between association rules should be ignored.
        /// If <c>true</c>, the difference will consider rules as equal regardless of their direction.
        /// </param>
        /// <returns>
        /// A sequence of association rules that are present in <paramref name="first"/> but not in <paramref name="second"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="first"/> or <paramref name="second"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="first"/> or <paramref name="second"/> contains <c>null</c> items.
        /// </exception>
        public static IEnumerable<AssociationRule> Except(
            this IEnumerable<AssociationRule> first,
            IEnumerable<AssociationRule> second,
            bool ignoreLinkDirection = false) =>
            PerformOperation(first, second, ignoreLinkDirection, false, nameof(first), nameof(second));

        /// <summary>
        /// Computes the intersection of two sequences of association rules.
        /// </summary>
        /// <param name="first">The first sequence of association rules.</param>
        /// <param name="second">The second sequence of association rules.</param>
        /// <param name="ignoreLinkDirection">
        /// A value indicating whether the direction of links between association rules should be ignored.
        /// If <c>true</c>, the intersection will consider rules as equal regardless of their direction.
        /// </param>
        /// <returns>
        /// A sequence of association rules that are present in both <paramref name="first"/> and <paramref name="second"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="first"/> or <paramref name="second"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="first"/> or <paramref name="second"/> contains <c>null</c> items.
        /// </exception>
        public static IEnumerable<AssociationRule> Intersect(
            this IEnumerable<AssociationRule> first,
            IEnumerable<AssociationRule> second,
            bool ignoreLinkDirection = false) =>
            PerformOperation(first, second, ignoreLinkDirection, true, nameof(first), nameof(second));

        private static IEnumerable<AssociationRule> PerformOperation(
            IEnumerable<AssociationRule> first,
            IEnumerable<AssociationRule> second,
            bool ignoreLinkDirection,
            bool isIntersection,
            string firstParamName,
            string secondParamName)
        {
            ValidateAssociationRules(first, firstParamName);
            ValidateAssociationRules(second, secondParamName);

            var keys = new HashSet<(int, int)>(second.Select(r => (r.LeftHandSide.Id, r.RightHandSide.Id)));
            var containsDelegate = ignoreLinkDirection
                ? new Func<AssociationRule, HashSet<(int, int)>, bool>((r, k) =>
                    k.Contains((r.LeftHandSide.Id, r.RightHandSide.Id)) ||
                    k.Contains((r.RightHandSide.Id, r.LeftHandSide.Id)))
                : new Func<AssociationRule, HashSet<(int, int)>, bool>((r, k) =>
                    k.Contains((r.LeftHandSide.Id, r.RightHandSide.Id)));
            
            return isIntersection ?
                first.Where(r => containsDelegate(r, keys)) :
                first.Where(r => !containsDelegate(r, keys));
        }

        private static void ValidateAssociationRules(IEnumerable<AssociationRule> associationRules, string paramName)
        {
            if (associationRules == null)
                throw new ArgumentNullException(paramName);

            if (associationRules.Any(item => item == null))
                throw new ArgumentException("Sequence of association rules cannot contain null items.", paramName);
        }

        #endregion
    }
}